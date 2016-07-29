using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Areas.Admin.Models
{
    public class commentsModel
    {
        public string StoryName { get; set; }
        public string SchoolName { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CommentID { get; set; }
        public int StoryID { get; set; }
    }
}