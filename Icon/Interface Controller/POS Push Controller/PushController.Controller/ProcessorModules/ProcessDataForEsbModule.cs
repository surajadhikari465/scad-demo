using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using PushController.Common;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.Controller.MessageGenerators;
using PushController.DataAccess.Commands;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using InterfaceController.Common;
using Irma.Framework;

namespace PushController.Controller.ProcessorModules
{
    public class ProcessDataForEsbModule : IIconPosDataProcessingModule
    {
        private const string flagKey = "GlobalPriceManagement";
        private IRenewableContext<IconContext> context;
        private ILogger<ProcessDataForEsbModule> logger;
        private ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper;
        private ICacheHelper<int, Locale> localeCacheHelper;
        private ICacheHelper<Tuple<string, int>, string> linkedScanCodeCacheHelper;
        private IQueryHandler<GetIconPosDataForEsbQuery, List<IRMAPush>> getIconPosDataForEsbQueryHandler;
        private ICommandHandler<MarkStagedRecordsAsInProcessForEsbCommand> markStagedRecordsAsInProcessForEsbCommandHandler;
        private ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler;
        private IMessageGenerator<MessageQueueItemLocale> messageGeneratorItemLocale;
        private IMessageGenerator<MessageQueuePrice> messageGeneratorPrice;

        public ProcessDataForEsbModule(
            IRenewableContext<IconContext> context,
            ILogger<ProcessDataForEsbModule> logger,
            ICacheHelper<string, ScanCodeModel> scanCodeCacheHelper,
            ICacheHelper<int, Locale> localeCacheHelper,
            ICacheHelper<Tuple<string, int>, string> linkedScanCodeCacheHelper,
            IQueryHandler<GetIconPosDataForEsbQuery, List<IRMAPush>> getIconPosDataForEsbQueryHandler,
            ICommandHandler<MarkStagedRecordsAsInProcessForEsbCommand> markStagedRecordsAsInProcessForEsbCommandHandler,
            ICommandHandler<UpdateStagingTableDatesForEsbCommand> updateStagingTableDatesForEsbCommandHandler,
            IMessageGenerator<MessageQueueItemLocale> messageGeneratorItemLocale,
            IMessageGenerator<MessageQueuePrice> messageGeneratorPrice)
        {
            this.context = context;
            this.logger = logger;
            this.scanCodeCacheHelper = scanCodeCacheHelper;
            this.localeCacheHelper = localeCacheHelper;
            this.linkedScanCodeCacheHelper = linkedScanCodeCacheHelper;
            this.getIconPosDataForEsbQueryHandler = getIconPosDataForEsbQueryHandler;
            this.markStagedRecordsAsInProcessForEsbCommandHandler = markStagedRecordsAsInProcessForEsbCommandHandler;
            this.updateStagingTableDatesForEsbCommandHandler = updateStagingTableDatesForEsbCommandHandler;
            this.messageGeneratorItemLocale = messageGeneratorItemLocale;
            this.messageGeneratorPrice = messageGeneratorPrice;
        }

        public void Execute()
        {
            MarkPosDataAsInProcess();

            var posDataReadyForEsb = GetPosDataForEsb();
            while (posDataReadyForEsb.Count > 0)
            {
                logger.Info(String.Format("Found {0} records for ESB processing.", posDataReadyForEsb.Count.ToString()));

                PopulateCaches(posDataReadyForEsb);

                var itemLocaleMessages = GenerateItemLocaleMessages(posDataReadyForEsb);

                if (itemLocaleMessages.Count > 0)
                {
                    SaveItemLocaleMessages(itemLocaleMessages);
                }
                List<String> nonGpmRegionList = Cache.regionCodeToGPMInstanceDataFlag
                                                      .Where(rg => rg.Value == false)
                                                      .Select(idf =>idf.Key)
                                                      .ToList();
                // get data for regions that are not on GPM 
                var posDataForNonGPMRegions = posDataReadyForEsb.Where(pdr => nonGpmRegionList.Contains(pdr.RegionCode)).ToList();

                if (posDataForNonGPMRegions.Count > 0)
                {
                    var priceMessages = GeneratePriceMessages(posDataForNonGPMRegions);
                    if (priceMessages.Count > 0)
                    {
                        SavePriceMessages(priceMessages);
                    }
                }

                UpdatePosDataProcessedDate(posDataReadyForEsb);

                logger.Info("Ending the main processing loop for ESB message generation.  Now preparing to retrieve a new set of staged POS data.");

                context.Refresh();
                MarkPosDataAsInProcess();
                posDataReadyForEsb = GetPosDataForEsb();
            }
        }

        private void PopulateCaches(List<IRMAPush> posDataReadyForEsb)
        {
            var identifiersToCache = posDataReadyForEsb.Select(p => p.Identifier).Distinct().ToList();
            scanCodeCacheHelper.Populate(identifiersToCache);

            var linkedIdentifiersToCache = posDataReadyForEsb.Where(p => !String.IsNullOrEmpty(p.LinkedIdentifier)).Select(p => p.LinkedIdentifier).Distinct().ToList();
            scanCodeCacheHelper.Populate(linkedIdentifiersToCache);

            var businessUnitsToCache = posDataReadyForEsb.Select(p => p.BusinessUnit_ID).Distinct().ToList();
            localeCacheHelper.Populate(businessUnitsToCache);

            var scanCodesToBusinessUnits = new List<Tuple<string, int>>();
            foreach (var posData in posDataReadyForEsb)
            {
                scanCodesToBusinessUnits.Add(new Tuple<string, int>(posData.Identifier, posData.BusinessUnit_ID));
            }

            linkedScanCodeCacheHelper.Populate(scanCodesToBusinessUnits);
        }

        private void UpdatePosDataProcessedDate(List<IRMAPush> stagedPosDataToUpdate)
        {
            var command = new UpdateStagingTableDatesForEsbCommand
            {
                ProcessedSuccessfully = true,
                StagedPosData = stagedPosDataToUpdate,
                Date = DateTime.Now
            };

            updateStagingTableDatesForEsbCommandHandler.Execute(command);
        }

        private void SavePriceMessages(List<MessageQueuePrice> priceMessages)
        {
            messageGeneratorPrice.SaveMessages(priceMessages);
        }

        private List<MessageQueuePrice> GeneratePriceMessages(List<IRMAPush> posDataReadyForEsb)
        {
            return messageGeneratorPrice.BuildMessages(posDataReadyForEsb);
        }

        private void SaveItemLocaleMessages(List<MessageQueueItemLocale> itemLocaleMessages)
        {
            messageGeneratorItemLocale.SaveMessages(itemLocaleMessages);
        }

        private List<MessageQueueItemLocale> GenerateItemLocaleMessages(List<IRMAPush> posDataReadyForEsb)
        {
            return messageGeneratorItemLocale.BuildMessages(posDataReadyForEsb);
        }

        private List<IRMAPush> GetPosDataForEsb()
        {
            var query = new GetIconPosDataForEsbQuery
            {
                Instance = StartupOptions.Instance
            };

            return getIconPosDataForEsbQueryHandler.Execute(query);
        }

        private void MarkPosDataAsInProcess()
        {
            var command = new MarkStagedRecordsAsInProcessForEsbCommand
            {
                Instance = StartupOptions.Instance,
                MaxRecordsToProcess = StartupOptions.MaxRecordsToProcess
            };

            markStagedRecordsAsInProcessForEsbCommandHandler.Execute(command);
        }
    }
}
