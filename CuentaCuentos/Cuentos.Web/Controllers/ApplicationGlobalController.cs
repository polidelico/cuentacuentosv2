using Cuentos.Lib;
using Cuentos.Lib.Utils;
using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace Cuentos.Controllers
{
    public class AdminGlobalControllerFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CuentosContext db = new CuentosContext();
            var userTask = db.Users.Include("ImageHolders").Where(u => u.UserName == HttpContext.Current.User.Identity.Name).FirstAsync();
            User loggedUser = null;
            try
            {
                Task.WaitAll(userTask);
            }catch(AggregateException e)
            {
                var errors = new List<string>();

                for(int i = 0; i < e.InnerExceptions.Count; i++)
                {
                    var exception = e.InnerExceptions[i];

                    errors.Add(exception.Message);
                }

                System.IO.File.WriteAllLines(@"C:\exceptions.text", errors.ToArray());
            }

            loggedUser = userTask.IsCompleted && userTask.Exception == null ? userTask.Result : null;
            filterContext.Controller.ViewBag.LoggedUser = loggedUser;
        }
    }

    [AdminGlobalControllerFilter]
    public class ApplicationGlobalController : Controller, IDataContext
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
            get
            {
                return Db.Users.Include("ImageHolders").Where(u => u.UserName == HttpContext.User.Identity.Name).First();
            }
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
    }
}
