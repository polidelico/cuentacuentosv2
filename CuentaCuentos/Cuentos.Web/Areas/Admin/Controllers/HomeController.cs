using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Models;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Cuentos.Areas.Admin.Controllers
{
    public class HomeController : AdminGlobalController
    {
        //
        // GET: /Admin/Home/

        public async Task<ActionResult> Index()
        {
            //TODO: Batch de aprobaciones con conteo de aprobaciones  vigentes.

            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            Task<int> contacts = null;
            Task<int> users = null;
            Task<int> stories = null;
            Task<int> comments = null;

            var ContactsBatch = 0;
            var UsersBatch = 0;
            var StoriesBatch = 0;
            var CommentsBatch = 0;
            if (IsSuperAdmin)
            {
                contacts = Db.Contacts.Where(c => c.isRead == false).CountAsync();
                users = Db.Users.Where(u => u.IsApproved == false).CountAsync();
                stories =  Db.Stories.Where(s => s.Status == StatusStory.InApproval).CountAsync();
                comments =  Db.Comments.Where(c => c.IsApproved == false).CountAsync();
            }
            else
            {
                var user = LoggedUser;
                contacts =  Db.Contacts.Where(c => c.isRead == false && c.SchoolId == user.SchoolId).CountAsync();
                users =  Db.Users.Where(u => u.IsApproved == false && u.SchoolId == user.SchoolId).CountAsync();
                stories =  Db.Stories.Where(s => s.Status == StatusStory.InApproval && s.User.SchoolId == user.SchoolId).CountAsync();
                comments =  Db.Comments.Where(c => c.IsApproved == false && c.User.SchoolId == user.SchoolId).CountAsync();
            }
            try
            {
                Task.WaitAll(contacts, users, stories, comments);

            }
            catch (AggregateException e)
            {

            }
            ContactsBatch = contacts.Exception == null && contacts.IsCompleted ? contacts.Result : 0;
            UsersBatch = users.Exception == null && users.IsCompleted ? users.Result : 0;
            StoriesBatch = stories.Exception == null && stories.IsCompleted ? stories.Result : 0;
            CommentsBatch = comments.Exception == null && comments.IsCompleted ? comments.Result : 0;
            ViewBag.ContactsBatch = ContactsBatch;
            ViewBag.UsersBatch = UsersBatch;
            ViewBag.StoriesBatch = StoriesBatch;
            ViewBag.CommentsBatch = CommentsBatch;

            return View();
        }



        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Home"),"Inicio"),
            };

            return breadcrumbs;
        }

    }
}
