namespace Icon.ApiController.Controller.Service
{
    using Icon.ApiController.Common;
    using Icon.ApiController.Controller;
    using Icon.ApiController.Controller.ControllerBuilders;
    using Icon.Common;
    using Icon.Framework;
    using Icon.Framework.RenewableContext;
    using Icon.Logging;
    using RenewableContext;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Timers;
    using System.Diagnostics;
    public class ApiControllerService : IApiControllerService
    {
        private static ILogger<Program> logger = new NLogLogger<Program>();
        private static IRenewableContext<IconContext> iconContextFactory;
        private System.Timers.Timer timer;
        private string controllerInstanceIdArgs;
        private string controllerType;
        private int dayOfTheWeek = 0;
        private TimeSpan startTime;
        private TimeSpan endTime;

        private static string sSource;
        private static string sLog;
        private static string sEvent;

        public ApiControllerService()
        {
            int runInterval = AppSettingsAccessor.GetIntSetting("RunInterval");
            this.timer = new System.Timers.Timer(runInterval);
            controllerInstanceIdArgs = ConfigurationManager.AppSettings["ControllerInstanceId"].ToString();
            controllerType = ConfigurationManager.AppSettings["ControllerType"].ToString();
            string maintenanceDaySetting = ConfigurationManager.AppSettings["MaintenanceDay"].ToString();
            string maintenanceStartTimeSetting = ConfigurationManager.AppSettings["MaintenanceStartTime"].ToString();
            string maintenanceEndTimeSetting = ConfigurationManager.AppSettings["MaintenanceEndTime"].ToString();
            int startHour = 0;
            int startMin = 0;
            int endHour = 0;
            int endMin = 0;
            int.TryParse(maintenanceDaySetting, out dayOfTheWeek);

            int.TryParse(maintenanceStartTimeSetting.Substring(0, maintenanceStartTimeSetting.IndexOf(':')), out startHour);
            int.TryParse(maintenanceStartTimeSetting.Substring(maintenanceStartTimeSetting.IndexOf(':') + 1), out startMin);
            int.TryParse(maintenanceEndTimeSetting.Substring(0, maintenanceEndTimeSetting.IndexOf(':')), out endHour);
            int.TryParse(maintenanceEndTimeSetting.Substring(maintenanceEndTimeSetting.IndexOf(':') + 1), out endMin);

            startTime = new TimeSpan(startHour, startMin, 0);
            endTime = new TimeSpan(endHour, endMin, 0);
        }
        public void Start()
        {
            this.timer.Elapsed += RunService;
            this.timer.Start();
        }

        private void RunService(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();

            if (DateTime.Now.DayOfWeek == (DayOfWeek)dayOfTheWeek
                && DateTime.Now.TimeOfDay >= startTime
                && DateTime.Now.TimeOfDay <= endTime)
            {
                //logger.Info(string.Format("API Controller exited because the it is in the maintenance window. Type: {0} - Instance: {1} - CurrentTime: {2}", controllerType, controllerInstanceIdArgs, DateTime.Now.ToString()));
                this.timer.Start();
                return;
            }

            if (string.IsNullOrEmpty(controllerInstanceIdArgs) || string.IsNullOrEmpty(controllerType))
            {
                logger.Error("Both the controller type argument and the instance ID argument are required.");
                return;
            }

            if (!StartupOptions.ValidArgs.Contains(controllerType))
            {
                logger.Error(string.Format("Invalid argument specified.  The valid arguments are: {0}", string.Join(",", StartupOptions.ValidArgs)));
                return;
            }

            int instance;
            if (!Int32.TryParse(controllerInstanceIdArgs, out instance) || instance < 1)
            {
                logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                return;
            }

            ControllerType.Instance = instance;

            logger = new NLogLoggerInstance<Program>(ControllerType.Instance.ToString());

            try
            {

                ApiControllerBase apiController = null;

                apiController = ControllerProvider.GetController(controllerType);

                logger.Info(string.Format("Starting API Controller Phase 2 - Type: {0} - Instance: {1}.", ControllerType.Type, ControllerType.Instance));

                if (apiController != null)
                {
                    apiController.Execute();
                }

                // There's almost no delay between the last log entry in Execute() and this one, which sometimes causes the statements
                // to appear in the wrong order in the database.  This brief nap will add enough of a delay to prevent that.
                Thread.Sleep(100);

                logger.Info(string.Format("Shutting down API Controller Phase 2 - Type: {0} - Instance: {1}.", ControllerType.Type, ControllerType.Instance));

                //clearing cache at end of process since it is now a service and these are not released.
                Cache.financialSubteamToHierarchyClassId.Clear();
                Cache.scanCodeToItem.Clear();
            }
            catch (Exception ex)
            {
                switch (controllerType)
                {
                    case "l":
                        sSource = "Icon - API Controller - Locale";
                        break;
                    case "h":
                        sSource = "Icon - API Controller - Hierarchy";
                        break;
                    case "i":
                        sSource = "Icon - API Controller - ItemLocale";
                        break;
                    case "r":
                        sSource = "Icon - API Controller - Price";
                        break;
                    case "p":
                        sSource = "Icon - API Controller - Product";
                        break;
                    case "g":
                        sSource = "Icon - API Controller - ProductSelectionGroup";
                        break;
                    default:
                        sSource = "Icon - API Controller - Unknown Type";
                        break;
                }

                sLog = "Application";

                sEvent = "ApiControllerService - RunService " + ex.Message;

                if (ex.InnerException != null)
                    sEvent = sEvent + " -- " + ex.InnerException.Message;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent,
                EventLogEntryType.Error);
            }

            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }
    }
}
