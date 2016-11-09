namespace RegionalEventController.Controller.Service
{
    using Icon.Logging;
    using Icon.Common.Email;
    using System;
    using RegionalEventController.Common;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;
    using Icon.Common;
    public class RegionalControllerService : IRegionalControllerService
    {
        private static NLogLogger<Program> logger = new NLogLogger<Program>();
        private static EmailClient emailClient;

        private Timer timer;
        
        public RegionalControllerService()
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
            try
            {
                int controllerInstanceId = AppSettingsAccessor.GetIntSetting("ControllerInstanceId");

                if (controllerInstanceId < 1)
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

                var regionsToProcess = ConfigurationManager.AppSettings["NewItemsEnabledRegionsList"].Split(',');
                StartupOptions.RegionsToProcess = regionsToProcess;

                logger.Info("Starting Regional Event Controller...");

                try
                {
                    PrepareControllerBase prepareController = ControllerProvider.PrepareRegionalController();
                    prepareController.Start();
                }
                catch (Exception ex)
                {
                    logger.Error(String.Format(Resource.ErrorFailedToRunPrepareRegionalController, ex));
                    emailClient.Send(String.Format(Resource.ErrorFailedToRunPrepareRegionalController, ex), Resource.EmailSubjectRunPrepareRegionalControllerError);
                    return;
                }

                foreach (string region in StartupOptions.RegionsToProcess)
                {
                    try
                    {
                        RegionalControllerBase regionalController = ControllerProvider.ComposeRegionalController(region);
                        regionalController.Start();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(String.Format(Resource.ErrorFailedToRunRegionalController, region, ex));
                        emailClient.Send(String.Format(Resource.ErrorFailedToRunRegionalController, region, ex), Resource.EmailSubjectRunRegionalControllerError);
                    }
                }

                logger.Info("Shutting down Regional Event Controller...");
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
