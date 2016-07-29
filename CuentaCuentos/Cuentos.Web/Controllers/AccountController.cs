using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CodeFirstAltairis.Models;
using Cuentos.Controllers;
using Cuentos.Models;
using Cuentos.Models.view;
using Cuentos.Lib.Extensions;
using Postal;
using System.Configuration;
using Cuentos.Areas.Admin.Lib;
using System.Globalization;
using System.Threading;
using Cuentos.Lib;
using Mandrill;
using Cuentos.Lib.Helpers;
using Mandrill.Models;

namespace CodeFirstAltairis.Controllers
{
    [Authorize]
    public class AccountController : ApplicationGlobalController
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {

            //CultureInfo es = new CultureInfo("es-PR");
            //Thread.CurrentThread.CurrentCulture = es;

            var model = Db.Users.Include("ImageHolders").Where(u => u.UserName == User.Identity.Name).First();
            var schools = Db.Schools.ToList().OrderBy(s => s.Name);
            var grades = Db.Grades.ToList().OrderBy(g => g.Position);



            var user = LoggedUser;
            var StoriesNotApproved = Db.Stories.Where(s => s.UserName == user.UserName && (s.Status == StatusStory.Draft || s.Status == StatusStory.InApproval || s.Status == StatusStory.UnPublished)).ToList();

            var storiesApproved = Db.Stories.Where(s => s.UserName == user.UserName && (s.Status == StatusStory.Published)).ToList();

            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            ViewBag.Interests = Db.Interests.ToList();
            ViewBag.StoriesNotApproved = StoriesNotApproved;
            ViewBag.StoriesApproved = storiesApproved;
            InitializeModelImages(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(User model, HttpPostedFileBase mainImage, string[] selectedInterests)
        {
            if (ModelState.IsValid)
            {
                var user = Db.Users.Include("ImageHolders").Where(u => u.UserName == model.UserName).First();
                model.ImageHolders = user.ImageHolders;

                model.IsApproved = true;
                if (AccountController.UpdateUser(model, null, selectedInterests))
                {
                    if (mainImage != null)
                    {
                        var image = model.ImageHolders.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

                        if (image == null)
                        {
                            image = new Cuentos.Models.Image
                            {
                                Target = ImageTarget.MAIN,
                                ImagebleId = user.ImageHolders.Id
                            };
                            model.ImageHolders.Images.Add(image);
                            Db.SaveChanges();
                        }

                        UploadImage(mainImage, image);
                        Db.SaveChanges();
                    }

                    var url = Url.RouteUrl(new { controller = "Account", action = "Index" });

                    return Redirect(url + "#ajustes").Success("Los cambios fueron guardados satisfactoriamente.");
                }
            }

            return RedirectToAction("Index").Error("Hubo un error, intente mas tarde.");
        }

        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nombre de usuario o contraseña incorrecta.");
                }
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PasswordLength = MembershipService.MinPasswordLength;
                setRegisterModel();

                return View();
            }
        }

        private void setRegisterModel()
        {
            var schools = Db.Schools.ToList().OrderBy(s => s.Name);
            var grades = Db.Grades.ToList().OrderBy(g => g.Position);
            var ownerTypesSelectList = new List<SelectListItem>();

            foreach (var ownerType in Enum.GetValues(typeof(User.OwnerType)).Cast<User.OwnerType>())
            {
                ownerTypesSelectList.Add(new SelectListItem
                {
                    Text = EnumHelper<User.OwnerType>.GetDisplayValue(ownerType),
                    Value = ownerType.ToString()
                });
            }

            ViewBag.OwnerTypesSelectList = ownerTypesSelectList;
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            ViewBag.Interests = Db.Interests.ToList();


            //RegisterModel model = new RegisterModel();
            //model.DDLSchool = GetDDLOptions("schools");
            //model.DDLGrade = GetDDLOptions("grades");
            //model.User.Interests = Db.Interests.ToList();

            //return model;
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Register(RegisterModel model, string[] selectedInterests)
        {
            if (ModelState.IsValid)
            {
                model.User.SchoolId = model.SchoolId;
                model.User.GradeId = model.GradeId;

                var createStatus = RegisterUser(model.User, model.Password, Role.RoleType.student, selectedInterests);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                    var email = new EmailMessage();
                    var recipients = new List<EmailAddress>();
                    var admins = Db.Users.Include("Roles").Where(u => u.SchoolId == model.SchoolId);

                    foreach (var admin in admins)
                    {
                        if (admin.Roles != null && admin.Roles.First().RoleName == Role.RoleType.schoolAdmin.ToString())
                        {
                            recipients.Add(new EmailAddress(admin.Email));
                            email.AddRecipientVariable(admin.Email, "NAME", admin.Name);
                        }

                    }

                    if (recipients.Count > 0)
                    {
                        email.To = recipients;
                        email.Subject = "Nuevo usuario";
                        email.AddGlobalVariable("TITLE", "Un nuevo usuario se ha registrado.");
                        email.AddGlobalVariable("CONTENT", "Para que el nuevo usuario pueda tener acceso a todas las funcionalidades de Cuenta Cuentos es necesario que sea aprovado.");
                        email.AddGlobalVariable("CALLTOACTION", "Apruebe el usuario <a href=\"" + Url.Action("Index", "Approvals", new { area = "admin" }, Request.Url.Scheme) + "\"> aqui </a>");
                        mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));
                    }

                    return RedirectToAction("Index", "Home").Success("Su cuenta ha sido creada satisfactoriamente, la misma esta en espera de aprobación.");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    setRegisterModel();
                }
            }
            else
            {
                setRegisterModel();
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        public static MembershipCreateStatus RegisterUser(User model, string password, Role.RoleType roleType, string[] selectedInterests = null)
        {
            IMembershipService MembershipService = new AccountMembershipService();
            CuentosContext Db = new CuentosContext();

            var createStatus = MembershipService.CreateUser(model.UserName, password, model.Email);

            if (createStatus == MembershipCreateStatus.Success)
            {
                var user = Db.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
                var role = Db.Roles.Find(roleType.ToString());

                user.Name = model.Name;
                user.LastName = model.LastName;
                user.SchoolId = model.SchoolId;
                user.GradeId = model.GradeId;
                user.Age = model.Age;
                user.Email = model.Email;
                user.Roles.Add(role);
                user.IsApproved = model.IsApproved;
                user.Owner = model.Owner;

                if (selectedInterests != null)
                {
                    user.Interests.Clear();

                    foreach (var selectedInterest in selectedInterests)
                    {
                        int interestId = Convert.ToInt32(selectedInterest);
                        var interest = Db.Interests.Where(i => i.Id == interestId).First();
                        user.Interests.Add(interest);
                    }
                }

                Db.SaveChanges();
            }

            return createStatus;
        }

        public static bool UpdateUser(User model, string password = null, string[] selectedInterests = null)
        {
            var result = false;
            try
            {
                IMembershipService MembershipService = new AccountMembershipService();
                CuentosContext Db = new CuentosContext();
                var user = Db.Users.Include("ImageHolders").Where(u => u.UserName == model.UserName).First();

                user.UpdateUserFields(model);

                if (model.Roles != null)
                {
                    string roleName = model.Roles.First().RoleName;
                    var role = Db.Roles.Find(roleName);
                    user.Roles.Clear();
                    user.Roles.Add(role);
                }

                if (!string.IsNullOrEmpty(password))
                {
                    string generatedPW = MembershipService.ResetPassword(user.UserName);
                    MembershipService.ChangePassword(user.UserName, generatedPW, password);
                }

                user.Interests.Clear();
                if (selectedInterests != null)
                {
                    foreach (var selectedInterest in selectedInterests)
                    {
                        int interestId = Convert.ToInt32(selectedInterest);
                        var interest = Db.Interests.Where(i => i.Id == interestId).First();
                        user.Interests.Add(interest);
                    }
                }

                Db.SaveChanges();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    var url = Url.RouteUrl(new { controller = "Account", action = "Index" });

                    return Redirect(url + "#ajustes").Success("Los cambios fueron guardados satisfactoriamente.");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult RecoverPassword(User model)
        {
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var user = Db.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
                string newPassword = "";

                if (user != null)
                {
                    newPassword = MembershipService.ResetPassword(user.UserName);

                    MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                    var recipients = new List<EmailAddress> { new EmailAddress(user.Email) };
                    var email = new EmailMessage
                    {
                        To = recipients,
                    };

                    email.Subject = "Nueva contraseña";
                    email.AddGlobalVariable("NAME", user.Name);
                    email.AddGlobalVariable("TITLE", "Hemos creado una nueva contraseña.");
                    email.AddGlobalVariable("CONTENT", "Puede volver a tener acceso a tu cuenta con la sigiguiente contraseña: <strong>" + newPassword + "</strong>.");
                    email.AddGlobalVariable("CALLTOACTION", "Accesede tu cuenta <a href=\"" + Url.Action("Login", "Account", Request.Url.Scheme) + "\"> aqui </a>");
                    mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));

                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Manage(User model)
        {
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var user = Db.Users.Where(u => u.UserName == model.UserName).FirstOrDefault();
                string newPassword = "";

                if (user != null)
                {
                    newPassword = MembershipService.ResetPassword(user.UserName);

                    dynamic email = new Email("RecoverPassword");
                    email.To = user.Email;
                    email.From = ConfigurationManager.AppSettings["NoreplyAddress"];
                    email.NewPassword = newPassword;
                    email.Send();
                }
            }
            return View();
        }

        public List<SelectListItem> GetDDLOptions(string type, string selected = "")
        {
            List<SelectListItem> result = null;
            //var defaultText = Resources.Tools.Contact.Default_DDL_Value.ToString();
            //bool isEnglish = CultureHelper.IsEnglish();

            switch (type)
            {
                case "schools":
                    {
                        List<School> options = null;

                        options = Db.Schools.OrderBy(c => c.Name).ToList();
                        result = new List<SelectListItem>();
                        result.Add(new SelectListItem { Text = "Selecciona tu escuela", Value = "" });
                        foreach (var school in options)
                        {
                            result.Add(new SelectListItem
                            {
                                Text = school.Name,
                                Value = school.Id.ToString()
                            });
                        }
                        break;
                    }
                case "grades":
                    {
                        List<Grade> options = null;

                        options = Db.Grades.OrderBy(c => c.Position).ToList();
                        result = new List<SelectListItem>();
                        result.Add(new SelectListItem { Text = "Selecciona", Value = "" });
                        foreach (var grade in options)
                        {
                            result.Add(new SelectListItem
                            {
                                Text = grade.Name,
                                Value = grade.Id.ToString()
                            });
                        }
                        break;
                    }
            }

            return result;
        }

        public void InitializeModelImages(User user)
        {
            var mainImage = user.ImageHolders.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

            if (mainImage == null)
            {
                mainImage = new Cuentos.Models.Image { Target = ImageTarget.MAIN.ToString() };
                user.ImageHolders.Images.Add(mainImage);
            }

            mainImage.Dimensions = user.GetImageDimensions(ImageTarget.MAIN);
        }
    }
}
