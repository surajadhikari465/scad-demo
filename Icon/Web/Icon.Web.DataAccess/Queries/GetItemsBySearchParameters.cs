using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Attributes;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsBySearchParameters : IQuery<ItemsBySearchResultsModel>
    {
        [SqlParameter(ParamName = "scanCode", SqlDbType = SqlDbType.VarChar)]
        public string ScanCode { get; set; }

        [SqlParameter(ParamName = "brandName", SqlDbType = SqlDbType.VarChar)]
        public string BrandName { get; set; }

        [SqlParameter(ParamName = "partialBrandName", SqlDbType = SqlDbType.Bit)]
        public bool PartialBrandName { get; set; }

        [SqlParameter(ParamName = "productDescription", SqlDbType = SqlDbType.VarChar)]
        public string ProductDescription { get; set; }

        [SqlParameter(ParamName = "retailSize", SqlDbType = SqlDbType.VarChar)]
        public string RetailSize { get; set; }

        [SqlParameter(ParamName = "retailUom", SqlDbType = SqlDbType.VarChar)]
        public string RetailUom { get; set; }

        [SqlParameter(ParamName = "deliverySystem", SqlDbType = SqlDbType.VarChar)]
        public string DeliverySystem { get; set; }

        [SqlParameter(ParamName = "merchandiseHierarchy", SqlDbType = SqlDbType.VarChar)]
        public string MerchandiseHierarchy { get; set; }

        [SqlParameter(ParamName = "taxRomanceName", SqlDbType = SqlDbType.VarChar)]
        public string TaxRomance { get; set; }

        [SqlParameter(ParamName = "itemStatus", SqlDbType = SqlDbType.VarChar)]
        public SearchStatus SearchStatus { get; set; }

        [SqlParameter(ParamName = "posDescription", SqlDbType = SqlDbType.VarChar)]
        public string PosDescription { get; set; }

        [SqlParameter(ParamName = "foodStampEligible", SqlDbType = SqlDbType.VarChar)]
        public string FoodStampEligible { get; set; }

        [SqlParameter(ParamName = "departmentSale", SqlDbType = SqlDbType.VarChar)]
        public string DepartmentSale { get; set; }

        [SqlParameter(ParamName = "posScaleTare", SqlDbType = SqlDbType.VarChar)]
        public string PosScaleTare { get; set; }

        [SqlParameter(ParamName = "pkgUnit", SqlDbType = SqlDbType.VarChar)]
        public string PackageUnit { get; set; }

        [SqlParameter(ParamName = "partialScanCodeSearch", SqlDbType = SqlDbType.Bit)]
        public bool PartialScanCode { get; set; }

        [SqlParameter(ParamName = "hiddenItemStatus", SqlDbType = SqlDbType.VarChar)]
        public HiddenStatus HiddenItemStatus { get; set; }

        [SqlParameter(ParamName = "nationalHierarchy", SqlDbType = SqlDbType.VarChar)]
        public string NationalClass { get; set; }

        [SqlParameter(ParamName = "animalWelfareRating", SqlDbType = SqlDbType.VarChar)]
        public string AnimalWelfareRating { get; set; }

        [SqlParameter(ParamName = "biodynamic", SqlDbType = SqlDbType.VarChar)]
        public string Biodynamic { get; set; }

        [SqlParameter(ParamName = "milkType", SqlDbType = SqlDbType.VarChar)]
        public string MilkType { get; set; }

        [SqlParameter(ParamName = "cheeseRaw", SqlDbType = SqlDbType.VarChar)]
        public string CheeseRaw { get; set; }

        [SqlParameter(ParamName = "ecoScaleRating", SqlDbType = SqlDbType.VarChar)]
        public string EcoScaleRating { get; set; }

        [SqlParameter(ParamName = "glutenFreeAgency", SqlDbType = SqlDbType.VarChar)]
        public string GlutenFreeAgency { get; set; }

        [SqlParameter(ParamName = "isMsc", SqlDbType = SqlDbType.VarChar)]
        public string Msc { get; set; }

        [SqlParameter(ParamName = "kosherAgency", SqlDbType = SqlDbType.VarChar)]
        public string KosherAgency { get; set; }

        [SqlParameter(ParamName = "nonGmoAgency", SqlDbType = SqlDbType.VarChar)]
        public string NonGmoAgency { get; set; }

        [SqlParameter(ParamName = "organicAgency", SqlDbType = SqlDbType.VarChar)]
        public string OrganicAgency { get; set; }

        [SqlParameter(ParamName = "premiumBodyCare", SqlDbType = SqlDbType.VarChar)]
        public string PremiumBodyCare { get; set; }

        [SqlParameter(ParamName = "seafoodFreshOrFrozen", SqlDbType = SqlDbType.VarChar)]
        public string SeafoodFreshOrFrozen { get; set; }

        [SqlParameter(ParamName = "seafoodCatchType", SqlDbType = SqlDbType.VarChar)]
        public string SeafoodCatchType { get; set; }

        [SqlParameter(ParamName = "veganAgency", SqlDbType = SqlDbType.VarChar)]
        public string VeganAgency { get; set; }

        [SqlParameter(ParamName = "vegetarian", SqlDbType = SqlDbType.VarChar)]
        public string Vegetarian { get; set; }

        [SqlParameter(ParamName = "wholeTrade", SqlDbType = SqlDbType.VarChar)]
        public string WholeTrade { get; set; }

        [SqlParameter(ParamName = "notes", SqlDbType = SqlDbType.VarChar)]
        public string Notes { get; set; }

        [SqlParameter(ParamName = "isGrassFed", SqlDbType = SqlDbType.VarChar)]
        public string GrassFed { get; set; }

        [SqlParameter(ParamName = "isPastureRaised", SqlDbType = SqlDbType.VarChar)]
        public string PastureRaised { get; set; }

        [SqlParameter(ParamName = "isFreeRange", SqlDbType = SqlDbType.VarChar)]
        public string FreeRange { get; set; }

        [SqlParameter(ParamName = "isDryAged", SqlDbType = SqlDbType.VarChar)]
        public string DryAged { get; set; }

        [SqlParameter(ParamName = "isAirChilled", SqlDbType = SqlDbType.VarChar)]
        public string AirChilled { get; set; }

        [SqlParameter(ParamName = "isMadeInHouse", SqlDbType = SqlDbType.VarChar)]
        public string MadeInHouse { get; set; }

        [SqlParameter(ParamName = "createDate", SqlDbType = SqlDbType.VarChar)]
        public string CreatedDate { get; set; }

        [SqlParameter(ParamName = "modifiedDate", SqlDbType = SqlDbType.VarChar)]
        public string LastModifiedDate { get; set; }

        [SqlParameter(ParamName = "modifiedUser", SqlDbType = SqlDbType.VarChar)]
        public string LastModifiedUser { get; set; }

        [SqlParameter(ParamName = "pageSize", SqlDbType = SqlDbType.Int)]
        public int? PageSize { get; set; }

        [SqlParameter(ParamName = "pageIndex", SqlDbType = SqlDbType.Int)]
        public int? PageIndex { get; set; }

        [SqlParameter(ParamName = "getOnlyCount", SqlDbType = SqlDbType.Bit)]
        public bool GetCountOnly { get; set; }

        [SqlParameter(ParamName = "sortOrder", SqlDbType = SqlDbType.VarChar)]
        public string SortOrder { get; set; }

        [SqlParameter(ParamName = "sortColumn", SqlDbType = SqlDbType.VarChar)]
        public string SortColumn { get; set; }

        [SqlParameter(ParamName = "cf", SqlDbType = SqlDbType.VarChar)]
        public string CaseinFree { get; set; }

        [SqlParameter(ParamName = "dw", SqlDbType = SqlDbType.VarChar)]
        public string DrainedWeight { get; set; }

        [SqlParameter(ParamName = "dwu", SqlDbType = SqlDbType.VarChar)]
        public string DrainedWeightUom { get; set; }

        [SqlParameter(ParamName = "nr", SqlDbType = SqlDbType.VarChar)]
        public string NutritionRequired { get; set; }

        [SqlParameter(ParamName = "opc", SqlDbType = SqlDbType.VarChar)]
        public string OrganicPersonalCare { get; set; }

        [SqlParameter(ParamName = "hem", SqlDbType = SqlDbType.VarChar)]
        public string Hemp { get; set; }

        [SqlParameter(ParamName = "ftc", SqlDbType = SqlDbType.VarChar)]
        public string FairTradeCertified { get; set; }

        [SqlParameter(ParamName = "abv", SqlDbType = SqlDbType.VarChar)]
        public string AlcoholByVolume { get; set; }

        [SqlParameter(ParamName = "mpn", SqlDbType = SqlDbType.VarChar)]
        public string MainProductName { get; set; }

        [SqlParameter(ParamName = "pft", SqlDbType = SqlDbType.VarChar)]
        public string ProductFlavorType { get; set; }
        [SqlParameter(ParamName = "plo", SqlDbType = SqlDbType.VarChar)]
        public string Paleo { get; set; }
        [SqlParameter(ParamName = "llp", SqlDbType = SqlDbType.VarChar)]
        public string LocalLoanProducer { get; set; }
    }
}
