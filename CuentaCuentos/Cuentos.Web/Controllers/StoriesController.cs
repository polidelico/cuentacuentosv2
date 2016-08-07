using Cuentos.Lib;
using Cuentos.Lib.Extensions;
using Cuentos.Models;
using Cuentos.Models.view;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using PagedList.Mvc;
using PagedList;
using System.Web.Helpers;
using Mandrill;
using ImageMagick;
using System.IO;
using System.Net;
using Mandrill.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Cuentos.Controllers
{
    public class StoriesController : ApplicationGlobalController
    {

        [Authorize]
        public ActionResult Create()
        {
            var story = new Story(true);
            story.UserName = LoggedUser.UserName;
            Db.Stories.Add(story);
            Db.SaveChanges();

            return RedirectToAction("Edit", new { id = story.Id });
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var story = await Db.Stories.FindAsync(id);

            var categories = await Db.Categories.Select(c => new { c.Id, c.Name, c.Active }).Where(c => c.Active == true).ToListAsync();
            var grades = await Db.Grades.Select(g => new { g.Id, g.Name, g.Position }).OrderBy(g => g.Position).ToListAsync();
            var pageTypes = await Db.PageTypes.Where(t => t.Active == true).OrderBy(t => t.Position).ToListAsync();
            var galleries = await Db.BuilderGalleries.Include("Images").Where(g => (g.Active == true && g.UserName == null)
                || (g.UserName == LoggedUser.UserName && g.Active == true)).ToListAsync();

            if (story == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<dynamic> images = new List<dynamic>();
            List<dynamic> templates = new List<dynamic>();
            List<ImageCategory> allImageCategories = await Db.ImageCategories.ToListAsync();
            List<ImageCategory> imageCategories = new List<ImageCategory>();
            var returnStory = new Story
            {
                Id = story.Id,
                Name = story.Name,
                Summary = story.Summary,
                Categories = story.Categories.Select(c => new Category { Id = c.Id, Name = c.Name }).ToList(),
                Grades = story.Grades.Select(g => new Grade { Id = g.Id, Name = g.Name }).ToList(),
            };
            
            foreach (BuilderGallery gallery in galleries)
            {
                var isUserGallery = string.IsNullOrEmpty(gallery.UserName) ? false : true;

                foreach (var img in gallery.Images)
                {
                    images.Add(new
                    {
                        id = img.Id,
                        imagebleId = img.ImagebleId,
                        filename = img.Filename,
                        category = img.Target,
                        imagePath = img.ImagePath,
                        belongToUser = isUserGallery
                    });

                    if (isUserGallery == false && !imageCategories.Exists(c => c.Id == Convert.ToInt32(img.Target)))
                        imageCategories.Add(allImageCategories.First(c => c.Id == Convert.ToInt32(img.Target)));
                }
            }

            foreach (PageType pageType in pageTypes)
            {
                templates.Add(new
                {
                    id = pageType.Id,
                    name = pageType.Name,
                    description = pageType.Description,
                    imagePath = pageType.GetImagePathByTargetOrDefault(ImageTarget.MAIN),
                    position = pageType.Position
                });
            }

            //ViewBag.Pages = story.Pages;
            ViewBag.Grades = grades;
            ViewBag.Categories = categories;
            ViewBag.ImageCategories = imageCategories;
            ViewBag.PageTypes = templates;
            ViewBag.Images = images;
            ViewBag.Id = story.Id;

            return View(returnStory);
        }

        [Authorize]
        [WebMethod, HttpPost]
        public async Task<ActionResult> Save(int id, string name, string summary, string selectedCategories, string selectedGrades)
        {
            var result = false;
            try
            {
                var story = await Db.Stories.FindAsync(id);
                dynamic newCategories = JsonConvert.DeserializeObject(selectedCategories);
                dynamic newGrades = JsonConvert.DeserializeObject(selectedGrades);
                int integer;

                story.Name = name;
                story.Summary = summary;
                story.Categories.Clear();
                story.Grades.Clear();

                foreach (string categoryId in newCategories)
                {
                    if (Int32.TryParse(categoryId, out integer))
                    {
                        var category = Db.Categories.Find(integer);
                        story.Categories.Add(category);
                    }
                }

                foreach (string gradeId in newGrades)
                {
                    if (Int32.TryParse(gradeId, out integer))
                    {
                        var grade = Db.Grades.Find(integer);
                        story.Grades.Add(grade);
                    }
                }

               

                Db.SaveChanges();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return (this.Json(result, "application/json"));
        }

        public async Task<ActionResult> Details(int id)
        {
            double average = 0;
            var story = await Db.Stories.Include("User")
                .Include("User.ImageHolders")
                .Include("Comments")
                .Include("Comments.User")
                .Include("Comments.User.ImageHolders")
                .Include("Ratings").Where(s => s.Id == id).FirstOrDefaultAsync();

            if (story != null && story.isViewable(id))
            {
                if (story.Ratings.Count > 0)
                {
                    double sum = story.Ratings.Sum(r => r.Rate);
                    double quantity = story.Ratings.Count;
                    average = sum / quantity;
                    average = Math.Round(average, 1, MidpointRounding.AwayFromZero);
                }

                if (Request.Cookies["views_cookie_id_" + story.Id] == null)
                {
                    var userCookie = new HttpCookie("views_cookie_id" + story.Id);
                    userCookie.Expires.AddDays(365);
                    HttpContext.Response.Cookies.Add(userCookie);
                    story.Views++;

                }
                //TODO: Mex adding a cookie to if cookie exist not add more views

                Db.SaveChanges();
                ViewBag.Average = average;

                return View(story);
            }

            return RedirectToAction("Index", "Home");


        }

        [HttpPost]
        public async Task<ActionResult> AddComment(int storyId, string newComment)
        {

            if (!string.IsNullOrEmpty(newComment))
            {
                //var story = Db.Stories.Find(storyId);
                var story = await Db.Stories.Include("User.ImageHolders").Include("User").Where(s => s.Id == storyId).FirstAsync();
                var comment = new Comment
                {
                    UserName = LoggedUser.UserName,
                    Text = newComment
                };
                story.Comments.Add(comment);
                Db.SaveChanges();

                MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                var email = new EmailMessage();
                var recipients = new List<EmailAddress>();
                var admins = await Db.Users.Include("Roles").Where(u => u.SchoolId == story.User.SchoolId).ToListAsync();

                foreach (var admin in admins)
                {
                    if (admin.Roles != null && admin.Roles.First().RoleName == Role.RoleType.schoolAdmin.ToString())
                    {
                        recipients.Add(new EmailAddress(admin.Email));
                        email.AddRecipientVariable(admin.Email, "NAME", admin.Name);
                    }

                }

                if (recipients.Count > 0)
                {
                    email.To = recipients;
                    email.Subject = "Comentario para aprobación";
                    email.AddGlobalVariable("TITLE", "Se ha sometido un comentario al cuento:  <strong>" + story.Name + "</strong> para aprobación");
                    email.AddGlobalVariable("CONTENT", "El contenido del comentario es: <i>" + newComment + "</i>.");
                    email.AddGlobalVariable("CALLTOACTION", "Aprueba el comentario <a href=\"" + Url.Action("Index", "Approvals", new { area = "admin" }, Request.Url.Scheme) + "\"> aqui </a>");
                   // mandrill.SendMessage(email, "general", null);
                }

                return RedirectToAction("Details", new { id = storyId }).Success("Comentario sometido para aprobación");
            }

            return RedirectToAction("Details", new { id = storyId }).Error("Ha ocurrido un error.");
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RateStory(int storyId, int rate)
        {
            HttpResponseMessage response = null;

            try
            {
                var ratings = await Db.Ratings.Where(r => r.UserName == LoggedUser.UserName && r.StoryId == storyId).ToListAsync();
                if (LoggedUser != null && ratings.Count == 0)
                {
                    var story = await Db.Stories.FindAsync(storyId);
                    var rating = new Rating
                    {
                        UserName = LoggedUser.UserName,
                        Rate = rate
                    };

                    story.Ratings.Add(rating);
                    Db.SaveChanges();
                    response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
                }
                else
                {
                    response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
                }
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var story = await Db.Stories.FindAsync(id);
                //Db.Stories.Remove(story);
                story.Status = StatusStory.Deleted;
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Publish(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var story = await Db.Stories.Include("User").Where(s => s.Id == id).FirstAsync();
                story.Status = StatusStory.InApproval;
                Db.SaveChanges();

                MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                var email = new EmailMessage();
                var recipients = new List<EmailAddress>();
                var admins = await Db.Users.Include("Roles").Where(u => u.SchoolId == story.User.SchoolId).ToListAsync();

                foreach (var admin in admins)
                {
                    if (admin.Roles != null && admin.Roles.First().RoleName == Role.RoleType.schoolAdmin.ToString())
                    {
                        recipients.Add(new EmailAddress(admin.Email));
                        email.AddRecipientVariable(admin.Email, "NAME", admin.Name);
                    }

                }

                if (recipients.Count > 0)
                {
                    email.To = recipients;
                    email.Subject = "Cuanto para aprobación";
                    email.AddGlobalVariable("TITLE", "El usuario <strong>" + story.UserName + "</strong> ha sometido un cuento para aprobación");
                    email.AddGlobalVariable("CONTENT", "El título del cuento para aprobar es: <strong>" + story.Name + "</strong>.");
                    email.AddGlobalVariable("CALLTOACTION", "Apruebe el cuento <a href=\"" + Url.Action("Index", "Approvals", new { area = "admin" }, Request.Url.Scheme) + "\"> aqui </a>");
                    mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));
                }

                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UnPublish(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var story = await Db.Stories.FindAsync(id);
                story.Status = StatusStory.UnPublished;
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        public async Task<ActionResult> Search(int? pageNum, SearchModel model)
        {
            model = model ?? new SearchModel();
            int pageSize = 6;
            int pageNumber = pageNum ?? 1;
            var cities = Db.Cities;
            var schools = Db.Schools;
            IEnumerable<Story> stories = null;

            if (model.q != null)
            {
                stories = await Db.Stories
                                  .Include("User").Include("Grades")
                                  .Include("Categories").Include("Images")
                                  .Where(s => s.Name.Contains(model.q) &&
                                        s.Status == StatusStory.Published)
                                  .ToListAsync();
            }
            if (model.CityId != null)
            {
                stories = stories != null ? stories.Where(s => s.User.School.CityId == model.CityId)
                    : await Db.Stories.Include("User").Include("Grades")
                                .Include("Categories").Include("Images")
                                .Where(s => s.User.School.CityId == model.CityId).ToListAsync();
            }
            if (model.SchoolId != null)
            {
                stories = stories != null ? stories.Where(s => s.User.SchoolId == model.SchoolId)
                    : await Db.Stories.Include("User").Include("Grades")
                                       .Include("Categories").Include("Images")
                                       .Where(s => s.User.SchoolId == model.SchoolId)
                                       .ToListAsync();
            }
            if (model.selectedGrades.Count() > 0)
            {
                IEnumerable<Story> gradeStories = Enumerable.Empty<Story>();

                foreach (var strGradeId in model.selectedGrades)
                {
                    int gradeId = Convert.ToInt32(strGradeId);
                    gradeStories = stories != null ? gradeStories.Union(stories
                                                                        .Where(s => 
                                                                        s.Grades.Select(g => g.Id)
                                                                        .Contains(gradeId)))
                        : gradeStories.Union(await Db.Stories.Include("User").Include("Categories")
                                                             .Where(s => s.Grades.Select(g => g.Id)
                                                             .Contains(gradeId)).ToListAsync());
                }

                stories = gradeStories;
            }
            if (model.selectedCategories.Count() > 0)
            {
                IEnumerable<Story> categoryStories = Enumerable.Empty<Story>();

                foreach (var strCategoryId in model.selectedCategories)
                {
                    int categoryId = Convert.ToInt32(strCategoryId);
                    categoryStories = stories != null ? categoryStories.Union(stories
                                                                              .Where(s => s.Categories
                                                                                     .Select(c => c.Id)
                                                                                     .Contains(categoryId)))
                        : categoryStories.Union(await Db.Stories.Include("User").Include("Categories")
                                                      .Where(s => s.Categories
                                                             .Select(c => c.Id)
                                                             .Contains(categoryId))
                                                      .ToListAsync());
                }

                stories = categoryStories;
            }
            if (stories != null)
                stories = stories.Where(s => s.Status == StatusStory.Published);
            else
                stories = await Db.Stories.Include("User").Include("Grades")
                                          .Include("Categories").Include("Images")
                                          .Where(s => s.Status == StatusStory.Published).ToListAsync();

            model.Stories = stories.ToPagedList(pageNumber, pageSize);
            var grades = await Db.Grades.OrderBy(g => g.Position).ToListAsync();
            var categories = await Db.Categories.Where(c => c.Active).ToListAsync();
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            ViewBag.Cities = new SelectList(cities, "Id", "Name");
            ViewBag.Grades = grades;
            ViewBag.Categories = categories;

            return View(model);
        }

        public async Task<ActionResult> ConvertPDF(int id, HttpPostedFileBase postedFiles)
        {
            if (postedFiles != null && postedFiles.ContentType == "application/pdf")
            {
                var story = await Db.Stories.FindAsync(id);
                var path = System.Web.HttpContext.Current.Server.MapPath("~/bin");
                List<Page> pages = JsonConvert.DeserializeObject<List<Page>>(story.Pages);

                MagickNET.SetGhostscriptDirectory(path);
                MagickReadSettings settings = new MagickReadSettings
                {
                    Density = new Density(240, 240),
                   
                };

                using (MagickImageCollection images = new MagickImageCollection())
                {
                    int pageNum = 1;
                    images.Read(postedFiles.InputStream, settings);

                    foreach (MagickImage image in images)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(postedFiles.FileName) + pageNum + ".jpeg";
                        var fileUrl = "/Content/dynamic/stories/" + id + "/" + fileName;
                        path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Dynamic/stories/" + id + "/");
                        image.Resize(390, 558);
                        image.Alpha(AlphaOption.Remove);
                        image.Format = MagickFormat.Jpeg;

                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);

                        image.Write(path + fileName);
                        pages.Add(new Page { Text = "", Type = "BigImage", ImageUrl = fileUrl, Position = pages.Count });
                        pageNum++;
                    }
                }

                story.Pages = JsonConvert.SerializeObject(pages);
                Db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = id });
        }

        public async Task<ActionResult> SendEmail(EmailShare model)
        {
            ContentResult result = new ContentResult();

            try
            {
                if (!string.IsNullOrEmpty(model.To))
                {
                    MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                    var email = new EmailMessage();
                    email.To = new List<EmailAddress> { new EmailAddress { Email = model.To } };
                    email.Subject = LoggedUser.Fullname + " ha compartido un cuento contigo";
                    email.AddGlobalVariable("NAME", model.To);
                    email.AddGlobalVariable("TITLE", model.Story.Name + " por " + model.Story.User.Name + " - En Cuenta Cuentos");
                    email.AddGlobalVariable("CONTENT", LoggedUser.Fullname + " ha compartido el cuento " + model.Story.Name + "contigo.");
                    email.AddGlobalVariable("CALLTOACTION", "Lee el cuento <a href=\"" + Url.Action("Details", "Stories", new { id = model.Story.Id }, Request.Url.Scheme) + "\"> aqui </a>");
                    var res = await mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));

                    result.Content = "success";
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Content = "error";
            }

            return result;
        }

        public void InitializeModelImages(Story story)
        {
            var mainImage = story.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

            if (mainImage == null)
            {
                mainImage = new Models.Image { Target = ImageTarget.MAIN.ToString() };
                story.Images.Add(mainImage);
            }

            mainImage.Dimensions = story.GetSectionItemImageDimensions(ImageTarget.MAIN);
        }
    }
}
