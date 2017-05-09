using Icon.Logging;
using Icon.Framework;
using Irma.Framework;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using TlogController.Common;
using TlogController.DataAccess.Queries;
using TlogController.DataAccess.Interfaces;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Models;

namespace TlogController.Controller.ProcessorModules
{
    public class IconTlogProcessorModule : IIconTlogProcessorModule
    {
        private ILogger<IconTlogProcessorModule> logger;
        private IQueryHandler<GetBusinessUnitToRegionCodeMappingQuery, Dictionary<int, string>> getBusinessUnitToRegionCodeMappingQueryHandler;
        private IQueryHandler<BulkUpdateItemMovementInProcessCommand, List<ItemMovement>> bulkUpdateItemMovementInProcessCommandHandler;
        private IBulkCommandHandler<BulkUpdateItemMovementCommand> bulkUpdateItemMovementCommandHandler;

        public IconTlogProcessorModule(
            ILogger<IconTlogProcessorModule> logger,
            IQueryHandler<GetBusinessUnitToRegionCodeMappingQuery, Dictionary<int, string>> getBusinessUnitToRegionCodeMappingQueryHandler,
            IQueryHandler<BulkUpdateItemMovementInProcessCommand, List<ItemMovement>> bulkUpdateItemMovementInProcessCommandHandler,
            IBulkCommandHandler<BulkUpdateItemMovementCommand> bulkUpdateItemMovementCommandHandler)
        {
            this.logger = logger;
            this.getBusinessUnitToRegionCodeMappingQueryHandler = getBusinessUnitToRegionCodeMappingQueryHandler;
            this.bulkUpdateItemMovementInProcessCommandHandler = bulkUpdateItemMovementInProcessCommandHandler;
            this.bulkUpdateItemMovementCommandHandler = bulkUpdateItemMovementCommandHandler;
        }

        public void LoadBusinessUnitToRegionCodeMapping()
        {
            if (Cache.BusinessUnitToRegionCode.Any() && Cache.CacheCreatedDate > DateTime.Today)
            {
                return;
            }
            else
            {
                Cache.BusinessUnitToRegionCode = getBusinessUnitToRegionCodeMappingQueryHandler.Execute(new GetBusinessUnitToRegionCodeMappingQuery());
                Cache.CacheCreatedDate = DateTime.Now;
            }
        }

        public void DeleteProcessedItemMovement(List<ItemMovementTransaction> data)
        {
            BulkUpdateItemMovementCommand bulkUpdateItemMovementCommand = new BulkUpdateItemMovementCommand();
            bulkUpdateItemMovementCommand.ItemMovementTransactionData = data;

            bulkUpdateItemMovementCommandHandler.Execute(bulkUpdateItemMovementCommand);
        }

