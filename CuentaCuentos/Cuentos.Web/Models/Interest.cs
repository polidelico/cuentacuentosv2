using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    [Table("Interests")]
    public class Interest : BaseModel
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Story> Stories { get; set; }

    }
}