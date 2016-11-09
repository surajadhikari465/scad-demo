using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Esb.EwicErrorResponseListener.Common;
using Icon.Esb.EwicErrorResponseListener.Common.Models;
using Icon.Esb.EwicErrorResponseListener.DataAccess.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Text;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb.EwicErrorResponseListener
{
    public class EwicErrorResponseListener : ListenerApplication<EwicErrorResponseListener, ListenerApplicationSettings>
    {
        private IRenewableContext<IconContext> globalContext;
        private IMessageParser<EwicErrorResponseModel> messageParser;
        private ICommandHandler<SaveToMessageResponseParameters> saveToMessageResponseCommand;
        private ICommandHandler<UpdateMessageHistoryStatusParameters> updateMessageHistoryStatusCommand;

        public EwicErrorResponseListener(
            IRenewableContext<IconContext> globalContext,
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<EwicErrorResponseListener> logger,
            IMessageParser<EwicErrorResponseModel> messageParser,
            ICommandHandler<SaveToMessageResponseParameters> saveToMessageResponseCommand,
            ICommandHandler<UpdateMessageHistoryStatusParameters> updateMessageHistoryStatusCommand)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.globalContext = globalContext;
            this.messageParser = messageParser;
            this.saveToMessageResponseCommand = saveToMessageResponseCommand;
            this.updateMessageHistoryStatusCommand = updateMessageHistoryStatusCommand;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            EwicErrorResponseModel errorResponse;

            try
            {
                errorResponse = ParseMessage(args);
            }
            catch (Exception ex)
            {
                LogAndNotify(args, ex, Constants.UnsuccessfulParse);
                AcknowledgeMessage(args.Message);
                return;
            }

            try
            {
                SaveResponse(errorResponse);
            }
            catch (Exception ex)
            {
                LogAndNotify(args, ex, Constants.UnsuccessfulSave);
                AcknowledgeMessage(args.Message);
                return;
            }

            try
            {
                if (SetMessageStatusToFailed(errorResponse))
                {
                    UpdateMessageStatus(errorResponse.MessageHistoryId);
                }
            }
            catch (Exception ex)
            {
                LogAndNotify(args, ex, Constants.UnsuccessfulProcessing);
                AcknowledgeMessage(args.Message);
                return;
            }

            if (ResponseRequiresNotification(errorResponse))
            {
                LogAndNotify(args, Constants.ErrorResponseReceived, errorResponse.MessageHistoryId, notify: true);
            }
            else
            {
                LogAndNotify(args, Constants.ErrorResponseReceived, errorResponse.MessageHistoryId, notify: false);
            }

            AcknowledgeMessage(args.Message);

            globalContext.Refresh();
        }

        private bool ResponseRequiresNotification(EwicErrorResponseModel errorResponse)
        {
            if (errorResponse.ResponseReason == Constants.ResponseReasonNoSequenceNumber)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool SetMessageStatusToFailed(EwicErrorResponseModel errorResponse)
        {
            if (errorResponse.ResponseReason == Constants.ResponseReasonNoSequenceNumber)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateMessageStatus(int messageHistoryId)
        {
            var parameters = new UpdateMessageHistoryStatusParameters { MessageHistoryId = messageHistoryId, MessageStatusId = MessageStatusTypes.Failed };
            updateMessageHistoryStatusCommand.Execute(parameters);
        }

        private void SaveResponse(EwicErrorResponseModel errorResponse)
        {
            var parameters = new SaveToMessageResponseParameters { ErrorResponse = errorResponse };
            saveToMessageResponseCommand.Execute(parameters);
        }

        private EwicErrorResponseModel ParseMessage(EsbMessageEventArgs args)
        {
            return messageParser.ParseMessage(args.Message);
        }

        private void AcknowledgeMessage(IEsbMessage message)
        {
            if (esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge)
            {
                message.Acknowledge();
            }
        }

        private void LogAndNotify(EsbMessageEventArgs args, Exception ex, string errorMessage)
        {
            StringBuilder builder = new StringBuilder();

            string emailTable = BuildEmailTable(args, ex.ToString());

            builder.Append("<h3>").Append(errorMessage).Append("</h3>").Append(emailTable);

            logger.Error(String.Format("{0} Message: {1}, Exception: {2}",
                errorMessage,
                args.Message,
                ex.ToString()));

            emailClient.Send(builder.ToString(), listenerApplicationSettings.EmailSubjectError);
        }

        private void LogAndNotify(EsbMessageEventArgs args, string errorMessage, int messageHistoryId, bool notify = true)
        {
            StringBuilder builder = new StringBuilder();

            string emailTable = BuildEmailTable(args, errorMessage);

            builder.Append("<h3>").Append(Constants.AlertNotification).Append("</h3>").Append(emailTable);

            logger.Error(String.Format("R10 was unable to successfully process eWIC message {0}.  The message will be marked as Failed.", messageHistoryId));

            if (notify)
            {
                emailClient.Send(builder.ToString(), listenerApplicationSettings.EmailSubjectError);
            }
        }

        private string BuildEmailTable(EsbMessageEventArgs args, string errorMessage)
        {
            string emailTable = new XElement("table",
                new XAttribute("style", "border-spacing:2px;border: 1px solid #bbb;border-collapse:collapse;table-layout:fixed;width:1200px;word-break:break-word;"),
                new XElement("colgroup",
                    new XElement("col",
                        new XAttribute("style", "width:200px")),
                    new XElement("col",
                        new XAttribute("style", "width:1050px;"))),
                new XAttribute("width", "1000"),
                new XElement("tbody",
                    new XElement("tr",
                        new XElement("th",
                            new XAttribute("background-color", "#ededed"),
                            new XAttribute("style", "background-color:#ededed;border:1px solid #bbb;"),
                            "Property"),
                        new XElement("th",
                            new XAttribute("style", "background-color:#ededed;border:1px solid #bbb;"),
                            "Value")),
                    new XElement("tr",
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                            "Message"),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                            args.Message)),
                    new XElement("tr",
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                            "Exception"),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                            errorMessage))
                    )
                ).ToString();

            return emailTable;
        }
    }
}
