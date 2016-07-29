using Cuentos.Lib.Binders;
using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cuentos
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            string[] webNamespace = new string[1] { "Cuentos.Controllers" };
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Privacidad",
               url: "privacidad",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "Privacy" }
           );

            routes.MapRoute(
               name: "Terminos",
               url: "terminos-de-uso",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "Terms" }
           );

            routes.MapRoute(
               name: "Ayuda",
               url: "ayuda",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "Help" }
           );

            routes.MapRoute(
               name: "SobreNosotros",
               url: "sobre-nosotros",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "About" }
           );

            routes.MapRoute(
               name: "Padres",
               url: "padres",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "Parents" }
           );

            routes.MapRoute(
               name: "Educadores",
               url: "educadores",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "Educators" }
           );

            routes.MapRoute(
               name: "CreaTuCuento",
               url: "crea-tu-cuento",
               namespaces: webNamespace,
               defaults: new { controller = "Home", action = "BuildYourStory" }
           );


            routes.MapRoute(
               name: "VerPerfilUsuario",
               url: "usuarios/{id}",
               namespaces: webNamespace,
               defaults: new { controller = "Users", action = "Details" }
           );


            routes.MapRoute(
               name: "VerEscuela",
               url: "escuelas/{id}",
               namespaces: webNamespace,
               defaults: new { controller = "Schools", action = "Details" }
           );

            routes.MapRoute(
               name: "Escuelas",
               url: "escuelas",
               namespaces: webNamespace,
               defaults: new { controller = "Schools", action = "Index" }
           );

            routes.MapRoute(
               name: "Contactanos",
               url: "contactanos",
               namespaces: webNamespace,
               defaults: new { controller = "ContactUs", action = "Index" }
           );


            routes.MapRoute(
               name: "RecuperarContrasena",
               url: "mi-cuenta/recuperar-contrasena",
               namespaces: webNamespace,
               defaults: new { controller = "Account", action = "RecoverPassword" }
           );

            routes.MapRoute(
               name: "CambiarContrasena",
               url: "mi-cuenta/cambiar-contrasena",
               namespaces: webNamespace,
               defaults: new { controller = "Account", action = "ChangePassword" }
           );

            routes.MapRoute(
               name: "LoginMiCuenta",
               url: "mi-cuenta/accesar",
               namespaces: webNamespace,
               defaults: new { controller = "Account", action = "LogIn" }
           );

            routes.MapRoute(
               name: "RegistraMiCuenta",
               url: "mi-cuenta/registrar",
               namespaces: webNamespace,
               defaults: new { controller = "Account", action = "Register" }
           );

            routes.MapRoute(
               name: "MiCuenta",
               url: "mi-cuenta",
               namespaces: webNamespace,
               defaults: new { controller = "Account", action = "Index" }
           );


            routes.MapRoute(
               name: "AnadirCommentCuento",
               url: "mis-cuentos/anadir-comentario/{storyId}/{newComment}",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "AddComment" }
           );

            routes.MapRoute(
               name: "BuscarCuento",
               url: "mis-cuentos/buscar",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "Search" }
           );

            routes.MapRoute(
               name: "CrearCuento",
               url: "mis-cuentos/crear",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "Create" }
           );

            routes.MapRoute(
               name: "VerCuento",
               url: "mis-cuentos/{id}",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "Details" }
           );

            routes.MapRoute(
               name: "EditarCuento",
               url: "mis-cuentos/{id}/editar/",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "Edit" }
           );

            routes.MapRoute(
               name: "BorrarCuento",
               url: "mis-cuentos/{id}/borrar",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "Delete" }
           );

            routes.MapRoute(
               name: "PublicarCuento",
               url: "mis-cuentos/{id}/publicar/",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "Publish" }
           );

            routes.MapRoute(
               name: "AnularPublicacionCuento",
               url: "mis-cuentos/{id}/anular-publicacion/",
               namespaces: webNamespace,
               defaults: new { controller = "Stories", action = "UnPublish" }
           );

            routes.MapRoute(
                name: "VerUsuario",
                url: "usuario/{id}",
                namespaces: webNamespace,
                defaults: new { controller = "Users", action = "Details" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: webNamespace
            );
        }
    }
}