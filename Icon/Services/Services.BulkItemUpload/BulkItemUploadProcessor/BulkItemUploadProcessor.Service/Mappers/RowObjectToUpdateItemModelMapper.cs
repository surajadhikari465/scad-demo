using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Queries;
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
using static Icon.Common.Constants;

namespace BulkItemUploadProcessor.Service.Mappers
{
    public class RowObjectToUpdateItemModelMapper : IRowObjectToUpdateItemModelMapper
    {
        private readonly IQueryHandler<GetItemsForUpdateQuery, IEnumerable<UpdateItemModel>> queryHandler;
        private readonly IMerchItemPropertiesCache merchItemPropertiesCache;

        public RowObjectToUpdateItemModelMapper(
            IQueryHandler<GetItemsForUpdateQuery, IEnumerable<UpdateItemModel>> queryHandler,
            IMerchItemPropertiesCache merchItemPropertiesCache)
        {
            this.queryHandler = queryHandler;
            this.merchItemPropertiesCache = merchItemPropertiesCache;
        }

        public RowObjectToItemMapperResponse<UpdateItemModel> Map(
            List<RowObject> rowObjects,
            List<ColumnHeader> columnHeaders,
            List<AttributeModel> attributeModels,
            string uploadedBy)
        {
            //Get ScanCode Index
            var scanCodeIndex = columnHeaders.First(c => c.Name == ScanCodeColumnHeader).ColumnIndex;

            //Get column headers that match non-readonly attributes DisplayNames or are one of the NonAttributeColumnNames
            var validAttributeColumnHeaders = attributeModels.Where(a => !a.IsReadOnly)
                .Join(columnHeaders,
                    a => a.DisplayName,
                    c => c.Name,
                    (a, c) => new { a.AttributeName, c.ColumnIndex, a.IsRequired })
                .ToList();

            // Get column headers that have boolean dataTypes
            var booleanAttributeColumnHeaders = attributeModels
                .Where(a => a.DataTypeName.Equals(DataTypeNames.Boolean, StringComparison.OrdinalIgnoreCase))
                .Join(columnHeaders,
                    a => a.DisplayName,
                    c => c.Name,
                    (a, c) => new { a.AttributeName })
                .ToList();

            var booleanAttributeList = new List<string>();
            foreach (var attribute in booleanAttributeColumnHeaders)
            {
                booleanAttributeList.Add(attribute.AttributeName);
            }

            //Create a list of actions that apply hierarchy values to the items only for the hierarchy columns present
            var hierarchyClassActions = columnHeaders
                .Join(HierarchyColumnNames,
                    c => c.Name,
                    h => h,
                    (c, h) => SetHierarchyClassActions(h, c.ColumnIndex))
                .ToList();

            //Convert the RowObjects to Dictionaries of ScanCode to CellValue by ColumnIndex
            var scanCodesToRowObjects = rowObjects
                .ToDictionary(
                    r => r.Cells.First(c => c.Column.ColumnIndex == scanCodeIndex).CellValue,
                    r => new
                    {
                        RowId = r.Row,
                        Row = r,
                        DictionaryValues =
                    r.Cells.ToDictionary(
                        c => c.Column.ColumnIndex,
                        c => c.CellValue)
                    });

            //Create DateTime for ModifiedDate and CreatedDate attributes
            var now = DateTime.UtcNow.ToFormattedDateTimeString();

            //Get the current items data so that we have the JSON attributes and can set those appropriately
            var updateItemModelsStream = queryHandler.Search(new GetItemsForUpdateQuery
            {
                ScanCodes = scanCodesToRowObjects.Keys.ToList()
            });

            Dictionary<UpdateItemModel, RowObject> dictionary = new Dictionary<UpdateItemModel, RowObject>();

            foreach (var itemModel in updateItemModelsStream)
            {
                var rowObject = scanCodesToRowObjects[itemModel.ScanCode];
                var itemAttributesJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(itemModel.ItemAttributesJson);

                itemAttributesJson[Constants.Attributes.ModifiedBy] = uploadedBy;
                itemAttributesJson[Constants.Attributes.ModifiedDateTimeUtc] = now;

                foreach (var columnHeader in validAttributeColumnHeaders)
                {
                    if (rowObject.DictionaryValues.ContainsKey(columnHeader.ColumnIndex))
                    {
                        var value = rowObject.DictionaryValues[columnHeader.ColumnIndex];
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (value == RemoveExcelValue)
                            {
                                if (!columnHeader.IsRequired)
                                    itemAttributesJson.Remove(columnHeader.AttributeName);
                            }
                            else
                            {
                                // ensure any Boolean values are stored in lowercase strings
                                if (booleanAttributeList.Contains(columnHeader.AttributeName))
                                {
                                    rowObject.DictionaryValues[columnHeader.ColumnIndex] =
                                        value.ToLower() == JsonTrue
                                            ? JsonTrue
                                            : JsonFalse;
                                }

                                itemAttributesJson[columnHeader.AttributeName] = rowObject.DictionaryValues[columnHeader.ColumnIndex];
                            }
                        }
                    }
                }

                foreach (var hierarchyClassAction in hierarchyClassActions)
                {
                    hierarchyClassAction(itemModel, itemAttributesJson, rowObject.DictionaryValues);
                }

                itemModel.ItemAttributesJson = JsonConvert.SerializeObject(itemAttributesJson);

                dictionary.Add(itemModel, rowObject.Row);
            }

