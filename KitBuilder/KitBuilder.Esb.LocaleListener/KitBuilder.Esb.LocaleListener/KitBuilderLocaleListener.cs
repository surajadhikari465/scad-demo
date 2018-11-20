using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using KitBuilder.Esb.LocaleListener.Commands;
using KitBuilder.Esb.LocaleListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TIBCO.EMS;

namespace KitBuilder.Esb.LocaleListener
{
    public class KitBuilderLocaleListener : ListenerApplication<KitBuilderLocaleListener, ListenerApplicationSettings>
    {
        private IMessageParser<List<LocaleModel>> messageParser;
        private ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler;

        public KitBuilderLocaleListener(ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber, 
            IEmailClient emailClient,
            ILogger<KitBuilderLocaleListener> logger,
            IMessageParser<List<LocaleModel>> messageParser,
            ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler)
            :base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.addOrUpdateLocalesCommandHandler = addOrUpdateLocalesCommandHandler;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            List<LocaleModel> locales = null;
            try
            {
                locales = messageParser.ParseMessage(args.Message);
            }
            catch (Exception ex)
            {
                LogAndNotifyError(ex);
            }

            if (locales != null && locales.Any())
            {
                try
                {

                    addOrUpdateLocalesCommandHandler.Execute(new AddOrUpdateLocalesCommand
                        {
                            Locales = locales
                        });
                }
                catch (Exception ex)
                {
                    LogAndNotifyError(ex);
                }
            }

            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }
    }
}
