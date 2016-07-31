using Cuentos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Data.Entity;

namespace Cuentos.Controllers
{
    public class GradesController : ApplicationGlobalController
    {
        [WebMethod]
        public async Task<JsonResult> GetPageTypes()
        {
            var pageTypes = await Db.PageTypes.ToListAsync();
            return Json(pageTypes, JsonRequestBehavior.AllowGet);
        }

    }
}
