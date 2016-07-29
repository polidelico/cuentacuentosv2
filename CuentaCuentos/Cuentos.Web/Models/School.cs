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
    [Table("Schools")]
    public class School : Imageble
    {
        public School()
        {
            Images = new List<Image>();
        }

        [Display(Name = "Nombre"), Required(ErrorMessage = "Nombre requerido")]
        public string Name { get; set; }

        [Display(Name = "Detalles"), Required(ErrorMessage = "Detalles requerido")]
        public string Details { get; set; }

        [Display(Name = "Dirección 1"), Required(ErrorMessage = "Direccion1 requerida")]
        public string Address1 { get; set; }

        [Display(Name = "Dirección 2")]
        public string Address2 { get; set; }

        [Display(Name = "Pueblo"), Required(ErrorMessage = "Pueblo requerido")]
        public int CityId { get; set; }

        [Display(Name = "Pueblo")]
        public virtual City City { get; set; }

        [Display(Name = "Código Postal"), Required(ErrorMessage = "Código Postal requerido"), RegularExpression(@"\d{5}", ErrorMessage = "Código Postal inválido")]
        public string Zip { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Size GetSectionItemImageDimensions(string target = "")
        {
            Size dimensions = new Size();

            if (target == "" || target == ImageTarget.MAIN)
            {
                dimensions.Width = 100;
                dimensions.Height = 100;
            }

            return dimensions;
        }

        public bool isAuthorized()
        {
            CuentosContext db = new CuentosContext();
            var result = false;
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = db.Users.Find(username);

            if (user.Roles.First().RoleName == "superAdmin")
            {
                result = true;
            }
            else
            {
                if (Id == user.SchoolId)
                {
                    result = true;
                }
            }

            return result;

        }

    }
}