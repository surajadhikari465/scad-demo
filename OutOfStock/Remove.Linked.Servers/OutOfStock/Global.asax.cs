using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OOS.Model;
using OOSCommon;
using OOSCommon.Movement;
using OOSCommon.VIM;
using StructureMap;

namespace OutOfStock
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public static class StringExtensions
    {
        public static string PadBoth(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        public const string oosConnectionStringName = "OOSConnectionString";
        public const string oosEFConnectionStringName = "OOSEntities";
        public const string oosVIMServiceNameAppSetting = "VIM_SERVICE_NAME";
        public const string oosEventLogSource = "WFMOutOfStock";
        public const string oosTemporaryDownloadFilePathURL = "~/App_Data/FilesToDownload/";
        public const string oosNLogLoggerName = "NLogEventLog";
        public const string oosFileAtRoot = "Global.asax";

        public static string oosEFConnectionString
        {
            get
            {
                if (_oosEFConnectionString.Length < 1)
                    _oosEFConnectionString = OOSCommon.AppConfig.ConnectionStrings[oosEFConnectionStringName].ConnectionString;
                return _oosEFConnectionString;
            }
            set { _oosEFConnectionString = value; }
        } static string _oosEFConnectionString = string.Empty;


        static OOSCommon.Movement.MovementRepository _MovementRepository = null;
        public static OOSCommon.Movement.MovementRepository MovementRepository
        {
            get
            {
                if (_MovementRepository == null)
                {
                    try
                    {
                        var config = ObjectFactory.GetInstance<IConfigurator>();
                        var oosMovementServiceName = config.GetMovementServiceName();
                        var oosConnectionString = config.GetOOSConnectionString();
                        var logger = ObjectFactory.GetInstance<ILogService>().GetLogger();

                        _MovementRepository = new MovementRepository(oosConnectionString, oosMovementServiceName, logger);
                    }
                    catch (Exception)
                    {
                        // TODO: Log error
                    }
                }
                return _MovementRepository;

            }


            set { _MovementRepository = value; }
        }
        public static OOSCommon.VIM.VIMRepository vimRepository
        {
            get
            {
                if (_vimRepository == null)
                {
                    try
                    {
                        var config = ObjectFactory.GetInstance<IConfigurator>();
                        string oosVIMServiceName = config.GetVIMServiceName();
                            //OOSCommon.AppConfig.AppSettings[oosVIMServiceNameAppSetting];

                        string oosConnectionString = config.GetOOSConnectionString();
                            //OOSCommon.AppConfig.ConnectionStrings[oosConnectionStringName].ConnectionString;

                        //_vimRepository = new OOSCommon.VIM.VIMRepository(oosConnectionString,
                        //    oosVIMServiceName, OutOfStock.MvcApplication.oosLog);

                        var logger = ObjectFactory.GetInstance<ILogService>().GetLogger();
                        _vimRepository = new VIMRepository(oosConnectionString, oosVIMServiceName, logger);
                    }
                    catch (Exception)
                    {
                        // TODO: Log error
                    }
                }
                return _vimRepository;
            }
            set { _vimRepository = value; }
        } static OOSCommon.VIM.VIMRepository _vimRepository = null;

        public static OOSCommon.IOOSLog oosLog
        {
            get
            {
                if (_oosLog == null)
                {
                    string basePath = System.Web.HttpContext.Current.Server.MapPath("~/" + oosFileAtRoot);
                    basePath = basePath.Substring(0, basePath.Length - oosFileAtRoot.Length);
                    _oosLog = new OOSCommon.OOSLog(oosNLogLoggerName, basePath,
                        new OOSCommon.OOSLog.GetSessionId(OOSWebGetSessionId), null);
                }
                return _oosLog;
            }
            set { _oosLog = value; }
        } static OOSCommon.IOOSLog _oosLog = null;

        /// <summary>
        /// Delgate for OOSCommon.OOSLog() in oosLog get
        /// </summary>
        /// <returns></returns>
        public static string OOSWebGetSessionId()
        {
            string sessionID = " ".PadBoth(24);
            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                !string.IsNullOrWhiteSpace(HttpContext.Current.Session.SessionID))
                sessionID = HttpContext.Current.Session.SessionID;
            return sessionID;
        }

        public static bool isEnabledAdvanceControls
        {
            get
            {
                if (!_isEnabledAdvanceControls.HasValue)
                {
                    try
                    {
                        _isEnabledAdvanceControls = (OOSCommon.AppConfig.AppSettings["UseTestData"] ?? "false")
                            .ToLower().Equals("true");
                    }
                    catch (Exception) { }
                }
                return _isEnabledAdvanceControls.GetValueOrDefault(false);
            }
            set { _isEnabledAdvanceControls = value; }
        } static bool? _isEnabledAdvanceControls = null;

        public static string oosVersion
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return String.Format("{0}.{1}.{2}.{3}", fvi.FileMajorPart,
                    fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
            }
        }

        // This is global so it can be initized on Application Start
        // It's initialization uses System.Web.HttpContext.Current which is not available
        // during Session End cleanup
        public static string oosTemporaryDownloadFilePath
        {
            get { return _oosTemporaryDownloadFilePath; }
            set { _oosTemporaryDownloadFilePath = value; } 
        } static string _oosTemporaryDownloadFilePath = string.Empty;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*.html|js|css|gif|jpg|jpeg|png|swf");
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "SummaryReport", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {

          //  Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey =
          //      System.Web.Configuration.WebConfigurationManager.AppSettings["iKey"];

            //OutOfStock.MvcApplication.oosLog.Trace("Enter");
            OutOfStock.MvcApplication.oosLog.Info("Version=" + oosVersion);

            // It's initialization uses System.Web.HttpContext.Current which is not available
            // during Session End cleanup
            oosTemporaryDownloadFilePath = 
                System.Web.HttpContext.Current.Server.MapPath(oosTemporaryDownloadFilePath);
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ContainerBootstrapper.Bootstrap();

            //OutOfStock.MvcApplication.oosLog.Trace("Exit");
        }

        void Session_End(Object sender, EventArgs E)
        {
            //OutOfStock.MvcApplication.oosLog.Trace("Enter");
            OutOfStock.Controllers.CustomReportController.TempFileCleanup();
            //OutOfStock.MvcApplication.oosLog.Trace("Exit");
        }

    }
}