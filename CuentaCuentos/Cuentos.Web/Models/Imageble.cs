using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Imageble
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual List<Image> Images { get; set; }

        public IEnumerable<Image> getImagesByTarget(String target)
        {
            try
            {
                return Images.Where(i => i.Target.ToLower() == target.ToLower());
            } catch (Exception ex) {
                return null;
            }
        }

        public string getDefaultImageUrlByTarget(string target)
        {
            var defaultImage = Images.Where(s => s.Target.ToLower() == target.ToLower()).FirstOrDefault();
            return defaultImage == null ? "" : defaultImage.ImagePath;
        }

        public string GetFirstImagePath(String target = "")
        {
            string result = Image.EmptyImage;
            if (Images.Count > 0)
            {
                if (string.IsNullOrEmpty(target))
                {
                    result = Images[0].ImagePath;
                }
                else
                {
                    var targetImg = Images.FirstOrDefault(s => s.Target.ToLower() == target.ToLower());
                    if (targetImg != null)
                        result = targetImg.ImagePath;
                }
            }
            return result;
        }

        public string GetImagePathByTargetOrDefault(string target)
        {
            string result = Image.EmptyImage;
            if (Images.Count > 0)
            {
                var img = Images.FirstOrDefault(s => s.Target.ToLower() == target.ToLower());
                if (img == null)
                    img = Images.FirstOrDefault(s => s.Target.ToLower() == "main");
                if (img == null)
                    img = Images.FirstOrDefault();

                result = img.ImagePath;
            }
            return result;
        }
    }
}