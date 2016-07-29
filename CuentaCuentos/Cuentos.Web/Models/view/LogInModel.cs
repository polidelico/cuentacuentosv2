using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Apodo de usuario requerido")]
        [Display(Name = "Apodo de usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Contraseña requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar?")]
        public bool RememberMe { get; set; }
    }
}