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
    public class RatingsController : AdminGlobalController
    {

        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> Details(int id)
        {

            Story story = await Db.Stories.Include("Images").Include("Grades").Include("Categories").FirstAsync(s => s.Id == id);

            var ratings = await Db.Ratings.Where(r => r.StoryId == id).ToListAsync();
            ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Stories"), "Cuentos"),
                    new KeyValuePair<String, String>(@Url.Action("Edit", "Stories", new { id = id }), story.Name),
                    new KeyValuePair<String, String>(@Url.Action("Details", "Ratings", new { id = id }), "Puntuaciones")
                };

            var model = new Rating
            {
                Ratings = ratings
            };
            return View(model);
        }



        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Interest mmodel = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Ratings"),"Puntuaciones"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (mmodel != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Ratings", new { id = mmodel.Id }), mmodel.Name);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }

    }
}
