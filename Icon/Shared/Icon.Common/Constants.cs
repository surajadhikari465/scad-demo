using System;

namespace Icon.Common
{
    public static class Constants
    {
        public const int IrmaBrandNameMaxLength = 25;
        public const int IconBrandNameMaxLength = 35;
        public const int ScanCodeMaxLength = 13;
        public const int RetailUomMaxLength = 15;
        public const int BusinessUnitLength = 5;
        public const int PackageUnitMaxLength = 3;
        public const int PosDescriptionMaxLength = 25;
        public const int PosScaleTareMax = 10;
        public const int PosScaleTareMaxLength = 5;
        public const int ProductDescriptionMaxLength = 60;
        public const double RetailSizeMax = 99999.9999;
        public const int PosPluMaxLength = 6;
        public const int ScalePluLength = 11;
        public const int BrandAbbreviationMaxLength = 10;
        public const int TaxAbbreviationMaxLength = 50;
        public const int TaxRomanceMaxLength = 150;
        public const int NotesMaxLength = 255;

        public const string UndefinedTaxName = "undefined";

        public const string ExcelImportRemoveValueKeyword = "None";
        public const string ExcelRemoveFieldIndicator = "0";

        public const string DefaultOrganicAgencyTraitValue = "Organic";
        public const string SpecialCharactersAll = "All";
        public const string SpecialCharactersAllRegexPattern = "^.*$";
        public const string SquareBracketOpen = "[";
        public const string SquareBracketClosed = "]";
        public const string PlusSign = "+";
        public const string AsteriskSign = "*";
        public const string CaretSign = "^";
        public const string DollarSign = "$";
        public const string Upc = "Upc";
        public const string Plu = "Plu";

        public const int TraitCodeMax = 10;
        public const int AttributeDisplayNameMax = 255;
        public const int NumberOfDecimalsMin = 0;
        public const int NumberOfDecimalsMax = 4;
        public const decimal MinimumNumberMin = -9999999999.9999m;
        public const decimal MinimumNumberMax = 9999999999.9999m;
        public const decimal MaximumNumberMin = -9999999999.9999m;
        public const decimal MaximumNumberMax = 9999999999.9999m;
        public const int MaxLengthAllowedMin = 1;
        public const int MaxLengthAllowedMax = 999999999;
        public const int MaxPickListValueLength = 50;

        public const string Brand = "Brand";
        public const string Tax = "Tax";
        public const string Merchandise = "Merchandise";
        public const string National = "National";
        public const string NationalClass = "NationalClass";
        public const string Manufacturer = "Manufacturer";
        public const string BrandsHierarchyClassId = "BrandsHierarchyClassId";
        public const string TaxHierarchyClassId = "TaxHierarchyClassId";
        public const string MerchandiseHierarchyClassId = "MerchandiseHierarchyClassId";
        public const string NationalHierarchyClassId = "NationalHierarchyClassId";
        
        public const string ManufacturerHierarchyClassId = "ManufacturerHierarchyClassId";
        public const int ManufacturerNameMaxLength = 255;

        public static class DataTypeNames
        {
            public const string Text = "Text";
            public const string Number = "Number";
            public const string Boolean = "Boolean";
            public const string Date = "Date";
        }

        /// <summary>
        /// Contains names of Attributes that either don't live on the Attributes table
        /// in the Icon database or that require special processing.
        /// </summary>
        public static class Attributes
        {
            public const string ScanCode = "ScanCode";
            public const string CreatedBy = "CreatedBy";
            public const string CreatedDateTimeUtc = "CreatedDateTimeUtc";
            public const string ModifiedBy = "ModifiedBy";
            public const string ModifiedDateTimeUtc = "ModifiedDateTimeUtc";
            public const string ProhibitDiscount = "ProhibitDiscount";
            public const string Organic = "Organic";

            // ItemTypeCode is not really an attribute in the attributes table. Item type is 
            // stored on the item record so we piggy back on the attributes sent to the client. There 
            // is special logic in the view for this attribute to make it display correctly to the client. 
            public const string ItemTypeCode = "ItemTypeCode";
        }


