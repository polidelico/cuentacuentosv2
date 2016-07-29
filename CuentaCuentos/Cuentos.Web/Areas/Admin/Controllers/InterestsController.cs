using Cuentos.Models;
using Cuentos.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using Cuentos.Areas.Admin.Lib;
using System.Net;

namespace Cuentos.Areas.Admin.Controllers
{
    public class InterestsController : AdminGlobalController
    {

        public ActionResult Index()
        {
            var interests = Db.Interests.ToList();
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            return View(interests);
        }

        public ActionResult Create()
        {
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "Interests"), "Crear"));

            return View();
        }

        [HttpPost]
        public ActionResult Create(Interest model)
        {
            if (ModelState.IsValid)
            {
                Db.Interests.Add(model);
                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.CreateSuccess);
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            var interest = Db.Interests.Find(id);
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Interests", new { id = interest.Id }), interest.Name), interest);

            return View(interest);
        }

        [HttpPost]
        public ActionResult Edit(Interest model)
        {
            if (ModelState.IsValid)
            {
                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return View(model).Error(SaveMessage.Error);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            ContentResult result = new ContentResult();

            try
            {
                var interest = Db.Interests.Find(id);
                Db.Interests.Remove(interest);
                Db.SaveChanges();
                result.Content = "success";
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Content = "inUse";
            }
            catch (Exception)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Content = "error";
            }

            return result;
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Interest mmodel = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Interests"),"Intereses"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (mmodel != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Interests", new { id = mmodel.Id }), mmodel.Name);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }

    }
}
