using Cuentos.Lib;
using Cuentos.Lib.Extensions;
using Cuentos.Models;
using Cuentos.Models.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Areas.Admin.Models;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Cuentos.Areas.Admin.Controllers
{
    public class ApprovalsController : AdminGlobalController
    {


        public async Task<ActionResult> Index()
        {
            IEnumerable<Story> stories = null;
            IEnumerable<User> users = null;
            IEnumerable<Comment> comments = null;

            //TODO: Hacerlo en tabs mas comodo cada tab con su batch

            if (IsSuperAdmin)
            {
                users = await Db.Users.Include("Roles").Include("School").Include("Grade").Include("Interests").Where(u => u.IsApproved == false).ToListAsync();
                stories = await Db.Stories.Include("User.School").Where(s => s.Status == StatusStory.InApproval).ToListAsync();
                comments = await Db.Comments.Include("User").Include("Story.User.School").Where(c => c.IsApproved == false).ToListAsync();

                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            }
            else
            {
                var user = LoggedUser;
                users = await Db.Users.Include("Roles").Include("School").Include("Grade").Include("Interests").Where(u => u.SchoolId == user.SchoolId).Where(u => u.IsApproved == false).ToListAsync();
                stories =await Db.Stories.Include("Ratings").Include("User.School").Where(s => s.User.SchoolId == user.SchoolId).Where(s => s.Status == StatusStory.InApproval).ToListAsync();
                comments = await Db.Comments.Include("User").Include("Story.User.School").Where(c => c.IsApproved == false && c.User.SchoolId == user.SchoolId).ToListAsync();

                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Index","Approvals"), "Aprobaciones"),
                };

            }

            var model = new ApprovalsModel
            {
                Stories = stories,
                Users = users,
                Comments = comments
            };

            return View(model);
        }

        public ActionResult GetUsersForApproval()
        {
            var values = from u in Db.Users
                         join s in Db.Schools on u.SchoolId equals s.Id
                         where u.IsApproved == false
                         select new UsersModel
                         {
                             Name = u.Name + " " + u.LastName,
                             SchoolName = s.Name,
                             UserRole = u.Roles.FirstOrDefault().RoleName,
                             UserDateCreated = u.DateCreated,
                             UserName = u.UserName
                         };

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStoriesForApproval()
        {
            var values = from stories in Db.Stories
                         join users in Db.Users on stories.UserName equals users.UserName
                         join school in Db.Schools on users.SchoolId equals school.Id into users2
                         from us in users2.DefaultIfEmpty()
                         where stories.Status == StatusStory.InApproval
                         select new StoriesModel
                         {
                             StoryName = stories.Name,
                             Author = stories.UserName,
                             School = us.Name,
                             StoryID = stories.Id,
                             Created = (DateTime)stories.CreatedAt
                         };

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCommentsForApproval()
        {
            var values = from comments in Db.Comments
                         join story in Db.Stories on comments.StoryId equals story.Id
                         join users in Db.Users on comments.UserName equals users.UserName
                         join school in Db.Schools on users.SchoolId equals school.Id into school2
                         from sc in school2.DefaultIfEmpty()
                         where comments.IsApproved == false
                         select new commentsModel
                         {
                             StoryName = story.Name,
                             SchoolName = sc.Name,
                             StoryID = story.Id,
                             UserName = users.UserName,
                             Comment = comments.Text,
                             CommentID = comments.Id,
                             CreatedDate = (DateTime)comments.CreatedAt
                         };

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Interest mmodel = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Approvals"),"Aprobaciones"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (mmodel != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Approvals", new { id = mmodel.Id }), mmodel.Name);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }

    }
}
