using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class Role
    {
        public enum RoleType
        {
            superAdmin,
            schoolAdmin,
            student
        }

        [Key]
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Name is required")]
        [MaxLength(100)]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}