using Cuentos.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using Cuentos.Lib.Enums;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Cuentos.Lib.Helpers;

namespace Cuentos.Models
{
    [Table("Stories")]
    public class Story : Imageble, ITimestamps
    {
        public Story()
        {
            Images = new List<Image>();
            CreatedAt = DateTime.UtcNow;
            Grades = new List<Grade>();
            Categories = new List<Category>();
        }

        public Story(bool initialize = false)
        {
            if (initialize)
            {
                Name = "Cuento Nuevo";
                Summary = "Descripcion del cuento";
                //IsApproved = false;
                Images = new List<Image>();
                CreatedAt = DateTime.UtcNow;
                Grades = new List<Grade>();
                Categories = new List<Category>();

                var pagesCollection = new List<Page>{
                    new Page{ Text = "", Type = "EmptyPage", ImageUrl="", Position = 0},
                    new Page{ Text = "<h2>Cambiar Titulo_1</h2>", Type = "ImageTopTextBottom", ImageUrl="http://placehold.it/390x280&text=cambiar%20imagen", Position = 1},
                    new Page{ Text = "<h2>Cambiar Texto_2</h2>", Type = "TextTopImageBottom", ImageUrl="http://placehold.it/390x280&text=cambiar%20imagen", Position = 2 },
                    new Page{ Text = "<h2>Cambiar Texto_3</h2>", Type = "BigImageTextOverlay", ImageUrl="http://placehold.it/390x558&text=cambiar%20imagen", Position = 3 },
                    new Page{ Text = "<h2>Cambiar Texto_4</h2>", Type = "ImageTopTextBottom", ImageUrl="http://placehold.it/390x280&text=cambiar%20imagen", Position = 4 },
                    new Page{ Text = "<h2>Cambiar Texto_5</h2>", Type = "BigImage", ImageUrl="http://placehold.it/390x558&text=cambiar%20imagen", Position = 5 }
                };

                Pages = JsonConvert.SerializeObject(pagesCollection);
                //Pages = new JavaScriptSerializer().Serialize(pagesCollection);

            }
            else
            {
                new Story();
            }
        }

        public virtual User User { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Nombre del cuento"), Required]
        public string Name { get; set; }

        //[Required]
        //public bool IsApproved { get; set; }

        public bool Featured { get; set; }

        //[Required]
        [Display(Name = "Resumen del cuento")]
        public string Summary { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        [Display(Name = "Categorías")]
        public virtual ICollection<Category> Categories { get; set; }

        [Display(Name = "Apto para")]
        public virtual ICollection<Grade> Grades { get; set; }

        [Display(Name = "Intereses")]
        public virtual ICollection<Interest> Interests { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public string Pages { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

        [Display(Name = "Estado")]
        public StatusStory Status { get; set; }

        public int Views { get; set; }


        public int GetAvgRating()
        {
            var sumOfAllRatings = 0;
            var result = 0;

            if (Ratings.Count > 0)
            {
                foreach (var rating in this.Ratings)
                {
                    sumOfAllRatings += rating.Rate;
                }

                result = sumOfAllRatings / this.Ratings.Count();
            }

            return result;
        }

        public Size GetSectionItemImageDimensions(string target = "")
        {
            Size dimensions = new Size();

            if (target == "" || target == ImageTarget.MAIN)
            {
                dimensions.Width = 200;
                dimensions.Height = 200;
            }

            return dimensions;
        }

        public bool isViewable(int id)
        {
            CuentosContext db = new CuentosContext();
            var result = false;
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = db.Users.Find(username);
            var story = db.Stories.Find(id);

            if (username != "")
            {

                if (story.Status != StatusStory.Published)
                {
                    if (user.Roles.First().RoleName == "superAdmin")
                    {
                        result = true;
                    }
                    else if (user.Roles.First().RoleName == "schoolAdmin")
                    {
                        //check if story.user.schoolId == user.schoolid
                        if (story.User.SchoolId == user.SchoolId)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (UserName == user.UserName)
                        {
                            result = true;
                        }
                    }

                }
                else
                {
                    result = true;
                }
            }
            else
            {
                if (story.Status == StatusStory.Published)
                {
                    result = true;
                }

            }

            return result;
        }



        public string CoverImage
        {
            get
            {
                var result = "";
                if (!string.IsNullOrEmpty(this.Pages))
                {
                    var PagesJson = JsonConvert.DeserializeObject<List<Page>>(this.Pages);
                    if (PagesJson.Count > 1 && PagesJson.ElementAt(1).ImageUrl != null)
                    {
                        result = PagesJson.ElementAt(1).ImageUrl;

                    }

                    if (string.IsNullOrEmpty(result))
                    {
                        result = "/Content/img/cuento-thumb.png";
                    }



                }

                return result;
            }
        }
    }

    public enum StatusStory
    {
        [Display(Name = "Borrador")]
        Draft = 0,

        [Display(Name = "Esperando Aprobación")]
        InApproval = 1,

        [Display(Name = "Publicado")]
        Published = 2,

        [Display(Name = "No Publicado")]
        UnPublished = 3,

        [Display(Name = "Eliminado")]
        Deleted = 4
    }


}