using Cuentos.Lib;
using Cuentos.Lib.Extensions;
using Cuentos.Models;
using Cuentos.Models.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace Cuentos.Areas.Admin.Controllers
{
    public class ContactsController : AdminGlobalController
    {
        //
        // GET: /Admin/Messages/

        public async Task<ActionResult> Index()
        {

            IEnumerable<Contact> contacts = null;



            if (IsSuperAdmin)
            {
                contacts =  await Db.Contacts.Include("School").OrderByDescending(c => c.CreatedAt).ToListAsync();
                ViewBag.breadcrumbs = Breadcrumbs(new KeyValuePair<String, String>("", ""));
            }
            else
            {
                var user = LoggedUser;
                contacts = await Db.Contacts.Include("School").Where(c => c.SchoolId == user.SchoolId).OrderByDescending(c => c.CreatedAt).ToListAsync();

                ViewBag.breadcrumbs = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<String, String>(Url.Action("Index","Home"), "Inicio"),
                    new KeyValuePair<String, String>(Url.Action("Index", "Home"), "Escuelas"),
                    new KeyValuePair<String, String>(Url.Action("Edit", "Schools", new {id = user.SchoolId}), user.School.Name),
                    new KeyValuePair<String, String>(Url.Action("Index"), "Messages")
                };


            }


            return View(contacts);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Read(int id)
        {
            HttpResponseMessage response = null;

            try
            {
                var contact = await Db.Contacts.FindAsync(id);
                contact.isRead = true;
                contact.UpdatedAt = DateTime.Now;
                Db.SaveChanges();
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
            }
            catch (Exception)
            {
                response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };
            }

            return response;
        }


        public List<KeyValuePair<String, String>> Breadcrumbs(KeyValuePair<String, String> currentItem)
        {
            List<KeyValuePair<String, String>> breadcrumbs = base.Breadcrumbs();

            List<KeyValuePair<String, String>> controllerBreadcrumbs = new List<KeyValuePair<String, String>>
            {
                new KeyValuePair<String, String>(@Url.Action("Index", "Messages"),"Mensajería"),
            };

            foreach (KeyValuePair<string, string> item in controllerBreadcrumbs)
                breadcrumbs.Add(item);

           

            if (currentItem.Key != "")
                breadcrumbs.Add(currentItem);

            return breadcrumbs;
        }

    }
}
