using CodeFirstAltairis.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Contraseña actual requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nueva contraseña requerida")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma la nueva contraseña")]
        [System.Web.Mvc.CompareAttribute("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}