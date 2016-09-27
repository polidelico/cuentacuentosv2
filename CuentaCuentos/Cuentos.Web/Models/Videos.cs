using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Videos
    {
        public string Token;
        public Story[] Stories { get; set; }
    }
}
