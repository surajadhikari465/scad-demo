using Icon.Common;
using Icon.Common.Email;
using Icon.Logging;
using InterfaceController.Common;
using PushController.Common;
using PushController.Controller.ProcessorModules;
using System;
using System.Reflection;

namespace PushController.Controller.Processors
{
    public class IconPosProcessor : IIconPosProcessor
    {
        private ILogger<IconPosProcessor> logger;
        private IEmailClient emailClient;
        private IIconPosDataProcessingModule processPosDataForEsbModule;
        private IIconPosDataProcessingModule processPosDataForUdmModule;

        public IconPosProcessor(
            ILogger<IconPosProcessor> logger,
            IEmailClient emailClient,
            IIconPosDataProcessingModule processPosDataForEsbModule,
            IIconPosDataProcessingModule processPosDataForUdmModule)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.processPosDataForEsbModule = processPosDataForEsbModule;
            this.processPosDataForUdmModule = processPosDataForUdmModule;
        }

        public void ProcessPosDataForEsb()
        {
            try
            {
                processPosDataForEsbModule.Execute();
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<IconPosProcessor>(this.logger);

                exceptionHandler.HandleException("An unhandled exception occurred in the ESB module.", ex, this.GetType(), MethodBase.GetCurrentMethod());

                string errorMessage = String.Format(Resource.EsbUnhandledExceptionMessage, StartupOptions.Instance.ToString());
                string emailSubject = Resource.EsbUnhandledExceptionEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForUnhandledException(errorMessage, ex.ToString());

                try
                {
                    if (EmailAlertApprover.ShouldSendEmailAlert(ex, AppSettingsAccessor.GetIntSetting("EmailAlertFrequencyMinutes", true)))
                    {
                        emailClient.Send(emailBody, emailSubject);
                    }
                }
                catch (Exception mailEx)
                {
                    string message = "A failure occurred while attempting to send the alert email.";
                    exceptionHandler.HandleException(message, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                }
            }
        }

        public void ProcessPosDataForUdm()
        {
            try
            {
                processPosDataForUdmModule.Execute();
            }
            catch (Exception ex)
            {
                var exceptionHandler = new ExceptionHandler<IconPosProcessor>(this.logger);
                
                exceptionHandler.HandleException("An unhandled exception occurred in the UDM module.", ex, this.GetType(), MethodBase.GetCurrentMethod());

                string errorMessage = String.Format(Resource.UdmUnhandledExceptionMessage, StartupOptions.Instance.ToString());
                string emailSubject = Resource.UdmUnhandledExceptionEmailSubject;
                string emailBody = EmailHelper.BuildMessageBodyForUnhandledException(errorMessage, ex.ToString());

                try
                {
                    if (EmailAlertApprover.ShouldSendEmailAlert(ex, AppSettingsAccessor.GetIntSetting("EmailAlertFrequencyMinutes", true)))
                    {
                        emailClient.Send(emailBody, emailSubject);
                    }
                }
                catch (Exception mailEx)
                {
                    string message = "A failure occurred while attempting to send the alert email.";
                    exceptionHandler.HandleException(message, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                }
            }
        }
    }
}
