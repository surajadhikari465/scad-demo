using Icon.Framework;
using System;

namespace Icon.Web.Common
{
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
        public static readonly string DrainedWeightUomError = String.Format("Drained Weight UOM is not recognized. Valid entries are {0}", string.Join(", ", DrainedWeightUoms.AsArray));
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
        public const string DateFormatPattern  = @"^(0?[1-9]|1[012])/(0?[1-9]|[12][0-9]|3[01])/\d{4}$";
        public const string UserFromatPattern  = @"^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,255}$";
        public const string RetailSizeFormatPattern = @"^([0-9]{1,5}(?:[\.][0-9]{0,4})?|\.[0-9]{1,4})$";
    }
}
