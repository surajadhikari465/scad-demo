using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Esb.EwicAplListener.NewAplProcessors;
using Icon.Esb.EwicAplListener.StorageServices;
using Icon.Esb.EwicAplListener.Transactions;
using Icon.Esb.Factory;
using Icon.Esb.Subscriber;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;

namespace Icon.Esb.EwicAplListener
{
    public class EwicAplListenerBuilder
    {
        private static IRenewableContext<IconContext> globalContext;

        public static EwicAplListener Build()
        {
            globalContext = new GlobalIconContext(new IconContext());

            var applicationSettings = EwicAplListenerApplicationSettings.CreateDefaultSettings<EwicAplListenerApplicationSettings>("eWIC APL Listener");
            var connectionSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("listener");
            var messageParser = new AplMessageParser(new NLogLogger<AplMessageParser>());
            var storageService = BuildStorageService();
            var newAplProcessor = BuildNewAplProcessor();

            var listener = new EwicAplListener(
                globalContext,
                applicationSettings,
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                EmailClient.CreateFromConfig(),
                new NLogLogger<EwicAplListener>(),
                messageParser,
                storageService,
                newAplProcessor);

            return listener;
        }

        private static IconDbStorageService BuildStorageService()
        {
            var logger = new NLogLogger<IconDbStorageService>();
            var addMessageHistoryCommand = new AddMessageHistoryCommand(globalContext);
            var agencyExistsQuery = new AgencyExistsQuery(globalContext);
            var addAgencyCommand = new AddAgencyCommand(globalContext);
            var updateAplCommand = new AddOrUpdateAuthorizedProductListCommand(globalContext);

            var storageService = new IconDbStorageService(
                logger,
                addMessageHistoryCommand,
                agencyExistsQuery,
                addAgencyCommand,
                updateAplCommand);

            return storageService;
        }

        private static AutoMappingAndExclusionProcessor BuildNewAplProcessor()
        {
            var logger = new NLogLogger<AutoMappingAndExclusionProcessor>();
            var exclusionGenerator = BuildExclusionGenerator();
            var mappingGenerator = BuildMappingGenerator();

            var processor = new AutoMappingAndExclusionProcessor(
                logger,
                exclusionGenerator,
                mappingGenerator);

            return processor;
        }

        private static ExclusionGeneratorTransaction BuildExclusionGenerator()
        {
            var exclusionGenerator = new ExclusionGenerator(
                new NLogLogger<ExclusionGenerator>(),
                new EwicExclusionSerializer(),
                new GetExclusionQuery(globalContext),
                new AddExclusionCommand(globalContext),
                new SaveToMessageHistoryCommand(globalContext),
                new UpdateMessageHistoryMessageCommand(globalContext),
                new EwicMessageProducer(new EsbConnectionFactory()));

            return new ExclusionGeneratorTransaction(exclusionGenerator, globalContext);
        }

        private static MappingGeneratorTransaction BuildMappingGenerator()
        {
            var mappingGenerator = new MappingGenerator(
                new NLogLogger<MappingGenerator>(),
                new EwicMappingSerializer(),
                new GetExistingMappingsQuery(globalContext),
                new AddMappingsCommand(globalContext),
                new SaveToMessageHistoryCommand(globalContext),
                new UpdateMessageHistoryMessageCommand(globalContext),
                new EwicMessageProducer(new EsbConnectionFactory()));

            return new MappingGeneratorTransaction(mappingGenerator, globalContext);
        }
    }
}
