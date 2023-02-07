using System;
using System.Collections.Generic;
using ErrorMessagesMonitor.DataAccess;
using ErrorMessagesMonitor.Model;
using ErrorMessagesMonitor.Settings;
using Icon.Common.Email;
using Icon.Logging;
using Newtonsoft.Json;
using OpsgenieAlert;

namespace ErrorMessagesMonitor.Message.Processor
{
    internal class ErrorMessagesProcessor : IErrorMessagesProcessor
    {
        private readonly ErrorMessagesMonitorServiceSettings serviceSettings;
        private readonly IErrorMessagesMonitorDAL errorMessagesMonitorDAL;
        private readonly ILogger<ErrorMessagesProcessor> logger;
        private readonly IEmailClient emailClient;
        private readonly IOpsgenieAlert opsGenieAlert;

        public ErrorMessagesProcessor(
            ErrorMessagesMonitorServiceSettings serviceSettings,
            IErrorMessagesMonitorDAL errorMessagesMonitorDAL,
            ILogger<ErrorMessagesProcessor> logger,
            IEmailClient emailClient,
            IOpsgenieAlert opsGenieAlert
        )
        {
            this.serviceSettings = serviceSettings;
            this.errorMessagesMonitorDAL = errorMessagesMonitorDAL;
            this.logger = logger;
            this.emailClient = emailClient;
            this.opsGenieAlert = opsGenieAlert;
        }

        public void Process()
        {
            try
            {
                logger.Info("Starting Error Messages Monitor Processor");
                string instanceID = Guid.NewGuid().ToString();
                errorMessagesMonitorDAL.MarkErrorMessageRecordsAsInProcess(instanceID);
                IList<ErrorMessageModel> errorMessageList = errorMessagesMonitorDAL.GetErrorMessages(instanceID);
                logger.Info("Starting sending error notification process");
                foreach (var errorMessage in errorMessageList)
                {
                    try
                    {
                        IList<ErrorDetailsModel> errorDetailsList =
                            errorMessagesMonitorDAL.GetErrorDetails(instanceID, errorMessage);

                        ErrorDetailsCanonicalModel errorDetailsCanonicalModel = new ErrorDetailsCanonicalModel
                        {
                            ErrorDetailsList = errorDetailsList,
                            Application = errorMessage.Application,
                            ErrorCode = errorMessage.ErrorCode,
                            ErrorSeverity = errorMessage.ErrorSeverity
                        };

                        string errorDetailsCanonicalHtml = CanonicalToHtml(errorDetailsCanonicalModel);
                        string mailSubject = String.Format(
                            "Application: {0}, Error Code: {1} Error Severity: {2}",
                            errorDetailsCanonicalModel.Application,
                            errorDetailsCanonicalModel.ErrorCode,
                            errorDetailsCanonicalModel.ErrorSeverity
                            );

                        emailClient.Send(errorDetailsCanonicalHtml, mailSubject);

                        if (errorDetailsCanonicalModel.ErrorSeverity == "Fatal" && serviceSettings.SendErrorAlerts == "true")
                        {
                            ContentModel content = MapContent(errorMessage);
                            string contentJsonString = JsonConvert.SerializeObject(content);

                            opsGenieAlert.CreateOpsgenieAlert(
                                serviceSettings.ErrorAlertIntegrationKey,
                                serviceSettings.ErrorAlertUri,
                                contentJsonString,
                                "Send Opsgenie Alert for Fatal Error"
                            );
                        }
                    }
                    catch (Exception sendErrorNotificationException)
                    {
                        logger.Error($"Error occurred while sending error notification. Error: {sendErrorNotificationException}");
                    }
                }
                errorMessagesMonitorDAL.MarkErrorMessagesAsProcessed(instanceID);
                logger.Info("Ending Error Messages Monitor Processor successfully");
            }
            catch (Exception processErrorMessagesException)
            {
                logger.Error($"Error occurred in Error Messages Monitor Processor. Error: {processErrorMessagesException}");
            }
        }

        private string CanonicalToHtml(ErrorDetailsCanonicalModel errorDetailsCanonicalModel)
        {
            string errorDetailsAsRows = "";
            foreach(var errorDetail in errorDetailsCanonicalModel.ErrorDetailsList)
            {
                errorDetailsAsRows +=
                    $@"
                    <tr>
                        <td>{errorDetail.MessageID}</td>
                        <td>{errorDetail.ErrorDetails}</td>
                    </tr>";
            }

            string errorDetailsCanonicalHtml =
                $@"
                <html>
                	<body>
                		<p>The following 
                            <b>{errorDetailsCanonicalModel.ErrorCode}</b> errors occurred in the <b>{errorDetailsCanonicalModel.Application}</b>.</p>
                        <table border=""1"">
                            <tr>
                                <th><b>MessageID</b></th>               
                                <th><b>ErrorDetails</b></th>
                            </tr>
                        {errorDetailsAsRows}                
                        </table>
                    </body>
                </html>";

            return errorDetailsCanonicalHtml;
        }

        private ContentModel MapContent(ErrorMessageModel errorMessage)
        {
            return new ContentModel
            {
                Message = string.Format("{0}:Issue", errorMessage.Application),
                Description = string.Format(
                    "{0} {1} {2} {3}", 
                    errorMessage.Application, 
                    errorMessage.NumberOfErrors.ToString(), 
                    errorMessage.ErrorSeverity, 
                    errorMessage.ErrorCode
                    )
            };
        }
    }
}
