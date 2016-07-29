using Cuentos.Lib;
using Cuentos.Lib.Extensions;
using Cuentos.Models;
using Cuentos.Models.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;

namespace Cuentos.Controllers
{
    public class UsersController : ApplicationGlobalController
    {
        ////
        //// GET: /Users/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //
        // GET: /Users/Details/5

        public ActionResult Details(string id)
        {

            CultureInfo es = new CultureInfo("es-PR");
            Thread.CurrentThread.CurrentCulture = es;


            var user = Db.Users.Include("ImageHolders").Where(u => u.UserName == id).First();


            ViewBag.Stories = Db.Stories.Where(s => s.UserName == id && s.Status == StatusStory.Published).ToList();
            return View(user);
        }

    }
}
