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

            var ContactsBatch = 0;
            var UsersBatch = 0;
            var StoriesBatch = 0;
            var CommentsBatch = 0;
            if (IsSuperAdmin)
            {
                ContactsBatch = await Db.Contacts.Where(c => c.isRead == false).CountAsync();
                UsersBatch = await Db.Users.Where(u => u.IsApproved == false).CountAsync();
                StoriesBatch = await Db.Stories.Where(s => s.Status == StatusStory.InApproval).CountAsync();
                CommentsBatch = await Db.Comments.Where(c => c.IsApproved == false).CountAsync();
            }
            else
            {
                var user = LoggedUser;
                ContactsBatch = await Db.Contacts.Where(c => c.isRead == false && c.SchoolId == user.SchoolId).CountAsync();
                UsersBatch = await Db.Users.Where(u => u.IsApproved == false && u.SchoolId == user.SchoolId).CountAsync();
                StoriesBatch = await Db.Stories.Where(s => s.Status == StatusStory.InApproval && s.User.SchoolId == user.SchoolId).CountAsync();
                CommentsBatch = await Db.Comments.Where(c => c.IsApproved == false && c.User.SchoolId == user.SchoolId).CountAsync();
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
