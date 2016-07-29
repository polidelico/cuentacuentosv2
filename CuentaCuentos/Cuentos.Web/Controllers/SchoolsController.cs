using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Models;
namespace Cuentos.Controllers
{
    public class SchoolsController : ApplicationGlobalController
    {

        public ActionResult Index()
        {
            var schools = Db.Schools.ToList();

            
            
            return View(schools);
        }

        public ActionResult Details(int id)
        {

            ViewBag.Users = Db.Users.Include("ImageHolders").Where(u => u.SchoolId == id && u.Featured == true).ToList();
            ViewBag.Stories = Db.Stories.Include("Ratings").Where(s => s.User.SchoolId == id && s.Featured == true && s.Status == StatusStory.Published).ToList();

            var school = Db.Schools.Find(id);
            return View(school);
        }


    }

}