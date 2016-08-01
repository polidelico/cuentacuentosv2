using Cuentos.Lib;
using Cuentos.Lib.Extensions;
using Cuentos.Models;
using Cuentos.Models.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
namespace Cuentos.Areas.Admin.Controllers
{
    public class FeaturedController : AdminGlobalController
    {
        //
        // GET: /Admin/Featured/

        public async Task<ActionResult> Index()
        {
            IEnumerable<Story> stories = null;
            IEnumerable<User> users = null;

            if (IsSuperAdmin)
            {
                users = Db.Users;
                stories = await Db.Stories.Include("Ratings").Where(s => s.Status == StatusStory.Published).ToListAsync();
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            }
            else
            {
                var user = LoggedUser;
                users = await Db.Users.Where(u => u.SchoolId == user.SchoolId).ToListAsync();

                stories = await Db.Stories.Include("Ratings").Where(s => s.User.SchoolId == user.SchoolId && s.Status == StatusStory.Published).ToListAsync();
                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Index"), "Destacados")
                };


            }

            var model = new FeaturedModel
            {
                Stories = stories,
                Users = users
            };



            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int[] selectedFeatured)
        {
            var stories = await Db.Stories.ToListAsync();
            foreach (var story in stories)
            {
                story.Featured = false;
            }

            if (selectedFeatured != null)
            {
                foreach (int id in selectedFeatured)
                {
                    var story = Db.Stories.Find(id);
                    story.Featured = true;
                }


            }

            Db.SaveChanges();

            return RedirectToAction("Index").Success("Cuentos actualizados por destacados/no destacados");
        }


        [HttpPost]
        public async Task<ActionResult> EditUsers(string[] selectedFeatured)
        {
            var users = await Db.Users.Include("ImageHolders").ToListAsync();
            foreach (var user in users)
            {
                user.Featured = false;
            }

            if (selectedFeatured != null)
            {
                foreach (string id in selectedFeatured)
                {
                    var user = Db.Users.Find(id);
                    user.Featured = true;
                }


            }

            Db.SaveChanges();

            return RedirectToAction("Index").Success("Usuarios actualizados por destacados/no destacados");
        }




        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Interest mmodel = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Featured"),"Destacados"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (mmodel != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Featured", new { id = mmodel.Id }), mmodel.Name);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }

    }
}
