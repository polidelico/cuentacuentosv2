using Cuentos.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    [Table("BuilderGalleries")]
    public class BuilderGallery : Imageble
    {
        public BuilderGallery()
        {
            Images = new List<Image>();
        }

        [Display(Name = "Nombre"), Required]
        public string Name { get; set; }

        [Display(Name = "Descripción"), Required]
        public string Description { get; set; }

        public virtual User User { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Activo"), Required]
        public bool Active { get; set; }

        public Size GetSectionItemImageDimensions(string target = "")
        {
            Size dimensions = new Size();

            if (target == "" || target == ImageTarget.MAIN)
            {
                dimensions.Width = 100;
                dimensions.Height = 100;
            }

            return dimensions;
        }

    }
}