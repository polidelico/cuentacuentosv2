using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Areas.Admin.Models
{
    public class StoriesModel
    {
        public string StoryName { get; set; }
        public string Author { get; set; }
        public string Score { get; set; }
        public string School { get; set; }
        public DateTime Created { get; set; }
        public string isAproved { get; set; }
        public string Status { get; set; }
        public int StoryID { get; set; }
    }
}