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
     [Table("Contacts")]
    public class Contact : BaseModel
    {
        public string From { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
      
        public string Phone { get; set; }

        public int? SchoolId { get; set; }

        public string Subject { get; set; }

        public string Comments { get; set; }

        public bool isRead { get; set; }

        public virtual School School { get; set; }

        //public IEnumerable<Contact> Contacts { get; set; }

    }
}
