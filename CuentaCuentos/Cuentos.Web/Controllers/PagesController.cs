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

        public async Task<ContentResult> GetPages(int id)
        {
            var story = await Db.Stories.FindAsync(id);
            return (this.Content(story.Pages, "application/json"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SavePages(int id, string jsonStr)
        {
            var result = false;
            var story = await Db.Stories.FindAsync(id);

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
        public async Task<JsonResult> AddNewImage(HttpPostedFileBase postedFiles, string id)
        {
            if (postedFiles != null)
            {
                var user =  Db.Users.FindAsync(id);
                var userGallery =  Db.BuilderGalleries.Where(g => g.UserName == id).FirstOrDefaultAsync();
                Task.WaitAll(user,userGallery);
                var galery = userGallery.Result;
                if (user.Result != null)
                {


                    if (galery == null)
                    {
                        galery = new BuilderGallery
                        {
                            Name = id + "Gallery",
                            Description = "Gallery automatically created for " + id,
                            UserName = id,
                            Active = true
                        };

                        Db.BuilderGalleries.Add(galery);
                        Db.SaveChanges();
                    }


                    var image = new Image
                    {
                        Target = ImageTarget.USERGALLERY,
                        ImagebleId = userGallery.Id
                    };

                    galery.Images.Add(image);
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
        public async Task<ContentResult> MoveAndSaveImage(int id, string imgJsonStr)
        {

            dynamic img = null;
            var story = await Db.Stories.FindAsync(id);

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
        public async Task<ContentResult> DeleteImage(int id)
        {

            var result = false;
            var image = Db.Images.FindAsync(id);
            var userGallery = Db.BuilderGalleries.Where(bg => bg.UserName == LoggedUser.UserName).FirstOrDefaultAsync();

            Task.WaitAll(image, userGallery);
            if (image.Result.ImagebleId == userGallery.Result.Id)
            {
                Db.Images.Remove(image.Result);
                Db.SaveChanges();
                result = true;
            }

            return (this.Content(JsonConvert.SerializeObject(result), "application/json"));
        }

        [HttpGet]
        [ValidateInput(false)]
        public async Task<ContentResult> GetImages(bool onlyUserImages = false)
        {
            List<dynamic> images = new List<dynamic>();
            List<BuilderGallery> galleries = null;
            Task<List<BuilderGallery>> galleryTask = null;

            if (onlyUserImages)
            {
                galleryTask =  Db.BuilderGalleries.Include("Images")
                                    .Where(g => g.UserName == LoggedUser.UserName
                                     && g.Active == true).ToListAsync();
            }
            else
            {
                galleryTask =   Db.BuilderGalleries.Include("Images").Where(g => (g.Active == true && g.UserName == null)
                    || (g.UserName == LoggedUser.UserName && g.Active == true)).ToListAsync();
            }

            List<ImageCategory> allImageCategories = null;
            var categoriesTask = Db.ImageCategories.ToListAsync();

            Task.WaitAll(galleryTask, categoriesTask);
            allImageCategories = categoriesTask.Result;
            galleries = galleryTask.Result;

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
