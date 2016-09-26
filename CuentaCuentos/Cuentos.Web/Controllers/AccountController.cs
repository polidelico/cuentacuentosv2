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
using System.Data.Entity;
using Cuentos.Lib;
using Mandrill;
using Cuentos.Lib.Helpers;
using Mandrill.Models;
using System.Threading.Tasks;

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

        public async Task<ActionResult> Index()
        {

            //CultureInfo es = new CultureInfo("es-PR");
            //Thread.CurrentThread.CurrentCulture = es;

            var model =  await Db.Users.Include("ImageHolders").Where(u => u.UserName == User.Identity.Name).FirstAsync();
            var schools = await Db.Schools.OrderBy(s => s.Name).ToListAsync();

            var user = LoggedUser;
            var StoriesNotApproved =  await Db.Stories.Where(s =>
                                                      s.UserName == user.UserName
                                                      && (s.Status == StatusStory.Draft || 
                                                      s.Status == StatusStory.InApproval || 
                                                      s.Status == StatusStory.UnPublished)).ToListAsync();

            var storiesApproved = await Db.Stories.Where(s => s.UserName == user.UserName
                                                   && (s.Status == StatusStory.Published)).ToListAsync();
            
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            ViewBag.StoriesNotApproved = StoriesNotApproved;
            ViewBag.StoriesApproved = storiesApproved;
            InitializeModelImages(model);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(User model, HttpPostedFileBase mainImage)
        {
            if (ModelState.IsValid)
            {
                var user = await Db.Users.Include("ImageHolders").Where(u => u.UserName == model.UserName).FirstAsync();
                model.ImageHolders = user.ImageHolders;

                model.IsApproved = true;
                var updated = await AccountController.UpdateUser(model, null);
                if (updated)
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
                       var result = await Db.SaveChangesAsync();
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
        public async Task<ActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PasswordLength = MembershipService.MinPasswordLength;
                var result = await setRegisterModel();

                return View();
            }
        }

        private async Task<bool> setRegisterModel()
        {
            var schoolsTask = await Db.Schools.OrderBy(s => s.Name).ToListAsync();
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
            ViewBag.Schools = new SelectList(schoolsTask, "Id", "Name");


            //RegisterModel model = new RegisterModel();
            //model.DDLSchool = GetDDLOptions("schools");
            //model.DDLGrade = GetDDLOptions("grades");
            return true;
            //return model;
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                model.User.SchoolId = model.SchoolId;
                model.User.Grade = model.Grade;

                var createStatus = await RegisterUser(model.User, model.Password, Role.RoleType.student);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                    var email = new EmailMessage();
                    var recipients = new List<EmailAddress>();
                    var admins = await Db.Users.Include("Roles").Where(u => u.SchoolId == model.SchoolId).ToListAsync();

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
                        var result = await mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));
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

        public static async Task<MembershipCreateStatus> RegisterUser(User model, string password, Role.RoleType roleType)
        {
            IMembershipService MembershipService = new AccountMembershipService();
            CuentosContext Db = new CuentosContext();

            var createStatus = MembershipService.CreateUser(model.UserName, password, model.Email);

            if (createStatus == MembershipCreateStatus.Success)
            {
                var user = await Db.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                var role = await Db.Roles.FindAsync(roleType.ToString());
               
                user.Name = model.Name == null ? string.Empty : model.Name;
                user.LastName = model.LastName;
                user.SchoolId = model.SchoolId;
                user.Age = model.Age;
                user.Email = model.Email;
                user.Roles.Add(role);
                user.IsApproved = model.IsApproved;
                user.Owner = model.Owner;

               

                Db.SaveChanges();
            }

            return createStatus;
        }

        public static async Task<bool> UpdateUser(User model, string password = null)
        {
            var result = false;
            try
            {
                IMembershipService MembershipService = new AccountMembershipService();
                CuentosContext Db = new CuentosContext();
                var user = await Db.Users.Include("ImageHolders").Where(u => u.UserName == model.UserName).FirstAsync();

                user.UpdateUserFields(model);

                if (model.Roles != null)
                {
                    string roleName = model.Roles.First().RoleName;
                    var role = await Db.Roles.FindAsync(roleName);
                    user.Roles.Clear();
                    user.Roles.Add(role);
                }

                if (!string.IsNullOrEmpty(password))
                {
                    string generatedPW = MembershipService.ResetPassword(user.UserName);
                    MembershipService.ChangePassword(user.UserName, generatedPW, password);
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
        public  async Task<ActionResult> RecoverPassword(User model)
        {
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var user = await Db.Users.Where(u => u.UserName == model.UserName).FirstOrDefaultAsync();
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
                   var sent = await mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));

                }
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Manage(User model)
        {
            if (!string.IsNullOrEmpty(model.UserName))
            {
                var user = await Db.Users.Where(u => u.UserName == model.UserName).FirstOrDefaultAsync();
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

        public async void ExternalLogin(string provider)
        {
            Redirect("http://redirectiontksflksd");
        }

        public async Task<List<SelectListItem>> GetDDLOptions(string type, string selected = "")
        {
            List<SelectListItem> result = null;
            //var defaultText = Resources.Tools.Contact.Default_DDL_Value.ToString();
            //bool isEnglish = CultureHelper.IsEnglish();

            switch (type)
            {
                case "schools":
                    {
                        List<School> options = null;

                        options = await Db.Schools.OrderBy(c => c.Name).ToListAsync();
                        result = new List<SelectListItem>();
                        result.Add(new SelectListItem { Text = "Selecciona tu escuela", Value = "" });
                        result.AddRange(options.Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }));

                        break;
                    }
                case "grades":
                    {
                        result = new List<SelectListItem>();
                        result.Add(new SelectListItem { Text = "Selecciona", Value = "" });
                        var names = Enum.GetNames(typeof(Grade));
                        for(int i = 0; i < names.Length; i++)
                            result.Add(new SelectListItem { Text = names[i], Value = i.ToString() });
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
