using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using InterfaceController.Common;
using PushController.Common;
using PushController.Controller.CacheHelpers;
using PushController.Controller.Decorators;
using PushController.Controller.MessageBuilders;
using PushController.Controller.MessageGenerators;
using PushController.Controller.MessageQueueServices;
using PushController.Controller.PosDataConverters;
using PushController.Controller.PosDataGenerators;
using PushController.Controller.PosDataStagingServices;
using PushController.Controller.ProcessorModules;
using PushController.Controller.Processors;
using PushController.Controller.UdmDeleteServices;
using PushController.Controller.UdmEntityBuilders;
using PushController.Controller.UdmEntityGenerators;
using PushController.Controller.UdmUpdateServices;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Queries;

namespace PushController.Controller
{
    public static class ControllerProvider
    {
        private static EmailClient emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
        private static EmailClient uomChangeEmailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
        private static string instance = StartupOptions.Instance.ToString();

        public static PosPushControllerBase ComposeController()
        {
            var controllerLogger = new NLogLoggerInstance<PosPushControllerBase>(instance);
            var irmaProcessorLogger = new NLogLoggerInstance<IrmaPosProcessor>(instance);
            var iconProcessorLogger = new NLogLoggerInstance<IconPosProcessor>(instance);

            var iconContext = new GlobalIconContext(new IconContext());
            var irmaContextProvider = new IrmaContextProvider();
            var irmaPosProcessor = new IrmaPosProcessor(irmaProcessorLogger, emailClient, BuildStagingModule(iconContext, irmaContextProvider));
            var iconPosProcessor = new IconPosProcessor(iconProcessorLogger, emailClient, BuildEsbModule(iconContext), BuildUdmModule(iconContext));

            return new PosPushControllerBase(controllerLogger, irmaPosProcessor, iconPosProcessor);
        }

        private static IIconPosDataProcessingModule BuildUdmModule(IRenewableContext<IconContext> iconContext)
        {
            var logger = new NLogLoggerInstance<ProcessDataForUdmModule>(instance);

            var getIconPosDataForUdmQueryHandler = new GetIconPosDataForUdmQueryHandler(
                new NLogLoggerInstance<GetIconPosDataForUdmQueryHandler>(instance),
                iconContext);

            var getItemSubscriptions = new GetIrmaItemSubscriptionsQueryHandler(iconContext);

            var scanCodeCacheHelper = new ScanCodeCacheHelper(new GetScanCodesByIdentifierBulkQueryHandler(
                new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                iconContext));

            var localeCacheHelper = new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(iconContext));

            var markStagedRecordsAsInProcessForUdmCommandHandler = new MarkStagedRecordsAsInProcessForUdmCommandHandler(iconContext);

            var updateStagingTableDatesForUdmCommandHandler = new UpdateStagingTableDatesForUdmCommandHandler(
                new NLogLoggerInstance<UpdateStagingTableDatesForUdmCommandHandler>(instance),
                iconContext);

