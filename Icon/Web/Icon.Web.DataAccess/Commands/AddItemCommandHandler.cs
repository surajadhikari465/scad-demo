using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Models;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddItemCommandHandler : ICommandHandler<AddItemCommand>
    {
        private readonly IDbProvider dbProvider;

        public AddItemCommandHandler(IDbProvider dbConnection)
        {
            this.dbProvider = dbConnection;
        }

        public void Execute(AddItemCommand data)
        {
            // get available scan code for bar code type if scan code isn't supplied
            if (string.IsNullOrWhiteSpace(data.ScanCode))
            {
                var scanCodes = this.dbProvider.Connection
                    .Query<string>(
                        "dbo.GetMultipleAvailableScanCodesForBarcodeTypeId", // Using this one to match the bulk import logic
                        new
                        {
                            BarcodeTypeId = data.SelectedBarCodeTypeId,
                            Count = 1,
                            ExcludedScanCodes = new List<ItemIdAndScanCode>()
                                .Select(isc => new { ScanCode = isc.ScanCode })
                                .ToDataTable()
                        },
                        commandType: CommandType.StoredProcedure,
                        transaction: this.dbProvider.Transaction)
                    .ToList();

                if (scanCodes.Count == 0)
                {
                    throw new ArgumentException("There is no available Scan Code for the selected Barcode Type.");
                }
                else
                {
                    data.ScanCode = scanCodes.First();
                }
            }

            // Add item
            DataTable items = new List<AddItemsType>
            {
                new AddItemsType
                {
                    ScanCode = data.ScanCode,
                    ItemTypeId = data.ItemTypeId,
                    BarCodeTypeId = data.SelectedBarCodeTypeId.Value,
                    ItemAttributesJson = JsonConvert.SerializeObject(data.ItemAttributes),
                    BrandsHierarchyClassId = data.BrandsHierarchyClassId,
                    FinancialHierarchyClassId = data.FinancialHierarchyClassId,
                    MerchandiseHierarchyClassId = data.MerchandiseHierarchyClassId,
                    NationalHierarchyClassId = data.NationalHierarchyClassId,
                    TaxHierarchyClassId = data.TaxHierarchyClassId,
                    ManufacturerHierarchyClassId = data.ManufacturerHierarchyClassId,
                }
            }.ToDataTable();

            var itemScanCode = (this.dbProvider.Connection
                .Query<ItemIdAndScanCode>(
                    sql: "dbo.AddItems",
                    param: new { Items = items },
                    commandType: CommandType.StoredProcedure,
                    transaction: this.dbProvider.Transaction))
                .First();

            data.ItemId = itemScanCode.ItemId;

            // Add events for app.EventQueue and esb.MessageQueueItem
            var ids = new List<int> { data.ItemId };
            var intList = ids.Select(i => new { I = i }).ToDataTable();
            var messageQueueItems = ids
                  .Select(i => new
                  {
                      ItemId = i,
                      EsbReadyDateTimeUtc = DateTime.UtcNow,
                      InsertDateUtc = DateTime.UtcNow
                  })
                  .ToDataTable();
            AddRegionalEvents(intList);
            AddMessageQueueItems(messageQueueItems);
        }

        private void AddRegionalEvents(DataTable itemIds)
        {
            dbProvider.Connection.Execute(
                "app.GenerateItemUpdateEvents",
                new { ItemIds = itemIds.AsTableValuedParameter("app.IntList") },
                commandType: CommandType.StoredProcedure,
                transaction: this.dbProvider.Transaction);
        }

        private void AddMessageQueueItems(DataTable messageQueueItems)
        {
            dbProvider.Connection.Execute(
                "esb.AddMessageQueueItem",
                new { @MessageQueueItems = messageQueueItems.AsTableValuedParameter("esb.MessageQueueItemIdsType") },
                commandType: CommandType.StoredProcedure,
                transaction: this.dbProvider.Transaction);
        }
    }
}