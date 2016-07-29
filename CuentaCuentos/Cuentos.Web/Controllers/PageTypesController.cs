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
    public class PageTypesController : ApplicationGlobalController
    {
        [WebMethod]
        public JsonResult GetPageTypes()
        {
            var pageTypes = Db.PageTypes.ToList();
            return Json(pageTypes, JsonRequestBehavior.AllowGet);
        }

    }
}