            return new RowObjectToItemMapperResponse<UpdateItemModel>
            {
                Items = dictionary.Keys.ToList(),
                ItemToRowDictionary = dictionary
            };
        }

        private Action<UpdateItemModel, Dictionary<string, string>, Dictionary<int, string>> SetHierarchyClassActions(string hierarchyName, int columnIndex)
        {
            switch (hierarchyName)
            {
                case HierarchyNames.Merchandise:
                    return (updateItemModel, itemAttributesJson, rowObject) =>
                    {
                        if (rowObject.ContainsKey(columnIndex))
                        {
                            var hierarchyClassId = ParseHierarchyClassId(rowObject[columnIndex]);
                            if (hierarchyClassId != null)
                            {
                                updateItemModel.MerchandiseHierarchyClassId = hierarchyClassId;
                                var properties = merchItemPropertiesCache.Properties[hierarchyClassId.Value];
                                updateItemModel.ItemTypeId = ItemTypes.Ids[properties.ItemTypeCode];
                                updateItemModel.FinancialHierarchyClassId = properties.FinancialHierarcyClassId;
                                itemAttributesJson[ProhibitDiscountAttributeName] = properties.ProhibitDiscount ? JsonTrue : JsonFalse;
                            }
                        }
                    };
                case HierarchyNames.Brands:
                    return (updateItemModel, itemAttributesJson, rowObject) =>
                    {
                        if (rowObject.ContainsKey(columnIndex))
                            updateItemModel.BrandsHierarchyClassId = ParseHierarchyClassId(rowObject[columnIndex]);
                    };
                case HierarchyNames.Tax:
                    return (updateItemModel, itemAttributesJson, rowObject) =>
                    {
                        if (rowObject.ContainsKey(columnIndex))
                            updateItemModel.TaxHierarchyClassId = ParseHierarchyClassId(rowObject[columnIndex]);
                    };
                case HierarchyNames.National:
                    return (updateItemModel, itemAttributesJson, rowObject) =>
                    {
                        if (rowObject.ContainsKey(columnIndex))
                            updateItemModel.NationalHierarchyClassId = ParseHierarchyClassId(rowObject[columnIndex]);
                    };
                case ManufacturerHierarchyName:
                    return (updateItemModel, itemJsonAttributes, rowObject) =>
                    {
                        if (rowObject.ContainsKey(columnIndex))
                        {
                            if (rowObject[columnIndex] == RemoveExcelValue)
                                updateItemModel.ManufacturerHierarchyClassId = 0;
                            else
                                updateItemModel.ManufacturerHierarchyClassId = ParseHierarchyClassId(rowObject[columnIndex]);
                        }
                    };
                default: throw new ArgumentException($"Unable to set value for hierarchy {hierarchyName}.", nameof(columnIndex));
            }
        }

        private int? ParseHierarchyClassId(string hierarchyValue)
        {
            var hierarchyClassId = hierarchyValue.ParseClassId();
            return string.IsNullOrWhiteSpace(hierarchyClassId) ? (int?)null : int.Parse(hierarchyClassId);
        }
    }
}
