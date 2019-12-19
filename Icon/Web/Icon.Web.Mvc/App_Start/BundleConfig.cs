using System.Configuration;
using System.Web.Optimization;

namespace Icon.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true; // set to false to see unminified css and js

            bundles.Add(new ScriptBundle("~/bundles/everything").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.bundle.js",
                "~/Scripts/jquery.unobtrusive-ajax*",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive*",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/infragistics/js/infragistics.core.js",
                "~/Scripts/infragistics/js/infragistics.lob.js",
                "~/Scripts/infragistics/js/infragistics.dv.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/base/jquery-ui.css")
                .Include("~/Content/Infragistics/css/themes/bootstrap4/default/infragistics.theme.css", new CssRewriteUrlTransform())
                .Include("~/Content/Infragistics/css/structure/infragistics.css", new CssRewriteUrlTransform())
                .Include("~/Content/site.css", new CssRewriteUrlTransform())
                .Include("~/Content/font-awesome.css", new CssRewriteUrlTransform()));

            // .NET MVC bundler does not currently suport CSS variables so Bootstrap won't minify
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootswatch.css"));
        }
    }
}