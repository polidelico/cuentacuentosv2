using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class ApprovalsModel
    {
        public IEnumerable<Story> Stories { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public virtual User User { get; set; }

        public virtual Story Story { get; set; }
    }
}