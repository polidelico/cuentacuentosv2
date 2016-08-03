using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using Cuentos.Models;
namespace Cuentos.Controllers
{
    public class SchoolsController : ApplicationGlobalController
    {

        public async Task<ActionResult> Index()
        {
            var schools = await Db.Schools.ToListAsync();
            
            return View(schools);
        }

        public async Task<ActionResult> Details(int id)
        {

            var usersTask = Db.Users.Include("ImageHolders")
                                    .Where(u => u.SchoolId == id && u.Featured == true)
                                    .ToListAsync();
            var storiesTask = Db.Stories.Include("Ratings").Where(s => s.User.SchoolId == id
                                                     && s.Featured == true
                                                     && s.Status == StatusStory.Published)
                                            .ToListAsync();
            var school = Db.Schools.FindAsync(id);

            Task.WaitAll(usersTask, storiesTask,school);
            ViewBag.Stories = storiesTask.Result;
            ViewBag.Users = usersTask.Result;

            return View(school.Result);
        }


    }

}