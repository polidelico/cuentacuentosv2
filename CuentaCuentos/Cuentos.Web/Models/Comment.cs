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
    [Table("Comments")]
    public class Comment : BaseModel
    {
        public virtual Story Story { get; set; }

        public int StoryId { get; set; }

        public virtual User User { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Texto"), Required]
        public string Text { get; set; }

        [Display(Name = "Aprobado"), Required]
        public bool IsApproved { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

    }
}