using System;
using Icon.Framework;
using System.Collections.Generic;

namespace KitBuilder.ESB.Listeners.Item.Service.Constants.ItemValidation
{
    public static class ValidationErrorMessages
    {
        private const string InvalidValue = "{PropertyName} has invalid value '{PropertyValue}'.";
        private const string RequiredValue = InvalidValue + " {PropertyName} is a required value.";
        private static readonly string InvalidLength255 = InvalidValue +
            " {PropertyName} " + InvalidLength(MaxLengths.StandardProperty255);

        public static readonly string InvalidProductDescription = InvalidValue +
            " {PropertyName} is required and must be less than " + MaxLengths.ProductDescription + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @, following the regular expression: " + TraitPatterns.ProductDescription;
        public static readonly string InvalidItemId = "Item ID is required and must be a non-negative integer.";
        public static readonly string InvalidItemTypeCode = InvalidProperty(ItemTypes.Codes.AsArray, true);
        public static readonly string InvalidBooleanStringZeroOrOne = "{PropertyName} has invalid value '{PropertyValue}'. {PropertyName} must be 1 or 0.";
        public static readonly string InvalidBooleanStringYorN_Nullable = "{PropertyName} has invalid value '{PropertyValue}'. {PropertyName} must be Y or N (cannot be null).";
        public static readonly string InvalidBooleanStringYorN_NotNullable = "{PropertyName} has invalid value '{PropertyValue}'. {PropertyName} must be Y or N (or null/empty).";
        public static readonly string InvalidAlcoholByVolume = "Alcohol By Volume has invalid value '{PropertyValue}'. " +
            "{PropertyName} must be empty or a number between 0 and 99.99 with up to 2 digits to the right of the decimal, following the regular expression pattern: " + TraitPatterns.AlcoholByVolume;
        public static readonly string InvalidAnimalWelfareRating = InvalidProperty(AnimalWelfareRatings.Descriptions.AsArray);
        public static readonly string InvalidCertificationAgency = "{PropertyName} has invalid value '{PropertyValue}'. {PropertyName} must be less than " + MaxLengths.StandardProperty255 + ".";
        public static readonly string InvalidBrand = RequiredValue + " {PropertyName} must be a valid integer.";
        public static readonly string InvalidCheeseMilkType = InvalidProperty(MilkTypes.Descriptions.AsArray);
        public static readonly string InvalidDeliverySystem = InvalidProperty(DeliverySystems.Descriptions.AsArray);
        public static readonly string InvalidDrainedWeight = InvalidValue +
            " {PropertyName} must be a number with maximum of 4 decimal places, following the regular expression pattern: " + TraitPatterns.DrainedWeight;
        public static readonly string InvalidDrainedWeightUom = InvalidProperty(DrainedWeightUoms.AsArray);
        public static readonly string InvalidEcoScaleRating = InvalidProperty(EcoScaleRatings.Descriptions.AsArray);
        public static readonly string InvalidFairTradeCertified = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.FairTradeCertified);
                public static readonly string InvalidFinancialHierarchyClassId = InvalidValue +
            " {PropertyName} is required and must be 4 numeric characters, following the regular expression pattern: " + RegularExpressions.SubTeamNumber;
        public static readonly string InvalidFreshOrFrozen = InvalidProperty(SeafoodFreshOrFrozenTypes.Descriptions.AsArray);
        public static readonly string InvalidInforMessageId = RequiredValue;
        public static readonly string InvalidMainProductName = InvalidValue +
            " {PropertyName} must be a text value with maximum of 255 characters, following the regular expression " + TraitPatterns.MainProductName;
        public static readonly string InvalidMerchandiseHierarchyClassId = RequiredValue;
        public static readonly string InvalidModifiedDate = InvalidValue +
            " {PropertyName} must be a valid date field, such as '2016-08-01T16:04:20.940Z'";
        public static readonly string InvalidModifiedUser = InvalidLength255;
        public static readonly string InvalidNationalHierarchyClassId = RequiredValue;
        public static readonly string InvalidNotes = InvalidLength255;
        public static readonly string InvalidPackageUnit = InvalidValue +
            " {PropertyName} is required and must be a whole number with " + MaxLengths.PackageUnit + " or fewer digits following the regular expression " + TraitPatterns.PackageUnit;
        public static readonly string InvalidPosDescription = InvalidValue +
            " {PropertyName} is requried and must be less than " + MaxLengths.PosDescription + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @, following the regular expression " + TraitPatterns.PosDescription;
        public static readonly string InvalidPosScaleTare = InvalidValue +
            " {PropertyName} is required and must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point, for example any value between 0.0000 and 9.9999.";
        public static readonly string InvalidProductFlavorType = InvalidValue +
            " {PropertyName} must be empty or a text value with a maximum of 255 characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @, following the regular expression " + TraitPatterns.ProductFlavorType;
        public static readonly string InvalidRetailSize = InvalidValue +
            " {PropertyName} is required and must be greater than 0, less than " + MaxValues.RetailSize + ", and with 4 or less digits to the right of the decimal.";
        public static readonly string InvalidRetailUom = InvalidProperty(UomCodes.ByName.Values);
        public static readonly string InvalidScanCode = InvalidValue +
            " {PropertyName} is required and must be " + MaxLengths.ScanCode + " or fewer numbers.";
        public static readonly string InvalidScanCodeType = InvalidProperty(ScanCodeTypes.Descriptions.AsArray, true);
        public static readonly string InvalidSeafoodCatchType = InvalidProperty(SeafoodCatchTypes.Descriptions.AsArray);
        public static readonly string InvalidTaxHierarchyClassId = RequiredValue;
        public static readonly string DuplicateMerchandiseHierarchyClass = "Item has multiple Merchandise Hierarchy Class associations. An item can only be associated to a single Merchandise Hierarchy Class.";
        public static readonly string DuplicateNationalHierarchyClass = "Item has multiple National Hierarchy Class associations. An item can only be associated to a single National Hierarchy Class.";
        public const string NonExistentBrand = "No Brand exists in Icon with a hierarchy class ID '{0}'.";
        public const string NonExistentSubTeam = "No Sub Team exists in Icon with an sub team code '{0}'.";
        public const string NonExistentSubBrick = "No Sub Brick exists in Icon with a hierarchy class ID '{0}'.";
        public const string NonExistentNationalClass = "No National Class exists in Icon with a hierarchy class ID '{0}'.";
        public const string NonExistentTax = "No Tax Class exists in Icon with a tax code '{0}'.";
        public const string OutOfSyncItemUpdateErrorCode = "Item update rejected: time stamp on update was '{0}' but the item was updated more recently at '{1}'.";
        public const string OutOfSyncItemUpdateSequenceIdErrorCode = "Item update rejected: message Sequence ID was '{0}' but the item has a larger Sequence ID of '{1}'.";
        public static readonly string InvalidCustomerFriendlyDescription = InvalidValue + InvalidLengthOrPattern(MaxLengths.CFD, TraitPatterns.CustomerFriendlyDescription);
        public static readonly string InvalidGlobalPricingProgram = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.GlobalPricingProgram);
        public static readonly string InvalidMadeWithOrganicGrapes = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.MadeWithOrganicGrapes);
        public static readonly string InvalidPrimeBeef = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.PrimeBeef);
        public static readonly string InvalidRainforestAlliance = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.RainforestAlliance);
        public static readonly string InvalidSmithsonianBirdFriendly = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.SmithsonianBirdFriendly);
        public static readonly string InvalidWicEligible = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.WicEligible);
        public static readonly string InvalidRefrigerated = InvalidValue + InvalidLengthOrPattern(MaxLengths.StandardProperty255, TraitPatterns.Refrigerated);
        public static readonly string InvalidShelfLife = InvalidValue + " {PropertyName} value must be between 0 and " + MaxValues.ShelfLife + " (or blank)";
        public static readonly string InvalidFlexibleTextLength = InvalidValue + InvalidLength(MaxLengths.FlexibleText);
        public static readonly string InvalidFlexibleTextPattern = InvalidValue + InvalidRegEx(TraitPatterns.FlexibleText);
        public static readonly string InvalidSelfCheckoutItemTareGroupLength = InvalidValue + InvalidLength(MaxLengths.SelfCheckoutItemTareGroup);
        public static readonly string InvalidSelfCheckoutItemTareGroupPattern = InvalidValue + InvalidRegEx(TraitPatterns.SelfCheckoutItemTareGroup);
        public const string InvalidRefrigeratedEnum = "was expected to be 'Refrigerated' or 'Shelf Stable'";
        public const string InvalidYesNoExpected = "was expected to be 'Yes' or 'No' (case-insensitive)";
        public const string InvalidLine = "was expected to be any text up to 255 chars (excluding commas)";
        public const string InvalidSKU = "was expected to be any text up to 255 chars (excluding commas)";
        public const string InvalidPriceLine = "was expected to be any text up to 255 chars (excluding commas)";
        public const string InvalidVariantSize = "was expected to be any text up to 255 chars (excluding commas)";
        public const string InvalidEStoreNutritionRequired = "InvalidEStoreNutritionRequired";
        public const string InvalidPrimeNowEligiblep = "InvalidPrimeNowEligiblep";
        public const string InvalidEstoreEligible = "InvalidEstoreEligible";
        public const string InvalidTSFEligible = "InvalidTSFEligible";
        public const string InvalidWFMEligilble = "InvalidWFMEligilble";
        public const string InvalidOther3PEligible = "InvalidOther3PEligible";

        private static string InvalidProperty(IEnumerable<string> collection, bool isRequired = false)
        {
            if (isRequired)
            {
                return InvalidValue + " {PropertyName} is required and must be one of the following: " + string.Join(",", collection);
            }
            return InvalidValue + " {PropertyName} must be empty or one of the following: " + string.Join(",", collection);
        }

        private static string InvalidLengthOrPattern(int maxLength, string traitRegExPattern)
        {
            var msg = " {PropertyName} " + InvalidLength(maxLength) + " and " + InvalidRegEx(traitRegExPattern);
            return msg;
        }

        private static string InvalidLength(int maxLength)
        {
            var msg = " {PropertyName} cannot be more than " + maxLength + " characters";
            return msg;
        }

        private static string InvalidRegEx(string traitRegExPattern)
        {
            var msg = String.Empty;
            if (traitRegExPattern.Contains(RegularExpressions.BooleanYesNo))
            {
                msg += InvalidYesNoExpected + "-";
            }
            else if (traitRegExPattern.Equals(TraitPatterns.Refrigerated))
            {
                msg += InvalidRefrigeratedEnum + "-";
            }
            msg += " {PropertyName} can contain any character following the regular expression: '" + traitRegExPattern + "'.'";
            return msg;
        }
    }
}

