using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    [Table("ImageCategories")]
    public class ImageCategory : BaseModel
    {
        [Display(Name = "Nombre"), Required]
        public string Name { get; set; }
    }
}