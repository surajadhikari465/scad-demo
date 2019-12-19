using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class ItemsAreAbleToBeValidatedExcelValidator : IExcelValidator<ItemExcelModel>
    {
        private const string Error = "Row is invalid. Row is marked to be validated but the item does not contain all required fields to be validated.";
        private const string ExcelYes = "Y";
        private IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>> getScanCodesNotReadyToValidateQuery;

        public ItemsAreAbleToBeValidatedExcelValidator(IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>> getScanCodesNotReadyToValidateQuery)
        {
            this.getScanCodesNotReadyToValidateQuery = getScanCodesNotReadyToValidateQuery;
        }

        public void Validate(IEnumerable<ItemExcelModel> excelModels)
        {
            var importItems = excelModels
                .Where(m => m.Validated == ExcelYes)
                .Select(item => new BulkImportItemModel
                {
                    ScanCode = item.ScanCode,
                    ProductDescription = item.ProductDescription,
                    PosDescription = item.PosDescription,
                    PackageUnit = item.PackageUnit,
                    FoodStampEligible = item.FoodStampEligible,
                    PosScaleTare = item.PosScaleTare,
                    RetailSize = item.RetailSize,
                    RetailUom = item.Uom,
                    BrandId = item.Brand.GetIdFromCellText(),
                    BrowsingId = item.Browsing.GetIdFromCellText(),
                    MerchandiseId = item.Merchandise.GetIdFromCellText(),
                    TaxId = item.Tax.GetIdFromCellText(),
                    NationalId = item.NationalClass.GetIdFromCellText()
                }).ToList();

            var scanCodes = getScanCodesNotReadyToValidateQuery.Search(new GetScanCodesNotReadyToValidateParameters { Items = importItems });

            excelModels.AsParallel().Where(m => scanCodes.Contains(m.ScanCode))
                .ForAll(m => m.Error = Error);
        }
    }
}