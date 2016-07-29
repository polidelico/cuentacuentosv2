using Cuentos.Lib.Binders;
using Cuentos.Models;
using System.Web.Mvc;

namespace Cuentos.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            string[] adminNamespace = new string[1] { "Cuentos.Areas.Admin.Controllers" };
            context.Routes.LowercaseUrls = true;

            context.MapRoute(
                name: "AdminSchoolUsers",
                url: "Admin/Schools/{id}/Users",
                defaults: new { controller = "Schools", action = "Users" },
                namespaces: adminNamespace
            );

            context.MapRoute(
                name: "AdminSchoolStories",
                url: "Admin/Schools/{id}/Stories",
                defaults: new { controller = "Schools", action = "Stories" },
                namespaces: adminNamespace
            );

            context.MapRoute(
                name: "AdminSchoolComments",
                url: "Admin/Schools/{id}/Comments",
                defaults: new { controller = "Schools", action = "Comments" },
                namespaces: adminNamespace
            );

            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: adminNamespace
            );

            ModelBinders.Binders.Add(typeof(School), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(Interest), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(Comment), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(Rating), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(Story), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(Category), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(BuilderGallery), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(ImageCategory), new PartialModelBinder());
            ModelBinders.Binders.Add(typeof(PageType), new PartialModelBinder());
        }
    }
}