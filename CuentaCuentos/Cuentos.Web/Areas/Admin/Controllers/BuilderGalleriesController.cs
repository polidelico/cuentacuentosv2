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
    public class BuilderGalleriesController : AdminGlobalController
    {

        public ActionResult Index()
        {
            var galleries = Db.BuilderGalleries.ToList();
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            return View(galleries);
        }

        public ActionResult Create()
        {
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "BuilderGalleries"), "Crear"));
            return View();
        }

        [HttpPost]
        public ActionResult Create(BuilderGallery model)
        {
            if (ModelState.IsValid)
            {
                Db.BuilderGalleries.Add(model);
                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.CreateSuccess);
            }

            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "BuilderGalleries"), "Crear"));
            return View(model).Error(SaveMessage.Error);
        }

        public ActionResult Edit(int id)
        {

            var gallery = Db.BuilderGalleries.Include("Images").First(s => s.Id == id);
            var categories = Db.ImageCategories.ToList();

            ViewBag.ImageCategoriesSelect = new SelectList(categories, "Id", "Name");
            ViewBag.ImageCategories = categories;
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "BuilderGalleries", new { id = gallery.Id }), gallery.Name), gallery);
            return View(gallery);
        }

        [HttpPost]
        public ActionResult Edit(BuilderGallery model)
        {
            if (ModelState.IsValid)
            {
                Db.SaveChanges();
                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return RedirectToAction("Edit", new { id = model.Id }).Error(SaveMessage.Error);
        }

        [HttpPost]
        public ActionResult SaveImage(int galleryId, string target, HttpPostedFileBase galleryImageFile)
        {
            var gallery = Db.BuilderGalleries.Include("Images").SingleOrDefault(g => g.Id == galleryId);

            if (galleryImageFile != null)
            {

                var image = new Image
                {
                    Target = target,
                    ImagebleId = gallery.Id
                };

                gallery.Images.Add(image);
                Db.SaveChanges();

                UploadImage(galleryImageFile, image, false);
                Db.SaveChanges();


            }

            return RedirectToAction("Edit", new { id = gallery.Id }).Success(SaveMessage.CreateSuccess);
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var gallery = Db.BuilderGalleries.Find(id);
                Db.BuilderGalleries.Remove(gallery);
                Db.SaveChanges();

                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpDelete]
        public HttpResponseMessage DeleteImage(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var image = Db.Images.Find(id);
                Db.Images.Remove(image);
                Db.SaveChanges();

                RemoveImage(image.ImagebleId.ToString(), image.Filename);
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, BuilderGallery model = null)
        {
            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                currentItem
            };
            return Breadcrumbs(controllerBreadcrumbs, model);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(List<KeyValuePair<String, String>> items, BuilderGallery model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();


            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(IsSuperAdmin ? @Url.Action("Index", "BuilderGalleries") : @Url.Action("Edit", "BuilderGalleries"),"Galerias"),
            };


            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "BuilderGalleries", new { id = model.Id }), model.Name);
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
