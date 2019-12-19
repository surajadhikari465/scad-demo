using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.Mappers.Interfaces;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static BulkItemUploadProcessor.Common.Constants;

namespace BulkItemUploadProcessor.Service.Mappers
{
    public class RowObjectToAddItemModelMapper : IRowObjectToAddItemModelMapper
    {
        private readonly IMerchItemPropertiesCache merchItemPropertiesCache;

        public RowObjectToAddItemModelMapper(IMerchItemPropertiesCache merchItemPropertiesCache)
        {
            this.merchItemPropertiesCache = merchItemPropertiesCache;
        }

        public RowObjectToItemMapperResponse<AddItemModel> Map(
            List<RowObject> rowObjects,
            List<ColumnHeader> columnHeaders,
            List<AttributeModel> attributeModels,
            string uploadedBy)
        {
            //Get column headers that match non-readonly attributes DisplayNames
            var validAttributeColumnHeaders = attributeModels.Where(a => !a.IsReadOnly)
                .Join(columnHeaders,
                    a => a.DisplayName,
                    c => c.Name,
                    (a, c) => new { a.AttributeName, ColumnIndex = c.ColumnIndex })
                .ToList();

            //Create DateTime for ModifiedDate and CreatedDate attributes
            var now = DateTime.UtcNow.ToFormattedDateTimeString();

            //Set up column indexes
            var scanCodeIndex = columnHeaders.First(c => c.Name == ScanCodeColumnHeader).ColumnIndex;
            var barcodeTypeIndex = columnHeaders.First(c => c.Name == BarcodeTypeColumnHeader).ColumnIndex;
            var brandsIndex = columnHeaders.First(c => c.Name == HierarchyNames.Brands).ColumnIndex;
            var merchIndex = columnHeaders.First(c => c.Name == HierarchyNames.Merchandise).ColumnIndex;
            var taxIndex = columnHeaders.First(c => c.Name == HierarchyNames.Tax).ColumnIndex;
            var nationalIndex = columnHeaders.First(c => c.Name == HierarchyNames.National).ColumnIndex;
            var manufacturerIndex = columnHeaders.First(c => c.Name == ManufacturerHierarchyName).ColumnIndex;

            //Convert RowObjects into AddItemModels
            var rowObjectDictionary = rowObjects.Select(r => new
            {
                Row = r,
                Cells = r.Cells.ToDictionary(
                        c => c.Column.ColumnIndex,
                        c => c.CellValue)
            });
            var addItemModels = rowObjectDictionary.Select(r =>
            {
                var merchandiseHierarchyClassId = ParseHierarchyClassId(r.Cells[merchIndex]).Value;
                var properties = merchItemPropertiesCache.Properties[merchandiseHierarchyClassId];
                var item = new AddItemModel
                {
                    BarCodeTypeId = int.Parse(r.Cells[barcodeTypeIndex].ParseClassId()),
                    BrandsHierarchyClassId = ParseHierarchyClassId(r.Cells[brandsIndex]).Value,
                    FinancialHierarchyClassId = properties.FinancialHierarcyClassId,
                    ItemTypeId = ItemTypes.Ids[properties.ItemTypeCode],
                    ManufacturerHierarchyClassId = r.Cells.ContainsKey(manufacturerIndex) ? ParseHierarchyClassId(r.Cells[manufacturerIndex]) : null,
                    MerchandiseHierarchyClassId = merchandiseHierarchyClassId,
                    NationalHierarchyClassId = ParseHierarchyClassId(r.Cells[nationalIndex]).Value,
                    ScanCode = r.Cells.ContainsKey(scanCodeIndex) ? r.Cells[scanCodeIndex] : null,
                    TaxHierarchyClassId = ParseHierarchyClassId(r.Cells[taxIndex]).Value
                };

                var itemAttributesJson = new Dictionary<string, string>();
                itemAttributesJson[ProhibitDiscountAttributeName] = properties.ProhibitDiscount ? JsonTrue : JsonFalse;
                itemAttributesJson[Constants.Attributes.ModifiedBy] = uploadedBy;
                itemAttributesJson[Constants.Attributes.ModifiedDateTimeUtc] = now;
                itemAttributesJson[Constants.Attributes.CreatedDateTimeUtc] = now;
                itemAttributesJson[Constants.Attributes.CreatedBy] = uploadedBy;

                foreach (var columnHeader in validAttributeColumnHeaders)
                {
                    r.Cells.TryGetValue(columnHeader.ColumnIndex, out string attributeValue);
                    if (!string.IsNullOrWhiteSpace(attributeValue))
                    {
                        if (attributeValue.Equals(JsonFalse, StringComparison.OrdinalIgnoreCase))
                            itemAttributesJson[columnHeader.AttributeName] = JsonFalse;
                        else if (attributeValue.Equals(JsonTrue, StringComparison.OrdinalIgnoreCase))
                            itemAttributesJson[columnHeader.AttributeName] = JsonTrue;
                        else
                            itemAttributesJson[columnHeader.AttributeName] = attributeValue;
                    }
                }
                if (!itemAttributesJson.ContainsKey(Constants.Attributes.Inactive)) itemAttributesJson["Inactive"] = JsonFalse;

                item.ItemAttributesJson = JsonConvert.SerializeObject(itemAttributesJson);

                return new { Item = item, r.Row };
            }).ToDictionary(
                i => i.Item,
                i => i.Row);

            return new RowObjectToItemMapperResponse<AddItemModel>
            {
                Items = addItemModels.Keys.ToList(),
                ItemToRowDictionary = addItemModels
            };
        }

        private int? ParseHierarchyClassId(string hierarchyValue)
        {
            var hierarchyClassId = hierarchyValue.ParseClassId();
            return string.IsNullOrWhiteSpace(hierarchyClassId) ? (int?)null : int.Parse(hierarchyClassId);
        }
    }
}
