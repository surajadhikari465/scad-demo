using System;
using System.Collections.Generic;
using ErrorMessagesMonitor.DataAccess;
using ErrorMessagesMonitor.Model;
using ErrorMessagesMonitor.Serializer;
using ErrorMessagesMonitor.Settings;
using Icon.Common.Email;
using Newtonsoft.Json;
using OpsgenieAlert;

namespace ErrorMessagesMonitor.Message.Processor
{
    internal class ErrorMessagesProcessor : IErrorMessagesProcessor
    {
        private readonly ErrorMessagesMonitorServiceSettings serviceSettings;
        private readonly IErrorMessagesMonitorDAL errorMessagesMonitorDAL;
        private readonly ISerializer<ErrorDetailsCanonicalModel> serializer;
        private readonly IEmailClient emailClient;
        private readonly IOpsgenieAlert opsGenieAlert;

        public ErrorMessagesProcessor(
            ErrorMessagesMonitorServiceSettings serviceSettings,
            IErrorMessagesMonitorDAL errorMessagesMonitorDAL,
            ISerializer<ErrorDetailsCanonicalModel> serializer,
            IEmailClient emailClient,
            IOpsgenieAlert opsGenieAlert
        )
        {
            this.serviceSettings = serviceSettings;
            this.errorMessagesMonitorDAL = errorMessagesMonitorDAL;
            this.serializer = serializer;
            this.emailClient = emailClient;
            this.opsGenieAlert = opsGenieAlert;
        }

        public void Process()
        {
            try
            {
                string instanceID = Guid.NewGuid().ToString();
                errorMessagesMonitorDAL.MarkErrorMessageRecordsAsInProcess(instanceID);
                IList<ErrorMessageModel> errorMessageList = errorMessagesMonitorDAL.GetErrorMessages(instanceID);
                try
                {
                    foreach (var errorMessage in errorMessageList)
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
                }
                catch (Exception errorNotificationException)
                {
                    // TODO - Log Error for Send Error Notifications - logger.error($"ErrorMessage: {errorNotificationException}");
                }

                errorMessagesMonitorDAL.MarkErrorMessagesAsProcessed(instanceID);
            }
            catch (Exception processErrorMessagesException)
            {
                // TODO - Log Error for Process Error Messages - logger.error($"ErrorMessage: {processErrorMessagesException}");
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