            var itemLinkEntityGenerator = new ItemLinkEntityGenerator(
                new ItemLinkEntityBuilder(
                    new NLogLoggerInstance<ItemLinkEntityBuilder>(instance),
                    emailClient,
                    new ScanCodeCacheHelper(
                        new GetScanCodesByIdentifierBulkQueryHandler(
                            new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                            iconContext)),
                    new LocaleCacheHelper(
                        new GetLocalesByBusinessUnitIdQueryHandler(iconContext)),
                    new GetLocalesByBusinessUnitIdQueryHandler(iconContext),
                    new UpdateStagingTableDatesForUdmCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForUdmCommandHandler>(instance),
                        iconContext)),
                new ItemLinkUpdateService(
                    new NLogLoggerInstance<ItemLinkUpdateService>(instance),
                    emailClient,
                    new AddOrUpdateItemLinkBulkCommandHandler(
                        new NLogLoggerInstance<AddOrUpdateItemLinkBulkCommandHandler>(instance),
                        iconContext),
                    new AddOrUpdateItemLinkRowByRowCommandHandler(iconContext),
                    new UpdateStagingTableDatesForUdmCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForUdmCommandHandler>(instance),
                        iconContext)));

            var itemPriceEntityGenerator = new ItemPriceEntityGenerator(
                new ItemPriceEntityBuilder(
                    new NLogLoggerInstance<ItemPriceEntityBuilder>(instance),
                    emailClient,
                    new ScanCodeCacheHelper(
                        new GetScanCodesByIdentifierBulkQueryHandler(
                            new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                            iconContext)),
                    new LocaleCacheHelper(
                        new GetLocalesByBusinessUnitIdQueryHandler(iconContext)),
                    new GetLocalesByBusinessUnitIdQueryHandler(iconContext),
                    new UpdateStagingTableDatesForUdmCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForUdmCommandHandler>(instance),
                        iconContext)),
                new ItemPriceUpdateService(
                    new NLogLoggerInstance<ItemPriceUpdateService>(instance),
                    emailClient,
                    new AddOrUpdateItemPriceBulkCommandHandler(
                        new NLogLoggerInstance<AddOrUpdateItemPriceBulkCommandHandler>(instance),
                        iconContext),
                    new AddOrUpdateItemPriceRowByRowCommandHandler(iconContext),
                    new UpdateStagingTableDatesForUdmCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForUdmCommandHandler>(instance),
                        iconContext)));

            var itemSubscriptionDeleteService = new IrmaItemSubscriptionDeleteService(
                new NLogLoggerInstance<IrmaItemSubscriptionDeleteService>(instance),
                emailClient,
                new DeleteItemSubscriptionCommandHandler(
                    new NLogLoggerInstance<DeleteItemSubscriptionCommandHandler>(instance),
                    iconContext));

            var temporaryPriceReductionDeleteService = new TemporaryPriceReductionDeleteService(
                new DeleteTemporaryPriceReductionsCommandHandler(
                    new NLogLoggerInstance<DeleteTemporaryPriceReductionsCommandHandler>(instance),
                    iconContext));

            var itemLinkDeleteService = new ItemLinkDeleteService(
                new DeleteItemLinksCommandHandler(
                    new NLogLoggerInstance<DeleteItemLinksCommandHandler>(instance),
                    iconContext));

            return new ProcessDataForUdmModule(
                iconContext,
                logger,
                getIconPosDataForUdmQueryHandler,
                getItemSubscriptions,
                scanCodeCacheHelper,
                localeCacheHelper,
                markStagedRecordsAsInProcessForUdmCommandHandler,
                updateStagingTableDatesForUdmCommandHandler,
                itemLinkEntityGenerator,
                itemPriceEntityGenerator,
                itemSubscriptionDeleteService,
                temporaryPriceReductionDeleteService,
                itemLinkDeleteService);
        }

        private static IIconPosDataProcessingModule BuildEsbModule(IRenewableContext<IconContext> iconContext)
        {
            var logger = new NLogLoggerInstance<ProcessDataForEsbModule>(instance);

            var getIconPosDataForEsbQueryHandler = new GetIconPosDataForEsbQueryHandler(
                new NLogLoggerInstance<GetIconPosDataForEsbQueryHandler>(instance),
                iconContext);

            var scanCodeCacheHelper = new ScanCodeCacheHelper(new GetScanCodesByIdentifierBulkQueryHandler(
                new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                iconContext));

            var localeCacheHelper = new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(iconContext));

            var linkedScanCodeCacheHelper = new LinkedScanCodeCacheHelper(new GetCurrentLinkedScanCodesQueryHandler(
                new NLogLoggerInstance<GetCurrentLinkedScanCodesQueryHandler>(instance),
                iconContext));

            var markPosDataAsInProcessCommandHandler = new MarkStagedRecordsAsInProcessForEsbCommandHandler(iconContext);

            var updateStagingTableDatesForEsbCommandHandler = new UpdateStagingTableDatesForEsbCommandHandler(
                new NLogLoggerInstance<UpdateStagingTableDatesForEsbCommandHandler>(instance),
                iconContext);

            var messageGeneratorItemLocale = new ItemLocaleMessageGenerator(
                new ItemLocaleMessageBuilder(
                    new NLogLoggerInstance<ItemLocaleMessageBuilder>(instance),
                    emailClient,
                    new ScanCodeCacheHelper(
                        new GetScanCodesByIdentifierBulkQueryHandler(
                            new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                            iconContext)),
                    new LinkedScanCodeCacheHelper(
                        new GetCurrentLinkedScanCodesQueryHandler(
                            new NLogLoggerInstance<GetCurrentLinkedScanCodesQueryHandler>(instance),
                            iconContext)),
                    new LocaleCacheHelper(
                        new GetLocalesByBusinessUnitIdQueryHandler(iconContext)),
                    new GetLocalesByBusinessUnitIdQueryHandler(iconContext),
                    new UpdateStagingTableDatesForEsbCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForEsbCommandHandler>(instance),
                        iconContext)),
                new ItemLocaleMessageQueueService(
                    new NLogLoggerInstance<ItemLocaleMessageQueueService>(instance),
                    emailClient,
                    new AddItemLocaleMessagesBulkCommandHandler(
                        new NLogLoggerInstance<AddItemLocaleMessagesBulkCommandHandler>(instance),
                        iconContext),
                    new AddItemLocaleMessagesRowByRowCommandHandler(iconContext),
                    new UpdateStagingTableDatesForEsbCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForEsbCommandHandler>(instance),
                        iconContext)));

            var messageGeneratorPrice = new PriceMessageGenerator(
                new PriceUomChangeAlertDecorator(
                    uomChangeEmailClient,
                    new GetItemPricesByPushDataQueryHandler(iconContext),
                    new PriceUomChangeAlertConfiguration(),
                    new LocaleCacheHelper(new GetLocalesByBusinessUnitIdQueryHandler(iconContext)),
                    new ScanCodeCacheHelper(
                            new GetScanCodesByIdentifierBulkQueryHandler(
                                new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                                iconContext)),
                    new NLogLoggerInstance<PriceUomChangeAlertDecorator>(instance),
                    new PriceMessageBuilder(
                        new NLogLoggerInstance<PriceMessageBuilder>(instance),
                        emailClient,
                        new ScanCodeCacheHelper(
                            new GetScanCodesByIdentifierBulkQueryHandler(
                                new NLogLoggerInstance<GetScanCodesByIdentifierBulkQueryHandler>(instance),
                                iconContext)),
                        new LocaleCacheHelper(
                            new GetLocalesByBusinessUnitIdQueryHandler(iconContext)),
                        new GetLocalesByBusinessUnitIdQueryHandler(iconContext),
                        new GetPriceUomQueryHandler(iconContext),
                        new GetItemPriceQueryHandler(iconContext),
                        new UpdateStagingTableDatesForEsbCommandHandler(
                            new NLogLoggerInstance<UpdateStagingTableDatesForEsbCommandHandler>(instance),
                            iconContext))),
                new PriceMessageQueueService(
                    new NLogLoggerInstance<PriceMessageQueueService>(instance),
                    emailClient,
                    new AddPriceMessagesBulkCommandHandler(
                        new NLogLoggerInstance<AddPriceMessagesBulkCommandHandler>(instance),
                        iconContext),
                    new AddPriceMessagesRowByRowCommandHandler(iconContext),
                    new UpdateStagingTableDatesForEsbCommandHandler(
                        new NLogLoggerInstance<UpdateStagingTableDatesForEsbCommandHandler>(instance),
                        iconContext)));

            return new ProcessDataForEsbModule(
                iconContext,
                logger,
                scanCodeCacheHelper,
                localeCacheHelper,
                linkedScanCodeCacheHelper,
                getIconPosDataForEsbQueryHandler,
                markPosDataAsInProcessCommandHandler,
                updateStagingTableDatesForEsbCommandHandler,
                messageGeneratorItemLocale,
                messageGeneratorPrice);
        }

        private static IIrmaPosDataProcessingModule BuildStagingModule(IRenewableContext<IconContext> iconContext, IrmaContextProvider irmaContextProvider)
        {
            var logger = new NLogLoggerInstance<StagePosDataModule>(instance);

            var getJobStatusQueryHandler = new GetJobStatusQueryHandler();

            var getIrmaPosPushDataQueryHandler = new GetIrmaPosDataQueryHandler(new NLogLoggerInstance<GetIrmaPosDataQueryHandler>(instance));

            var markPublishedRecordsAsInProcessCommandHandler = new MarkPublishedRecordsAsInProcessCommandHandler();

            var updatePublishTableDatesCommandHandler = new UpdatePublishTableDatesCommandHandler(
                new NLogLoggerInstance<UpdatePublishTableDatesCommandHandler>(instance));

            var irmaPushDataGenerator = new IrmaPushDataGenerator(
                new IrmaPushDataConverter(
                    new NLogLoggerInstance<IrmaPushDataConverter>(instance),
                    emailClient,
                    new IrmaContextProvider(),
                    new GetAppConfigKeysQueryHandler(),
                    new UpdatePublishTableDatesCommandHandler(new NLogLoggerInstance<UpdatePublishTableDatesCommandHandler>(instance))),
                new IrmaPushStagingService(
                    new NLogLoggerInstance<IrmaPushStagingService>(instance),
                    new IrmaContextProvider(),
                    emailClient,
                    new StagePosDataBulkCommandHandler(
                        new NLogLoggerInstance<StagePosDataBulkCommandHandler>(instance),
                        iconContext),
                    new StagePosDataRowByRowCommandHandler(iconContext),
                    new UpdatePublishTableDatesCommandHandler(new NLogLoggerInstance<UpdatePublishTableDatesCommandHandler>(instance))));

            return new StagePosDataModule(
                logger,
                irmaContextProvider,
                getJobStatusQueryHandler,
                getIrmaPosPushDataQueryHandler,
                markPublishedRecordsAsInProcessCommandHandler,
                updatePublishTableDatesCommandHandler,
                irmaPushDataGenerator);
        }
    }
}
