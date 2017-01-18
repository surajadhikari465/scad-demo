using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.UdmDeleteServices;
using PushController.Controller.UdmEntityGenerators;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushController.Controller.ProcessorModules
{
    public class ProcessDataForUdmModule : IIconPosDataProcessingModule
    {
        private IRenewableContext<IconContext> context;
        private ILogger<ProcessDataForUdmModule> logger;
        private IQueryHandler<GetIconPosDataForUdmQuery, List<IRMAPush>> getIconPosDataForUdmQueryHandler;
        private IQueryHandler<GetIrmaItemSubscriptionsQuery, List<IRMAItemSubscription>> getItemSubscriptions;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private ICommandHandler<MarkStagedRecordsAsInProcessForUdmCommand> markStagedRecordsAsInProcessForUdmCommandHandler;
        private ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler;
        private IUdmEntityGenerator<ItemLinkModel> itemLinkUpdateGenerator;
        private IUdmEntityGenerator<ItemPriceModel> itemPriceUpdateGenerator;
        private IUdmDeleteService<IRMAItemSubscription> itemSubscriptionDeleteService;
        private IUdmDeleteService<TemporaryPriceReductionModel> temporaryPriceReductionDeleteService;
        private IUdmDeleteService<ItemLinkModel> itemLinkDeleteService;

        public ProcessDataForUdmModule(
            IRenewableContext<IconContext> context,
            ILogger<ProcessDataForUdmModule> logger,
            IQueryHandler<GetIconPosDataForUdmQuery, List<IRMAPush>> getIconPosDataForUdmQueryHandler,
            IQueryHandler<GetIrmaItemSubscriptionsQuery, List<IRMAItemSubscription>> getItemSubscriptions,
            ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper,
            ICacheHelper<int, Locale> localeCacheHelper,
            ICommandHandler<MarkStagedRecordsAsInProcessForUdmCommand> markStagedRecordsAsInProcessForUdmCommandHandler,
            ICommandHandler<UpdateStagingTableDatesForUdmCommand> updateStagingTableDatesForUdmCommandHandler,
            IUdmEntityGenerator<ItemLinkModel> itemLinkUpdateGenerator,
            IUdmEntityGenerator<ItemPriceModel> itemPriceUpdateGenerator,
            IUdmDeleteService<IRMAItemSubscription> itemSubscriptionDeleteService,
            IUdmDeleteService<TemporaryPriceReductionModel> temporaryPriceReductionDeleteService,
            IUdmDeleteService<ItemLinkModel> itemLinkDeleteService)
        {
            this.context = context;
            this.logger = logger;
            this.getIconPosDataForUdmQueryHandler = getIconPosDataForUdmQueryHandler;
            this.getItemSubscriptions = getItemSubscriptions;
            this.scanCodeCacheHelper = scanCodeCacheHelper;
            this.localeCacheHelper = localeCacheHelper;
            this.markStagedRecordsAsInProcessForUdmCommandHandler = markStagedRecordsAsInProcessForUdmCommandHandler;
            this.updateStagingTableDatesForUdmCommandHandler = updateStagingTableDatesForUdmCommandHandler;
            this.itemLinkUpdateGenerator = itemLinkUpdateGenerator;
            this.itemPriceUpdateGenerator = itemPriceUpdateGenerator;
            this.itemSubscriptionDeleteService = itemSubscriptionDeleteService;
            this.temporaryPriceReductionDeleteService = temporaryPriceReductionDeleteService;
            this.itemLinkDeleteService = itemLinkDeleteService;
        }

        public void Execute()
        {
            MarkPosDataAsInProcess();
            var posDataReadyForUdm = GetPosDataForUdm();

            while (posDataReadyForUdm.Count > 0)
            {
                logger.Info(String.Format("Found {0} records for UDM processing.", posDataReadyForUdm.Count.ToString()));

                scanCodeCacheHelper.Populate(posDataReadyForUdm.Select(p => p.Identifier).Distinct().ToList());

                var businessUnitsToCache = posDataReadyForUdm.Select(p => p.BusinessUnit_ID).Distinct().ToList();
                localeCacheHelper.Populate(businessUnitsToCache);

                var itemLinks = BuildItemLinks(posDataReadyForUdm);

                var itemLinkUpdates = itemLinks.Where(il => !il.IsDelete).ToList();

                if (itemLinkUpdates.Count > 0)
                {
                    SaveItemLinkUpdates(itemLinkUpdates);
                }

                DeleteCancelledTemporaryPriceReductions(posDataReadyForUdm);

                var itemPriceUpdates = BuildItemPriceUpdates(posDataReadyForUdm);

                if (itemPriceUpdates.Count > 0)
                {
                    SaveItemPriceUpdates(itemPriceUpdates);
                }

                var unsubscribedItems = CheckForUnsubscribedItems(posDataReadyForUdm);

                if (unsubscribedItems.Count > 0)
                {
                    DeleteInvalidIrmaItemSubscriptions(unsubscribedItems);
                }

                List<ItemLinkModel> itemLinkDeletes = itemLinks.Where(il => il.IsDelete).ToList();

                if(itemLinkDeletes.Any())
                {
                    DeleteItemLinks(itemLinkDeletes);
                }

                UpdatePosDataProcessedDate(posDataReadyForUdm);

                logger.Info("Ending the main processing loop for UDM updates.  Now preparing to retrieve a new set of staged POS data.");

                context.Refresh();
                MarkPosDataAsInProcess();
                posDataReadyForUdm = GetPosDataForUdm();
            }
        }

        private void DeleteItemLinks(List<ItemLinkModel> itemLinkDeletes)
        {
            itemLinkDeleteService.DeleteEntitiesBulk(itemLinkDeletes);
        }

        private void DeleteCancelledTemporaryPriceReductions(List<IRMAPush> posDataReadyForUdm)
        {
            var cancelledTprs = posDataReadyForUdm
                .Where(ip => ip.ChangeType == Constants.IrmaPushChangeTypes.CancelAllSales)
                .Select(ip => new TemporaryPriceReductionModel
                {
                    ItemId = scanCodeCacheHelper.Retrieve(ip.Identifier).ItemId,
                    LocaleId = localeCacheHelper.Retrieve(ip.BusinessUnit_ID).localeID
                }).ToList();

            if (cancelledTprs.Count > 0)
            {
                try
                {
                    temporaryPriceReductionDeleteService.DeleteEntitiesBulk(cancelledTprs);
                }
                catch (Exception)
                {
                    temporaryPriceReductionDeleteService.DeleteEntitiesRowByRow(cancelledTprs);
                }
            }
        }

        private void UpdatePosDataProcessedDate(List<IRMAPush> stagedPosDataToUpdate)
        {
            var command = new UpdateStagingTableDatesForUdmCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = stagedPosDataToUpdate,
                Date = DateTime.Now
            };

            updateStagingTableDatesForUdmCommandHandler.Execute(command);
        }

        private void DeleteInvalidIrmaItemSubscriptions(List<IRMAPush> unsubscribedItems)
        {
            var query = new GetIrmaItemSubscriptionsQuery
            {
                IrmaPushData = unsubscribedItems
            };

            var subscriptionsToDelete = getItemSubscriptions.Execute(query);

            if (subscriptionsToDelete.Count > 0)
            {
                try
                {
                    itemSubscriptionDeleteService.DeleteEntitiesBulk(subscriptionsToDelete);
                }
                catch (Exception)
                {
                    itemSubscriptionDeleteService.DeleteEntitiesRowByRow(subscriptionsToDelete);
                }
            }
        }

        private List<IRMAPush> CheckForUnsubscribedItems(List<IRMAPush> posDataReadyForUdm)
        {
            var itemsToUnsubscribe = posDataReadyForUdm.Where(pos => pos.ChangeType == Constants.IrmaPushChangeTypes.ScanCodeDelete).ToList();
            return itemsToUnsubscribe;
        }

        private void SaveItemPriceUpdates(List<ItemPriceModel> itemPriceUpdates)
        {
            itemPriceUpdateGenerator.SaveEntities(itemPriceUpdates);
        }

        private List<ItemPriceModel> BuildItemPriceUpdates(List<IRMAPush> posDataReadyForUdm)
        {
            return itemPriceUpdateGenerator.BuildEntities(posDataReadyForUdm);
        }

        private void SaveItemLinkUpdates(List<ItemLinkModel> itemLinkUpdates)
        {
            itemLinkUpdateGenerator.SaveEntities(itemLinkUpdates);
        }

        private List<ItemLinkModel> BuildItemLinks(List<IRMAPush> posDataReadyForUdm)
        {
            return itemLinkUpdateGenerator.BuildEntities(posDataReadyForUdm);
        }

        private List<IRMAPush> GetPosDataForUdm()
        {
            var query = new GetIconPosDataForUdmQuery
            {
                Instance = StartupOptions.Instance
            };

            return getIconPosDataForUdmQueryHandler.Execute(query);
        }

        private void MarkPosDataAsInProcess()
        {
            var command = new MarkStagedRecordsAsInProcessForUdmCommand
            {
                Instance = StartupOptions.Instance,
                MaxRecordsToProcess = StartupOptions.MaxRecordsToProcess
            };

            markStagedRecordsAsInProcessForUdmCommandHandler.Execute(command);
        }
    }
}
