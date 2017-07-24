using System.Web.Optimization;

namespace CompunetCbte
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-{version}.js",

                "~/assets/js/libs/jquery/jquery-1.11.2.min.js",
                "~/assets/js/libs/jquery/jquery-migrate-1.2.1.min.js",
                "~/assets/js/libs/bootstrap/bootstrap.min.js",
                "~/assets/js/core/source/App.js",
                "~/assets/js/core/source/AppNavigation.js",
                "~/assets/js/core/source/AppOffcanvas.js",
                "~/assets/js/core/source/AppCard.js",
                "~/assets/js/core/source/AppForm.js",
                "~/assets/js/core/source/AppVendor.js",
                "~/assets/js/core/demo/Demo.js",
                "~/assets/js/core/demo/DemoDashboard.js",

                "~/Scripts/alertify/alertify.js"
            // needed for drag/move events in fullcalendar

            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*",
                "~/Scripts/chosen.jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                //"~/Content/site.css",
                //"~/Content/bootstrap.css",
                // "~/Content/fullcalendar.css",
                //"~/Content/fullcalendar.print.css",
                "~/Content/chosen.css",
                "~/Content/alertify/alertify.bootstrap.css",
                "~/Content/alertify/alertify.core.css",
                "~/Content/alertify/alertify.default.css"
            ));

            bundles.Add(new StyleBundle("~/Content/fullcalendarcss").Include(
                "~/Content/themes/jquery.ui.all.css",
                "~/Content/fullcalendar.css"));

            //Calendar Script file

            bundles.Add(new ScriptBundle("~/bundles/fullcalendarjs").Include(
                "~/Scripts/jquery-ui-{version}.js",

                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-modal.js",
                "~/Scripts/fullcalendar.min.js",
                "~/Scripts/alertify.min.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}