using Cuentos.Lib;
using Cuentos.Lib.Extensions;
using Cuentos.Models;
using Cuentos.Models.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<ActionResult> Details(string id)
        {

            CultureInfo es = new CultureInfo("es-PR");
            Thread.CurrentThread.CurrentCulture = es;


            var user = await Db.Users.Include("ImageHolders").FirstOrDefaultAsync(u => u.UserName == id);

            var stories = await Db.Stories.Where(s => s.UserName == id && s.Status == StatusStory.Published).ToListAsync();
            ViewBag.Stories = stories;
            return View(user);
        }

    }
}
