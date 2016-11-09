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
    public class IrmaPosProcessor : IIrmaPosProcessor
    {
        private ILogger<IrmaPosProcessor> logger;
        private IEmailClient emailClient;
        private IIrmaPosDataProcessingModule stagePosDataModule;

        public IrmaPosProcessor(
            ILogger<IrmaPosProcessor> logger,
            IEmailClient emailClient,
            IIrmaPosDataProcessingModule stagePosDataModule)
        {
            this.logger = logger;
            this.emailClient = emailClient;
            this.stagePosDataModule = stagePosDataModule;
        }

        public void StageIrmaPosData()
        {
            try
            {
                stagePosDataModule.Execute();
            }
            catch (Exception ex)
            {
                string failedRegion = stagePosDataModule.CurrentRegion;

                var exceptionHandler = new ExceptionHandler<IrmaPosProcessor>(this.logger);
                exceptionHandler.HandleException(String.Format("{0} - An unhandled exception occurred in the Staging module.", failedRegion), ex, this.GetType(), MethodBase.GetCurrentMethod());

                string errorMessage = String.Format(Resource.StagingUnhandledExceptionMessage, failedRegion, StartupOptions.Instance.ToString());
                string emailSubject = Resource.StagingUnhandledExceptionEmailSubject;
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
                    string message = String.Format("{0} - A failure occurred while attempting to send the alert email.", failedRegion);
                    exceptionHandler.HandleException(message, mailEx, this.GetType(), MethodBase.GetCurrentMethod());
                }
            }
        }
    }
}
