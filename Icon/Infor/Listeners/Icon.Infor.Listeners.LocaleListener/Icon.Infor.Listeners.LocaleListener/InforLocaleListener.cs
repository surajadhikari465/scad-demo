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
using System.Collections.Generic;
using System.Linq;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.LocaleListener.Queries;

namespace Icon.Infor.Listeners.LocaleListener
{
    public class InforLocaleListener : ListenerApplication<InforLocaleListener, ListenerApplicationSettings>
    {
        private IMessageParser<LocaleModel> messageParser;
        private ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler;
        private IQueryHandler<GetSequenceIdFromLocaleIdParameters, int> getSequenceIdFromLocaleIdQueryHandler;
        private IQueryHandler<GetSequenceIdFromBusinessUnitIdParameters, int> getSequenceIdFromBusinessUnitIdQueryHandler;
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
            ILogger<InforLocaleListener> logger,
            IQueryHandler<GetSequenceIdFromLocaleIdParameters, int> getSequenceIdFromLocaleIdQueryHandler,
             IQueryHandler<GetSequenceIdFromBusinessUnitIdParameters, int> getSequenceIdFromBusinessUnitIdQueryHandler
            )
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
            this.getSequenceIdFromLocaleIdQueryHandler = getSequenceIdFromLocaleIdQueryHandler;
            this.getSequenceIdFromBusinessUnitIdQueryHandler = getSequenceIdFromBusinessUnitIdQueryHandler;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            try
            {
                var locale = messageParser.ParseMessage(args.Message);
                var organization = locale;
                var chains = organization.Locales;
                var regions = chains.SelectMany(p => p.Locales).Where(c => c.Action == ActionEnum.AddOrUpdate);
                var metros = regions.SelectMany(t => t.Locales).Where(c => c.Action == ActionEnum.AddOrUpdate);
                var stores = metros.SelectMany(s => s.Locales).Where(c => c.Action == ActionEnum.AddOrUpdate);

                ValidateLocalesforSequenceId(chains, false);
                ValidateLocalesforSequenceId(regions, false);
                ValidateLocalesforSequenceId(metros, false);
                ValidateLocalesforSequenceId(stores, true);

                AddOrUpdateLocales( chains.Where(c=>c.ErrorCode==null), 
                                    regions.Where(r => r.ErrorCode == null),
                                    metros.Where(m => m.ErrorCode == null), 
                                    stores.Where(s => s.ErrorCode == null)
                                   );
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

        private void ValidateLocalesforSequenceId(IEnumerable<LocaleModel> locales, bool isStore)
        {
            int sequenceId;
            foreach (LocaleModel locale in locales)
            {
                if(isStore)
                {
                    sequenceId = GetSequenceIdFromBusinessUnitId(locale.BusinessUnitId);
                }
                else
                {
                    sequenceId = GetSequenceIdFromLocaleId(locale.LocaleId);
                }
 
                if (sequenceId >= locale.SequenceId)
                {
                    locale.ErrorCode = ApplicationErrors.Codes.SequenceIdError;
                    locale.ErrorDetails = "Invalid Sequence Id. A higher SequenceId already exists for this localeId.";
                }
            }
        }

        private int GetSequenceIdFromLocaleId(int currentLocaleId)
        {
            return getSequenceIdFromLocaleIdQueryHandler.Search(new GetSequenceIdFromLocaleIdParameters
            {
                localeId = currentLocaleId
            });
        }

        private int GetSequenceIdFromBusinessUnitId(int currentBusinessUnitId)
        {
            return getSequenceIdFromBusinessUnitIdQueryHandler.Search(new GetSequenceIdFromBusinessUnitIdParameters
            {
                businessUnitId = currentBusinessUnitId
            });
        }

        private void AddOrUpdateLocales(IEnumerable<LocaleModel> chains, IEnumerable<LocaleModel> regions, IEnumerable<LocaleModel> metros, IEnumerable<LocaleModel> stores)
        {
            try
            {
                addOrUpdateLocalesCommandHandler.Execute(new AddOrUpdateLocalesCommand
                {
                    chains = chains,
                    regions = regions,
                    metros = metros,
                    stores = stores
                });
            }
            catch (Exception ex)
            {
                foreach (var chain in chains)
                {
                    chain.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                    chain.ErrorDetails = ex.ToString();
                }
                foreach (var region in regions)
                {
                    region.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                    region.ErrorDetails = ex.ToString();
                }
                foreach (var metro in metros)
                {
                    metro.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                    metro.ErrorDetails = ex.ToString();
                }
                foreach (var store in stores)
                {
                    store.ErrorCode = ApplicationErrors.Codes.AddOrUpdateLocaleError;
                    store.ErrorDetails = ex.ToString();
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