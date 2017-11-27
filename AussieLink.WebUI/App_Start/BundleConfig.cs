using System.Web;
using System.Web.Optimization;

namespace AussieLink.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Vendors/JavaScript/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Vendors/JavaScript/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Vendors/JavaScript/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Vendors/JavaScript/bootstrap.js",
                      "~/Content/Vendors/JavaScript/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Vendors/Css/normalize.css",
                      "~/Content/Resources/Css/style.css",
                      "~/Content/Resources/Css/mediaquery.css"));

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                      "~/Content/Resources/JavaScript/mainScript.js"));
        }
    }
}
