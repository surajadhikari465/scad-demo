using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Common.Validators;
using Icon.Common.Validators.ItemAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using static BulkItemUploadProcessor.Common.Constants;
using static BulkItemUploadProcessor.Common.Enums;

namespace BulkItemUploadProcessor.Service.Validation
{
    public class RowObjectsValidator : IRowObjectsValidator
    {
        private readonly IItemAttributesValidatorFactory itemAttributesValidatorFactory;
        private readonly IHierarchyValidator hierarchyValidator;
        private readonly ScanCodeValidator scanCodeValidator;
        private readonly IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler;
        private readonly IQueryHandler<GetScanCodesThatExistParameters, HashSet<string>> getScanCodesThatExistQueryHandler;

        public RowObjectsValidator(
            IItemAttributesValidatorFactory itemAttributesValidatorFactory,
            IHierarchyValidator hierarchyValidator,
            ScanCodeValidator scanCodeValidator,
            IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>> getBarcodeTypesQueryHandler,
            IQueryHandler<GetScanCodesThatExistParameters, HashSet<string>> getScanCodesThatExistQueryHandler)
        {
            this.itemAttributesValidatorFactory = itemAttributesValidatorFactory;
            this.hierarchyValidator = hierarchyValidator;
            this.scanCodeValidator = scanCodeValidator;
            this.getBarcodeTypesQueryHandler = getBarcodeTypesQueryHandler;
            this.getScanCodesThatExistQueryHandler = getScanCodesThatExistQueryHandler;
        }

        public RowObjectValidatorResponse Validate(FileModeTypeEnum fileModeType, List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<AttributeModel> attributeModels)
        {
            if (fileModeType == FileModeTypeEnum.CreateNew)
            {
                return ValidateCreateNew(rowObjects, columnHeaders, attributeModels);
            }
            else if (fileModeType == FileModeTypeEnum.UpdateExisting)
            {
                return ValidateUpdateExisting(rowObjects, columnHeaders, attributeModels);
            }
            else
            {
                throw new ArgumentException($"No validator is set for fileModeType {fileModeType.ToString()}", nameof(fileModeType));
            }
        }

