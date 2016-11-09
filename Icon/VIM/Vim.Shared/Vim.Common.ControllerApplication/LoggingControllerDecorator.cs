using System;
using Vim.Common.Email;
using Vim.Logging;

namespace Vim.Common.ControllerApplication
{
    public class LoggingControllerDecorator : IControllerApplication
    {
        private IControllerApplication controller;
        private IControllerApplicationSettings settings;
        private IEmailClient emailClient;
        private ILogger logger;

        public LoggingControllerDecorator(IControllerApplication controller, IControllerApplicationSettings settings, IEmailClient emailClient, ILogger logger)
        {
            this.controller = controller;
            this.settings = settings;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        public void Run()
        {
            try
            {
                logger.Info("Starting VIM " + settings.ControllerName + " controller.");
                this.controller.Run();
                logger.Info("Shutting down VIM " + settings.ControllerName + " controller.");
            }
            catch (Exception e)
            {
                logger.Error("An exception occurred.", e);
                emailClient.Send("An unexpected error occurred while processing VIM events. Error : " + e.ToString(), "VIM " + settings.ControllerName + " controller");
                throw;
            }
        }
    }
}