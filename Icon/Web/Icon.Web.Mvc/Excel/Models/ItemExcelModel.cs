using Icon.Common;

namespace Icon.Web.Mvc.Excel.Models
{
    using Common.Utility;
    using DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;
    using static ExcelHelper;

    public class ItemExcelModel : ExcelModel
    {
        [ExcelColumn(ExcelExportColumnNames.ScanCode, ExcelExportColumnWidths.ScanCode)]
        public string ScanCode { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Brand, ExcelExportColumnWidths.Brand)]
        public string Brand { get; set; }

        [ExcelColumn(ExcelExportColumnNames.ProductDescription, ExcelExportColumnWidths.ProductDescription)]
        public string ProductDescription { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PosDescription, ExcelExportColumnWidths.PosDescription)]
        public string PosDescription { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PackageUnit, ExcelExportColumnWidths.PackageUnit)]
        public string PackageUnit { get; set; }

        [ExcelColumn(ExcelExportColumnNames.FoodStampEligible, ExcelExportColumnWidths.FoodStampEligible)]
        public string FoodStampEligible { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PosScaleTare, ExcelExportColumnWidths.PosScaleTare)]
        public string PosScaleTare { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Size, ExcelExportColumnWidths.Size)]
        public string RetailSize { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Uom, ExcelExportColumnWidths.Uom)]
        public string Uom { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DeliverySystem, ExcelExportColumnWidths.DeliverySystem)]
        public string DeliverySystem { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Merchandise, ExcelExportColumnWidths.Merchandise)]
        public string Merchandise { get; set; }

        [ExcelColumn(ExcelExportColumnNames.NationalClass, ExcelExportColumnWidths.NationalClass)]
        public string NationalClass { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Tax, ExcelExportColumnWidths.Tax)]
        public string Tax { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Browsing, ExcelExportColumnWidths.Browsing)]
        public string Browsing { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Validated, ExcelExportColumnWidths.Validated)]
        public string Validated { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DepartmentSale, ExcelExportColumnWidths.DepartmentSale)]
        public string DepartmentSale { get; set; }

        [ExcelColumn(ExcelExportColumnNames.HiddenItem, ExcelExportColumnWidths.HiddenItem)]
        public string HiddenItem { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Notes, ExcelExportColumnWidths.Notes)]
        public string Notes { get; set; }

        [ExcelColumn(ExcelExportColumnNames.AnimalWelfareRating, ExcelExportColumnWidths.AnimalWelfareRating)]
        public string AnimalWelfareRating { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Biodynamic, ExcelExportColumnWidths.Biodynamic)]
        public string Biodynamic { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CheeseAttributeMilkType, ExcelExportColumnWidths.CheeseAttributeMilkType)]
        public string CheeseAttributeMilkType { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CheeseAttributeRaw, ExcelExportColumnWidths.CheeseAttributeRaw)]
        public string CheeseAttributeRaw { get; set; }

        [ExcelColumn(ExcelExportColumnNames.EcoScaleRating, ExcelExportColumnWidths.EcoScaleRating)]
        public string EcoScaleRating { get; set; }

        [ExcelColumn(ExcelExportColumnNames.GlutenFree, ExcelExportColumnWidths.GlutenFree)]
        public string GlutenFree { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Kosher, ExcelExportColumnWidths.Kosher)]
        public string Kosher { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Msc, ExcelExportColumnWidths.Msc)]
        public string Msc { get; set; }

        [ExcelColumn(ExcelExportColumnNames.NonGmo, ExcelExportColumnWidths.NonGmo)]
        public string NonGmo { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Organic, ExcelExportColumnWidths.Organic)]
        public string Organic { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PremiumBodyCare, ExcelExportColumnWidths.PremiumBodyCare)]
        public string PremiumBodyCare { get; set; }

        [ExcelColumn(ExcelExportColumnNames.SeafoodFreshOrFrozen, ExcelExportColumnWidths.SeafoodFreshOrFrozen)]
        public string SeafoodFreshOrFrozen { get; set; }

        [ExcelColumn(ExcelExportColumnNames.SeafoodWildOrFarmRaised, ExcelExportColumnWidths.SeafoodWildOrFarmRaised)]
        public string SeafoodWildOrFarmRaised { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Vegan, ExcelExportColumnWidths.Vegan)]
        public string Vegan { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Vegetarian, ExcelExportColumnWidths.Vegetarian)]
        public string Vegetarian { get; set; }

        [ExcelColumn(ExcelExportColumnNames.WholeTrade, ExcelExportColumnWidths.WholeTrade)]
        public string WholeTrade { get; set; }

        [ExcelColumn(ExcelExportColumnNames.GrassFed, ExcelExportColumnWidths.GrassFed)]
        public string GrassFed { get; set; }

        [ExcelColumn(ExcelExportColumnNames.PastureRaised, ExcelExportColumnWidths.PastureRaised)]
        public string PastureRaised { get; set; }

        [ExcelColumn(ExcelExportColumnNames.FreeRange, ExcelExportColumnWidths.FreeRange)]
        public string FreeRange { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DryAged, ExcelExportColumnWidths.DryAged)]
        public string DryAged { get; set; }

        [ExcelColumn(ExcelExportColumnNames.AirChilled, ExcelExportColumnWidths.AirChilled)]
        public string AirChilled { get; set; }

        [ExcelColumn(ExcelExportColumnNames.MadeInHouse, ExcelExportColumnWidths.MadeInHouse)]
        public string MadeInHouse { get; set; }

        [ExcelColumn(ExcelExportColumnNames.AlcoholByVolume, ExcelExportColumnWidths.AlcoholByVolume)]
        public string AlcoholByVolume { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CreatedDate, ExcelExportColumnWidths.CreatedDate)]
        public string CreatedDate { get; set; }

        [ExcelColumn(ExcelExportColumnNames.LastModifiedDate, ExcelExportColumnWidths.LastModifiedDate)]
        public string LastModifiedDate { get; set; }

        [ExcelColumn(ExcelExportColumnNames.LastModifiedUser, ExcelExportColumnWidths.LastModifiedUser)]
        public string LastModifiedUser { get; set; }

        [ExcelColumn(ExcelExportColumnNames.CaseinFree, ExcelExportColumnWidths.CaseinFree)]
        public string CaseinFree { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DrainedWeight, ExcelExportColumnWidths.DrainedWeight)]
        public string DrainedWeight { get; set; }

        [ExcelColumn(ExcelExportColumnNames.DrainedWeightUom, ExcelExportColumnWidths.DrainedWeightUom)]
        public string DrainedWeightUom { get; set; }

        [ExcelColumn(ExcelExportColumnNames.FairTradeCertified, ExcelExportColumnWidths.FairTradeCertified)]
        public string FairTradeCertified { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Hemp, ExcelExportColumnWidths.Hemp)]
        public string Hemp { get; set; }

        [ExcelColumn(ExcelExportColumnNames.LocalLoanProducer, ExcelExportColumnWidths.LocalLoanProducer)]
        public string LocalLoanProducer { get; set; }

        [ExcelColumn(ExcelExportColumnNames.MainProductName, ExcelExportColumnWidths.MainProductName)]
        public string MainProductName { get; set; }

        [ExcelColumn(ExcelExportColumnNames.NutritionRequired, ExcelExportColumnWidths.NutritionRequired)]
        public string NutritionRequired { get; set; }

        [ExcelColumn(ExcelExportColumnNames.OrganicPersonalCare, ExcelExportColumnWidths.OrganicPersonalCare)]
        public string OrganicPersonalCare { get; set; }

        [ExcelColumn(ExcelExportColumnNames.Paleo, ExcelExportColumnWidths.Paleo)]
        public string Paleo { get; set; }

        [ExcelColumn(ExcelExportColumnNames.ProductFlavorType, ExcelExportColumnWidths.ProductFlavorType)]
        public string ProductFlavorType { get; set; }

        public ItemExcelModel()
        {
        }  

        public BulkImportItemModel ConvertToDataModel()
        {
            var item = new BulkImportItemModel
            {
                ScanCode = this.ScanCode,
                ProductDescription = this.ProductDescription,
                PosDescription = this.PosDescription,
                PackageUnit = this.PackageUnit,
                FoodStampEligible = this.FoodStampEligible.GetBoolStringFromCellText(),
                PosScaleTare = this.PosScaleTare,
                RetailSize = this.RetailSize,
                RetailUom = this.Uom,
                DeliverySystem = this.DeliverySystem,
                BrandId = this.Brand.GetIdFromCellText(),
                BrowsingId = this.Browsing.GetIdFromCellText(),
                MerchandiseId = this.Merchandise.GetIdFromCellText(),
                TaxId = this.Tax.GetIdFromCellText(),
                IsValidated = ConversionUtility.ConvertYesNoToDatabaseValue(this.Validated),
                DepartmentSale = ConversionUtility.ConvertYesNoToDatabaseValue(this.DepartmentSale),
                HiddenItem = ConversionUtility.ConvertYesNoToDatabaseValue(this.HiddenItem),
                NationalId = this.NationalClass.GetIdFromCellText(),
                Notes = this.Notes,
                AnimalWelfareRating = this.AnimalWelfareRating,
                Biodynamic = this.Biodynamic.GetBoolStringFromCellText(),
                CheeseAttributeMilkType = this.CheeseAttributeMilkType,
                CheeseAttributeRaw = this.CheeseAttributeRaw.GetBoolStringFromCellText(),
                EcoScaleRating = this.EcoScaleRating,
                GlutenFreeAgency = this.GetAgencyIdFromText(this.GlutenFree),
                KosherAgency = this.GetAgencyIdFromText(this.Kosher),
                Msc = this.Msc.GetBoolStringFromCellText(),
                NonGmoAgency = this.GetAgencyIdFromText(this.NonGmo),
                OrganicAgency = this.GetAgencyIdFromText(this.Organic),
                PremiumBodyCare = this.PremiumBodyCare.GetBoolStringFromCellText(),
                SeafoodFreshOrFrozen = this.SeafoodFreshOrFrozen,
                SeafoodWildOrFarmRaised = this.SeafoodWildOrFarmRaised,
                VeganAgency = this.GetAgencyIdFromText(this.Vegan),
                Vegetarian = this.Vegetarian.GetBoolStringFromCellText(),
                WholeTrade = this.WholeTrade.GetBoolStringFromCellText(),
                GrassFed = this.GrassFed.GetBoolStringFromCellText(),
                PastureRaised = this.PastureRaised.GetBoolStringFromCellText(),
                FreeRange = this.FreeRange.GetBoolStringFromCellText(),
                DryAged = this.DryAged.GetBoolStringFromCellText(),
                AirChilled = this.AirChilled.GetBoolStringFromCellText(),
                MadeInHouse = this.MadeInHouse.GetBoolStringFromCellText(),
                AlcoholByVolume = this.AlcoholByVolume,
                CaseinFree = this.CaseinFree.GetBoolStringFromCellText(),
                DrainedWeight = this.DrainedWeight,
                DrainedWeightUom = this.GetNullableStringValue(this.DrainedWeightUom),
                FairTradeCertified = this.GetNullableStringValue(this.FairTradeCertified),
                Hemp = this.Hemp.GetBoolStringFromCellText(),
                LocalLoanProducer = this.LocalLoanProducer.GetBoolStringFromCellText(),
                MainProductName = this.MainProductName,
                NutritionRequired = this.NutritionRequired.GetBoolStringFromCellText(),
                OrganicPersonalCare = this.OrganicPersonalCare.GetBoolStringFromCellText(),
                Paleo = this.Paleo.GetBoolStringFromCellText(),
                ProductFlavorType = this.ProductFlavorType
            };         

            return item;
        }

        private string GetNullableStringValue(string value)
        {
            if(value == Constants.ExcelImportRemoveValueKeyword)
            {
                return null;
            }
            else
            {
                return value;
            }
        }

        private string GetIdFromDescription(Dictionary<int, string> descriptions, string description)
        {
            if (description == Constants.ExcelImportRemoveValueKeyword) return null;

            var reverseDictionary = descriptions.ToDictionary(e => e.Value, e => e.Key);

            return reverseDictionary.ContainsKey(description) ? reverseDictionary[description].ToString() : string.Empty;
        }

        private string GetAgencyIdFromText(string agencyText)
        {
            return agencyText == Constants.ExcelImportRemoveValueKeyword ? null : agencyText.GetIdFromCellText();
        }
    }
}