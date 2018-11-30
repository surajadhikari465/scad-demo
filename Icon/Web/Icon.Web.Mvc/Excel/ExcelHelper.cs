using Icon.Framework;
using Icon.Web.Common;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;

namespace Icon.Web.Mvc.Excel
{
    public static class ExcelHelper
    {
        private static string[] trueFalseOrEmptyString = new string[] { null, "Y", "N" };

        [SecuritySafeCritical]
        public static void SendForDownload(HttpResponseBase response, Workbook document, WorkbookFormat excelFormat, string source)
        {
            string documentFileNameRoot = String.Format("IconExport_{0}_{1}.xlsx", source, DateTime.Now.ToString("yyyyMMddHHmmss"));

            response.Clear();
            response.AppendHeader("content-disposition", "attachment; filename=" + documentFileNameRoot);
            response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
            document.SetCurrentFormat(excelFormat);
            document.Save(response.OutputStream);
            response.End();
        }
       
        public static string[] GetExcelValidationValues(string trait, bool includeRemoveOption)
        {
            switch (trait)
            {
                case "Food Stamp Eligible":
                case "Department Sale":
                case "Hidden Item":
                case "Validated":
                case "Biodynamic":
                case "Cheese Attribute: Raw":
                case "Premium Body Care":
                case "Vegetarian":
                case "Whole Trade":
                case "Grass Fed":
                case "Pasture Raised":
                case "Free Range":
                case "Dry Aged":
                case "Air Chilled":
                case "Made In House":
                case "MSC":
                case "Casein Free":
                case "Hemp":
                case "Local Loan Producer":
                case "Nutrition Required":
                case "Organic Personal Care":
                case "Paleo":
                case "Animal Welfare Rating":
                case "Cheese Attribute: Milk Type":
                case "Eco-Scale Rating":
                case "Fresh Or Frozen":
                case "Seafood: Wild Or Farm Raised":
                case "Delivery System":
                case "Drained Weight Uom":
                    return trueFalseOrEmptyString;
                    
                default:
                    throw new ArgumentException(String.Format("{0} is not a known trait.", trait));
            }
        }

        public static class IrmaItemColumnIndexes
        {
            public const int ScanCodeColumnIndex = 0;
            public const int BrandColumnIndex = 1;
            public const int ProductDescriptionColumnIndex = 2;
            public const int PosDescriptionColumnIndex = 3;
            public const int PackageUnitColumnIndex = 4;
            public const int FoodStampEligibleColumnIndex = 5;
            public const int PosScaleTareColumnIndex = 6;
            public const int SizeColumnIndex = 7;
            public const int UomColumnIndex = 8;
            public const int DeliverySystemColumnIndex = 9;
            public const int IrmaSubTeamColumnIndex = 10;
            public const int MerchandiseColumnIndex = 11;
            public const int TaxColumnIndex = 12;
            public const int AlcoholByVolumeIndex = 13;
            public const int NationalColumnIndex = 14;
            public const int BrowsingColumnIndex = 15;
            public const int ValidatedColumnIndex = 16;
            public const int RegionCodeColumnIndex = 17;
            public const int AirChilledColumnIndex = 18;
            public const int AnimalWelfareRatingColumnIndex = 19;
            public const int BiodynamicColumnIndex = 20;
            public const int CheeseAttributeMilkTypeColumnIndex = 21;
            public const int CheeseAttributeRawColumnIndex = 22;
            public const int DryAgedColumnIndex = 23;
            public const int EcoScaleRatingColumnIndex = 24;
            public const int FreeRangeColumnIndex = 25;
            public const int SeafoodFreshOrFrozenColumnIndex = 26;
            public const int GrassFedColumnIndex = 27;
            public const int MadeInHouseColumnIndex = 28;
            public const int MscColumnIndex = 29;
            public const int NutritionRequiredIndex = 30;
            public const int PastureRaisedColumnIndex = 31;
            public const int PremiumBodyCareColumnIndex = 32;           
            public const int SeafoodWildOrFarmRaisedColumnIndex = 33;
            public const int VegetarianColumnIndex = 34;
            public const int WholeTradeColumnIndex = 35;
            public const int ErrorColumnIndex = 36;
        }

