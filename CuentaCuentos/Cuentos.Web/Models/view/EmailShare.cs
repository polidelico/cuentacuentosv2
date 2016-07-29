using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cuentos.Models.view
{
    public class EmailShare
    {
        [Required(ErrorMessage = "Requerido")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string To { get; set; }

        //[Required(ErrorMessage = "Requerido")]
        public string Description { get; set; }

        public Story Story { get; set; }

    }
}