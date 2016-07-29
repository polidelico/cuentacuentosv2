using System.Web;
using System.Web.Optimization;

namespace Cuentos
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/vendor/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/vendor/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include(
                "~/Scripts/vendor/jquery.validate.js",
                "~/Scripts/vendor/jquery.validate.unobtrusive.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/flash").Include(
                        "~/Areas/Admin/Content/js/jquery.cookie.js",
                        "~/Areas/Admin/Content/js/jQuery.flashMessage.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminscripts").Include(
                "~/Scripts/vendor/jquery.validate.unobtrusive.js",
                "~/Scripts/vendor/jquery.validate.js",
                "~/Areas/Admin/Content/js/application.js",
                "~/Areas/Admin/Content/js/confirmDelete.js",
                "~/Areas/Admin/Content/js/confirmApprove.js",
                "~/Areas/Admin/Content/js/messageModal.js",
                "~/Areas/Admin/Content/js/vendor/jasny-bootstrap.js",
                "~/Areas/Admin/Content/js/jquery.cookie.js",
                "~/Areas/Admin/Content/js/jQuery.flashMessage.js",
                "~/Areas/Admin/Content/js/main.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/publicscripts").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/vendor/slick/slick.js",
                "~/Scripts/vendor/sweetalert/sweet-alert.js",
                "~/Scripts/main.js",
                "~/Areas/Admin/Content/js/jquery.cookie.js",
                "~/Areas/Admin/Content/js/jQuery.flashMessage.js"
            ));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/vendor/modernizr-2.6.2.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/normalize.css",
                "~/Content/css/main.css",
                "~/Content/css/main_rodri.css",
                "~/Content/css/main_micuentos.css",
                "~/Content/css/main_galeriacuentos.css",
                "~/Content/css/main_ayuda.css",
                "~/Scripts/vendor/slick/slick.css",
                "~/Scripts/vendor/sweetalert/sweet-alert.css",
                "~/Scripts/vendor/fancybox/jquery.fancybox.css",
                "~/Content/css/mainOverride.css"
            ));



            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));




        }
    }
}