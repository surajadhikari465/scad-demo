using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class AddItemsCommandHandler : ICommandHandler<AddItemsCommand>
    {
        private IDbConnection dbConnection;

        public AddItemsCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(AddItemsCommand data)
        {
            var itemsCopy = data.Items.ToList();
            var upcBarcodeTypeId = dbConnection.QueryFirst<int>(
                "SELECT TOP 1 BarcodeTypeId FROM dbo.BarcodeType WHERE BarcodeType = 'UPC'");
            var barcodeGroups = itemsCopy
                .Where(i => string.IsNullOrWhiteSpace(i.ScanCode) && i.BarCodeTypeId != upcBarcodeTypeId)
                .GroupBy(i => i.BarCodeTypeId)
                .ToList();
            var invalidItems = new List<ErrorItem<AddItemModel>>();
            foreach (var group in barcodeGroups)
            {
                var barcodeTypeId = group.Key;
                var barcodeScanCodes = dbConnection.Query<string>(
                    "dbo.GetMultipleAvailableScanCodesForBarcodeTypeId",
                    new
                    {
                        BarcodeTypeId = barcodeTypeId,
                        Count = group.Count(),
                        ExcludedScanCodes = itemsCopy
                            .Where(i => i.BarCodeTypeId == barcodeTypeId && i.ScanCode != null)
                            .Select(i => new { i.ScanCode })
                            .ToDataTable()
                            .AsTableValuedParameter("app.ScanCodeListType")
                    },
                    commandType: CommandType.StoredProcedure)
                    .ToList();
                var items = group.ToList();
                for (int i = 0; i < barcodeScanCodes.Count; i++)
                {
                    items[i].ScanCode = barcodeScanCodes[i];
                }
                if (barcodeScanCodes.Count < items.Count)
                {
                    foreach (var item in items.Skip(barcodeScanCodes.Count))
                    {
                        itemsCopy.Remove(item);
                        invalidItems.Add(new ErrorItem<AddItemModel>(item, "No available Scan Codes for given Barcode Type range."));
                    }
                }
            }

            try
            {
                data.AddedItems = dbConnection.Query<ItemIdAndScanCode>(
                    "dbo.AddItems",
                    new 
                    { 
                        Items = itemsCopy
                            .Select(i => i)
                            .ToList()
                            .ToDataTable()
                            .AsTableValuedParameter("dbo.AddItemsType") 
                    },
                    commandType: CommandType.StoredProcedure)
                    .ToList();
            }
            catch (Exception ex)
            {
                invalidItems.AddRange(itemsCopy.Select(i => new ErrorItem<AddItemModel>(i, ex.Message)));
            }

            data.InvalidItems = invalidItems;
        }
    }
}
