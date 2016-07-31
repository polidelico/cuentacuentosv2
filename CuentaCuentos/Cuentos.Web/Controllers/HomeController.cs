using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Cuentos.Controllers
{
    public class HomeController : ApplicationGlobalController
    {
        public async Task<ActionResult> Index()
        {           
            var stories = await Db.Stories
                .Where(b => b.Featured == true && b.Status == StatusStory.Published)
                .ToListAsync();

            return View(stories);
        }

        public ActionResult BuildYourStory()
        {
            ViewBag.Message = "Your app description page.";
                
            return View();
        }


        public ActionResult Educators()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }


        public ActionResult Parents()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Terms()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        
    }
}
