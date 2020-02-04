﻿using SimpleInjector;

namespace Mammoth.ApiController.Service
{
    using Icon.ApiController.Controller;
    using Icon.Common;
    using Icon.Logging;
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Timers;

    public class ApiControllerService : IApiControllerService
    {       
        private System.Timers.Timer timer;
        private string controllerInstanceIdArgs;
        private string controllerType;
        private int dayOfTheWeek = 0;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private Container container;
        private int instance;
        private NLogLogger logger;

        public ApiControllerService()
        {
            logger = new NLogLogger(typeof(Program));
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

            if (!int.TryParse(controllerInstanceIdArgs, out instance) || instance < 1)
            {
                logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                throw new Exception("Please provide an integer greater than zero to be used as the unique instance ID.");
            }
            container = SimpleInjectorInitializer.InitializeContainer(instance, controllerType);

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

            try
            {
                container.GetInstance<ApiControllerBase>().Execute();
            }
            catch (Exception ex)
            {
                logger.Error("An unexpected exception occurred. Exception details - " + ex);
            }
            
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }
    }
}
