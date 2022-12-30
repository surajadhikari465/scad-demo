using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Esb.EwicAplListener.Common;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Esb.EwicAplListener.NewAplProcessors;
using Icon.Esb.EwicAplListener.StorageServices;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Text;
using System.Xml.Linq;
using Icon.Esb.ListenerApplication;

namespace Icon.Esb.EwicAplListener
{
    public class EwicAplListener : ListenerApplication<EwicAplListener>
    {
        private IRenewableContext<IconContext> globalContext;
        private IMessageParser<AuthorizedProductListModel> messageParser;
        private IAplStorageService storageService;
        private INewAplProcessor newAplProcessor;
        private IEmailClient emailClient;
        private ListenerApplicationSettings listenerApplicationSettings;

        public EwicAplListener(
            IRenewableContext<IconContext> globalContext,
            ListenerApplicationSettings listenerApplicationSettings,
            DvsListenerSettings listenerSettings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<EwicAplListener> logger,
            IMessageParser<AuthorizedProductListModel> messageParser,
            IAplStorageService storageService,
            INewAplProcessor newAplProcessor)
            : base(listenerSettings, subscriber, emailClient, logger)
        {
            this.globalContext = globalContext;
            this.messageParser = messageParser;
            this.storageService = storageService;
            this.newAplProcessor = newAplProcessor;
            this.emailClient = emailClient;
            this.listenerApplicationSettings = listenerApplicationSettings;
        }

        public override void HandleMessage(DvsMessage message)
        {
            AuthorizedProductListModel aplModel;
            aplModel = messageParser.ParseMessage(message);

            try
            {
                storageService.Save(aplModel);
            }
            catch (Exception ex)
            {
                LogAndNotify(message, ex, Constants.UnsuccessfulSave);
                return;
            }

            try
            {
                ApplyAutomaticBusinessLogic(aplModel);
            }
            catch (Exception ex)
            {
                LogAndNotify(message, ex, Constants.UnsuccessfulProcessing);
                return;
            }

            globalContext.Refresh();

            logger.Info("Message processing complete.");
        }

        private void ApplyAutomaticBusinessLogic(AuthorizedProductListModel aplModel)
        {
            newAplProcessor.ApplyMappings(aplModel);
            newAplProcessor.ApplyExclusions(aplModel);
        }

        private void LogAndNotify(DvsMessage message, Exception ex, string errorMessage)
        {
            StringBuilder builder = new StringBuilder();

            string emailTable = BuildEmailTable(message, ex.ToString());

            builder.Append("<h3>").Append(errorMessage).Append("</h3>").Append(emailTable);

            logger.Error(String.Format("{0} Message: {1}, Exception: {2}",
                errorMessage,
                message,
                ex.ToString()));

            emailClient.Send(builder.ToString(), listenerApplicationSettings.EmailSubjectError);
        }

        private string BuildEmailTable(DvsMessage message, string errorMessage)
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
                            message)),
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
