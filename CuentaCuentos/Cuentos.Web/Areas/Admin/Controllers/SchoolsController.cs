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
using System.IO;
using PagedList;
using System.Text;

namespace Cuentos.Areas.Admin.Controllers
{
    public class SchoolsController : AdminGlobalController
    {

        public ActionResult Index()
        {
            var schools = Db.Schools.ToList();
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));

            return View(schools);
        }

        [Authorize(Roles = "superAdmin")]
        public ActionResult Create()
        {
            var school = new School();
            var schools = Db.Schools.ToList().OrderBy(s => s.Name);

            InitializeModelImages(school);


            ViewBag.Cities = GetDDLOptions("cities");
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "Schools"), "Crear"));

            return View(school);
        }

        [HttpPost]
        public ActionResult Create(School model, HttpPostedFileBase mainImage)
        {

            if (ModelState.IsValid)
            {
                Db.Schools.Add(model);
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

            ViewBag.Cities = GetDDLOptions("cities");
            ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "Schools"), "Crear"));
            return View(model).Error(SaveMessage.Error);
        }

        public ActionResult Edit(int id)
        {

            var school = Db.Schools.Include("images").First(s => s.Id == id);

            if (school != null && school.isAuthorized())
            {
                InitializeModelImages(school);
                ViewBag.Cities = GetDDLOptions("cities");
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Schools", new { id = school.Id }), school.Name), school);

                return View(school);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(School model, HttpPostedFileBase mainImage)
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
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var school = Db.Schools.Find(id);
                Db.Schools.Remove(school);
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        public ActionResult Users(int id, int? page)
        {
            //IsAuthorizedRedirect(id);
            ViewBag.SchoolId = id;
            var school = Db.Schools.Find(id);

            if (school != null && school.isAuthorized())
            {
                var users = Db.Users.Where(u => u.SchoolId == id).ToList();
                var controllerBreadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(@Url.Action("Edit", "Schools", new {id = school.Id}), school.Name),
                    new KeyValuePair<String, String>(@Url.Action("Users", "Schools", new {id = school.Id}), "Usuarios")
                };
                ViewBag.breadcrumbs = Breadcrumbs(controllerBreadcrumbs, school);

                int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
                //return View(users);
            }

            return RedirectToAction("Index", "Home");
        }

        public void ExportUsersListToCSV(int id)
        {

            StringWriter sw = new StringWriter();

            sw.WriteLine("\"UserName\",\"Name\",\"Last Name\",\"Age\",\"School\",\"Grade\",\"Is Approved?\",\"DateCreated\"");

            Response.ClearContent();
            var school = Db.Schools.Find(id);
            var FileName = Guid.NewGuid().ToString();
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".csv");
            Response.ContentType = "text/csv";
            Response.ContentEncoding = Encoding.Unicode;

            
            if (school != null && school.isAuthorized())
            {
                var users = Db.Users.Where(u => u.SchoolId == id).ToList();
                foreach (var line in users)
                {
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"",
                                               line.UserName,
                                               line.Name,
                                               line.LastName,
                                               line.Age,
                                               line.School.Name,
                                               line.GradeId,
                                               line.IsApproved.ToString(),
                                               line.DateCreated.ToShortDateString()));
                }

                Response.Write(sw.ToString());

                Response.End();

            }

        }

        public ActionResult Stories(int id)
        {
            IsAuthorizedRedirect(id);

            var user = LoggedUser;
            var users = Db.Users.Where(u => u.SchoolId == id).Select(u => u.UserName);
            var stories = Db.Stories.Where(s => users.Contains(s.UserName)).ToList();
            var school = Db.Schools.Find(id);

            if (school != null && school.isAuthorized())
            {
                var controllerBreadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(@Url.Action("Edit", "Schools", new {id = school.Id}), school.Name),
                    new KeyValuePair<String, String>(@Url.Action("Stories", "Schools", new {id = school.Id}), "Cuentos")
                };

                ViewBag.breadcrumbs = Breadcrumbs(controllerBreadcrumbs, school);

                return View(stories);
            }

            return RedirectToAction("Index", "Home");
        }

        //public ActionResult Comments(int id)
        //{
        //    var comments = Db.Comments.Include("User").Include("Story.User.School").Where(c => c.User.SchoolId == id).ToList();
        //    var school = Db.Schools.Find(id);

        //    if (school != null && school.isAuthorized())
        //    {

        //        var controllerBreadcrumbs = new List<KeyValuePair<String, String>>
        //        {
        //            new KeyValuePair<String, String>(@Url.Action("Edit", "Schools", new {id = school.Id}), school.Name),
        //            new KeyValuePair<String, String>(@Url.Action("Comments", "Schools", new {id = school.Id}), "Comentarios")
        //        };

        //        ViewBag.breadcrumbs = Breadcrumbs(controllerBreadcrumbs, school);

        //        return View(comments);
        //    }

        //    return RedirectToAction("Index", "Home");


        //}

        public void InitializeModelImages(School school)
        {
            var mainImage = school.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

            if (mainImage == null)
            {
                mainImage = new Image { Target = ImageTarget.MAIN.ToString() };
                school.Images.Add(mainImage);
            }

            mainImage.Dimensions = school.GetSectionItemImageDimensions(ImageTarget.MAIN);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, School model = null)
        {
            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                currentItem
            };
            return Breadcrumbs(controllerBreadcrumbs, model);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(List<KeyValuePair<String, String>> items, School model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            
            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(IsSuperAdmin ? @Url.Action("Index", "Schools") : @Url.Action("Index", "Home"), "Escuelas"),
            };


            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Schools", new { id = model.Id }), model.Name);
            }

            foreach (KeyValuePair<string, string> item in items)
            {
                if (item.Key != "")
                    breadcrumbs.Add(item);
            }

            return breadcrumbs;
        }

        public List<SelectListItem> GetDDLOptions(string type, string selected = "")
        {
            List<SelectListItem> result = null;
            //var defaultText = Resources.Tools.Contact.Default_DDL_Value.ToString();
            //bool isEnglish = CultureHelper.IsEnglish();

            switch (type)
            {
                case "cities":
                    {
                        List<City> options = null;

                        options = Db.Cities.OrderBy(c => c.Name).ToList();
                        result = new List<SelectListItem>();
                        result.Add(new SelectListItem { Text = "Selecciona un pueblo", Value = "" });
                        foreach (var city in options)
                        {
                            result.Add(new SelectListItem
                            {
                                Text = city.Name,
                                Value = city.Id.ToString()
                            });
                        }
                        break;
                    }
            }

            return result;
        }

    }
}
