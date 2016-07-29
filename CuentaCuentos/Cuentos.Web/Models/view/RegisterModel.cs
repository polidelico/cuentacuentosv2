using CodeFirstAltairis.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Lib.Validations;

namespace Cuentos.Models.view
{
    public class RegisterModel
    {
        public User User { get; set; }

        [Required(ErrorMessage="Grado requrido")]
        [Display(Name = "Grado")]
        public int? GradeId { get; set; }

        [Required(ErrorMessage="Escuela requerida")]
        [Display(Name = "Escuela")]
        public int? SchoolId { get; set; }

        [Required(ErrorMessage = "Contraseña requrida")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-entrar Contraseña")]
        [System.Web.Mvc.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Terminos y Condiciones")]
        [CustomDataAnnotation.MustBeTrue(ErrorMessage = "Necesitas aceptar")]
        public bool TermsConditions { get; set; }

    }
}