        public List<IrmaTlog> GroupTlogEntiesByRegion()
        {
            List<IrmaTlog> irmaTlogs = new List<IrmaTlog>();
            List<ItemMovement> itemMovements = RetrieveTlogEntries();

            if (itemMovements != null && itemMovements.Count > 0)
            {
                foreach (ItemMovement itemMovement in itemMovements)
                {
                    try
                    {
                        string tlogRegionCode = itemMovement.BusinessUnitID.MapToRegionCode();

                        IrmaTlog irmaTlog = irmaTlogs.Find(t => t.RegionCode == tlogRegionCode);

                        ItemMovementToIrma itemMovementToIrma = CreateItemMovementToIrma(itemMovement);

                        if (irmaTlog != null)
                        {
                            irmaTlog.ItemMovementToIrmaList.Add(itemMovementToIrma);

                            ItemMovementTransaction itemMovementTransaction = irmaTlog.ItemMovementTransactionList.Find(t => t.ESBMessageID == itemMovement.ESBMessageID);

                            if (itemMovementTransaction != null)
                                itemMovementTransaction.LastItemMovementToIrmaIndex = irmaTlog.ItemMovementToIrmaList.Count - 1;
                            else
                            {
                                irmaTlog.ItemMovementTransactionList.Add(new ItemMovementTransaction
                                {
                                    ESBMessageID = itemMovement.ESBMessageID,
                                    FirstItemMovementToIrmaIndex = irmaTlog.ItemMovementToIrmaList.Count - 1,
                                    LastItemMovementToIrmaIndex = irmaTlog.ItemMovementToIrmaList.Count - 1
                                });
                            }
                        }
                        else
                        {
                            List<ItemMovementToIrma> itemMovementToIrmaList = new List<ItemMovementToIrma>();
                            List<ItemMovementTransaction> itemMovementTransactionList = new List<ItemMovementTransaction>();

                            itemMovementToIrmaList.Add(itemMovementToIrma);

                            itemMovementTransactionList.Add(new ItemMovementTransaction
                            {
                                ESBMessageID = itemMovement.ESBMessageID,
                                FirstItemMovementToIrmaIndex = 0,
                                LastItemMovementToIrmaIndex = 0
                            });

                            IrmaTlog newIrmaLog = new IrmaTlog
                            {
                                RegionCode = tlogRegionCode,
                                ItemMovementToIrmaList = itemMovementToIrmaList,
                                ItemMovementTransactionList = itemMovementTransactionList,
                            };
                            irmaTlogs.Add(newIrmaLog);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler<IconTlogProcessorModule>.logger = this.logger;
                        ExceptionHandler<IconTlogProcessorModule>.HandleException(String.Format("An unhandled exception occurred when grouping Tlog enties by region. BusinessUnitID: {0}; ItemMovementID: {1} ", itemMovement.BusinessUnitID, itemMovement.ItemMovementID), ex, this.GetType(), MethodBase.GetCurrentMethod());

                        IrmaTlog irmaTlog = irmaTlogs.Find(t => String.IsNullOrEmpty(t.RegionCode));

                        if (irmaTlog != null)
                        {
                            ItemMovementTransaction itemMovementTransaction = irmaTlog.ItemMovementTransactionList.Find(t => t.ESBMessageID == itemMovement.ESBMessageID);

                            if (itemMovementTransaction == null)
                                irmaTlog.ItemMovementTransactionList.Add(new ItemMovementTransaction
                                {
                                    ESBMessageID = itemMovement.ESBMessageID,
                                    Processed = false
                                });
                        }
                        else
                        {
                            IrmaTlog newIrmaLog = new IrmaTlog
                            {
                                RegionCode = String.Empty,
                                ItemMovementTransactionList = new List<ItemMovementTransaction>
                                {
                                    new ItemMovementTransaction
                                    {
                                        ESBMessageID = itemMovement.ESBMessageID,
                                        Processed = false
                                    }
                                }
                            };
                            irmaTlogs.Add(newIrmaLog);
                        }

                        continue;
                    }
                }
            }
            return irmaTlogs;
        }

        private List<ItemMovement> RetrieveTlogEntries()
        {
            try
            {
                BulkUpdateItemMovementInProcessCommand inProcessQuery = new BulkUpdateItemMovementInProcessCommand();
                inProcessQuery.MaxTransaction = StartupOptions.MaxTransactionsToProcess;
                inProcessQuery.Instance = StartupOptions.Instance.ToString();

                List<ItemMovement> itemMovements = bulkUpdateItemMovementInProcessCommandHandler.Execute(inProcessQuery);

                if (itemMovements != null && itemMovements.Count() > 0)
                    logger.Info(String.Format("Successfully retrieved {0} ItemMovement entries {1} through {2}.",
                        itemMovements.Count, itemMovements[0].ItemMovementID, itemMovements[itemMovements.Count - 1].ItemMovementID));

                return itemMovements.Select(im => new ItemMovement
                {
                    ItemMovementID = im.ItemMovementID,
                    ESBMessageID = im.ESBMessageID,
                    TransactionSequenceNumber = im.TransactionSequenceNumber,
                    BusinessUnitID = im.BusinessUnitID,
                    LineItemNumber = im.LineItemNumber,
                    Identifier = im.Identifier,
                    TransDate = im.TransDate.Date,
                    Quantity = im.Quantity,
                    ItemVoid = im.ItemVoid,
                    ItemType = im.ItemType,
                    BasePrice = im.BasePrice,
                    Weight = im.Weight,
                    MarkDownAmount = im.MarkDownAmount,
                    InsertDate = im.InsertDate,
                    InProcessBy = im.InProcessBy,
                    ProcessFailedDate = im.ProcessFailedDate
                }).ToList();
            }
            catch (Exception ex)
            {
                ExceptionHandler<IconTlogProcessorModule>.logger = this.logger;
                ExceptionHandler<IconTlogProcessorModule>.HandleException("An unhandled exception occurred in Retrieving Tlog Entries from the app.ItemMovement table. ", ex, this.GetType(), MethodBase.GetCurrentMethod());

                return null;
            }
        }

        private ItemMovementToIrma CreateItemMovementToIrma(ItemMovement itemMovement)
        {
            ItemMovementToIrma itemMovementToIrma = new ItemMovementToIrma
            {
                ItemMovementID = itemMovement.ItemMovementID,
                ESBMessageID = itemMovement.ESBMessageID,
                TransDate = itemMovement.TransDate,
                BusinessUnitId = itemMovement.BusinessUnitID,
                Identifier = itemMovement.Identifier,
                ItemType = itemMovement.ItemType,
                ItemVoid = itemMovement.ItemVoid,
                Quantity = itemMovement.Quantity,
                Weight = itemMovement.Weight,
                BasePrice = itemMovement.BasePrice,
                MarkdownAmount = itemMovement.MarkDownAmount
            };

            return itemMovementToIrma;
        }
    }
}