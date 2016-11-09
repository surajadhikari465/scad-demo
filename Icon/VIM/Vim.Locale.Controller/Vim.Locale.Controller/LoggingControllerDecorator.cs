using System;
using Vim.Common.ControllerApplication;
using Vim.Common.Email;
using Vim.Logging;

namespace Vim.Locale.Controller
{
    public class LoggingControllerDecorator : IControllerApplication
    {
        private IControllerApplication controller;
        private IEmailClient emailClient;
        private ILogger logger;

        public LoggingControllerDecorator(IControllerApplication controller, IEmailClient emailClient, ILogger logger)
        {
            this.controller = controller;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        public void Run()
        {
            try
            {
                logger.Info("Starting VIM Locale controller.");
                this.controller.Run();
                logger.Info("Successfully shutting down VIM Locale controller.");
            }
            catch (Exception e)
            {
                logger.Error("An exception occurred.", e);
                emailClient.Send("An unexpected error occurred while processing VIM Locale events. Error : " + e.ToString(), "VIM Locale Controller");
                throw;
            }
        }
    }
}

