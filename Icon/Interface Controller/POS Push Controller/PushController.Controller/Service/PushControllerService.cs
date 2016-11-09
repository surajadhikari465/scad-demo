
namespace PushController.Controller.Service
{
    using Icon.Common;
    using Icon.Logging;
    using PushController.Common;
    using System;
    using System.Configuration;
    using System.Timers;

    public class PushControllerService : IPushControllerService
    {
        private static NLogLogger<Program> logger = new NLogLogger<Program>();
        private Timer timer;

        public PushControllerService()
        {
            int runInterval = AppSettingsAccessor.GetIntSetting("RunInterval");
            this.timer = new Timer(runInterval);
        }

        public void Start()
        {
            this.timer.Elapsed += RunService;
            this.timer.Start();
        }

        private void RunService(object sender, ElapsedEventArgs eventArgs)
        {
            this.timer.Stop();

            Cache.ClearAll();

            try
            {
                int controllerInstanceId = AppSettingsAccessor.GetIntSetting("ControllerInstanceId");

                if (controllerInstanceId < 1)
                {
                    logger.Error("Please provide an integer greater than zero to be used as the unique instance ID.");
                    return;
                }

                StartupOptions.Instance = controllerInstanceId;

                string[] configuredRegions = ConfigurationManager.AppSettings["RegionsToProcess"].Split(',');

                if (configuredRegions.Length == 0)
                {
                    logger.Error("No regions are configured for processing.");
                    return;
                }

                StartupOptions.RegionsToProcess = configuredRegions;

                int maxRecordsToProcess;
                if (!Int32.TryParse(ConfigurationManager.AppSettings["MaxRecordsToProcess"], out maxRecordsToProcess))
                {
                    maxRecordsToProcess = 100;
                }

                StartupOptions.MaxRecordsToProcess = maxRecordsToProcess;

                var posController = ControllerProvider.ComposeController();

                logger.Info("Starting POS Push Controller...");

                posController.Start();

                logger.Info("Shutting down POS Push Controller...");
            }
            catch (Exception)
            {
                throw;
            }

            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }
    }
}
