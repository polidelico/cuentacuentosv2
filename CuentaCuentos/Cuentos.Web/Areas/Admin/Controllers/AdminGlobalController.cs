using Cuentos.Lib;
using Cuentos.Lib.Utils;
using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuentos.Areas.Admin.Controllers
{

    public class AdminGlobalControllerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CuentosContext db = new CuentosContext();
            var user = db.Users.Find(HttpContext.Current.User.Identity.Name);
            var userRole = (Role.RoleType)Enum.Parse(typeof(Role.RoleType), user.Roles.First().RoleName, true);

            filterContext.Controller.ViewBag.LoggedUser = user;
            filterContext.Controller.ViewBag.IsSuperAdmin = userRole == Role.RoleType.superAdmin ? true : false;
            filterContext.Controller.ViewBag.StoriesCounterBatch = db.Stories.Count();
            filterContext.Controller.ViewBag.SchoolsCounterBatch = db.Schools.Count();
            filterContext.Controller.ViewBag.UsersCounterBatch = db.Users.Count();
        }
    }

    [Authorize(Roles = "superAdmin, schoolAdmin")]
    [AdminGlobalControllerFilter]
    public class AdminGlobalController : Controller, IDataContext
    {
        public readonly CuentosContext cuentosContext = new CuentosContext();

        public CuentosContext Db
        {
            get
            {
                return cuentosContext;
            }
        }

        public User LoggedUser
        {
            get { return Db.Users.Find(HttpContext.User.Identity.Name); }
        }

        public bool IsSuperAdmin
        {
            get
            {
                var userRole = (Role.RoleType)Enum.Parse(typeof(Role.RoleType), LoggedUser.Roles.First().RoleName, true);
                return userRole == Role.RoleType.superAdmin ? true : false;
            }
        }

        public bool IsAuthorized(int schoolId)
        {
            return (IsSuperAdmin || !IsSuperAdmin && LoggedUser.SchoolId == schoolId);
        }

        public void IsAuthorizedRedirect(int schoolId)
        {
            if (!IsAuthorized(schoolId))
                Response.Redirect(Url.Action("Index", "Home"));
        }

        public List<KeyValuePair<String, String>> Breadcrumbs()
        {
            return new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Inicio")
            };
        }

        public void UploadImage(HttpPostedFileBase imageFile, Image imageModel, bool emptyDir = true, int width = 0, int height = 0)
        {
            UploadFilesResult status = null;

            if (width == 0 && height == 0)
                status = Uploader.UploadFile(this, imageFile, imageModel.Id, emptyDir);
            else
                status = Uploader.UploadAndScaleFile(this, imageFile, imageModel.Id, width, height);

            imageModel.ContentType = status.type;
            imageModel.Size = status.size;
            imageModel.Filename = status.name;
        }

        public void RemoveImage(string assedId, string filename)
        {
            Uploader.RemoveFile(this, assedId, filename);
        }
    }
}
