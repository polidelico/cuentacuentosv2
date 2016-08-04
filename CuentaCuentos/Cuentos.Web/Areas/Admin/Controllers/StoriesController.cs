using Cuentos.Models;
using Cuentos.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Cuentos.Lib;
using Cuentos.Areas.Admin.Lib;
using Cuentos.Lib.Helpers;
using System.Net;
using Mandrill;
using PagedList;
using Cuentos.Areas.Admin.Models;
using Mandrill.Models;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Cuentos.Areas.Admin.Controllers
{
    public class StoriesController : AdminGlobalController
    {

        public async Task<ActionResult> Index(int? page)
        {
            IEnumerable<Story> model = null;

            if (IsSuperAdmin)
            {
                model = await Db.Stories.Include("Ratings").ToListAsync();
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            }
            else
            {
                var user = LoggedUser;
                var users = await Db.Users.Where(u => u.SchoolId == user.SchoolId).Select(u => u.UserName).ToListAsync();

                model = await Db.Stories.Include("Ratings").Where(s => users.Contains(s.User.UserName)).ToListAsync();
                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Index"), "Cuentos")
                };
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
            //return View(model);
        }

        public async Task<ActionResult> GetStories()
        {
            var values = await Db.Stories.Select(s => new StoriesModel() {
                                                      StoryID = s.Id,
                                                      StoryName = s.Name,
                                                      Author = s.UserName, School = s.Status.ToString(),
                                                      Created = (DateTime)s.CreatedAt,
                                                      Status = s.Status.ToString()
                                                    }).ToListAsync();

            return Json(values, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Edit(int id)
        {
            Story story = null;
            Task<Story> storyTask = Db.Stories.Include("Images").Include("Grades").Include("Categories").Include("Interests").FirstAsync(s => s.Id == id);
            IEnumerable<StatusStory> statuses = Enum.GetValues(typeof(StatusStory)).Cast<StatusStory>();
            var statusSelect = new List<SelectListItem>();

            foreach (StatusStory status in statuses)
            {
                statusSelect.Add(new SelectListItem
                {
                    Text = EnumHelper<StatusStory>.GetDisplayValue(status),
                    Value = status.ToString()
                });
            }

            ViewBag.StatusDDL = statusSelect;
            var gradesTask = Db.Grades.ToListAsync();
            var categories = Db.Categories.Where(c => c.Active).ToListAsync();
            var interest = Db.Interests.ToListAsync();
            try
            {
                Task.WaitAll(gradesTask, categories, interest,storyTask);
               
            }
            catch (AggregateException e)
            {

            }

            ViewBag.Grades = gradesTask.IsCompleted && gradesTask.Exception == null ? gradesTask.Result : null;
            ViewBag.Categories = categories.IsCompleted && categories.Exception == null ? categories.Result : null;
            ViewBag.Interests = interest.IsCompleted && interest.Exception == null ?  interest.Result : null;
            story = storyTask.IsCompleted && storyTask.Exception == null ? storyTask.Result : null;

            InitializeModelImages(story);


            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Stories", new { id = id }), story.Name));

            return View(story);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Story model, HttpPostedFileBase mainImage, int[] selectedGrades, int[] selectedCategories, int[] selectedInterests)
        {
            if (ModelState.IsValid)
            {
                if (mainImage != null)
                {
                    var image = model.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

                    if (image == null)
                    {
                        image = new Cuentos.Models.Image
                        {
                            Target = ImageTarget.MAIN,
                        };
                        model.Images.Add(image);
                        Db.SaveChanges();
                    }

                    UploadImage(mainImage, image);
                }

                model.Grades.Clear();
                if (selectedGrades != null)
                {
                    foreach (var gradeId in selectedGrades)
                    {
                        var grade = await Db.Grades.FindAsync(gradeId);
                        model.Grades.Add(grade);
                    }
                }

                model.Categories.Clear();
                if (selectedCategories != null)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        var category = await Db.Categories.FindAsync(categoryId);
                        model.Categories.Add(category);
                    }
                }

                model.Interests.Clear();
                if (selectedInterests != null)
                {
                    foreach (var interestId in selectedInterests)
                    {
                        var interest = await Db.Interests.FindAsync(interestId);
                        model.Interests.Add(interest);
                    }
                }

                Db.SaveChanges();

                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return RedirectToAction("Edit", new { id = model.Id }).Error(SaveMessage.Error);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Approve(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var story = await Db.Stories.Include("User").Include("User.ImageHolders").Where(s => s.Id == id).FirstAsync();
                story.ApprovedDate = DateTime.Now;
                story.ApprovedBy = LoggedUser.UserName;
                story.Status = StatusStory.Published;
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };


                MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                var recipients = new List<EmailAddress> { new EmailAddress(story.User.Email) };
                var email = new EmailMessage
                {
                    To = recipients,
                };

                email.Subject = "Tu cuento ha sido aprobado";
                email.AddGlobalVariable("NAME", story.User.Name);
                email.AddGlobalVariable("TITLE", "Tu cuento <strong>" + story.Name + "</strong> ha sido aprobado.");
                email.AddGlobalVariable("CONTENT", "Ve lee y comparte tu cuento");
                email.AddGlobalVariable("CALLTOACTION", "Lee tu cuento <a href=\"" + Url.Action("Details", "Stories", new { id = story.Id, area = "" }, Request.Url.Scheme) + "\"> aqui </a>");
                mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));

            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            ContentResult result = new ContentResult();

            try
            {
                var story = await Db.Stories.FindAsync(id);
                story.Status = StatusStory.Deleted;
                Db.SaveChanges();
                result.Content = "story";
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Content = "error";
            }

            return result;
        }

        public async Task<ActionResult> GetUserStories(string username)
        {
            var values = await Db.Stories.Where(s => s.UserName == username).Select(s => new StoriesModel
            {
                StoryID = s.Id,
                StoryName = s.Name,
                Author = s.UserName,
                Status = s.Status.ToString(),
                Created = (DateTime)s.CreatedAt
            }).ToListAsync();

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public void InitializeModelImages(Story story)
        {
            var mainImage = story.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

            if (mainImage == null)
            {
                mainImage = new Cuentos.Models.Image { Target = ImageTarget.MAIN.ToString() };
                story.Images.Add(mainImage);
            }

            mainImage.Dimensions = story.GetSectionItemImageDimensions(ImageTarget.MAIN);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Story model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Stories"),"Cuentos"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Stories", new { id = model.Id }), model.Name);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }
    }
}
