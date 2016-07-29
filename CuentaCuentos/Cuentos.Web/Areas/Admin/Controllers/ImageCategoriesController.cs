using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Lib.Extensions;
using Cuentos.Lib;
using System.Net.Http;
using Cuentos.Areas.Admin.Lib;

namespace Cuentos.Areas.Admin.Controllers
{
    [Authorize(Roles = "superAdmin")]
    public class ImageCategoriesController : AdminGlobalController
    {

        public ActionResult Index()
        {
            var imageCategories = Db.ImageCategories.ToList();
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            return View(imageCategories);
        }

        public ActionResult Create()
        {
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "ImageCategories"), "Crear"));
            return View();
        }

        [HttpPost]
        public ActionResult Create(ImageCategory model, HttpPostedFileBase mainImage)
        {

            if (ModelState.IsValid)
            {
                Db.ImageCategories.Add(model);
                Db.SaveChanges();

                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.CreateSuccess);
            }

            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "ImageCategories"), "Crear"));
            return View(model).Error(SaveMessage.Error);
        }

        public ActionResult Edit(int id)
        {
            var imageCategory = Db.ImageCategories.Find(id);
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "ImageCategories", new { id = imageCategory.Id }), imageCategory.Name), imageCategory);

            return View(imageCategory);
        }

        [HttpPost]
        public ActionResult Edit(ImageCategory model, HttpPostedFileBase mainImage)
        {
            if (ModelState.IsValid)
            {

                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return RedirectToAction("Edit", new { id = model.Id }).Error(SaveMessage.Error);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var imageCategory = Db.ImageCategories.Find(id);
                Db.ImageCategories.Remove(imageCategory);
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }




        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, ImageCategory model = null)
        {
            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                currentItem
            };
            return Breadcrumbs(controllerBreadcrumbs, model);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(List<KeyValuePair<String, String>> items, ImageCategory model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();


            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(IsSuperAdmin ? @Url.Action("Index", "ImageCategories") : @Url.Action("Edit", "ImageCategories"),"Categorias de Imágenes"),
            };


            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "ImageCategories", new { id = model.Id }), model.Name);
            }

            foreach (KeyValuePair<string, string> item in items)
            {
                if (item.Key != "")
                    breadcrumbs.Add(item);
            }

            return breadcrumbs;
        }

    }
}
