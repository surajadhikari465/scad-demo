using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.Email;
using Mammoth.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Mammoth.ItemLocale.Controller
{
    public class ItemLocaleTopShelfService : ITopShelfService
    {
        private ItemLocaleControllerApplicationSettings settings;
        private IControllerApplication controllerApplication;
        private IEmailClient emailClient;
        private ILogger logger;
        private Timer runTimer;

        public ItemLocaleTopShelfService(
            ItemLocaleControllerApplicationSettings settings,
            IControllerApplication controllerApplication,
            IEmailClient emailClient,
            ILogger logger)
        {
            this.settings = settings;
            this.controllerApplication = controllerApplication;
            this.emailClient = emailClient;
            this.logger = logger;
            this.runTimer = new Timer(AppSettingsAccessor.GetIntSetting("RunIntervalInMilliseconds"));
            this.runTimer.Elapsed += this.RunService;
        }

        public void Start()
        {
            this.runTimer.Start();
        }

        public void Stop()
        {
            this.runTimer.Stop();
        }

        private void RunService(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.runTimer.Stop();

                logger.Info("Starting Mammoth " + settings.ControllerName + " Controller.");

                controllerApplication.Run();

                logger.Info("Shutting down Mammoth " + settings.ControllerName + " Controller.");
            }
            catch (Exception ex)
            {
                logger.Error("An exception occurred.", ex);
                emailClient.Send("An unexpected error occurred while processing Mammoth events. Error : " + ex.ToString(), "Mammoth " + settings.ControllerName + " Controller");
            }
            finally
            {
                this.runTimer.Start();
            }
        }
    }
}
