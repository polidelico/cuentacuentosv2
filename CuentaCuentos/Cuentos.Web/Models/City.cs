using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    [Table("Cities")]
    public class City : BaseModel
    {
        public string Name { get; set; }

    }
}