        private RowObjectValidatorResponse ValidateCreateNew(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<AttributeModel> attributeModels)
        {
            var response = new RowObjectValidatorResponse();
            var scanCodeIndex = columnHeaders.First(c => c.Name == ScanCodeColumnHeader).ColumnIndex;
            var barcodeTypeIndex = columnHeaders.First(c => c.Name == BarcodeTypeColumnHeader).ColumnIndex;

            var attributeColumns = columnHeaders
                .Join(attributeModels.Where(a => !a.IsReadOnly),
                    c => c.Name,
                    a => a.DisplayName,
                    (c, a) => new
                    {
                        ColumnHeader = c,
                        a.IsRequired,
                        AttributeValidator = itemAttributesValidatorFactory.CreateItemAttributesJsonValidator(a.AttributeName)
                    })
                .ToList();
            var hierarchyColumns = columnHeaders
                .Join(HierarchyColumnNames,
                    c => c.Name,
                    h => h,
                    (c, h) => new { ColumnHeader = c, HierarchyName = h })
                .ToList();
            var barcodeTypes = getBarcodeTypesQueryHandler.Search(new EmptyQueryParameters<List<BarcodeTypeModel>>());
            var upcBarcodeType = barcodeTypes.First(b => b.BarcodeType == "UPC");

            var rowObjectDictionaries = rowObjects
                .Select(r => new
                {
                    r.Row,
                    Cells = r.Cells.ToDictionary(
                        c => c.Column.ColumnIndex,
                        c => c.CellValue),
                    RowObject = r
                });

            var existingScanCodes = getScanCodesThatExistQueryHandler.Search(new GetScanCodesThatExistParameters
            {
                ScanCodes = rowObjectDictionaries
                    .Where(r => r.Cells.ContainsKey(scanCodeIndex)
                        && !string.IsNullOrEmpty(r.Cells[scanCodeIndex])
                        && r.Cells[scanCodeIndex].Length <= Icon.Common.Constants.ScanCodeMaxLength)
                    .Select(r => r.Cells[scanCodeIndex])
                    .ToList()
            });
            foreach (var rowObjectDictionary in rowObjectDictionaries)
            {
                List<InvalidRowError> errors = new List<InvalidRowError>();

                try
                {
                    string barcodeType = rowObjectDictionary.Cells.ContainsKey(barcodeTypeIndex) ? rowObjectDictionary.Cells[barcodeTypeIndex] : null;
                    int.TryParse(barcodeType.ParseClassId(), out int barcodeTypeId);
                    if (string.IsNullOrWhiteSpace(barcodeType))
                    {
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"Missing '{BarcodeTypeColumnHeader}'. Barcode Type cannot be empty and must exist in Icon." });
                    }

                    if (string.IsNullOrWhiteSpace(barcodeType) || !barcodeTypes.Any(b => b.BarcodeTypeId == barcodeTypeId))
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{BarcodeTypeColumnHeader}' has invalid value: '{barcodeType}'. Barcode Type cannot be empty and must exist in Icon." });

                    string scanCode = rowObjectDictionary.Cells.ContainsKey(scanCodeIndex) ? rowObjectDictionary.Cells[scanCodeIndex] : null;
                    if (!string.IsNullOrWhiteSpace(scanCode) && !scanCodeValidator.Validate(scanCode))
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{ScanCodeColumnHeader}' has invalid value. Scan Code must be less than {Icon.Common.Constants.ScanCodeMaxLength} characters, must not start with 0, and must be numeric." });
                    if (!string.IsNullOrWhiteSpace(scanCode) && existingScanCodes.Contains(scanCode))
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{ScanCodeColumnHeader}' has invalid value. '{scanCode}' already exists." });

                    if (barcodeTypeId == upcBarcodeType.BarcodeTypeId && string.IsNullOrWhiteSpace(scanCode))
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{ScanCodeColumnHeader}' is required when selecting UPC Barcode Type." });

                    Tuple<bool, string> isScanCodeValidForBarcodeType = IsScanCodeValidForBarcodeType(scanCode, barcodeType, barcodeTypeId, upcBarcodeType, barcodeTypes);
                    if (!isScanCodeValidForBarcodeType.Item1)
                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = isScanCodeValidForBarcodeType.Item2 });

