using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Esb.EwicAplListener.Common;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Esb.EwicAplListener.NewAplProcessors;
using Icon.Esb.EwicAplListener.StorageServices;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Text;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb.EwicAplListener
{
    public class EwicAplListener : ListenerApplication<EwicAplListener, ListenerApplicationSettings>
    {
        private IRenewableContext<IconContext> globalContext;
        private IMessageParser<AuthorizedProductListModel> messageParser;
        private IAplStorageService storageService;
        private INewAplProcessor newAplProcessor;

        public EwicAplListener(
            IRenewableContext<IconContext> globalContext,
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<EwicAplListener> logger,
            IMessageParser<AuthorizedProductListModel> messageParser,
            IAplStorageService storageService,
            INewAplProcessor newAplProcessor)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.globalContext = globalContext;
            this.messageParser = messageParser;
            this.storageService = storageService;
            this.newAplProcessor = newAplProcessor;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            AuthorizedProductListModel aplModel;

            try
            {
                aplModel = messageParser.ParseMessage(args.Message);
            }
            catch (Exception ex)
            {
                LogAndNotify(args, ex, Constants.UnsuccessfulParse);
                AcknowledgeMessage(args.Message);
                return;
            }

            try
            {
                storageService.Save(aplModel);
            }
            catch (Exception ex)
            {
                LogAndNotify(args, ex, Constants.UnsuccessfulSave);
                AcknowledgeMessage(args.Message);
                return;
            }

            try
            {
                ApplyAutomaticBusinessLogic(aplModel);
            }
            catch (Exception ex)
            {
                LogAndNotify(args, ex, Constants.UnsuccessfulProcessing);
                AcknowledgeMessage(args.Message);
                return;
            }

            AcknowledgeMessage(args.Message);

            globalContext.Refresh();

            logger.Info("Message processing complete.");
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

        private void ApplyAutomaticBusinessLogic(AuthorizedProductListModel aplModel)
        {
            newAplProcessor.ApplyMappings(aplModel);
            newAplProcessor.ApplyExclusions(aplModel);
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
