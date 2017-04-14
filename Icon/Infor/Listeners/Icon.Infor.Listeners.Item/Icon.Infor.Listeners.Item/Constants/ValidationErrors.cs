using System;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Item.Constants
{
    public static class ValidationErrors
    {        
        public static class Codes
        {
            public const string InvalidProductDescription = "InvalidProductDescription";
            public const string InvalidItemId = "ItemIdRequired";
            public const string InvalidItemTypeCode = "InvalidItemTypeCode";
            public const string InvalidBooleanString = "InvalidBooleanString";
            public const string InvalidAirChilled = "InvalidAirChilled";
            public const string InvalidAlcoholByVolume = "InvalidAlcoholByVolume";
            public const string InvalidAnimalWelfareRating = "InvalidAnimalWelfareRating";
            public const string InvalidBiodynamic = "InvalidBiodynamic";
            public const string InvalidBrand = "InvalidBrand";
            public const string InvalidCaseinFree = "InvalidCaseinFree";
            public const string InvalidCheeseMilkType = "InvalidCheeseMilkType";
            public const string InvalidCheeseRaw = "InvalidCheeseRaw";
            public const string InvalidDeliverySystem = "InvalidDeliverySystem";
            public const string InvalidDrainedWeight = "InvalidDrainedWeight";
            public const string InvalidDrainedWeightUom = "InvalidDrainedWeightUom";
            public const string InvalidDryAged = "InvalidDryAged";
            public const string InvalidEcoScaleRating = "InvalidEcoScaleRating";
            public const string InvalidFairTradeCertified = "InvalidFairTradeCertified";
            public const string InvalidFinancialHierarchyClassId = "InvalidFinancialHierarchyClassId";
            public const string InvalidFoodStampEligible = "InvalidFoodStampEligible";
            public const string InvalidFreeRange = "InvalidFreeRange";
            public const string InvalidFreshOrFrozen = "InvalidFreshOrFrozen";
            public const string InvalidGrassFed = "InvalidGrassFed";
            public const string InvalidGlutenFree = "InvalidGlutenFree";
            public const string InvalidHemp = "InvalidHemp";
            public const string InvalidHiddenItem = "InvalidHiddenItem";
            public const string InvalidInforMessageId = "InvalidMessageId";
            public const string InvalidKosher = "InvalidKosher";
            public const string InvalidLocalLoanProducer = "InvalidLocalLoanProducer";
            public const string InvalidMadeInHouse = "InvalidMadeInHouse";
            public const string InvalidMainProductName = "InvalidMainProductName";
            public const string InvalidMerchandiseHierarchyClassId = "InvalidMerchandiseHierarchyClassId";
            public const string InvalidModifiedDate = "InvalidModifiedDate";
            public const string InvalidModifiedUser = "InvalidModifiedUser";
            public const string InvalidMsc = "InvalidMsc";
            public const string InvalidNationalHierarchyClassId = "InvalidNationalHierarchyClassId";
            public const string InvalidNotes = "InvalidNotes";
            public const string InvalidNonGmo = "InvalidNonGmo";
            public const string InvalidNutritionRequired = "InvalidNutritionRequired";
            public const string InvalidOrganic = "InvalidOrganic";
            public const string InvalidOrganicPersonalCare = "InvalidOrganicPersonalCare";
            public const string InvalidPackageUnit = "InvalidPackageUnit";
            public const string InvalidPaleo = "InvalidPaleo";
            public const string InvalidPastureRaised = "InvalidPastureRaised";
            public const string InvalidPosDescription = "InvalidPosDescription";
            public const string RequiredPosScaleTare = "RequiredPosScaleTare";
            public const string InvalidPosScaleTare = "InvalidPosScaleTare";
            public const string InvalidPremiumBodyCare = "InvalidPremiumBodyCare";
            public const string InvalidProductFlavorType = "InvalidProductFlavorType";
            public const string InvalidProhibitDiscount = "InvalidProhibitDiscount";
            public const string InvalidRetailSize = "InvalidRetailSize";
            public const string InvalidRetailUom = "InvalidRetailUom";
            public const string InvalidScanCode = "InvalidScanCode";
            public const string InvalidScanCodeType = "InvalidScanCodeType";
            public const string InvalidSeafoodCatchType = "InvalidSeafoodCatchType";
            public const string InvalidTaxHierarchyClassId = "InvalidTaxHierarchyClassId";
            public const string InvalidVegan = "InvalidVegan";
            public const string InvalidVegetarian = "InvalidVegetarian";
            public const string InvalidWholeTrade = "InvalidWholeTrade";
            public const string DuplicateMerchandiseHierarchyClass = "DuplicateMerchandiseHierarchyClass";
            public const string DuplicateNationalHierarchyClass = "DuplicateNationalHierarchyClass";
            public const string NonExistentBrand = "NonExistentBrand";
            public const string NonExistentSubTeam = "NonExistentSubTeam";
            public const string NonExistentSubBrick = "NonExistentSubBrick";
            public const string NonExistentNationalClass = "NonExistentNationalClass";
            public const string NonExistentTax = "NonExistentTax";
            public const string OutOfSyncItemUpdateErrorCode = "OutOfSyncItemUpdateErrorCode";
        }

        public static class Messages
        {
            private const string InvalidValueMessage = "{PropertyName} has invalid value '{PropertyValue}'.";
            private const string RequiredValueMessage = InvalidValueMessage + " {PropertyName} is a required value.";
            private static readonly string MaxLengthMessage = InvalidValueMessage + " {PropertyName} must be less than " + ItemValues.MaxPropertyStringLength;

            public static readonly string InvalidProductDescription = InvalidValueMessage + 
                " {PropertyName} is required and must be less than " + ItemValues.ProductDescriptionMaxLength + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @, following the regular expression: " + TraitPatterns.ProductDescription;
            public static readonly string InvalidItemId = "Item ID is required and must be a non-negative integer.";
            public static readonly string InvalidItemTypeCode = BuildInvalidPropertyMessageForCollection(ItemTypes.Codes.AsArray, true);
            public static readonly string InvalidBooleanString = "{PropertyName} has invalid value '{PropertyValue}'. {PropertyName} must be 1 or 0.";
            public static readonly string InvalidAlcoholByVolume = "Alcohol By Volume has invalid value '{PropertyValue}'. " +
                "{PropertyName} must be empty or a number between 0 and 99.99 with up to 2 digits to the right of the decimal, following the regular expression pattern: " + TraitPatterns.AlcoholByVolume;
            public static readonly string InvalidAnimalWelfareRating = BuildInvalidPropertyMessageForCollection(AnimalWelfareRatings.Descriptions.AsArray);
            public static readonly string InvalidCertificationAgency = "{PropertyName} has invalid value '{PropertyValue}'. {PropertyName} must be less than " + ItemValues.MaxPropertyStringLength + ".";
            public static readonly string InvalidBrand = RequiredValueMessage + " {PropertyName} must be a valid integer.";
            public static readonly string InvalidCheeseMilkType = BuildInvalidPropertyMessageForCollection(MilkTypes.Descriptions.AsArray);
            public static readonly string InvalidDeliverySystem = BuildInvalidPropertyMessageForCollection(DeliverySystems.Descriptions.AsArray);
            public static readonly string InvalidDrainedWeight = InvalidValueMessage +
                " {PropertyName} must be a number with maximum of 4 decimal places, following the regular expression pattern: " + TraitPatterns.DrainedWeight;
            public static readonly string InvalidDrainedWeightUom = BuildInvalidPropertyMessageForCollection(DrainedWeightUoms.AsArray);
            public static readonly string InvalidEcoScaleRating = BuildInvalidPropertyMessageForCollection(EcoScaleRatings.Descriptions.AsArray);
            public static readonly string InvalidFairTradeCertified = BuildInvalidPropertyMessageForCollection(FairTradeCertifiedValues.AsArray);
            public static readonly string InvalidFinancialHierarchyClassId = InvalidValueMessage +
                " {PropertyName} is required and must be 4 numeric characters, following the regular expression pattern: " + CustomRegexPatterns.SubTeamNumber;
            public static readonly string InvalidFreshOrFrozen = BuildInvalidPropertyMessageForCollection(SeafoodFreshOrFrozenTypes.Descriptions.AsArray);

            public static readonly string InvalidInforMessageId = RequiredValueMessage;
            public static readonly string InvalidMainProductName = InvalidValueMessage +
                " {PropertyName} must be a text value with maximum of 255 characters, following the regular expression " + TraitPatterns.MainProductName;
            public static readonly string InvalidMerchandiseHierarchyClassId = RequiredValueMessage;
            public static readonly string InvalidModifiedDate = InvalidValueMessage +
                " {PropertyName} must be a valid date field, such as '2016-08-01T16:04:20.940Z'";
            public static readonly string InvalidModifiedUser = MaxLengthMessage;
            public static readonly string InvalidNationalHierarchyClassId = RequiredValueMessage;
            public static readonly string InvalidNotes = MaxLengthMessage;
            public static readonly string InvalidPackageUnit = InvalidValueMessage +
                " {PropertyName} is required and must be a whole number with " + ItemValues.PackageUnitMaxLength + " or fewer digits following the regular expression " + TraitPatterns.PackageUnit;
            public static readonly string InvalidPosDescription = InvalidValueMessage +
                " {PropertyName} is requried and must be less than " + ItemValues.PosDescriptionMaxLength + " characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @, following the regular expression " + TraitPatterns.PosDescription;
            public static readonly string InvalidPosScaleTare = InvalidValueMessage +
                " {PropertyName} is required and must be a valid decimal value less than 10 with no more than 4 digits plus the decimal point, for example any value between 0.0000 and 9.9999.";
            public static readonly string InvalidProductFlavorType = InvalidValueMessage +
                " {PropertyName} must be empty or a text value with a maximum of 255 characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @, following the regular expression " + TraitPatterns.ProductFlavorType;
            public static readonly string InvalidRetailSize = InvalidValueMessage +
                " {PropertyName} is required and must be greater than 0, less than " + ItemValues.RetailSizeMax + ", and with 4 or less digits to the right of the decimal.";
            public static readonly string InvalidRetailUom = BuildInvalidPropertyMessageForCollection(UomCodes.ByName.Values);
            public static readonly string InvalidScanCode = InvalidValueMessage +
                " {PropertyName} is required and must be " + ItemValues.ScanCodeMaxLength + " or fewer numbers.";
            public static readonly string InvalidScanCodeType = BuildInvalidPropertyMessageForCollection(ScanCodeTypes.Descriptions.AsArray, true);
            public static readonly string InvalidSeafoodCatchType = BuildInvalidPropertyMessageForCollection(SeafoodCatchTypes.Descriptions.AsArray);
            public static readonly string InvalidTaxHierarchyClassId = RequiredValueMessage;
            public static readonly string DuplicateMerchandiseHierarchyClass = "Item has multiple Merchandise Hierarchy Class associations. An item can only be associated to a single Merchandise Hierarchy Class.";
            public static readonly string DuplicateNationalHierarchyClass = "Item has multiple National Hierarchy Class associations. An item can only be associated to a single National Hierarchy Class.";
            public const string NonExistentBrand = "No Brand exists in Icon with a hierarchy class ID '{0}'.";
            public const string NonExistentSubTeam = "No Sub Team exists in Icon with an sub team code '{0}'.";
            public const string NonExistentSubBrick = "No Sub Brick exists in Icon with a hierarchy class ID '{0}'.";
            public const string NonExistentNationalClass = "No National Class exists in Icon with a hierarchy class ID '{0}'.";
            public const string NonExistentTax = "No Tax Class exists in Icon with a tax code '{0}'.";
            public const string OutOfSyncItemUpdateErrorCode = "Item update rejected: time stamp on update was ''{0}'' but the item was updated more recently at ''{1}''.";

            private static string BuildInvalidPropertyMessageForCollection(IEnumerable<string> collection, bool isRequired = false)
            {
                if(isRequired)
                {
                    return InvalidValueMessage + " {PropertyName} is required and must be one of the following: " + string.Join(",", collection);
                }
                return InvalidValueMessage + " {PropertyName} must be empty or one of the following: " + string.Join(",", collection);
            }
        }
    }
}
