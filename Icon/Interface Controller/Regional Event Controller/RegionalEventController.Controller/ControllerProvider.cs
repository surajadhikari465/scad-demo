using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Irma.Framework;
using RegionalEventController.Common;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.Controller.Processors;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Queries;
using RegionalEventController.DataAccess.Infrastructure;
using RegionalEventController.Controller.UpdateServices;
using RegionalEventController.Controller.Email;

namespace RegionalEventController.Controller
{
    public static class ControllerProvider
    {
        private static IconContext iconContext = new IconContext();
        private static IrmaContext irmaContext;

        private static EmailClient emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
        private static string instance = StartupOptions.Instance.ToString();
        public static PrepareControllerBase PrepareRegionalController()
        {
            var controllerLogger = new NLogLoggerInstance<PrepareControllerBase>(instance);

            var iconReferenceProcessor = BuildIconReferenceProcessor();

            return new PrepareControllerBase(iconReferenceProcessor);
        }

        public static RegionalControllerBase ComposeRegionalController(string region)
        {
            irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(region));

            var newItemProcessor = BuildNewItemProcessor(region);

            return new RegionalControllerBase(newItemProcessor);
        }

        private static INewItemProcessor BuildIconReferenceProcessor()
        {
            var logger = new NLogLoggerInstance<IconReferenceProcessor>(instance);
            
            var getTaxCodeToTaxIdMappingQueryHandler = new GetTaxCodeToTaxIdMappingQueryHandler(iconContext);
            var getNationalClassCodeToIdMapping = new GetNationalClassCodeToClassIdMappingQueryHandler(iconContext);
            var getBrandAbbreviationQueryQueryHandler = new GetBrandAbbreviationQueryHandler(iconContext);
            var getDefaultCertificationAgenciesQueryHandler = new GetDefaultCertificationAgenciesQueryHandler(iconContext);

            return new IconReferenceProcessor(
                logger,
                emailClient,
                iconContext,
                getTaxCodeToTaxIdMappingQueryHandler,
                new GetRegionalSettingsBySettingsKeyNameQuery(iconContext),
                getNationalClassCodeToIdMapping,
                getBrandAbbreviationQueryQueryHandler,
                getDefaultCertificationAgenciesQueryHandler);
        }

        private static INewItemProcessor BuildNewItemProcessor(string region)
        {
            var logger = new NLogLoggerInstance<NewItemProcessor>(instance);

            var getIrmaNewItemsQueryHandler = new GetIrmaNewItemsQueryHandler(irmaContext);

            var getAppConfigValueQueryHandler = new GetAppConfigValueQueryHandler(irmaContext);

            var getInvalidInProcessedQueueEntriesQueryHandler = new GetInvalidInProcessedQueueEntriesQueryHandler(irmaContext);

            var deleteNewItemsFromIrmaQueueCommandHandler = new DeleteNewItemsFromIrmaQueueCommandHandler(new NLogLoggerInstance<DeleteNewItemsFromIrmaQueueCommandHandler>(instance),
                irmaContext);

            var markIconItemChangeQueueEntriesInProcessByCommandHandler = new MarkIconItemChangeQueueEntriesInProcessByCommandHandler(new NLogLoggerInstance<MarkIconItemChangeQueueEntriesInProcessByCommandHandler>(instance),
                irmaContext);
 
            var getValidatedItemsQueryHandler = new GetValidatedItemsQueryHandler(iconContext);

            var getBrandNewScanCodesQueryHandler = new GetBrandNewScanCodesQueryHandler(iconContext);

            var getScanCodesNeedSubscriptionQueryHandler = new GetScanCodesNeedSubscriptionQueryHandler(iconContext);

            
            return new NewItemProcessor(
                logger,
                iconContext,
                region,
                getIrmaNewItemsQueryHandler,
                getAppConfigValueQueryHandler,
                getInvalidInProcessedQueueEntriesQueryHandler,
                deleteNewItemsFromIrmaQueueCommandHandler,
                markIconItemChangeQueueEntriesInProcessByCommandHandler,
                getValidatedItemsQueryHandler,
                getBrandNewScanCodesQueryHandler,
                getScanCodesNeedSubscriptionQueryHandler);
        }
    }
}
