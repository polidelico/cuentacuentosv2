using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Image : BaseModel
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public int Size { get; set; }
        public Size Dimensions { get; set; }
        public int ImagebleId { get; set; }
        
        [Display(Name = "Galería")]
        public string Target { get; set; }

        public Imageble Imageble { get; set; }

        public string ImagePath
        {
            get
            {
                if (String.IsNullOrEmpty(Filename))
                    return EmptyImage;

                return "/Content/dynamic/" + Id + "/" + Filename;
            }
        }

        public static string EmptyImage
        {
            get
            {
                return "/Content/img/cuento-thumb.png";
            }
        }

        public Image() { }

        public Image(string filename)
        {
            Filename = filename;
        }

        internal void Add(Image image)
        {
            throw new NotImplementedException();
        }
    }
}