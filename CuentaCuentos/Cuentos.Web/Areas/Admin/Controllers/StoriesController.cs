﻿using Cuentos.Models;
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

namespace Cuentos.Areas.Admin.Controllers
{
    public class StoriesController : AdminGlobalController
    {

        public ActionResult Index(int? page)
        {
            IEnumerable<Story> model = null;

            if (IsSuperAdmin)
            {
                model = Db.Stories.Include("Ratings").ToList();
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            }
            else
            {
                var user = LoggedUser;
                var users = Db.Users.Where(u => u.SchoolId == user.SchoolId).Select(u => u.UserName).ToList();

                model = Db.Stories.Include("Ratings").Where(s => users.Contains(s.User.UserName)).ToList();
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

        public ActionResult GetStories()
        {
            var values = from s in Db.Stories
                         select new StoriesModel
                         {
                             StoryID = s.Id,
                             StoryName = s.Name,
                             Author = s.UserName,
                             Status = s.Status.ToString(),
                             Created = (DateTime)s.CreatedAt
                         };

            return Json(values, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int id)
        {
            Story story = Db.Stories.Include("Images").Include("Grades").Include("Categories").Include("Interests").First(s => s.Id == id);
            IEnumerable<StatusStory> statuses = Enum.GetValues(typeof(StatusStory)).Cast<StatusStory>();
            var statusSelect = new List<SelectListItem>();

            InitializeModelImages(story);
            foreach (StatusStory status in statuses)
            {
                statusSelect.Add(new SelectListItem
                {
                    Text = EnumHelper<StatusStory>.GetDisplayValue(status),
                    Value = status.ToString()
                });
            }

            ViewBag.StatusDDL = statusSelect;
            ViewBag.Grades = Db.Grades;
            ViewBag.Categories = Db.Categories.Where(c => c.Active == true);
            ViewBag.Interests = Db.Interests;
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Stories", new { id = id }), story.Name));

            return View(story);
        }

        [HttpPost]
        public ActionResult Edit(Story model, HttpPostedFileBase mainImage, int[] selectedGrades, int[] selectedCategories, int[] selectedInterests)
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
                        var grade = Db.Grades.Find(gradeId);
                        model.Grades.Add(grade);
                    }
                }

                model.Categories.Clear();
                if (selectedCategories != null)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        var category = Db.Categories.Find(categoryId);
                        model.Categories.Add(category);
                    }
                }

                model.Interests.Clear();
                if (selectedInterests != null)
                {
                    foreach (var interestId in selectedInterests)
                    {
                        var interest = Db.Interests.Find(interestId);
                        model.Interests.Add(interest);
                    }
                }

                Db.SaveChanges();

                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return RedirectToAction("Edit", new { id = model.Id }).Error(SaveMessage.Error);
        }

        [HttpPost]
        public HttpResponseMessage Approve(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var story = Db.Stories.Include("User").Include("User.ImageHolders").Where(s => s.Id == id).First();
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
        public ActionResult Delete(int id)
        {
            ContentResult result = new ContentResult();

            try
            {
                var story = Db.Stories.Find(id);
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

        public ActionResult GetUserStories(string username)
        {
            var values = from s in Db.Stories
                         where s.UserName == username
                         select new StoriesModel
                         {
                             StoryID = s.Id,
                             StoryName = s.Name,
                             Author = s.UserName,
                             Status = s.Status.ToString(),
                             Created = (DateTime)s.CreatedAt
                         };

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