        public static class ValidatorPropertyNames
        {
            public const string ProductDescription = "ProductDescription";
            public const string PosDescription = "PosDescription";
            public const string TaxAbbreviation = "TaxAbbreviation";
            public const string BrandAbbreviation = "BrandAbbreviation";
            public const string BrandName = "BrandName";
            public const string TaxRomance = "TaxRomance";
            public const string MaxPlus = "MaxPlus";
            public const string CertificationAgencyName = "AgencyName";
            public const string Notes = "Notes";
            public const string CreatedDate = "CreatedDate";
            public const string ModifiedDate = "ModifiedDate";
            public const string ModifiedUser = "ModifieUser";
            public const string RetailSize = "RetailSize";
            public const string AlcoholByVolume = "AlcoholByVolume";
            public const string DrainedWeightUom = "DrainedWeightUom";
            public const string DrainedWeight = "DrainedWeight";
            public const string FairTradeCertified = "FairTradeCertified";
            public const string MainProductName = "MainProductName";
            public const string ProductFlavorType = "ProductFlavorType";
        }

        public static class ValidatorErrorMessages
        {
            public static readonly string ProductDescriptionError = String.Format("Please enter {0} or fewer valid characters.", Constants.ProductDescriptionMaxLength);
            public static readonly string PosDescriptionError = String.Format("Please enter {0} or fewer valid characters.", Constants.PosDescriptionMaxLength);
            public static readonly string TaxAbbreviationError = String.Format("The tax abbreviation must start with the 7 digit numerical tax code followed by a space and may contain {0} or fewer letters, numbers, or the following characters: % & ( ) - $.", Constants.TaxAbbreviationMaxLength);
            public static readonly string BrandAbbreviationError = String.Format("Please enter {0} or fewer valid characters (Letters, Numbers, and Ampersands only).", Constants.BrandAbbreviationMaxLength);
            public static readonly string BrandNameError = String.Format("Please enter {0} or fewer valid characters.", Constants.IconBrandNameMaxLength);
            public static readonly string HierarchyClassNameError = "Please enter only valid characters.";
            public static readonly string TaxRomanceError = String.Format("The Tax Romance is required and may contain {0} or fewer letters, numbers, or the following characters: % & ( ) - $.", Constants.TaxRomanceMaxLength);
            public static readonly string MaxPlusError = String.Format("Number of PLUs must be a number");
            public static readonly string AgencyNameError = String.Format("Agency Name should contain no more than 255 characters.");
            public static readonly string NotesError = String.Format("Please enter {0} or fewer valid characters.", Constants.NotesMaxLength);
            public static readonly string DateFormatError = String.Format("Please enter date in mm/dd/yyyy format.");
            public static readonly string UserFormatError = String.Format("Please enter {0} or fewer valid characters.", Constants.NotesMaxLength);
            public static readonly string RetailSizeError = String.Format("Please enter a number greater than zero with 5 or less digits to the left of the decimal and 4 or less digits to the right of the decimal.");
            public static readonly string AlcoholByVolumeError = String.Format("Alcohol By Volume must be a number between 0 and 99.99 with up to two decimal places.");
            public static readonly string DrainedWeightError = "Drained Weight is not recognized. Valid entry should be a number with maximum of 4 decimal places.";
            //public static readonly string FairTradeCertifiedError = string.Format("Fair Trade Certified is not recognized.  Valid entries are {0}.", string.Join(", ", FairTradeCertifiedValues.AsArray));
            public static readonly string MainProductNameError = "Main Product Name is not recognized. Valid entry should be a text value with maximum of 255 characters";
            public static readonly string ProductFlavorTypeError = "Product Flavor or Type is not recognized. Valid entry should be a text value with maximum of 255 characters";
        }

        public static class CustomValidationPatterns
        {
            public const string BrandNamePattern = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,35}$";
            public const string MaxPluPattern = @"^[1-9][0-9]*$";
            public const string AgencyNamePattern = @"^.{1,255}$";
            public const string DateFormatPattern = @"^(0?[1-9]|1[012])/(0?[1-9]|[12][0-9]|3[01])/\d{4}$";
            public const string UserFromatPattern = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$";
            public const string RetailSizeFormatPattern = @"^([0-9]{1,5}(?:[\.][0-9]{0,4})?|\.[0-9]{1,4})$";
        }
    }
}