        public static class ExcelExportColumnNames
        {
            public const string ScanCode = "Scan Code";
            public const string Brand = "Brand";
            public const string ProductDescription = "Product Description";
            public const string PosDescription = "POS Description";
            public const string PackageUnit = "Item Pack";
            public const string FoodStampEligible = "Food Stamp Eligible";
            public const string PosScaleTare = "POS Scale Tare";
            public const string Size = "Size";
            public const string Uom = "UOM";
            public const string DeliverySystem = "Delivery System";
            public const string IrmaSubTeam = "IRMA SubTeam";
            public const string Merchandise = "Merchandise";
            public const string NationalClass = "National Class";
            public const string Tax = "Tax";
            public const string Browsing = "Browsing";
            public const string Validated = "Validated";
            public const string DepartmentSale = "Department Sale";
            public const string HiddenItem = "Hidden Item";
            public const string Notes = "Notes";
            public const string Region = "Region";
            public const string AnimalWelfareRating = "Animal Welfare Rating";
            public const string Biodynamic = "Biodynamic";
            public const string CheeseAttributeMilkType = "Cheese Attribute: Milk Type";
            public const string CheeseAttributeRaw = "Cheese Attribute: Raw";
            public const string EcoScaleRating = "Eco-Scale Rating";
            public const string GlutenFree = "Gluten-Free";
            public const string Kosher = "Kosher";
            public const string Msc = "MSC";
            public const string NonGmo = "Non-GMO";
            public const string Organic = "Organic";
            public const string PremiumBodyCare = "Premium Body Care";
            public const string SeafoodFreshOrFrozen = "Fresh Or Frozen";
            public const string SeafoodWildOrFarmRaised = "Seafood: Wild Or Farm Raised";
            public const string Vegan = "Vegan";
            public const string Vegetarian = "Vegetarian";
            public const string WholeTrade = "Whole Trade";
            public const string GrassFed = "Grass Fed";
            public const string PastureRaised = "Pasture Raised";
            public const string FreeRange = "Free Range";
            public const string DryAged = "Dry Aged";
            public const string AirChilled = "Air Chilled";
            public const string MadeInHouse = "Made In House";
            public const string AlcoholByVolume = "Alcohol By Volume";
            public const string CaseinFree = "Casein Free";
            public const string DrainedWeight = "Drained Weight";
            public const string DrainedWeightUom = "Drained Weight UOM";
            public const string FairTradeCertified = "Fair Trade Certified";
            public const string Hemp = "Hemp";
            public const string LocalLoanProducer = "Local Loan Producer";
            public const string MainProductName = "Main Product Name";
            public const string NutritionRequired = "Nutrition Required";
            public const string OrganicPersonalCare = "Organic Personal Care";
            public const string Paleo = "Paleo";
            public const string ProductFlavorType = "Product Flavor Type";
            public const string CreatedDate = "Created Date";
            public const string LastModifiedDate = "Modified";
            public const string LastModifiedUser = "Modifier";
            public const string Error = "Error";
        }

        public static class DefaultTaxMismatchColumnNames
        {
            public const string DefaultTaxClass = "Default Tax Class";
            public const string TaxClassOverride = "Tax Class Override";
        }

        public static class ExcelExportColumnWidths
        {
            public const int ScanCode = 4000;
            public const int Brand = 8000;
            public const int ProductDescription = 14000;
            public const int PosDescription = 9000;
            public const int PackageUnit = 3500;
            public const int FoodStampEligible = 5000;
            public const int PosScaleTare = 3500;
            public const int Size = 3500;
            public const int Uom = 3500;
            public const int DeliverySystem = 3500;
            public const int IrmaSubTeam = 8000;
            public const int Merchandise = 27000;
            public const int NationalClass = 15000;
            public const int Tax = 15000;
            public const int Browsing = 5000;
            public const int Validated = 5000;
            public const int DepartmentSale = 5000;
            public const int HiddenItem = 5000;
            public const int Notes = 7000;
            public const int Region = 5000;
            public const int AnimalWelfareRating = 8000;
            public const int Biodynamic = 4000;
            public const int CheeseAttributeMilkType = 8000;
            public const int CheeseAttributeRaw = 8000;
            public const int EcoScaleRating = 7000;
            public const int GlutenFree = 7000;
            public const int Kosher = 7000;
            public const int Msc = 5000;
            public const int NonGmo = 7000;
            public const int Organic = 7000;
            public const int PremiumBodyCare = 5000;
            public const int SeafoodFreshOrFrozen = 7000;
            public const int SeafoodWildOrFarmRaised = 8000;
            public const int Vegan = 7000;
            public const int Vegetarian = 5000;
            public const int WholeTrade = 5000;
            public const int GrassFed = 5000;
            public const int PastureRaised = 5000;
            public const int FreeRange = 5000;
            public const int DryAged = 5000;
            public const int AirChilled = 5000;
            public const int MadeInHouse = 5000;
            public const int AlcoholByVolume = 5000;
            public const int CaseinFree = 3000;
            public const int DrainedWeight = 5000;
            public const int DrainedWeightUom = 5000;
            public const int FairTradeCertified = 5000;
            public const int Hemp = 2000;
            public const int LocalLoanProducer = 5000;
            public const int MainProductName = 5000;
            public const int NutritionRequired = 5000;
            public const int OrganicPersonalCare = 5000;
            public const int Paleo = 5000;
            public const int ProductFlavorType = 5000;
            public const int CreatedDate = 3500;
            public const int LastModifiedDate = 3500;
            public const int LastModifiedUser = 3500;
            public const int Error = 15000;
        }

