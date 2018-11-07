﻿namespace Icon.ApiController.Controller.Service
{
    using Icon.ApiController.Common;
    using Icon.ApiController.Controller;
    using Icon.ApiController.Controller.ControllerBuilders;
    using Icon.Common;
    using Icon.Logging;
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Timers;

    public class ApiControllerService : IApiControllerService
    {
        private static ILogger<Program> logger = new NLogLogger<Program>();
        private System.Timers.Timer timer = null;
        private readonly string controllerType;
        private readonly int dayOfTheWeek = 0;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private readonly int controllerInstanceID;

        private static string sSource;
        private static string sLog;
        private static string sEvent;

        public ApiControllerService()
        {
          try
          {
            int.TryParse(ConfigurationManager.AppSettings["ControllerInstanceId"], out this.controllerInstanceID);
            controllerType = ConfigurationManager.AppSettings["ControllerType"].ToString();
            int.TryParse(ConfigurationManager.AppSettings["MaintenanceDay"], out dayOfTheWeek);

            DateTime timeStamp;
            if(!DateTime.TryParse(ConfigurationManager.AppSettings["MaintenanceStartTime"], out timeStamp))
              throw new ArgumentException("Invalid or missing MaintenanceStartTime configuration setting");
            
            startTime = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0);

            if(!DateTime.TryParse(ConfigurationManager.AppSettings["MaintenanceEndTime"], out timeStamp))
              throw new ArgumentException("Invalid or missing MaintenanceEndTime configuration setting");
            
            endTime = new TimeSpan(timeStamp.Hour, timeStamp.Minute, 0);

            //Validate config settings
            if(this.controllerInstanceID < 1)
                throw new Exception("Please provide an integer greater than zero to be used as the unique instance ID.");
            if(this.controllerInstanceID < 1 || string.IsNullOrEmpty(controllerType))
                throw new Exception("Both the controller type argument and the instance ID argument are required.");
            if(!StartupOptions.ValidArgs.Contains(controllerType))
                throw new Exception(string.Format("Invalid argument specified.  The valid arguments are: {0}", string.Join(",", StartupOptions.ValidArgs)));

            //Initilize timer if all settings have been validated
            int runInterval;
            int.TryParse(ConfigurationManager.AppSettings["RunInterval"], out runInterval);
            this.timer = new System.Timers.Timer(runInterval > 0 ? runInterval : 30000); //Use default interval == 30000 in case if config setting is missing or invalid
          }
          catch(Exception ex) { logger.Error(ex.Message); }
        }
        public void Start()
        {
          if(this.timer == null) return;
          this.timer.Elapsed += RunService;
          this.timer.Start();
		}

        private void RunService(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();
            var now = DateTime.Now;

            if(now.DayOfWeek == (DayOfWeek)dayOfTheWeek && now.TimeOfDay >= startTime && now.TimeOfDay <= endTime)
            {
                //logger.Info(string.Format("API Controller exited because the it is in the maintenance window. Type: {0} - Instance: {1} - CurrentTime: {2}", controllerType, controllerInstanceIdArgs, DateTime.Now.ToString()));
                this.timer.Start();
                return;
            }

            ControllerType.Instance = this.controllerInstanceID;
            logger = new NLogLoggerInstance<Program>(ControllerType.Instance.ToString());

            try
            {
                var apiController = ControllerProvider.GetController(controllerType);

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

                //Log to the Event Log
                try
                {
                    sLog = "Application";

                    sEvent = "ApiControllerService - RunService " + ex.Message;

                    if (ex.InnerException != null)
                        sEvent = sEvent + " -- " + ex.InnerException.Message;

                    if (!EventLog.SourceExists(sSource))
                        EventLog.CreateEventSource(sSource, sLog);

                    EventLog.WriteEntry(sSource, sEvent,
                    EventLogEntryType.Error);
                }
                catch (Exception eventLogException)
                {
                    logger.Error($"Unable to log to the Event Log. API Controller type: {sSource}. Error: {eventLogException.ToString()}");
                }
                finally
                {
                    logger.Error($"An unexpected error occurred. API Controller type: {sSource}. Error: {ex.ToString()}");
                }
            }
            finally
            {
                timer.Start();
            }
        }

        public void Stop()
        {
          if(timer == null) return;

          timer.Stop();
          timer.Elapsed -= RunService;
        }
    }
}