using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class FeaturedModel
    {
        public IEnumerable<Story> Stories { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}