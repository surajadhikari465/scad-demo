using Mammoth.Common;
using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.DataAccess;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Mammoth.Price.Controller
{
    public class PriceTopShelfService : ITopShelfService
    {
        private PriceControllerApplicationSettings settings;
        private IControllerApplication controllerApplication;
        private IEmailClient emailClient;
        private ILogger logger;
        private Timer runTimer;

        public PriceTopShelfService(
            PriceControllerApplicationSettings settings,
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
                if (!IsSqlTimeoutException(ex))
                {
                    emailClient.Send("An unexpected error occurred while processing Mammoth events. Error : " + ex.ToString(), "Mammoth " + settings.ControllerName + " Controller");
                }
            }
            finally
            {
                this.runTimer.Start();
            }
        }

        private bool IsSqlTimeoutException(Exception ex)
        {
            return ex.GetType() == typeof(SqlException)
                && ((SqlException)ex).Number == Constants.TimeoutErrorNumber;
        }
    }
}
