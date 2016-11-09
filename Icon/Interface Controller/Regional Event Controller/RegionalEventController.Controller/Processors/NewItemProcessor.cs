using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using RegionalEventController.Common;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.Controller.UpdateServices;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace RegionalEventController.Controller.Processors
{
    public class NewItemProcessor : INewItemProcessor
    {
        private const string ApplicationName = "IRMA CLIENT";
        private const string ConfigurationKeyName = "EnableUPCIConToIRMAFlow";
        private bool enableUPCIConToIRMAFlow = false;
        private bool enableUPCIConToIRMAFlowIsChecked = false;
        private List<IrmaNewItem> newItems = null;
        private List<IrmaNewItem> validatedItems = null;
        private List<IrmaNewItem> brandNewUpcItems = null;
        private List<IrmaNewItem> itemsNeedSubscription = null;

        private INewItemProcessor scanCodeProcessor;
        private IEventFinalizer eventFinalizer;
        private EmailClient emailClient = EmailClient.CreateFromConfig();

        private ILogger<NewItemProcessor> logger;
        private IconContext iconContext;
        private string regionCode;
        private IQueryHandler<GetIrmaNewItemsQuery, List<IrmaNewItem>> getIrmaNewItemsQueryHandler;
        private IQueryHandler<GetAppConfigValueQuery, string> getAppConfigValueQueryHandler;
        private IQueryHandler<GetInvalidInProcessedQueueEntriesQuery, List<int>> getInvalidInProcessedQueueEntriesQueryHandler;
        private IBulkCommandHandler<DeleteNewItemsFromIrmaQueueCommand> deleteNewItemsFromIrmaQueueCommandHandler;
        private IBulkCommandHandler<MarkIconItemChangeQueueEntriesInProcessByCommand> markIconItemChangeQueueEntriesInProcessByCommandHandler;
        private IQueryHandler<GetValidatedItemsQuery, Dictionary<string, int>> getValidatedItemsQueryHandler;
        private IQueryHandler<GetBrandNewScanCodesQuery, List<string>> getBrandNewScanCodesQueryHandler;
        private IQueryHandler<GetScanCodesNeedSubscriptionQuery, List<string>> getScanCodesNeedSubscriptionQueryHandler;

        public NewItemProcessor(ILogger<NewItemProcessor> logger,
            IconContext iconContext,
            string regionCode,
            IQueryHandler<GetIrmaNewItemsQuery, List<IrmaNewItem>> getIrmaNewItemsQueryHandler,
            IQueryHandler<GetAppConfigValueQuery, string> getAppConfigValueQueryHandler,
            IQueryHandler<GetInvalidInProcessedQueueEntriesQuery, List<int>> getInvalidInProcessedQueueEntriesQueryHandler,
            IBulkCommandHandler<DeleteNewItemsFromIrmaQueueCommand> deleteNewItemsFromIrmaQueueCommandHandler,
            IBulkCommandHandler<MarkIconItemChangeQueueEntriesInProcessByCommand> markIconItemChangeQueueEntriesInProcessByCommandHandler,
            IQueryHandler<GetValidatedItemsQuery, Dictionary<string, int>> getValidatedItemsQueryHandler,
            IQueryHandler<GetBrandNewScanCodesQuery, List<string>> getBrandNewScanCodesQueryHandler,
            IQueryHandler<GetScanCodesNeedSubscriptionQuery, List<string>> getScanCodesNeedSubscriptionQueryHandler)
        {
            this.logger = logger;
            this.iconContext = iconContext;
            this.regionCode = regionCode;
            this.getIrmaNewItemsQueryHandler = getIrmaNewItemsQueryHandler;
            this.getAppConfigValueQueryHandler = getAppConfigValueQueryHandler;
            this.getInvalidInProcessedQueueEntriesQueryHandler = getInvalidInProcessedQueueEntriesQueryHandler;
            this.deleteNewItemsFromIrmaQueueCommandHandler = deleteNewItemsFromIrmaQueueCommandHandler;
            this.markIconItemChangeQueueEntriesInProcessByCommandHandler = markIconItemChangeQueueEntriesInProcessByCommandHandler;
            this.getValidatedItemsQueryHandler = getValidatedItemsQueryHandler;
            this.getBrandNewScanCodesQueryHandler = getBrandNewScanCodesQueryHandler;
            this.getScanCodesNeedSubscriptionQueryHandler = getScanCodesNeedSubscriptionQueryHandler;
        }
        public void Run()
        {
            try
            {
                int numberOfLockedNewItems = MarkQueueDataAsInProcess();

                while (numberOfLockedNewItems > 0)
                {
                    logger.Info(String.Format("{0} new items have been marked as in process by instance {1}.", numberOfLockedNewItems.ToString(), StartupOptions.Instance.ToString()));

                    List<IrmaNewItem> invalidQueueEntries = null;

                    newItems = GetNewItems();
                    logger.Info(String.Format("{0} new items have been retrieved from region {1}.", newItems.Count.ToString(), regionCode));

                    if (numberOfLockedNewItems != newItems.Count())
                    {
                        invalidQueueEntries = GetInProcessedQueueEntriesNotInNewItems(newItems, regionCode);
                    }

                    //Generate item sub team events if events are enabled for this region
                    if (Cache.itemSbTeamEventEnabledRegions.Contains(regionCode))
                    {
                        List<IrmaNewItem> irmaDefaultItemsList = GetDefaultIrmaItems(newItems);
                        logger.Info(String.Format("Out of {0} new items {1} are default items for Item subteam updates.", newItems.Count.ToString(), irmaDefaultItemsList.Count().ToString()));
                        scanCodeProcessor = BuildItemSubTeamEventProcessor(irmaDefaultItemsList);
                        scanCodeProcessor.Run();
                    }
                    if (!enableUPCIConToIRMAFlowIsChecked)
                    {
                        enableUPCIConToIRMAFlow = GetEnableUPCIConToIRMAFlowKeyValue(ApplicationName, ConfigurationKeyName);
                        enableUPCIConToIRMAFlowIsChecked = true;
                        logger.Info(String.Format("The region's enableUPCIConToIRMAFlow config is set to {0}.", enableUPCIConToIRMAFlow.ToString()));
                    }

                    GroupScanCodesByCategories(enableUPCIConToIRMAFlow);

                    if (validatedItems.Count() > 0)
                    {
                        logger.Info(String.Format("Out of {0} new items {1} are validated.", newItems.Count.ToString(), validatedItems.Count().ToString()));
                        scanCodeProcessor = BuildValidatedScanCodeProcessor(validatedItems);
                        scanCodeProcessor.Run();
                    }

                    if (brandNewUpcItems.Count() > 0 && enableUPCIConToIRMAFlow)
                    {
                        logger.Info(String.Format("Out of {0} new items {1} are new UPCs.", newItems.Count.ToString(), brandNewUpcItems.Count().ToString()));
                        scanCodeProcessor = BuildBrandNewUpcProcessor(brandNewUpcItems);
                        scanCodeProcessor.Run();
                    }
                    else if (brandNewUpcItems.Count() > 0 && !enableUPCIConToIRMAFlow)
                    {
                        logger.Info(String.Format("Out of {0} new items {1} are new UPCs.", newItems.Count.ToString(), brandNewUpcItems.Count().ToString()));
                        scanCodeProcessor = BuildBrandNewUpcWithoutSubscriptionProcessor(brandNewUpcItems);
                        scanCodeProcessor.Run();
                    }

                    if (itemsNeedSubscription.Count() > 0)
                    {
                        logger.Info(String.Format("Out of {0} new items {1} need subscriptions to be created.", newItems.Count.ToString(), itemsNeedSubscription.Count().ToString()));
                        scanCodeProcessor = BuildSubscriptionOnlyScanCodeProcessor(itemsNeedSubscription);
                        scanCodeProcessor.Run();
                    }

                    if (invalidQueueEntries != null)
                    {
                        eventFinalizer = BuildEmailEventFinalizer(newItems.Union(invalidQueueEntries).ToList());
                    }
                    else
                    {
                        eventFinalizer = BuildEmailEventFinalizer(newItems);
                    }
                    eventFinalizer.HandleFailedEvents();
                    eventFinalizer.DeleteEvents();

                    numberOfLockedNewItems = MarkQueueDataAsInProcess();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler<NewItemProcessor>.logger = this.logger;
                ExceptionHandler<NewItemProcessor>.HandleException(String.Format("An unhandled exception occurred in the New Item Processing module for {0} region", regionCode), ex, this.GetType(), MethodBase.GetCurrentMethod());

                emailClient.Send(String.Format(Resource.ProcessingNewItemUnhandledExceptionMessage, regionCode, ex), Resource.ProcessingNewItemUnhandledExceptionEmailSubject);
            }
        }

        private bool GetEnableUPCIConToIRMAFlowKeyValue(string applicationName, string configurationKeyName)
        {
            GetAppConfigValueQuery getAppConfigValueQuery = new GetAppConfigValueQuery();
            getAppConfigValueQuery.applicationName = ApplicationName;
            getAppConfigValueQuery.configurationKey = ConfigurationKeyName;

            int enabled = 0;
            bool successfulParse = Int32.TryParse(getAppConfigValueQueryHandler.Execute(getAppConfigValueQuery), out enabled);

            return Convert.ToBoolean(enabled);
        }

        private List<IrmaNewItem> GetNewItems()
        {
            GetIrmaNewItemsQuery irmaNewItemsQuery = new GetIrmaNewItemsQuery();
            return getIrmaNewItemsQueryHandler.Execute(irmaNewItemsQuery);
        }

        private IEventFinalizer BuildEmailEventFinalizer(List<IrmaNewItem> newItems)
        {
            return new EventFinalizer(
                newItems,
                emailClient,
                deleteNewItemsFromIrmaQueueCommandHandler
            );
        }

        private int MarkQueueDataAsInProcess()
        {
            int maxQueueEventsToProcess;
            bool successfulParse = Int32.TryParse(ConfigurationManager.AppSettings["MaxQueueEntriesToProcess"], out maxQueueEventsToProcess);

            var command = new MarkIconItemChangeQueueEntriesInProcessByCommand
            {
                MaxQueueEntriesToProcess = maxQueueEventsToProcess,
                Instance = StartupOptions.Instance.ToString()
            };

            return markIconItemChangeQueueEntriesInProcessByCommandHandler.Execute(command);
        }

        private void GroupScanCodesByCategories(bool IConToIRMAUPCFlowEnabled)
        {
            Dictionary<string, int> validatedScanCodes = new Dictionary<string, int>();
            List<string> brandNewUpcs = new List<string>();
            List<string> scanCodesNeedSubscription = new List<string>();

            List<IrmaNewItem> remainingNewItems = new List<IrmaNewItem>();
            List<IrmaNewItem> itemsNeedToBeProcessed = newItems.Where(i => string.IsNullOrEmpty(i.FailureReason)).ToList();

            if (!IConToIRMAUPCFlowEnabled)
            {
                validatedScanCodes = GetValidatedItems(itemsNeedToBeProcessed, false);
            }
            else
            {
                validatedScanCodes = GetValidatedItems(itemsNeedToBeProcessed, true);
            }

            validatedItems = itemsNeedToBeProcessed.Where(i => validatedScanCodes.ContainsKey(i.Identifier)).ToList();

            foreach (IrmaNewItem item in validatedItems)
            {
                foreach (KeyValuePair<string, int> validatedScanCode in validatedScanCodes)
                {
                    if (validatedScanCode.Key == item.Identifier)
                    {
                        item.IconItemId = (int)validatedScanCode.Value;
                        break;
                    }

                }
            }

            //The remainingNewItems now contains only IrmaNewItems that are in originalItems, but not in validatedScanCodes
            remainingNewItems = itemsNeedToBeProcessed.Except(validatedItems).ToList();

            brandNewUpcs = GetBrandNewUpcs(remainingNewItems);

            brandNewUpcItems = remainingNewItems.Where(i => brandNewUpcs.Contains(i.Identifier)).ToList();

            //The remainingNewItems now contains only IrmaNewItems that are in originalItems, but not in validatedScanCodes
            //and not in brandNewUpcs
            remainingNewItems = remainingNewItems.Except(brandNewUpcItems).ToList();

            if (!IConToIRMAUPCFlowEnabled)
            {
                scanCodesNeedSubscription = GetScanCodesNeedSubscription(remainingNewItems, false);
            }
            else
            {
                scanCodesNeedSubscription = GetScanCodesNeedSubscription(remainingNewItems, true);
            }

            itemsNeedSubscription = remainingNewItems.Where(i => scanCodesNeedSubscription.Contains(i.Identifier)).ToList();

            // No any kinds of updates are needs by the remaining new items. Mark them as processed so that they can be removed from the queue.
            remainingNewItems = remainingNewItems.Except(itemsNeedSubscription).ToList();
            foreach (IrmaNewItem item in remainingNewItems)
            {
                item.ProcessedByController = true;
            }
        }

        private Dictionary<string, int> GetValidatedItems(List<IrmaNewItem> items, bool includeUPC)
        {
            GetValidatedItemsQuery getValidatedItemsQuery = new GetValidatedItemsQuery();

            if (!includeUPC)
            {
                List<IrmaNewItem> Upcs = items.Where(i => i.IdentifierType == ScanCodeTypes.Upc).ToList();
                getValidatedItemsQuery.identifiers = items.Except(Upcs).Select(i => i.Identifier).ToList();
            }
            else
            {
                getValidatedItemsQuery.identifiers = items.Select(i => i.Identifier).ToList();
            }

            return getValidatedItemsQueryHandler.Execute(getValidatedItemsQuery);
        }

        //Only brandnew UPCs need to be inserted into the app.IRMAItem table.
        private List<string> GetBrandNewUpcs(List<IrmaNewItem> newItems)
        {
            List<IrmaNewItem> Upcs = newItems.Where(i => i.IdentifierType == ScanCodeTypes.Upc).ToList();

            GetBrandNewScanCodesQuery getBrandNewScanCodesQuery = new GetBrandNewScanCodesQuery();
            getBrandNewScanCodesQuery.scanCodes = Upcs.Select(u => u.Identifier).ToList();

            return getBrandNewScanCodesQueryHandler.Execute(getBrandNewScanCodesQuery);
        }

        private List<string> GetScanCodesNeedSubscription(List<IrmaNewItem> items, bool includeUPC)
        {
            GetScanCodesNeedSubscriptionQuery getScanCodesNeedSubscriptionQuery = new GetScanCodesNeedSubscriptionQuery();

            if (!includeUPC)
            {
                List<IrmaNewItem> Upcs = items.Where(i => i.IdentifierType == ScanCodeTypes.Upc).ToList();
                getScanCodesNeedSubscriptionQuery.scanCodes = items.Except(Upcs).Select(i => i.Identifier).ToList();
            }
            else
            {
                getScanCodesNeedSubscriptionQuery.scanCodes = items.Select(i => i.Identifier).ToList();
            }

            getScanCodesNeedSubscriptionQuery.regionCode = regionCode;
            return getScanCodesNeedSubscriptionQueryHandler.Execute(getScanCodesNeedSubscriptionQuery);
        }

        private List<IrmaNewItem> GetInProcessedQueueEntriesNotInNewItems(List<IrmaNewItem> newItems, string regionCode)
        {
            List<IrmaNewItem> invalidQueueEntries = new List<IrmaNewItem>();

            GetInvalidInProcessedQueueEntriesQuery getInvalidInProcessedQueueEntriesQuery = new GetInvalidInProcessedQueueEntriesQuery();
            getInvalidInProcessedQueueEntriesQuery.queueIds = newItems.Select(n => n.QueueId).ToList();

            List<int> invalidInProcessedQueueIds = getInvalidInProcessedQueueEntriesQueryHandler.Execute(getInvalidInProcessedQueueEntriesQuery);

            foreach (int invalidInProcessedQueueId in invalidInProcessedQueueIds)
            {
                invalidQueueEntries.Add(new IrmaNewItem()
                {
                    RegionCode = regionCode,
                    QueueId = invalidInProcessedQueueId,
                    ProcessedByController = false,
                    FailureReason = "Invalid entry. The item/identifier is no longer active, or the identifier is associated with a different item.",
                    IsInvalidQueueEntry = true
                });
            }

            return invalidQueueEntries;
        }

        private INewItemProcessor BuildValidatedScanCodeProcessor(List<IrmaNewItem> items)
        {
            return new UpdateServiceProcessor(
                new ValidatedScanCodeInsertionService(
                new NLogLoggerInstance<ValidatedScanCodeInsertionService>(StartupOptions.Instance.ToString()),
                iconContext,
                items,
                new InsertIrmaItemSubscriptionsToIconBulkCommandHandler(new NLogLoggerInstance<InsertIrmaItemSubscriptionsToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                new InsertEventQueuesToIconBulkCommandHandler(new NLogLoggerInstance<InsertEventQueuesToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                BuildNewItemProcessingModule()
                )
            );
        }

        private INewItemProcessor BuildBrandNewUpcProcessor(List<IrmaNewItem> items)
        {
            return new UpdateServiceProcessor(
                new BrandNewUpcInsertionService(
                new NLogLoggerInstance<BrandNewUpcInsertionService>(StartupOptions.Instance.ToString()),
                iconContext,
                items,
                new InsertIrmaItemSubscriptionsToIconBulkCommandHandler(new NLogLoggerInstance<InsertIrmaItemSubscriptionsToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                new InsertIrmaItemsToIconBulkCommandHandler(new NLogLoggerInstance<InsertIrmaItemsToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                BuildNewItemProcessingModule()
                )
            );
        }

        private INewItemProcessor BuildBrandNewUpcWithoutSubscriptionProcessor(List<IrmaNewItem> items)
        {
            return new UpdateServiceProcessor(
                new BrandNewUpcWithoutSubscriptionInsertionService(
                new NLogLoggerInstance<BrandNewUpcWithoutSubscriptionInsertionService>(StartupOptions.Instance.ToString()),
                iconContext,
                items,
                new InsertIrmaItemsToIconBulkCommandHandler(new NLogLoggerInstance<InsertIrmaItemsToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                BuildNewItemProcessingModule()
                )
            );
        }

        private INewItemProcessor BuildSubscriptionOnlyScanCodeProcessor(List<IrmaNewItem> items)
        {
            return new UpdateServiceProcessor(
                new SubscriptionOnlyInsertionService(
                new NLogLoggerInstance<SubscriptionOnlyInsertionService>(StartupOptions.Instance.ToString()),
                iconContext,
                items,
                new InsertIrmaItemSubscriptionsToIconBulkCommandHandler(new NLogLoggerInstance<InsertIrmaItemSubscriptionsToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                BuildNewItemProcessingModule()
                )
            );
        }
        private INewItemProcessingModule BuildNewItemProcessingModule()
        {
            var logger = new NLogLoggerInstance<NewItemProcessingModule>(StartupOptions.Instance.ToString());
            var insertEventQueueToIconCommandHandler = new InsertEventQueueToIconCommandHandler(new NLogLoggerInstance<InsertEventQueueToIconCommandHandler>(StartupOptions.Instance.ToString()), iconContext);
            var insertIrmaItemSubscriptionToIconCommandHandler = new InsertIrmaItemSubscriptionToIconCommandHandler(new NLogLoggerInstance<InsertIrmaItemSubscriptionToIconCommandHandler>(StartupOptions.Instance.ToString()), iconContext);
            var insertIrmaItemToIconCommandHandler = new InsertIrmaItemToIconCommandHandler(new NLogLoggerInstance<InsertIrmaItemToIconCommandHandler>(StartupOptions.Instance.ToString()), iconContext);
            return new NewItemProcessingModule(
                logger,
                insertEventQueueToIconCommandHandler,
                insertIrmaItemSubscriptionToIconCommandHandler,
                insertIrmaItemToIconCommandHandler
                );
        }

        private INewItemProcessor BuildItemSubTeamEventProcessor(List<IrmaNewItem> items)
        {
            return new UpdateServiceProcessor(
                new ItemSubTeamEventService(
                new NLogLoggerInstance<ItemSubTeamEventService>(StartupOptions.Instance.ToString()),
                iconContext,
                items,
                new GetIconIrmaItemsBulkQueryHandler(iconContext),
                new InsertEventQueuesToIconBulkCommandHandler(new NLogLoggerInstance<InsertEventQueuesToIconBulkCommandHandler>(StartupOptions.Instance.ToString()), iconContext),
                BuildNewItemProcessingModule(),
                regionCode
                )
            );
        }

        private List<IrmaNewItem> GetDefaultIrmaItems(List<IrmaNewItem> items)
        {
            return items.Where(i => i.IrmaItem.defaultIdentifier == true).ToList();
        }
    }
}
