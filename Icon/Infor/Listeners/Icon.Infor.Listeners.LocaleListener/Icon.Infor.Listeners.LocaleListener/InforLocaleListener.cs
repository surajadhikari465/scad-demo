using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Icon.Infor.Listeners.LocaleListener.Models;
using Icon.Logging;
using System;
using TIBCO.EMS;

namespace Icon.Infor.Listeners.LocaleListener
{
    public class InforLocaleListener : ListenerApplication<InforLocaleListener, ListenerApplicationSettings>
    {
        private IMessageParser<LocaleModel> messageParser;
        private ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler;
        private ICommandHandler<ArchiveLocaleMessageCommand> archiveLocaleMessageCommandHandler;
        private ICommandHandler<GenerateLocaleMessagesCommand> generateLocaleMessagesCommandHandler;

        public InforLocaleListener(
            IMessageParser<LocaleModel> messageParser,
            ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler,
            ICommandHandler<GenerateLocaleMessagesCommand> generateLocaleMessagesCommandHandler,
            ICommandHandler<ArchiveLocaleMessageCommand> archiveLocaleMessageCommandHandler,
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<InforLocaleListener> logger)
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.addOrUpdateLocalesCommandHandler = addOrUpdateLocalesCommandHandler;
            this.generateLocaleMessagesCommandHandler = generateLocaleMessagesCommandHandler;
            this.archiveLocaleMessageCommandHandler = archiveLocaleMessageCommandHandler;
            this.listenerApplicationSettings = listenerApplicationSettings;
            this.esbConnectionSettings = esbConnectionSettings;
            this.subscriber = subscriber;
            this.emailClient = emailClient;
            this.logger = logger;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            try
            {
                var locale = messageParser.ParseMessage(args.Message);
                AddOrUpdateLocales(locale);
                GenerateLocaleMessages(locale);
                ArchiveLocaleMessage(args, locale);
            }
            catch (Exception ex)
            {
                LogAndNotifyError(ex);
            }

            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }

        private void AddOrUpdateLocales(LocaleModel locale)
        {
            try
            {
                addOrUpdateLocalesCommandHandler.Execute(new AddOrUpdateLocalesCommand
                {
                    Locale = locale
                });
            }
            catch (Exception ex)
            {
                locale.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                locale.ErrorDetails = ex.ToString();
                foreach (var chain in locale.Locales)
                {
                    chain.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                    chain.ErrorDetails = ex.ToString();
                    foreach (var region in chain.Locales)
                    {
                        region.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                        region.ErrorDetails = ex.ToString();
                        foreach (var metro in region.Locales)
                        {
                            metro.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                            metro.ErrorDetails = ex.ToString();
                            foreach (var store in metro.Locales)
                            {
                                store.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                                store.ErrorDetails = ex.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void GenerateLocaleMessages(LocaleModel locale)
        {
            try
            {
                generateLocaleMessagesCommandHandler.Execute(new GenerateLocaleMessagesCommand
                {
                    Locale = locale
                });
            }
            catch (Exception ex)
            {
                locale.ErrorCode = ApplicationErrors.Codes.GenerateLocaleMessageError;
                locale.ErrorDetails = ex.ToString();
                foreach (var chain in locale.Locales)
                {
                    chain.ErrorCode = ApplicationErrors.Codes.GenerateLocaleMessageError;
                    chain.ErrorDetails = ex.ToString();
                    foreach (var region in chain.Locales)
                    {
                        region.ErrorCode = ApplicationErrors.Codes.GenerateLocaleMessageError;
                        region.ErrorDetails = ex.ToString();
                        foreach (var metro in region.Locales)
                        {
                            metro.ErrorCode = ApplicationErrors.Codes.GenerateLocaleMessageError;
                            metro.ErrorDetails = ex.ToString();
                            foreach (var store in metro.Locales)
                            {
                                store.ErrorCode = ApplicationErrors.Codes.GenerateLocaleMessageError;
                                store.ErrorDetails = ex.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void ArchiveLocaleMessage(EsbMessageEventArgs args, LocaleModel locale)
        {
            archiveLocaleMessageCommandHandler.Execute(new ArchiveLocaleMessageCommand
            {
                Locale = locale,
                Message = args.Message
            });
        }
    }
}
