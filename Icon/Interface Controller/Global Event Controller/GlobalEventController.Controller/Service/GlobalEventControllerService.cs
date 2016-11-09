

namespace GlobalEventController.Controller.Service
{
    using Icon.Common;
    using Icon.Logging;
    using Common;
    using System;
    using System.Configuration;
    using System.Timers;
    using Icon.Common.Email;
    using System.Diagnostics;
    public class GlobalEventControllerService : IGlobalEventControllerService
    {
        private static ILogger<Program> logger = new NLogLogger<Program>();
        private Timer timer;
        private static EmailClient emailClient;
        private string controllerInstanceIdArgs;
        private int dayOfTheWeek = 0;
        private TimeSpan startTime;
        private TimeSpan endTime;

        private static string sSource;
        private static string sLog;
        private static string sEvent;

        public GlobalEventControllerService()
        {
            int runInterval = AppSettingsAccessor.GetIntSetting("RunInterval");
            this.timer = new Timer(runInterval);
            controllerInstanceIdArgs = ConfigurationManager.AppSettings["ControllerInstanceId"].ToString();
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

        public void Stop()
        {
            this.timer.Stop();
        }

        private void RunService(object sender, ElapsedEventArgs eventArgs)
        {
            this.timer.Stop();


            if (DateTime.Now.DayOfWeek == (DayOfWeek)dayOfTheWeek
                && DateTime.Now.TimeOfDay >= startTime
                && DateTime.Now.TimeOfDay <= endTime)
            {
                //logger.Info(String.Format("Global Controller exited because the it is in the maintenance window. Instance: {0} - CurrentTime: {1}", controllerInstanceIdArgs, DateTime.Now));
                this.timer.Start();
                return;
            }


            if (controllerInstanceIdArgs == null || controllerInstanceIdArgs.Length == 0)
            {
                logger.Error("No app.config setting for ControllerInstanceId provided.  The program cannot continue.");
                return;
            }

            int controllerInstanceId = 0;
            if (!Int32.TryParse(controllerInstanceIdArgs, out controllerInstanceId) || controllerInstanceId < 1)
            {
                logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                return;
            }

            try
            {
                emailClient = EmailClient.CreateFromConfig();
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Unable to create email client. {0}", ex));
                return;
            }

            StartupOptions.Instance = controllerInstanceId;
            logger = new NLogLoggerInstance<Program>(StartupOptions.Instance.ToString());
            GlobalControllerBase globalController = null;

            sSource = "Icon - Global Controller";
            sLog = "Application";
            try
            {
                ControllerProvider.IntializeSettings();
                globalController = ControllerProvider.ComposeController();
            }
            catch (Exception ex)
            {
                sEvent = "GlobalEventControllerService.ControllerProvider.IntializeSettings " + ex.Message;
                if (ex.InnerException != null)
                    sEvent = sEvent + " -- " + ex.InnerException.Message;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent,
                EventLogEntryType.Error);

                logger.Error(String.Format(Resources.ErrorFailedToBuildGlobalController, ex));
                emailClient.Send(String.Format(Resources.ErrorFailedToBuildGlobalController, ex), Resources.EmailSubjectBuildGlobalControllerError);
            }

            logger.Info("Starting Global Event Controller...");

            if (globalController != null)
            {
                try
                {
                    globalController.Start();
                }
                catch (Exception ex)
                {
                    sEvent = "GlobalEventControllerService.globalController.Start " + ex.Message;
                    if (ex.InnerException != null)
                        sEvent = sEvent + " -- " + ex.InnerException.Message;

                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);

                    EventLog.WriteEntry(sSource, sEvent,
                    EventLogEntryType.Error);

                    logger.Error(String.Format(Resources.ErrorUnexpectedException, ex));
                    emailClient.Send(String.Format(Resources.ErrorUnexpectedException, ex), Resources.EmailSubjectUnexpectedException);
                }
            }

            logger.Info("Shutting down Global Event Controller...");


            this.timer.Start();
        }
    }
}
