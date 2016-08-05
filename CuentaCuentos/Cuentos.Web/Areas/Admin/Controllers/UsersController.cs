using CodeFirstAltairis.Models;
using Cuentos.Models;
using Cuentos.Models.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Cuentos.Lib.Extensions;
using System.Web.Routing;
using CodeFirstAltairis.Controllers;
using Cuentos.Areas.Admin.Lib;
using Cuentos.Lib;
using System.Net;
using Mandrill;
using Cuentos.Lib.Helpers;
using PagedList;
using System.Data.Entity;
using System.Threading.Tasks;
using System.IO;
using Mandrill.Models;

namespace Cuentos.Areas.Admin.Controllers
{
    public class UsersController : AdminGlobalController
    {
        public async Task<ActionResult> Index(int? page)
        {
            IEnumerable<User> users = null;
            Task<List<User>> usersTask = null;

            if (IsSuperAdmin)
            {
                usersTask =  Db.Users.OrderBy(u => u.Name).ToListAsync();
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            }
            else
            {
                var user = LoggedUser;

                usersTask =  Db.Users.Where(u => u.SchoolId == user.SchoolId).OrderBy(u => u.Name).ToListAsync();
                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Users", "Schools", new{ id = user.SchoolId}), "Usuarios")
                };
            }
            try
            {
                Task.WaitAll(usersTask);

                users = usersTask.IsCompleted && usersTask.Exception == null ? usersTask.Result : null;
            }catch (AggregateException e)
            {
                users = null;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));
            //return View(users);
        }

        public async Task<ActionResult> GetAllUsers()
        {
            var values = await Db.Users.Select(u => new Cuentos.Models.User.UsersModel
            {
                Name = u.Name,
                Email = u.Email,
                LastName = u.LastName,
                UserName = u.UserName,
                CreatedDate = u.DateCreated
            }).ToListAsync();

            var allvalues =  Json(values, JsonRequestBehavior.AllowGet);
            allvalues.MaxJsonLength = int.MaxValue;
            return allvalues;
        }

        public async Task<ActionResult> Create()
        {
            User emptyUser = new User();
            InitializeModelImages(emptyUser);

            var model = new CreateUserModel
            {
                User = emptyUser
            };

            if (IsSuperAdmin)
            {
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Create", "Users"), "Crear"));
            }
            else
            {
                var user = LoggedUser;
                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Users", "Schools", new{ id = user.SchoolId}), "Usuarios"),
                    new KeyValuePair<String, String>(Url.Action("Create", "Users"), "Crear")
                };
            }


            setDropDowns();
            ViewBag.Interests = await Db.Interests.ToListAsync();
            ViewBag.Edit = false;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserModel model, string[] selectedInterests)
        {
            
            if (ModelState.IsValid)
            {
                var role = (Role.RoleType)Enum.Parse(typeof(Role.RoleType), model.Role, true);
                var createStatus = await AccountController.RegisterUser(model.User, model.Password, role, selectedInterests);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    var createdUser = await Db.Users.Include("ImageHolders").Where(u => u.UserName == model.User.UserName).FirstAsync();
                    model.User = createdUser;

                    return RedirectToAction("Index", "Users").Success(SaveMessage.CreateSuccess);
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    setDropDowns();
                    ViewBag.Interests = await Db.Interests.ToListAsync();
                }
            }

            User emptyUser = new User();
            InitializeModelImages(emptyUser);

            ViewBag.Interests = await Db.Interests.ToListAsync();
            setDropDowns();

            return View();
        }

        public async Task<ActionResult> Edit(string id)
        {
            User user = await Db.Users.Include("ImageHolders").Where(u => u.UserName == id).FirstAsync();
            if (!IsSuperAdmin)
            {

                var current = LoggedUser;

                if (!current.isAuthorized(id))
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            CreateUserModel model = new CreateUserModel
            {
                User = user,
                Role = user.Roles.First().RoleName
            };

            if (user.School != null)
            {
                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Users", "Schools", new{ id = user.SchoolId}), "Usuarios"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Users", new {id = user.UserName}), user.UserName)
                };
            }
            else
            {
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Users", new { id = id }), id), user);
            }

            InitializeModelImages(user);
            setDropDowns();
            ViewBag.Edit = true;
            ViewBag.Interests = await Db.Interests.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateUserModel model, HttpPostedFileBase mainImage, string[] selectedInterests)
        {
            if (ModelState.IsValid)
            {
                //
                var userTask = Db.Users.Include("ImageHolders").Where(u => u.UserName == model.User.UserName).FirstAsync();
                var rolesTask = Db.Roles.Where(r => r.RoleName == model.Role).ToListAsync();
                List <Role> roles = null;
                User user = null;
                try
                {
                    Task.WaitAll(userTask, rolesTask);
                    roles = rolesTask.Result;
                    user = userTask.Result;
                }catch(AggregateException e)
                {
                    
                }

                model.User.Roles = roles;
                model.User.ImageHolders = user.ImageHolders;
                var updated = await AccountController.UpdateUser(model.User, model.Password, selectedInterests);
                if (updated)
                {
                    if (mainImage != null)
                    {
                        var image = model.User.ImageHolders.getImagesByTarget(ImageTarget.MAIN).FirstOrDefault();

                        if (image == null)
                        {
                            image = new Cuentos.Models.Image
                            {
                                Target = ImageTarget.MAIN,
                                ImagebleId = model.User.ImageHolders.Id
                            };
                            model.User.ImageHolders.Images.Add(image);
                            Db.SaveChanges();
                        }

                        UploadImage(mainImage, image);
                        Db.SaveChanges();
                    }

                    return RedirectToAction("Edit", "Users", new { id = model.User.UserName }).Success(SaveMessage.ChangesSuccess);
                }
                else
                {
                    return RedirectToAction("Edit", "Users", new { id = model.User.UserName }).Error(SaveMessage.Error);
                }
            }
            else
            {
                setDropDowns();
            }

            return View(model);
        }

        

        public async Task<ActionResult> Comments(string id)
        {
            IEnumerable<Comment> comments = null;
            Task<List<Comment>> commentsTask = null;
            if (!IsSuperAdmin)
            {

                var current = LoggedUser;

                if (!current.isAuthorized(id))
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            commentsTask =  Db.Comments.Include("Story.User.School").Where(c => c.UserName == id).ToListAsync();
            var userTask =  Db.Users.Include("School").Where(u => u.UserName == id).FirstAsync();
            User user = null;
            try
            {
                Task.WaitAll(userTask, commentsTask);

                comments = commentsTask.Result;
                 user = userTask.Result;
            }catch (AggregateException e)
            {

            }
            ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                new KeyValuePair<String, String>(Url.Action("Index", "School"), "Escuelas"),
                new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                new KeyValuePair<String, String>(Url.Action("Users", "Schools", new{ id = user.SchoolId}), "Usuarios"),
                new KeyValuePair<String, String>(Url.Action("Edit", "Users", new {id = user.UserName}), user.UserName),
                new KeyValuePair<String, String>(Url.Action("Stories", "Users", new {id = user.UserName}), "Cuentos")
            };



            return View(comments);
        }

        public async Task<ActionResult> Stories(string id)
        {
            ViewBag.UserName = id;

            User user = await Db.Users.Include("ImageHolders").Where(u => u.UserName == id).FirstAsync();
            if (!IsSuperAdmin)
            {

                var current = LoggedUser;

                if (!current.isAuthorized(id))
                {
                    return RedirectToAction("Index", "Home");
                }
            }


            CreateUserModel model = new CreateUserModel
            {
                User = user,
                Role = user.Roles.First().RoleName
            };

            if (user.School != null)
            {
                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Users", "Schools", new{ id = user.SchoolId}), "Usuarios"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Users", new {id = user.UserName}), user.UserName)
                };
            }
            else
            {
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>(@Url.Action("Edit", "Users", new { id = id }), id), user);
            }

            return View("UserStories");
        }

        public async void setDropDowns()
        {
            //var schools = Db.Schools.ToList().OrderBy(s => s.Name);
            var grades = await Db.Grades.OrderBy(g => g.Position).ToListAsync();
            var sprAdminRole = Role.RoleType.superAdmin.ToString();
            var ownerTypesSelectList = new List<SelectListItem>();
            IEnumerable<School> schools = null;
            IEnumerable<Role> roles = null;
            if (IsSuperAdmin)
            {
                schools = await  Db.Schools.OrderBy(s => s.Name).ToListAsync();
                roles =  await Db.Roles.OrderBy(r => r.RoleName).ToListAsync();
            }
            else
            {
                var user = LoggedUser;
                schools = await  Db.Schools.Where(s => s.Id == user.SchoolId).OrderBy(s => s.Name).ToListAsync();
                roles = await Db.Roles.Where(r => r.RoleName != sprAdminRole).ToListAsync();
            }

            
            foreach (var ownerType in Enum.GetValues(typeof(User.OwnerType)).Cast<User.OwnerType>())
            {
                ownerTypesSelectList.Add(new SelectListItem
                {
                    Text = EnumHelper<User.OwnerType>.GetDisplayValue(ownerType),
                    Value = ownerType.ToString()
                });
            }
            ViewBag.OwnerTypesSelectList = ownerTypesSelectList;
            ViewBag.Roles = new SelectList(roles, "RoleName", "RoleName");
            ViewBag.Schools = new SelectList(schools, "Id", "Name");
            ViewBag.Grades = new SelectList(grades, "Id", "Name");

        }

        [HttpPost]
        public async Task<HttpResponseMessage> Approve(string username)
        {
            HttpResponseMessage response = null;

            try
            {
                var user = await Db.Users.Where(u => u.UserName == username).FirstAsync();
                var userRole = (Role.RoleType)Enum.Parse(typeof(Role.RoleType), user.Roles.First().RoleName, true);
                user.IsApproved = true;
                user.ApprovedDate = DateTime.Now;
                user.ApprovedBy = LoggedUser.UserName;
                Db.SaveChanges();

                if (userRole == Role.RoleType.student)
                {
                    MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                    var recipients = new List<EmailAddress> { new EmailAddress(user.Email) };
                    var email = new EmailMessage
                    {
                        To = recipients,
                    };

                    email.Subject = "Tu cuenta ha sido aprobada";
                    email.AddGlobalVariable("NAME", user.Name);
                    email.AddGlobalVariable("TITLE", "Tu cuenta ha sido aprobada.");
                    email.AddGlobalVariable("CONTENT", "Libera tu imaginación y atrévete a soñar! Imagina tu cuento, constrúyelo y comparte con otros...");
                    email.AddGlobalVariable("CALLTOACTION", "Comienza <a href=\"" + Url.Action("Create", "Stories", new { area = "" }, Request.Url.Scheme) + "\"> aqui </a>");
                    mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));
                }

                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };

            }
            catch (Exception e)
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(string username)
        {
            HttpResponseMessage response = null;

            try
            {
                var user = await Db.Users.FindAsync(username);
                Db.Users.Remove(user);
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem, User model = null)
        {
            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                currentItem
            };
            return Breadcrumbs(controllerBreadcrumbs, model);
        }

        public List<KeyValuePair<String, String>> Breadcrumbs(List<KeyValuePair<String, String>> items, User model = null)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Users"),"Usuarios"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

            if (model != null)
            {
                var item = new KeyValuePair<string, string>(@Url.Action("Index", "Users", new { id = model.UserName }), model.Name);
            }

            foreach (KeyValuePair<string, string> item in items)
            {
                if (item.Key != "")
                    breadcrumbs.Add(item);
            }

            return breadcrumbs;
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
