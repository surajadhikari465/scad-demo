using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using OfficeOpenXml;
using System.Collections.Generic;

namespace BulkItemUploadProcessor.Service.Interfaces
{
    public interface IValidationManager
    {
        void ValidateFile(ExcelPackage excelFile);
        void ValidateNewItemViewModels(List<NewItemViewModel> items, Enums.FileModeTypeEnum fileModeType);
        void ValidateEditItemViewModels(List<EditItemViewModel> items, Enums.FileModeTypeEnum fileModeType);
        bool IsUpcInBarcodeTypeRanges(string upc, List<BarcodeTypeModel> barcodeTypes);
        Dictionary<string, string> OverlayExistingItemAttributes(EditItemViewModel uploadedItem, ItemDbModel existingItem);
        void ValidateSingleItemUpload(EditItemViewModel item, List<BarcodeTypeModel> barcodeTypes);
        void ValidateSingleItemAdd(NewItemViewModel item, List<BarcodeTypeModel> barcodeTypes);
        Dictionary<string, string> RemoveAttributesToBeDeleted(Dictionary<string, string> attributesDictionary);
        
    }
}