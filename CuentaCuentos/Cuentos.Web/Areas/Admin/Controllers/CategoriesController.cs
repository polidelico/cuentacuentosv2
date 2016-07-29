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
    public class CategoriesController : AdminGlobalController
    {

        public ActionResult Index()
        {
            var categories = Db.Categories.ToList();
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            return View(categories);
        }

        public ActionResult Create()
        {
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "Categories"), "Crear"));

            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                Db.Categories.Add(model);
                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.CreateSuccess);
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            var category = Db.Categories.Find(id);
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Categories", new { id = category.Id }), category.Name), category);

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category model)
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
                var category = Db.Categories.Find(id);
                Db.Categories.Remove(category);
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

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, Category model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Categories"),"Categorías"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Categories", new { id = model.Id }), model.Name);
            }

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }

    }
}
