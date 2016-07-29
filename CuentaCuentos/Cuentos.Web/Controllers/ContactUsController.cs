using Cuentos.Models.view;
using Cuentos.Models;
using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cuentos.Lib.Extensions;
using Mandrill;
using Mandrill.Models;

namespace Cuentos.Controllers
{
    public class ContactUsController : ApplicationGlobalController
    {

        public ActionResult Index()
        {
            var schools = Db.Schools.ToList().OrderBy(s => s.Name);
            ViewBag.Schools = new SelectList(schools, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactUsModel model)
        {
            var school = Db.Schools.Find(model.SchoolId);

            if (ModelState.IsValid)
            {
                MandrillApi mandrill = new MandrillApi("GnPxzjqcdDv66CSmE-06DA");
                var email = new EmailMessage();
                var recipients = new List<EmailAddress>();
                var admins = Db.Users.Include("Roles").Where(u => u.SchoolId == school.Id);

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
                    email.Subject = "Nuevo mensaje de contacto";
                    email.AddGlobalVariable("TITLE", "<strong>" + model.Name + "</strong> ha sometido un mensaje de contacto");
                    email.AddGlobalVariable("CONTENT", "El contenido del mensaje es: <br/>" + model.Comments + ".");
                    email.AddGlobalVariable("CALLTOACTION", "Ver detalles del mensaje <a href=\"" + Url.Action("Index", "Contacts", new { area = "admin" }, Request.Url.Scheme) + "\"> aqui </a>");
                    mandrill.SendMessage(new Mandrill.Requests.Messages.SendMessageRequest(email));
                }

                var contact = new Contact();
                contact.From = model.Email;
                contact.Name = model.Name;
                contact.Email = model.Email;
                contact.Phone = model.Phone;
                contact.SchoolId = school.Id;
                contact.Subject = model.Subject;
                contact.Comments = model.Comments;
                Db.Contacts.Add(contact);
                Db.SaveChanges();




            }

            return RedirectToAction("Index").Success("Su mensaje ha sido enviado, satisfactoriamente.");
        }
    }
}
