using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class ContactUsModel
    {
        public User User { get; set; }

        [Display(Name = "NOMBRE")]
        [Required(ErrorMessage = "Nombre requerido")]
        public string Name { get; set; }

        [Display(Name = "CORREO ELECTRÓNICO")]
        [Required(ErrorMessage = "Correo Electrónico requerido")]
        public string Email { get; set; }


        [Display(Name = "TELÉFONO")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Teléfono inválido")]
        [Required(ErrorMessage = "Teléfono requerido")]
        public string Phone { get; set; }

        [Display(Name = "ESCUELA")]
        [Required(ErrorMessage = "Escuela requerida")]
        public int SchoolId { get; set; }

        //[Required]
        [Display(Name = "ASUNTO O TEMA")]
        [Required(ErrorMessage = "Asunto o tema requerido")]
        public string Subject { get; set; }

        //[Required]
        [Display(Name = "PREGUNTA O COMENTARIO")]
        [Required(ErrorMessage = "Pregunta o comentario requerido")]
        public string Comments { get; set; }

    }
}