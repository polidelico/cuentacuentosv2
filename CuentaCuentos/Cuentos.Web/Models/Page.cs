using Cuentos.Lib;
using Cuentos.Lib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Page
    {
        public string Type { get; set; }

        public string Text { get; set; }

        public int ImageId { get; set; }

        public string ImageUrl { get; set; }

        public int Position { get; set; }

    }
}