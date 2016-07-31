﻿using Cuentos.Lib;
using Cuentos.Lib.Utils;
using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace Cuentos.Controllers
{
    public class AdminGlobalControllerFilter : ActionFilterAttribute
    {
        public override async void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CuentosContext db = new CuentosContext();
            var user = await db.Users.Include("ImageHolders").Where(u => u.UserName == HttpContext.Current.User.Identity.Name).FirstOrDefaultAsync();

            filterContext.Controller.ViewBag.LoggedUser = user;
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
