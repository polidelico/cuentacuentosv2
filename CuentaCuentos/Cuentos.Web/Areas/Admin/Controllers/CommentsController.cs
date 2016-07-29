using Cuentos.Models;
using Cuentos.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Cuentos.Areas.Admin.Lib;
using Mandrill;
using Mandrill.Models;

namespace Cuentos.Areas.Admin.Controllers
{
    public class CommentsController : AdminGlobalController
    {

        public ActionResult Index(int id)
        {
            var model = Db.Comments.Include("User").Include("Story.User.School").Where(c => c.StoryId == id);

            Story story = Db.Stories.Include("Images").Include("Grades").Include("Categories").First(s => s.Id == id);

            ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                new KeyValuePair<String, String>(Url.Action("Index", "Stories"), "Cuentos"),
                new KeyValuePair<String, String>(@Url.Action("Edit", "Stories", new { id = id }), story.Name),
                new KeyValuePair<String, String>(@Url.Action("Index", "Comments", new { id = id }), "Comentarios")
            };


            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = Db.Comments.Find(id);

            Story story = Db.Stories.Include("Images").Include("Grades").Include("Categories").First(s => s.Id == model.StoryId);

            ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                new KeyValuePair<String, String>(Url.Action("Index", "Stories"), "Cuentos"),
                new KeyValuePair<String, String>(@Url.Action("Edit", "Stories", new { id = model.StoryId }), story.Name),
                new KeyValuePair<String, String>(@Url.Action("Index", "Comments", new { id = model.StoryId }), "Comentarios"),
                new KeyValuePair<String, String>(@Url.Action("Edit", "Comments", new { id = id }), "Editar")
            };



            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Comment model)
        {
            if (ModelState.IsValid)
            {
                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return RedirectToAction("Details", new { id = model.Id }).Error(SaveMessage.Error);
        }

        [HttpPost]
        public HttpResponseMessage Approve(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var comment = Db.Comments.Include("User").Include("User.ImageHolders").Include("Story").Where(c => c.Id == id).First();
                comment.IsApproved = true;
                comment.ApprovedDate = DateTime.Now;
                comment.ApprovedBy = LoggedUser.UserName;
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };

                MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                var recipients = new List<EmailAddress> { new EmailAddress(comment.User.Email) };
                var email = new EmailMessage
                {
                    To = recipients,
                };

                email.Subject = "Tu cuento ha sido aprobado";
                email.AddGlobalVariable("NAME", comment.User.Name);
                email.AddGlobalVariable("TITLE", "Tu comentario ha sido aprobado.");
                email.AddGlobalVariable("CONTENT", "El comentario hecho en el cuento <strong>" + comment.Story.Name + "</strong> ha sido aprobado.");
                email.AddGlobalVariable("CALLTOACTION", "Lee tu comentario <a href=\"" + Url.Action("Details", "Stories", new { id = comment.Story.Id, area = "" }, Request.Url.Scheme) + "\"> aqui </a>");
                mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));

            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var story = Db.Comments.Find(id);
                Db.Comments.Remove(story);
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Comment model = null)
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
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Stories", new { id = model.Id }), model.Text);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }
    }
}