                    foreach (var attributeColumn in attributeColumns)
                    {
                        var rowObjectContainsAttribute = rowObjectDictionary.Cells.ContainsKey(attributeColumn.ColumnHeader.ColumnIndex);
                        if (attributeColumn.IsRequired && !rowObjectContainsAttribute)
                        {
                            errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{attributeColumn.ColumnHeader.Name}' is required." });
                        }
                        else if (rowObjectContainsAttribute)
                        {
                            var value = rowObjectDictionary.Cells[attributeColumn.ColumnHeader.ColumnIndex];
                            if (attributeColumn.IsRequired || value != string.Empty)
                            {
                                var result = attributeColumn.AttributeValidator.Validate(value);
                                if (!result.IsValid)
                                {
                                    foreach (var errorMessage in result.ErrorMessages)
                                    {
                                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = errorMessage });
                                    }
                                }
                            }
                        }
                    }

                    foreach (var hierarchyColumn in hierarchyColumns)
                    {
                        if (rowObjectDictionary.Cells.ContainsKey(hierarchyColumn.ColumnHeader.ColumnIndex))
                        {
                            var hierarchyValidatorResponse = hierarchyValidator.Validate(
                                hierarchyColumn.HierarchyName,
                                rowObjectDictionary.Cells[hierarchyColumn.ColumnHeader.ColumnIndex],
                                allowEmptyString: hierarchyColumn.HierarchyName == ManufacturerHierarchyName,
                                allowRemove: false);
                            if (!hierarchyValidatorResponse.IsValid)
                                errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = hierarchyValidatorResponse.Error });
                        }
                        else if (hierarchyColumn.HierarchyName != ManufacturerHierarchyName)
                        {
                            errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"Missing '{hierarchyColumn.HierarchyName}'." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"Unexpected error occurred while validating row. Error: {ex.Message}" });
                }

                if (errors.Count > 0)
                    response.InvalidRows.AddRange(errors);
                else
                    response.ValidRows.Add(rowObjectDictionary.RowObject);
            }
            return response;
        }

        private Tuple<bool, string> IsScanCodeValidForBarcodeType(string scanCode, string barcodeType, int barcodeTypeId, BarcodeTypeModel upcBarcodeType, List<BarcodeTypeModel> barcodeTypes)
        {
            if (string.IsNullOrWhiteSpace(scanCode) || string.IsNullOrWhiteSpace(barcodeType))
            {
                return new Tuple<bool, string>(true, null);
            }
            else
            {
                if (long.TryParse(scanCode, out long longScanCode))
                {
                    if (barcodeTypeId == upcBarcodeType.BarcodeTypeId)
                    {
                        if (scanCode.Length == 11 && scanCode.EndsWith("00000") && scanCode.StartsWith("2"))
                        {
                            return new Tuple<bool, string>(false, $"'{scanCode}'exists in a Barcode Type range. Please enter a scan code not within a Barcode Type range.");
                        }

                        var pluBarcodeTypes = barcodeTypes
                            .Where(b => b.BarcodeTypeId != upcBarcodeType.BarcodeTypeId)
                            .Select(b => new { b.BarcodeType, b.BeginRange, b.EndRange, b.ScalePlu });

                        foreach (var pluBarcodeType in pluBarcodeTypes)
                        {
                            long beginRange;
                            long endRange;

                            if (pluBarcodeType.ScalePlu == true)
                            {
                                beginRange = long.Parse(pluBarcodeType.BeginRange.Substring(0, pluBarcodeType.BeginRange.Length - 5));
                                endRange = long.Parse(pluBarcodeType.EndRange.Substring(0, pluBarcodeType.EndRange.Length - 5));
                            }
                            else
                            {
                                beginRange = long.Parse(pluBarcodeType.BeginRange);
                                endRange = long.Parse(pluBarcodeType.EndRange);
                            }

                            if (longScanCode >= beginRange && longScanCode <= endRange)
                                return new Tuple<bool, string>(false, $"Scan Code '{scanCode}' is marked as UPC but falls under {pluBarcodeType.BarcodeType} Barcode range.");
                        }
                        return new Tuple<bool, string>(true, null);
                    }
                    else
                    {
                        var pluRange = barcodeTypes
                            .FirstOrDefault(b => b.BarcodeTypeId == barcodeTypeId);

                        if (pluRange.ScalePlu == true && !(scanCode.Length == 11 && scanCode.EndsWith("00000") && scanCode.StartsWith("2")))
                        {
                            return new Tuple<bool, string>(false, $"Scan Code '{scanCode}' does not fall under '{pluRange.BarcodeType}' range.");
                        }

                        if (pluRange != null && longScanCode < long.Parse(pluRange.BeginRange) || longScanCode > long.Parse(pluRange.EndRange))
                        {
                            return new Tuple<bool, string>(false, $"Scan Code '{scanCode}' does not fall under '{pluRange.BarcodeType}' range.");
                        }
                        else
                        {
                            return new Tuple<bool, string>(true, null);
                        }
                    }
                }
                else
                {
                    return new Tuple<bool, string>(true, null);
                }
            }
        }

        private RowObjectValidatorResponse ValidateUpdateExisting(List<RowObject> rowObjects, List<ColumnHeader> columnHeaders, List<AttributeModel> attributeModels)
        {
            var response = new RowObjectValidatorResponse();
            var scanCodeIndex = columnHeaders.First(c => c.Name == ScanCodeColumnHeader).ColumnIndex;
            var attributeColumns = columnHeaders
                .Join(attributeModels.Where(a => !a.IsReadOnly),
                    c => c.Name,
                    a => a.DisplayName,
                    (c, a) => new { a.IsRequired, ColumnHeader = c, AttributeValidator = itemAttributesValidatorFactory.CreateItemAttributesJsonValidator(a.AttributeName) })
                .ToList();
            var hierarchyColumns = columnHeaders
                .Join(HierarchyColumnNames,
                    c => c.Name,
                    h => h,
                    (c, h) => new { ColumnHeader = c, HierarchyName = h })
                .ToList();

            var rowObjectDictionaries = rowObjects
                .Select(r => new
                {
                    r.Row,
                    Cells = r.Cells.ToDictionary(
                        c => c.Column.ColumnIndex,
                        c => c.CellValue),
                    RowObject = r
                });

            var existingScanCodes = getScanCodesThatExistQueryHandler.Search(new GetScanCodesThatExistParameters
            {
                ScanCodes = rowObjectDictionaries
                    .Where(r => r.Cells.ContainsKey(scanCodeIndex)
                        && !string.IsNullOrEmpty(r.Cells[scanCodeIndex])
                        && r.Cells[scanCodeIndex].Length <= Icon.Common.Constants.ScanCodeMaxLength)
                    .Select(r => r.Cells[scanCodeIndex])
                    .ToList()
            });
            foreach (var rowObjectDictionary in rowObjectDictionaries)
            {
                List<InvalidRowError> errors = new List<InvalidRowError>();
                var scanCode = string.Empty;

                if (rowObjectDictionary.Cells.ContainsKey(scanCodeIndex))
                {
                    scanCode = rowObjectDictionary.Cells[scanCodeIndex];
                }

                if (!scanCodeValidator.Validate(scanCode))
                    errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{ScanCodeColumnHeader}' has invalid value. Scan Code is required, must be less than {Icon.Common.Constants.ScanCodeMaxLength} characters, must not start with 0, and must be numeric." });

                if (!existingScanCodes.Contains(scanCode))
                    errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{ScanCodeColumnHeader}' has invalid value. '{scanCode}' does not exist." });

                foreach (var attributeColumn in attributeColumns)
                {
                    if (rowObjectDictionary.Cells.ContainsKey(attributeColumn.ColumnHeader.ColumnIndex))
                    {
                        var value = rowObjectDictionary.Cells[attributeColumn.ColumnHeader.ColumnIndex];
                        if (value != string.Empty)
                        {
                            if (value == RemoveExcelValue)
                            {
                                if (attributeColumn.IsRequired)
                                {
                                    errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = $"'{attributeColumn.ColumnHeader.Name}' is required and can't be removed." });
                                }
                            }
                            else
                            {
                                var result = attributeColumn.AttributeValidator.Validate(value);
                                if (!result.IsValid)
                                {
                                    foreach (var errorMessage in result.ErrorMessages)
                                    {
                                        errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = errorMessage });
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var hierarchyColumn in hierarchyColumns)
                {
                    if (rowObjectDictionary.Cells.ContainsKey(hierarchyColumn.ColumnHeader.ColumnIndex))
                    {
                        var hierarchyValidatorResponse = hierarchyValidator.Validate(
                            hierarchyColumn.HierarchyName,
                            rowObjectDictionary.Cells[hierarchyColumn.ColumnHeader.ColumnIndex],
                            allowEmptyString: true,
                            allowRemove: true);
                        if (!hierarchyValidatorResponse.IsValid)
                        {
                            errors.Add(new InvalidRowError { RowId = rowObjectDictionary.Row, Error = hierarchyValidatorResponse.Error });
                        }
                    }
                }

                if (errors.Count > 0)
                    response.InvalidRows.AddRange(errors);
                else
                    response.ValidRows.Add(rowObjectDictionary.RowObject);
            }
            return response;
        }
    }
}
