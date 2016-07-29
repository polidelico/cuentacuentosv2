using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Category : BaseModel
    {
        [Display(Name = "Nombre"), Required]
        public string Name { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }

        public virtual ICollection<Story> Stories { get; set; }
    }
}