        public static class ConsolidatedItemColumnIndexes
        {
            public const int ScanCodeColumnIndex = 0;
            public const int BrandColumnIndex = 1;
            public const int ProductDescriptionColumnIndex = 2;
            public const int PosDescriptionColumnIndex = 3;
            public const int PackageUnitColumnIndex = 4;
            public const int FoodStampEligibleColumnIndex = 5;
            public const int PosScaleTareColumnIndex = 6;
            public const int SizeColumnIndex = 7;
            public const int UomColumnIndex = 8;
            public const int DeliverySystemColumnIndex = 9;
            public const int MerchandiseColumnIndex = 10;
            public const int TaxColumnIndex = 11;
            public const int AlcoholByVolumeColumnIndex = 12;
            public const int NationalColumnIndex = 13;
            public const int BrowsingColumnIndex = 14;
            public const int ValidatedColumnIndex = 15;
            public const int HiddenItemColumnIndex = 16;
            public const int DepartmentSaleColumnIndex = 17;
            public const int NotesColumnIndex = 18;
            public const int AirChilledColumnIndex = 19;
            public const int AnimalWelfareRatingColumnIndex = 20;
            public const int BiodynamicColumnIndex = 21;
            public const int CheeseAttributeMilkTypeColumnIndex = 22;
            public const int CheeseAttributeRawColumnIndex = 23;
            public const int DryAgedColumnIndex = 24;
            public const int EcoScaleRatingColumnIndex = 25;
            public const int FreeRangeColumnIndex = 26;
            public const int SeafoodFreshOrFrozenColumnIndex = 27;
            public const int GrassFedColumnIndex = 28;
            public const int MadeInHouseColumnIndex = 29;
            public const int MscColumnIndex = 30;
            public const int PastureRaisedColumnIndex = 31;
            public const int PremiumBodyCareColumnIndex = 32;            
            public const int SeafoodWildOrFarmRaisedColumnIndex = 33;
            public const int VegetarianColumnIndex = 34;
            public const int WholeTradeColumnIndex = 35;
            public const int CaseinFreeColumnIndex = 36;
            public const int DrainedWeightColumnIndex = 37;
            public const int DrainedWeightUomColumnIndex = 38;
            public const int FairTradeCertifiedColumnIndex = 39;
            public const int HempColumnIndex = 40;
            public const int LocalLoanProducerColumnIndex = 41;
            public const int MainProductNameColumnIndex = 42;
            public const int NutritionRequiredColumnIndex = 43;
            public const int OrganicPersonalCareColumnIndex = 44;
            public const int PaleoColumnIndex = 45;
            public const int ProductFlavorTypeColumnIndex = 46;
            public const int CreatedDateColumnIndex = 47;
            public const int LastModifiedDateColumnIndex = 48;
            public const int LastModifiedUserColumnIndex = 49;
            public const int ErrorColumnIndex = 50;
        }

        public static class DefaultTaxMismatchesColumnIndexes
        {
            public const int ScanCodeColumnIndex = 0;
            public const int BrandColumnIndex = 1;
            public const int ProductDescriptionColumnIndex = 2;
            public const int MerchandiseLineage = 3;
            public const int DefaultTaxClassColumnIndex = 4;
            public const int TaxClassOverrideColumnIndex = 5;
            public const int ErrorColumnIndex = 6;
        }

        public static class DefaultTaxMismatchesColumnNames
        {
            public const string DefaultTaxClass = "Default Tax Class";
            public const string TaxClassOverride = "Tax Class Override";
        }

        public static string GetCellStringValue(object cellValue)
        {
            return cellValue == null ? String.Empty : cellValue.ToString();
        }

        public static string GetBoolStringFromCellText(this string cellText)
        {
            if (String.Equals(cellText, "Y", StringComparison.InvariantCultureIgnoreCase))
            {
                return "1";
            }
            else if (String.Equals(cellText, "N", StringComparison.InvariantCultureIgnoreCase))
            {
                return "0";
            }
            else
            {
                return cellText;
            }
        }

        public static string ToSpreadsheetBoolean(this string value)
        {
            if (String.Equals(value, "1", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Y";
            }
            else if (String.Equals(value, "0", StringComparison.InvariantCultureIgnoreCase))
            {
                return "N";
            }
            else
            {
                return value;
            }
        }

        public static string ToSpreadsheetBoolean(this bool? value)
        {
            if (value.HasValue && value.Value)
            {
                return "Y";
            }
            else if (value.HasValue && !value.Value)
            {
                return "N";
            }
            else
            {
                return String.Empty;
            }
        }

        public static string GetIdFromCellText(this string cellText)
        {
            return cellText.Split('|').First().Trim();
        }

        public static bool IsDigitsOnly(this string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            else
            {
                foreach (var c in value)
                {
                    if (!Char.IsDigit(c))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static string GetValueFromDictionary(Dictionary<int, string> dictionary, int? id)
        {
            if (id.HasValue)
            {
                if (dictionary.ContainsKey(id.Value))
                {
                    return dictionary[id.Value];
                }
            }

            return String.Empty;
        }
    }
}