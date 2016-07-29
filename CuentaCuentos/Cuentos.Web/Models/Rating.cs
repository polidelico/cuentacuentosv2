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
    [Table("Ratings")]
    public class Rating : BaseModel
    {
        public virtual Story Story { get; set; }

        public int StoryId { get; set; }

        public virtual User User { get; set; }

        public string UserName { get; set; }

        [Required]
        public int Rate { get; set; }


        public IEnumerable<Rating> Ratings { get; set; }

    }
}