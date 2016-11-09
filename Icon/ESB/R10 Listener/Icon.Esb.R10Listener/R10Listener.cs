using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.R10Listener.Models;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System;
using System.Text;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb.R10Listener
{
    public class R10Listener : ListenerApplication<R10Listener, ListenerApplicationSettings>
    {
        private ICommandHandler<ProcessR10MessageResponseCommand> processR10MessageResponseCommandHandler;
        private IMessageParser<R10MessageResponseModel> messageParser;

        public R10Listener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            ICommandHandler<ProcessR10MessageResponseCommand> processR10MessageResponseCommandHandler,
            IMessageParser<R10MessageResponseModel> messageParser,
            IEmailClient emailClient,
            ILogger<R10Listener> logger)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.processR10MessageResponseCommandHandler = processR10MessageResponseCommandHandler;
            this.messageParser = messageParser;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            R10MessageResponseModel r10MessageResponse = null;

            try
            {
                r10MessageResponse = messageParser.ParseMessage(args.Message);
            }
            catch (Exception ex)
            {
                LogAndNotifyUnsuccessfulParse(args, ex);
            }

            if (r10MessageResponse != null)
            {
                try
                {
                    processR10MessageResponseCommandHandler.Execute(new ProcessR10MessageResponseCommand
                        {
                            MessageResponse = r10MessageResponse
                        });

                    if (!r10MessageResponse.RequestSuccess)
                    {
                        LogAndNotifyUnsuccessfulMessage(r10MessageResponse);
                    }
                }
                catch (Exception ex)
                {
                    LogAndNotifyUnsuccessfulMessageProcessing(r10MessageResponse, ex);
                }
            }

            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }

        private void LogAndNotifyUnsuccessfulMessage(R10MessageResponseModel messageResponse)
        {
            StringBuilder builder = new StringBuilder();
            string emailTable = new XElement("table",
                new XAttribute("style", "border-spacing:2px;border: 1px solid #bbb;border-collapse:collapse;table-layout:fixed;width:1200px;word-break:break-word;"),
                new XElement("colgroup",
                    new XElement("col",
                        new XAttribute("style", "width:200px")),
                    new XElement("col",
                        new XAttribute("style", "width:1050px;"))),
                new XElement("tbody",
                    new XElement("tr",
                        new XElement("th",
                            new XAttribute("background-color", "#ededed"),
                            new XAttribute("width", "150"),
                            "Message Property"),
                        new XElement("th",
                            new XAttribute("background-color", "#ededed"),
                            "Value")),
                    new XElement("tr",
                        new XElement("td", "Message History Id",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                             messageResponse.MessageHistoryId)),
                    new XElement("tr",
                        new XElement("td", "Request Success",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", messageResponse.RequestSuccess,
                            new XAttribute("style", "border:1px solid #bbb;"))),
                    new XElement("tr",
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                             "System Error"),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                             messageResponse.SystemError)),
                    new XElement("tr",
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                             "Failure Reason Code"),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                             messageResponse.FailureReasonCode)),
                    new XElement("tr",
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                             "Message"),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                            messageResponse.ResponseText))
                )
            ).ToString();

            builder.Append("<h2>").Append(NotificationConstants.UnsuccessfulRequest).Append("</h2>")
                .Append(emailTable);

            logger.Error(String.Format("{0} MessageHistoryId: {1}, SystemError: {2}, FailureReasonCode: {3}, Message: {4}",
                NotificationConstants.UnsuccessfulRequest,
                messageResponse.MessageHistoryId,
                messageResponse.SystemError,
                messageResponse.FailureReasonCode,
                messageResponse.ResponseText));
            emailClient.Send(builder.ToString(), listenerApplicationSettings.EmailSubjectError);
        }

        private void LogAndNotifyUnsuccessfulParse(EsbMessageEventArgs args, Exception ex)
        {
            StringBuilder builder = new StringBuilder();
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
                            "Exception details"),
                        new XElement("td",
                            new XAttribute("style", "border:1px solid #bbb;"),
                            ex.ToString()))
                    )
                ).ToString();

            builder.Append("<h2>").Append(NotificationConstants.UnsuccessfulParse).Append("</h2>")
                .Append(emailTable);

            logger.Error(String.Format("{0} Message: {1}, Exception: {2}",
                NotificationConstants.UnsuccessfulParse,
                args.Message,
                ex.ToString()));
            emailClient.Send(builder.ToString(), listenerApplicationSettings.EmailSubjectError);
        }

        private void LogAndNotifyUnsuccessfulMessageProcessing(R10MessageResponseModel messageResponse, Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            string emailTable = new XElement("table",
                new XAttribute("style", "border-spacing:2px;border: 1px solid #bbb;border-collapse:collapse;table-layout:fixed;width:1200px;word-break:break-word;"),
                new XElement("colgroup",
                    new XElement("col",
                        new XAttribute("style", "width:200px")),
                    new XElement("col",
                        new XAttribute("style", "width:1050px;"))),
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
                        new XElement("td", "Message History Id",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", messageResponse.MessageHistoryId,
                            new XAttribute("style", "border:1px solid #bbb;"))),
                    new XElement("tr",
                        new XElement("td", "Request Success",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", messageResponse.RequestSuccess,
                            new XAttribute("style", "border:1px solid #bbb;"))),                         
                    new XElement("tr",
                        new XElement("td", "System Error",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", messageResponse.SystemError,
                            new XAttribute("style", "border:1px solid #bbb;"))),
                    new XElement("tr",
                        new XElement("td", "Failure Reason Code",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", messageResponse.FailureReasonCode,
                            new XAttribute("style", "border:1px solid #bbb;"))),
                    new XElement("tr",
                        new XElement("td", "Message",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", messageResponse.ResponseText,
                            new XAttribute("style", "border:1px solid #bbb;"))),
                    new XElement("tr",
                        new XElement("td", "Exception details",
                            new XAttribute("style", "border:1px solid #bbb;")),
                        new XElement("td", ex.ToString(),
                            new XAttribute("style", "border:1px solid #bbb;")))
                )
            ).ToString();

            builder.Append("<h2>").Append(NotificationConstants.UnsuccessfulMessageProcessing).Append("</h2>")
                .Append(emailTable);

            logger.Error(String.Format("{0} MessageHistoryId: {1}, SystemError: {2}, FailureReasonCode: {3}, Message: {4}, Exception: {5}",
                NotificationConstants.UnsuccessfulMessageProcessing,
                messageResponse.MessageHistoryId,
                messageResponse.SystemError,
                messageResponse.FailureReasonCode,
                messageResponse.ResponseText,
                ex.ToString()));
            emailClient.Send(builder.ToString(), listenerApplicationSettings.EmailSubjectError);
        }
    }
}
