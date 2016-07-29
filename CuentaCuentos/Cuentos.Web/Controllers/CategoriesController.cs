using Cuentos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Cuentos.Controllers
{
    public class CategoriesController : ApplicationGlobalController
    {
        [WebMethod]
        public JsonResult GetCategories()
        {
            var categories = Db.Categories.ToList();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        //[WebMethod]
        //public JsonResult GetStoryCategories(int id)
        //{
        //    var categories = Db.Stories.Include("Categories").Where(s => s.Id == id).Select(s => new { s.Categories }).ToList();
        //    return Json(categories, JsonRequestBehavior.AllowGet);
        //}

    }
}
