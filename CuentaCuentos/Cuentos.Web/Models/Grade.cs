using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Grade : BaseModel
    {
        public string Name { get; set; }
        public string Name_EN { get; set; }
        public int Position { get; set; }

        public virtual ICollection<Story> Stories { get; set; }
    }
}