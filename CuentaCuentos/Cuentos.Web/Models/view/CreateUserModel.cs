using CodeFirstAltairis.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cuentos.Models.view
{
    public class CreateUserModel
    {
        public User User { get; set; }

        [Required]
        [Display(Name = "Rol")]

        public string Role { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-entrar Contraseña")]
        [System.Web.Mvc.CompareAttribute("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool TermsConditions { get; set; }

        [Display(Name = "Intereses")]
        public List<Interest> Interests { get; set; }
    }
}