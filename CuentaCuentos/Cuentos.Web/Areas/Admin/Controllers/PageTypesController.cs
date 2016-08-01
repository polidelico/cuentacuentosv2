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
using System.Data.Entity;
using System.Threading.Tasks;

namespace Cuentos.Areas.Admin.Controllers
{
    public class PageTypesController : AdminGlobalController
    {

        public async Task<ActionResult> Index()
        {
            var pageTypes = await Db.PageTypes.OrderBy(t => t.Position).ToListAsync();
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            return View(pageTypes);
        }

        [Authorize(Roles = "superAdmin")]
        public ActionResult Create()
        {
            var pageType = new PageType();

            InitializeModelImages(pageType);
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "PageTypes"), "Crear"));

            return View(pageType);
        }

        [HttpPost]
        public ActionResult Create(PageType model, HttpPostedFileBase mainImage)
        {

            if (ModelState.IsValid)
            {
                Db.PageTypes.Add(model);
                Db.SaveChanges();

                if (mainImage != null)
                {
                    var image = new Image
                    {
                        Target = "main",
                    };
                    model.Images.Add(image);
                    Db.SaveChanges();

                    UploadImage(mainImage, image);
                    Db.SaveChanges();
                }

                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.CreateSuccess);
            }

            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "PageTypes"), "Crear"));
            return View(model).Error(SaveMessage.Error);
        }

        public async Task<ActionResult> Edit(int id)
        {

            var pageType = await Db.PageTypes.Include("images").FirstAsync(s => s.Id == id);

            InitializeModelImages(pageType);
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "PageTypes", new { id = pageType.Id }), pageType.Name), pageType);

            return View(pageType);
        }

        [HttpPost]
        public ActionResult Edit(PageType model, HttpPostedFileBase mainImage)
        {
            if (ModelState.IsValid)
            {
                if (mainImage != null)
                {
                    var image = model.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

                    if (image == null)
                    {
                        image = new Image
                        {
                            Target = ImageTarget.MAIN,
                        };
                        model.Images.Add(image);
                        Db.SaveChanges();
                    }

                    UploadImage(mainImage, image);
                }

                Db.SaveChanges();

                return RedirectToAction("Edit", new { id = model.Id }).Success(SaveMessage.ChangesSuccess);
            }

            return RedirectToAction("Edit", new { id = model.Id }).Error(SaveMessage.Error);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var pageType = await Db.PageTypes.FindAsync(id);
                Db.PageTypes.Remove(pageType);
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        public void InitializeModelImages(PageType pageType)
        {
            var mainImage = pageType.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

            if (mainImage == null)
            {
                mainImage = new Image { Target = ImageTarget.MAIN.ToString() };
                pageType.Images.Add(mainImage);
            }

            mainImage.Dimensions = pageType.GetSectionItemImageDimensions(ImageTarget.MAIN);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, PageType model = null)
        {
            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                currentItem
            };
            return Breadcrumbs(controllerBreadcrumbs, model);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(List<KeyValuePair<String, String>> items, PageType model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();


            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(IsSuperAdmin ? @Url.Action("Index", "PageTypes") : @Url.Action("Index", "Home"), "Templates"),
            };


            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "PageTypes", new { id = model.Id }), model.Name);
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
