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
    [Table("PageTypes")]
    public class PageType : Imageble
    {
        public PageType()
        {
            Images = new List<Image>();
        }
        
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }
        public int Position { get; set; }

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