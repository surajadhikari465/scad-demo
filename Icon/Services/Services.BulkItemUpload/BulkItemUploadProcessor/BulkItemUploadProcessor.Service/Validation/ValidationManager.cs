using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.Interfaces;
using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Common.Validators.ItemAttributes;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BulkItemUploadProcessor.Service
{
    public class ValidationManager : IValidationManager
    {
        private readonly IItemAttributesValidatorFactory ItemAttributesValidatorFactory;
        private readonly IQueryHandler<DoesScanCodeExistParameters, bool> DoesScanCodeExistQueryHandler;
        private readonly IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>> GetBarcodeTypeQueryHandler;
        private readonly IHierarchyCache HierarchyCache;
        private readonly IMerchItemPropertiesCache MerchItemPropertiesCache;
        private readonly IQueryHandler<GetItemParameters, ItemDbModel> GetItemQueryHandler;

        private readonly IValidator<ExcelPackage> excelPackageValidator;

        public ValidationManager(
            IItemAttributesValidatorFactory itemAttributesValidatorFactory,
            IQueryHandler<DoesScanCodeExistParameters, bool> doesScanCodeExistQueryHandler,
            IQueryHandler<EmptyQueryParameters<List<BarcodeTypeModel>>, List<BarcodeTypeModel>> getBarcodeTypeQueryHandler,
            IQueryHandler<GetItemParameters, ItemDbModel> getItemQueryHandler,
            IHierarchyCache hierarchyCache,
            IMerchItemPropertiesCache merchItemPropertiesCache,
            IValidator<ExcelPackage> excelPackageValidator)
        {
            ItemAttributesValidatorFactory = itemAttributesValidatorFactory;
            DoesScanCodeExistQueryHandler = doesScanCodeExistQueryHandler;
            GetBarcodeTypeQueryHandler = getBarcodeTypeQueryHandler;
            GetItemQueryHandler = getItemQueryHandler;
            HierarchyCache = hierarchyCache;
            MerchItemPropertiesCache = merchItemPropertiesCache;
            this.excelPackageValidator = excelPackageValidator;
        }

        public void ValidateFile(ExcelPackage excelFile)
        {
            excelPackageValidator.ValidateAndThrow(excelFile);
        }

        public void ValidateNewItemViewModels(List<NewItemViewModel> items, Enums.FileModeTypeEnum fileModeType)
        {
            var barcodeTypes = GetBarcodeTypeQueryHandler.Search(new EmptyQueryParameters<List<BarcodeTypeModel>>());
            HierarchyCache.Refresh();

            foreach (var item in items)
            {
                ValidateSingleItemAdd(item, barcodeTypes);
            }
        }

        public void ValidateEditItemViewModels(List<EditItemViewModel> items, Enums.FileModeTypeEnum fileModeType)
        {
            var barcodeTypes = GetBarcodeTypeQueryHandler.Search(new EmptyQueryParameters<List<BarcodeTypeModel>>());
            HierarchyCache.Refresh();
            MerchItemPropertiesCache.Refresh();

            foreach (var item in items)
            {
                ValidateSingleItemUpload(item, barcodeTypes);
            }
        }

        public Dictionary<string, string> OverlayExistingItemAttributes(EditItemViewModel uploadedItem, ItemDbModel existingItem)
        {
            var mergedItemAttributes = uploadedItem.ValuesToUpdate;
            var existingItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(existingItem.ItemAttributesJson);
            var missingAttributes = from e in existingItemAttributes
                                    where !uploadedItem.ValuesToUpdate.ContainsKey(e.Key)
                                    select e;

            foreach (var missingAttribute in missingAttributes)
            {
                mergedItemAttributes.Add(missingAttribute.Key, missingAttribute.Value);
            }
            return mergedItemAttributes;
        }

        public void ValidateSingleItemUpload(EditItemViewModel item, List<BarcodeTypeModel> barcodeTypes)
        {
            try
            {
                if (item.ScanCodeIncluded)
                {
                    var ignoreKeys = new string[] { };

                    var attributeValidationErrors = item.ValuesToUpdate
                        .Where(i => !ignoreKeys.Contains(i.Key))
                        .Select(kvp => ItemAttributesValidatorFactory.CreateItemAttributesJsonValidator(kvp.Key).Validate(kvp.Value))
                        .Where(r => !r.IsValid)
                        .SelectMany(r => r.ErrorMessages)
                        .ToList();

                    item.Errors.AddRange(attributeValidationErrors);

                    var existingItem = GetItemQueryHandler.Search(new GetItemParameters { ScanCode = item.ScanCode });
                    item.ItemId = existingItem.ItemId;
                    item.MergedItemAttributes = OverlayExistingItemAttributes(item, existingItem);

                    // if value is "remove", remove it. 
                    foreach (var mergedItemAttribute in item.MergedItemAttributes)
                    {
                        if (mergedItemAttribute.Value.ToLower() == "remove")
                            item.MergedItemAttributes.Remove(mergedItemAttribute.Key);
                    }

                    item.MergedItemAttributes = RemoveAttributesToBeDeleted(item.MergedItemAttributes);

                    if (item.MerchandiseHierarchyClassIdIncluded)
                    {
                        if (!string.IsNullOrWhiteSpace(item.MerchandiseHierarchyClassId))
                            if (MerchItemPropertiesCache.Properties.ContainsKey(item.ItemId))
                            {
                                var itemProperties = MerchItemPropertiesCache.Properties[item.ItemId];
                                //todo:  apply item properties

                            }
                            else
                            {
                                item.Errors.Add("Merchandise Hierarchy was included but releated Item Properties could not be found.");
                            }
                    }
                }
                else
                {
                    item.Errors.Add("ScanCode is requried when updating an item.");
                }
            }
            catch (Exception ex)
            {
                item.Errors.Add(ex.Message);
            }
        }

        public Dictionary<string, string> RemoveAttributesToBeDeleted(Dictionary<string, string> attributesDictionary)
        {
            foreach (var attribute in attributesDictionary.Where(a => a.Value.ToLower() == "remove").ToList())
            {
                attributesDictionary.Remove(attribute.Key);
            }

            return attributesDictionary;
        }

        public void ValidateSingleItemAdd(NewItemViewModel item, List<BarcodeTypeModel> barcodeTypes)
        {
            try
            {
                var validBarcodeType = NumericGreaterThanZero(item.BarcodeTypeId);
                if (!validBarcodeType)
                {
                    var barcodeTypeIdLookup = barcodeTypes.FirstOrDefault(b => b.BarcodeType == item.BarcodeTypeName);
                    if (barcodeTypeIdLookup != null)
                    {
                        item.BarcodeTypeId = barcodeTypeIdLookup.BarcodeTypeId.ToString();
                        item.BarcodeType = barcodeTypeIdLookup.BarcodeType;
                        validBarcodeType = true;
                    }
                    else
                    {
                        item.Errors.Add($"Barcode Type Id is required. {item.BarcodeTypeName}");
                    }
                }
                if (validBarcodeType && string.IsNullOrWhiteSpace(item.BarcodeType))
                    item.Errors.Add($"Barcode Type is required. {item.BarcodeTypeName}");

                var validBrandHieararchy = NumericGreaterThanZero(item.BrandHierarchyClassId);
                if (!validBrandHieararchy)
                    item.Errors.Add("Brand Hierarchy Id is required.");

                if (validBrandHieararchy && !HierarchyCache.IsValidBrandHierarchyClassId(Convert.ToInt32(item.BrandHierarchyClassId)))
                    item.Errors.Add($"[{item.BrandHierarchyClassId}] is not a valid Brand HierarchyClassId");

                var validTaxHierarchy = NumericGreaterThanZero(item.TaxHierarchyClassId);
                if (!validTaxHierarchy) item.Errors.Add("Tax Hierarchy Id is required.");

                if (validTaxHierarchy && !HierarchyCache.IsValidTaxHierarchyClassId(Convert.ToInt32(item.TaxHierarchyClassId)))
                    item.Errors.Add($"[{item.TaxHierarchyClassId}] is not a valid Tax HierarchyClassId");

                if (!NumericGreaterThanZero(item.MerchandiseHierarchyClassId))
                    item.Errors.Add("Merchandise Hierarchy Id is required.");

                if (!HierarchyCache.IsValidMerchandiseHierarchyClassId(Convert.ToInt32(item.MerchandiseHierarchyClassId)))
                    item.Errors.Add($"[{item.MerchandiseHierarchyClassId}] is not a valid Merch HierarchyClassId");

                if (!NumericGreaterThanZero(item.NationalHierarchyClassId))
                    item.Errors.Add("National Class Hierarchy Id is required.");

                if (!HierarchyCache.IsValidNationalHierarchyClassId(Convert.ToInt32(item.NationalHierarchyClassId)))
                    item.Errors.Add($"[{item.NationalHierarchyClassId}] is not a valid National HierarchyClassId");


                // When barcode Type is UPC.
                if (item.BarcodeType.ToLower() == Icon.Common.Constants.Upc.ToLower())
                {
                    var scancodeProvided = !string.IsNullOrWhiteSpace(item.ScanCode);
                    if (!scancodeProvided)
                        item.Errors.Add("Scan Code is required when choosing UPC");


                    if (scancodeProvided)
                    {
                        var scancodeAlreadyExists = DoesScanCodeExistQueryHandler.Search(new DoesScanCodeExistParameters
                        { ScanCode = item.ScanCode });
                        var isUpcInBarcodeTypeRange = IsUpcInBarcodeTypeRanges(item.ScanCode, barcodeTypes);
                        var validUpcRegexMatch = new Regex("^[1-9]\\d{0,12}$").Match(item.ScanCode);

                        if (scancodeAlreadyExists)
                            item.Errors.Add(
                                $"'{item.ScanCode}' is already associated to an item. Please use another scan code");
                        if (isUpcInBarcodeTypeRange)
                            item.Errors.Add(
                                $"'{item.ScanCode}' exists in a Barcode Type range. Please enter a scan code not within a Barcode Type range.");
                        if (!validUpcRegexMatch.Success)
                            item.Errors.Add(
                                "Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.");
                    }
                }

                // When barcode type is NOT UPC
                if (item.BarcodeType != Icon.Common.Constants.Upc)
                {

                    var scancodeProvided = !string.IsNullOrWhiteSpace(item.ScanCode);

                    if (scancodeProvided)
                    {
                        var validUpcRegexMatch = new Regex("^[1-9]\\d{0,12}$").Match(item.ScanCode);
                        var doesScanCodeExist = DoesScanCodeExistQueryHandler.Search(new DoesScanCodeExistParameters { ScanCode = item.ScanCode });
                        var inSelectedBarcodeRange = IsScanCodeInSelectedBarcodeTypeRange(item.ScanCode, item.BarcodeTypeId, barcodeTypes);

                        if (doesScanCodeExist)
                            item.Errors.Add($"'{item.ScanCode}' is already associated to an item. Please use another scan code.");

                        if (!inSelectedBarcodeRange)
                            item.Errors.Add($"'{item.ScanCode}' should be in selected Barcode Type range. Please enter a scan code within selected Barcode Type range.");

                        if (!validUpcRegexMatch.Success)
                            item.Errors.Add("Scan Code must contain only digits, not start with a 0, and must be 1 to 13 characters long.");
                    }
                }

                item.ItemAttributes.Remove("ItemId");
                item.ItemAttributes.Remove("Scan Code");
                item.ItemAttributes.Remove("ScanCodeTypeId");
                item.ItemAttributes.Remove("Barcode Type");
                item.ItemAttributes.Remove("ScanCodeTypeDescription");
                item.ItemAttributes.Remove("ItemType");
                item.ItemAttributes.Remove("ItemTypeId");
                item.ItemAttributes.Remove("Item Type Description");
                item.ItemAttributes.Remove("MerchandiseHierarchyClassId");
                item.ItemAttributes.Remove("BrandsHierarchyClassId");
                item.ItemAttributes.Remove("TaxHierarchyClassId");
                item.ItemAttributes.Remove("FinancialHierarchyClassId");
                item.ItemAttributes.Remove("NationalHierarchyClassId");
                item.ItemAttributes.Remove("ManufacturerHierarchyClassId");
                item.ItemAttributes.Remove("Financial");
                item.ItemAttributes.Remove("Manufacturer");

                var attributeValidationErrors = item.ItemAttributes
                    .Select(kvp => ItemAttributesValidatorFactory.CreateItemAttributesJsonValidator(kvp.Key).Validate(kvp.Value))
                    .Where(r => !r.IsValid)
                    .SelectMany(r => r.ErrorMessages)
                    .ToList();

                item.Errors.AddRange(attributeValidationErrors);
            }
            catch (Exception ex)
            {
                item.Errors.Add(ex.Message);
            }
        }

        private bool IsScanCodeInSelectedBarcodeTypeRange(string scanCode, string barcodeTypeId, List<BarcodeTypeModel> barcodeTypes)
        {

            if (!string.IsNullOrWhiteSpace(barcodeTypeId)) return false;

            int intBarCodeTypeId;
            var validBarcodeTypeId = int.TryParse(barcodeTypeId, out intBarCodeTypeId);

            if (!validBarcodeTypeId) throw new Exception("BarcodeTypeId must be numeric");


            long scanCodeLong = 0;
            if (long.TryParse(scanCode, out scanCodeLong))
            {
                var barcodeSelected = barcodeTypes.Where(s => s.BarcodeTypeId == intBarCodeTypeId).FirstOrDefault();
                if (barcodeSelected == null) return false;

                long beginRange = long.Parse(barcodeSelected.BeginRange);
                long endRange = long.Parse(barcodeSelected.EndRange);
                if (scanCodeLong >= beginRange && scanCodeLong <= endRange)
                {
                    return true;
                }
            }

            return false;
        }

        private bool NumericGreaterThanZero(string input)
        {
            var returnVal = true;
            returnVal = int.TryParse(input, out var parsedVal);
            returnVal = parsedVal > 0 && returnVal;
            return returnVal;
        }

        public bool IsUpcInBarcodeTypeRanges(string upc, List<BarcodeTypeModel> barcodeTypes)
        {
            long upcLong = 0;

            if (upc.Length == 11 && upc.EndsWith("00000") && upc.StartsWith("2"))
            {
                return true;
            }

            if (long.TryParse(upc, out upcLong))
            {
                foreach (var barcodeType in barcodeTypes)
                {
                    long beginRange;
                    long endRange;

                    if (barcodeType.ScalePlu == true)
                    {
                        beginRange = long.Parse(barcodeType.BeginRange.Substring(0, barcodeType.BeginRange.Length - 5));
                        endRange = long.Parse(barcodeType.EndRange.Substring(0, barcodeType.EndRange.Length - 5));
                    }
                    else
                    {
                        beginRange = long.Parse(barcodeType.BeginRange);
                        endRange = long.Parse(barcodeType.EndRange);
                    }

                    if (upcLong >= beginRange && upcLong <= endRange)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}