using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Models;
namespace Cuentos.Areas.Admin.Controllers
{
    public class HomeController : AdminGlobalController
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            //TODO: Batch de aprobaciones con conteo de aprobaciones  vigentes.

            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            var ContactsBatch = 0;
            var UsersBatch = 0;
            var StoriesBatch = 0;
            var CommentsBatch = 0;
            if (IsSuperAdmin)
            {
                ContactsBatch = Db.Contacts.Where(c => c.isRead == false).Count();
                UsersBatch = Db.Users.Where(u => u.IsApproved == false).Count();
                StoriesBatch = Db.Stories.Where(s => s.Status == StatusStory.InApproval).Count();
                CommentsBatch = Db.Comments.Where(c => c.IsApproved == false).Count();
            }
            else
            {
                var user = LoggedUser;
                ContactsBatch = Db.Contacts.Where(c => c.isRead == false && c.SchoolId == user.SchoolId).Count();
                UsersBatch = Db.Users.Where(u => u.IsApproved == false && u.SchoolId == user.SchoolId).Count();
                StoriesBatch = Db.Stories.Where(s => s.Status == StatusStory.InApproval && s.User.SchoolId == user.SchoolId).Count();
                CommentsBatch = Db.Comments.Where(c => c.IsApproved == false && c.User.SchoolId == user.SchoolId).Count();
            }

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
