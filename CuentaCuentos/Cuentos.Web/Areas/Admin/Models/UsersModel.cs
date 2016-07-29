using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Areas.Admin.Models
{
    public class UsersModel
    {
        public string Name { get; set; }
        public string SchoolName { get; set; }
        public string UserName { get; set; }
        public int SchoolID { get; set; }
        public DateTime UserDateCreated { get; set; }
        public string IsApproved { get; set; }
        public string UserRole { get; set; }
    }
}