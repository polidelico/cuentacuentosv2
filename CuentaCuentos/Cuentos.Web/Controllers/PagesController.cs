using Cuentos.Lib;
using Cuentos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Cuentos.Controllers
{
    [Authorize]
    public class PagesController : ApplicationGlobalController
    {
        public static string StorageRootPath = "~/Content/dynamic";
        public static string StoriesPath = "stories";

        public ContentResult GetPages(int id)
        {
            var story = Db.Stories.Find(id);
            return (this.Content(story.Pages, "application/json"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SavePages(int id, string jsonStr)
        {
            var result = false;
            var story = Db.Stories.Find(id);

            if (story != null && !String.IsNullOrEmpty(jsonStr))
            {
                story.Pages = jsonStr;
                story.Status = StatusStory.UnPublished;
                Db.SaveChanges();
                result = true;
            }

            return (this.Json(result, "application/json"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddNewImage(HttpPostedFileBase postedFiles, string id)
        {
            if (postedFiles != null)
            {
                if (Db.Users.Find(id) != null)
                {

                    var userGallery = Db.BuilderGalleries.Where(g => g.UserName == id).FirstOrDefault();

                    if (userGallery == null)
                    {
                        userGallery = new BuilderGallery
                        {
                            Name = id + "Gallery",
                            Description = "Gallery automatically created for " + id,
                            UserName = id,
                            Active = true
                        };

                        Db.BuilderGalleries.Add(userGallery);
                        Db.SaveChanges();
                    }


                    var image = new Image
                    {
                        Target = ImageTarget.USERGALLERY,
                        ImagebleId = userGallery.Id
                    };

                    userGallery.Images.Add(image);
                    Db.SaveChanges();

                    UploadImage(postedFiles, image, false);
                    Db.SaveChanges();

                }

            }


            //return Json(true);
            return Json(true, "text/html", System.Text.Encoding.UTF8, JsonRequestBehavior.AllowGet);
            //return (this.Content("", "application/json"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ContentResult MoveAndSaveImage(int id, string imgJsonStr)
        {

            dynamic img = null;
            var story = Db.Stories.Find(id);

            if (!String.IsNullOrEmpty(imgJsonStr))
            {
                var storageRoot = Server.MapPath(StorageRootPath);
                img = JsonConvert.DeserializeObject(imgJsonStr);
                string t = img.imagebleId.ToString();
                var currentFilePath = Path.Combine(storageRoot, img.id.ToString(), (string)img.filename);
                var newFilePath = Path.Combine(storageRoot, StoriesPath, id.ToString(), (string)img.filename);
                var file = new System.IO.FileInfo(newFilePath);

                file.Directory.Create();
                System.IO.File.Copy(currentFilePath, newFilePath, true);
                img.ImagePath = "/Content/dynamic/stories/" + id.ToString() + "/" + img.filename;
            }

            return (this.Content(JsonConvert.SerializeObject(img), "application/json"));
        }

        [HttpDelete]
        [ValidateInput(false)]
        public ContentResult DeleteImage(int id)
        {

            var result = false;
            var image = Db.Images.Find(id);
            var userGallery = Db.BuilderGalleries.Where(bg => bg.UserName == LoggedUser.UserName).FirstOrDefault();

            if (image.ImagebleId == userGallery.Id)
            {
                Db.Images.Remove(image);
                Db.SaveChanges();
                result = true;
            }

            return (this.Content(JsonConvert.SerializeObject(result), "application/json"));
        }

        [HttpGet]
        [ValidateInput(false)]
        public ContentResult GetImages(bool onlyUserImages = false)
        {
            List<dynamic> images = new List<dynamic>();
            List<BuilderGallery> galleries = null;

            if (onlyUserImages)
            {
                galleries = Db.BuilderGalleries.Include("Images").Where(g => g.UserName == LoggedUser.UserName && g.Active == true).ToList();
            }
            else
            {
                galleries = Db.BuilderGalleries.Include("Images").Where(g => (g.Active == true && g.UserName == null)
                    || (g.UserName == LoggedUser.UserName && g.Active == true)).ToList();
            }

            List<ImageCategory> allImageCategories = Db.ImageCategories.ToList(); ;

            foreach (BuilderGallery gallery in galleries)
            {
                var isUserGallery = string.IsNullOrEmpty(gallery.UserName) ? false : true;

                foreach (Image img in gallery.Images)
                {
                    images.Add(new
                    {
                        id = img.Id,
                        imagebleId = img.ImagebleId,
                        filename = img.Filename,
                        category = img.Target,
                        imagePath = img.ImagePath,
                        belongToUser = isUserGallery
                    });
                }
            }

            return (this.Content(JsonConvert.SerializeObject(images), "application/json"));
        }
    }
}
