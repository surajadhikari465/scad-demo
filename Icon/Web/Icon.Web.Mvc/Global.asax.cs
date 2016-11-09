using Icon.Web.Mvc.App_Start;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Icon.Web.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string IconVersionWithDate = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperWebConfiguration.Configure();
            GetBuildDate();
        }

        public void GetBuildDate()
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytesForDate = new byte[2048];
            string assemblyFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            System.IO.FileStream assemblyFileStream = null;
            try
            {
                assemblyFileStream = new System.IO.FileStream(assemblyFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                assemblyFileStream.Read(bytesForDate, 0, 2048);
            }
            finally
            {
                if (assemblyFileStream != null)
                    assemblyFileStream.Close();
            }
            DateTime localBuiltTime = new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(System.BitConverter.ToInt32(bytesForDate, System.BitConverter.ToInt32(bytesForDate, peHeaderOffset) + linkerTimestampOffset));

            DateTime linkerDateTime = localBuiltTime.AddHours(System.TimeZone.CurrentTimeZone.GetUtcOffset(localBuiltTime).Hours);
            IconVersionWithDate = "Version " + IconVersionWithDate + " Built: " + linkerDateTime.ToString("MMM dd, yyyy");
        }
    }
}
