using Cuentos.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class User
    {
        public class UsersModel
        {
            public string UserName { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string isAproved { get; set; }
            public string Email { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public User()
        {
            ImageHolders = new Imageble
            {
                Images = new List<Image>()
            };
        }

        public void UpdateUserFields(User user)
        {
            Name = user.Name;
            LastName = user.LastName;
            Age = user.Age;
            Grade = user.Grade;
            SchoolId = user.SchoolId;
            School = user.School;
            Email = user.Email;
            IsApproved = user.IsApproved;
            ImageHolders = ImageHolders;
            Owner = user.Owner;
        }

        
        [Key]
        [Required(ErrorMessage = "Apodo de usuario requerido")]
        [Display(Name = "Apodo de usuario")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Edad")]
        public int? Age { get; set; }

        [Display(Name = "¿Quien eres?")]
        public string Owner { get; set; }

        [Display(Name = "Grado: ")]
        public Grade Grade { get; set; }

        [Display(Name = "Escuela")]
        public int? SchoolId { get; set; }

        public virtual School School { get; set; }

        public virtual IEnumerable<Story> Stories { get; set; }


        [MaxLength(64)]
        public byte[] PasswordHash { get; set; }

        [MaxLength(128)]
        public byte[] PasswordSalt { get; set; }

        [Required(ErrorMessage = "Correo electrónico requerido")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [Display(Name = "Aprobado")]
        public bool IsApproved { get; set; }

        [Display(Name = "Create Date")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? DateLastLogin { get; set; }

        [Display(Name = "Last Activity Date")]
        public DateTime? DateLastActivity { get; set; }

        [Display(Name = "Last Password Change Date")]
        public DateTime DateLastPasswordChange { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string ApprovedBy { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public bool Featured { get; set; }

        public Imageble ImageHolders { get; set; }

        public string Fullname
        {
            get{
                return Name + " " + LastName;
            }
        }

        public string AvatarImage
        {
            get
            {
                var result = "";
                if (!string.IsNullOrEmpty(this.ImageHolders.GetFirstImagePath(ImageTarget.MAIN)))
                {
                    var image = this.ImageHolders.GetFirstImagePath(ImageTarget.MAIN);
                    if (image != "/Content/img/cuento-thumb.png")
                    {
                        result = this.ImageHolders.GetFirstImagePath(ImageTarget.MAIN);
                    }
                    else
                    {
                        result = "/Content/img/user-thumb.jpg";
                    }


                }
                else
                {
                    result = "/Content/img/user-thumb.jpg";
                }

                return result;
            }


        }

        public Size GetImageDimensions(string target = "")
        {
            Size dimensions = new Size();

            if (target == "" || target == ImageTarget.MAIN)
            {
                dimensions.Width = 100;
                dimensions.Height = 100;
            }

            return dimensions;
        }

        public bool isAuthorized(string userToValidate)
        {
            CuentosContext db = new CuentosContext();
            var result = false;
            var username = System.Web.HttpContext.Current.User.Identity.Name;
            var user = db.Users.Find(username);
            var otherUser = db.Users.Find(userToValidate);

            if (otherUser.SchoolId == user.SchoolId)
            {
                result = true;
            }
            return result;

        }

        public Size GetSectionItemImageDimensions(string target = "")
        {
            Size dimensions = new Size();

            if (target == "" || target == ImageTarget.MAIN)
            {
                dimensions.Width = 200;
                dimensions.Height = 200;
            }

            return dimensions;
        }

        public enum OwnerType
        {
            [Display(Name = "Estudiante")]
            Student,
            [Display(Name = "Padre")]
            Parent,
            [Display(Name = "Maestro")]
            Teacher
        }

    }
}