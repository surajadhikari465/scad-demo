using MammothWebApi.Models;
using MammothWebApi.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.Extensions
{
    public static class Extensions
    {
        public static List<ItemLocaleServiceModel> ToItemLocaleServiceModel(this List<ItemLocaleModel> itemLocales)
        {
            List<ItemLocaleServiceModel> itemLocaleServiceModels = new List<ItemLocaleServiceModel>();
            foreach (var itemLocale in itemLocales)
            {
                var serviceModel = new ItemLocaleServiceModel
                {
                    AgeRestriction = itemLocale.AgeRestriction,
                    Authorized = itemLocale.Authorized.HasValue ? itemLocale.Authorized.Value : false,
                    BusinessUnitId = itemLocale.BusinessUnitId,
                    CaseDiscount = itemLocale.CaseDiscount.HasValue ? itemLocale.CaseDiscount.Value : false,
                    ChicagoBaby = itemLocale.ChicagoBaby,
                    ColorAdded = itemLocale.ColorAdded,
                    CountryOfProcessing = itemLocale.CountryOfProcessing,
                    Discontinued = itemLocale.Discontinued,
                    ElectronicShelfTag = (itemLocale.ElectronicShelfTag.HasValue && itemLocale.ElectronicShelfTag.Value) ? true : (bool?)null,
                    Exclusive = itemLocale.Exclusive,
                    LabelTypeDescription = itemLocale.LabelTypeDescription,
                    LinkedItem = itemLocale.LinkedItem,
                    LocalItem = itemLocale.LocalItem,
                    Locality = itemLocale.Locality,
                    NumberOfDigitsSentToScale = itemLocale.NumberOfDigitsSentToScale,
                    Origin = itemLocale.Origin,
                    ProductCode = itemLocale.ProductCode,
                    Region = itemLocale.Region,
                    RestrictedHours = itemLocale.RestrictedHours.HasValue ? itemLocale.RestrictedHours.Value : false,
                    RetailUnit = itemLocale.RetailUnit,
                    ScaleExtraText = itemLocale.ScaleExtraText,
                    ScanCode = itemLocale.ScanCode,
                    SignDescription = itemLocale.SignDescription,
                    SignRomanceLong = itemLocale.SignRomanceLong,
                    SignRomanceShort = itemLocale.SignRomanceShort,
                    TagUom = itemLocale.TagUom,
                    TMDiscount = itemLocale.TmDiscount.HasValue ? itemLocale.TmDiscount.Value : false,
                    Msrp = itemLocale.MSRP,
                    OrderedByInfor = itemLocale.OrderedByInfor,
                    AltRetailSize = itemLocale.AltRetailSize,
                    AltRetailUOM = itemLocale.AltRetailUOM,
                    DefaultScanCode = itemLocale.DefaultScanCode,
                    SupplierName = itemLocale.VendorCompanyName,
                    SupplierItemId = itemLocale.VendorItemId,
                    SupplierCaseSize = itemLocale.VendorCaseSize,
                    IrmaVendorKey = itemLocale.VendorKey,
                    ForceTare = itemLocale.ForceTare,
                    SendtoCFS = itemLocale.SendtoCFS,
                    WrappedTareWeight = itemLocale.WrappedTareWeight,
                    UnwrappedTareWeight = itemLocale.UnwrappedTareWeight,
                    ScaleItem = itemLocale.ScaleItem,
                    UseBy = itemLocale.UseBy,
                    ShelfLife = itemLocale.ShelfLife,
                    IrmaItemKey = itemLocale.IrmaItemKey,
                    PosScaleTare = itemLocale.PosScaleTare,
                    LockedForSale = itemLocale.LockedForSale
                };
                itemLocaleServiceModels.Add(serviceModel);
            }

            return itemLocaleServiceModels;
        }

        public static List<CancelAllSalesServiceModel> ToCancelAllSalesServiceModel(this List<CancelAllSalesModel> cancelAllSales)
        {
            List<CancelAllSalesServiceModel> cancelAllSalesServiceModelList = new List<CancelAllSalesServiceModel>();
            foreach (var cancelAllSale in cancelAllSales)
            {
                var cancelAllSalesServiceModel = new CancelAllSalesServiceModel
                {
                    Region = cancelAllSale.Region,
                    BusinessUnitId = cancelAllSale.BusinessUnitId,
                    ScanCode = cancelAllSale.ScanCode,
                    EndDate = cancelAllSale.EndDate,
                    EventCreatedDate = cancelAllSale.EventCreatedDate
                };

                cancelAllSalesServiceModelList.Add(cancelAllSalesServiceModel);
            }
            return cancelAllSalesServiceModelList;
        }

        public static List<PriceServiceModel> ToPriceServiceModel(this List<PriceModel> prices)
        {
            List<PriceServiceModel> priceServiceList = new List<PriceServiceModel>();
            foreach (var price in prices)
            {
                var serviceModel = new PriceServiceModel
                {
                    Region = price.Region,
                    ScanCode = price.ScanCode,
                    BusinessUnitId = price.BusinessUnitId,
                    Multiple = price.Multiple,
                    Price = price.Price,
                    StartDate = price.StartDate,
                    EndDate = price.EndDate?.Date.AddDays(1).AddMilliseconds(-3),
                    PriceType = price.PriceType,
                    PriceUom = price.PriceUom,
                    CurrencyCode = price.CurrencyCode
                };

                priceServiceList.Add(serviceModel);
            }

            return priceServiceList;
        }

        public static IEnumerable<StoreScanCodeServiceModel> ToStoreScanCodeServiceModel(this IEnumerable<PriceRequestModel> requests)
        {
            List<StoreScanCodeServiceModel> storeScanCodeCollection = new List<StoreScanCodeServiceModel>();

            foreach (var request in requests)
            {
                var serviceModel = new StoreScanCodeServiceModel
                {
                    BusinessUnitId = request.BusinessUnitId,
                    ScanCode = request.ScanCode
                };

                storeScanCodeCollection.Add(serviceModel);
            }

            return storeScanCodeCollection;
        }

        public static IEnumerable<StoreScanCodeServiceModel> ToStoreScanCodeServiceModel(this IEnumerable<StoreItem> storeItems)
        {
            return storeItems.Select(si => new StoreScanCodeServiceModel { BusinessUnitId = si.BusinessUnitId, ScanCode = si.ScanCode });
        }

        public static string ToLogString(this ItemLocaleModel itemLocaleModel)
        {
            return string.Format("ScanCode: {0}, BusinessUnit: {1}, Region: {2}", itemLocaleModel.ScanCode, itemLocaleModel.BusinessUnitId, itemLocaleModel.Region);
        }

        public static string ToLogString(this PriceModel priceModel)
        {
            return string.Format("ScanCode: {0}, BusinessUnit: {1}, Region: {2}", priceModel.ScanCode, priceModel.BusinessUnitId, priceModel.Region);
        }
    }
}