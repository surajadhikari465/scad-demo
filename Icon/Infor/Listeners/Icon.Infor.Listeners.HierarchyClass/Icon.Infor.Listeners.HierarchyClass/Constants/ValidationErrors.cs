using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Constants
{
    public static class ValidationErrors
    {
        public static class Codes
        {
            // ActionCode
            public const string InvalidAction = "InvalidAction";
            public const string RequiredAction = "RequiredAction";

            // Main HierarchyClass properties
            public const string RequiredHierarchyClassId = "RequiredHierarchyClassId";
            public const string RequiredHierarchyClassName = "RequiredHierarchyClassName";
            public const string RequiredParentHierarchyClassId = "RequiredParentHierarchyClassId";
            public const string RequiredHierarchyName = "RequiredHierarchyName";
            public const string InvalidHierarchyClassId = "InivalidHierarchyClassId";
            public const string InvalidParentHierarchyClassId = "InvalidHierarchyClassId";
            public const string InvalidHierarchyName = "InvalidHierarchyName";

            // Hierarchy Level Names
            public const string RequiredHierarchyLevelName = "RequiredHierarchyLevelName";
            public const string InvalidTaxHierarchyLevelName = "InvalidTaxHierarchyLevelName";
            public const string InvalidBrandHierarchyLevelName = "InvalidBrandHierarchyLevelName";
            public const string InvalidMerchandiseHierarchyLevelName = "InvalidMerchandiseHierarchyLevelName";
            public const string InvalidNationalHierarchyLevelName = "InvalidNationalHierarchyLevelName";
            public const string InvalidFinancialHierarchyLevelName = "InvalidFinancialHierarchyLevelName";

            // Tax
            public const string RequiredTaxAbbreviation = "RequiredTaxAbbreviation";
            public const string RequiredTaxRomance = "RequiredTaxRomanceSpecified";
            public const string InvalidTaxClassName = "InvalidTaxClassName";
            public const string InvalidTaxAbbreviation = "InvalidTaxAbbreviation";
            public const string InvalidTaxRomance = "InvalidTaxRomance";

            // Brand
            public const string RequiredBrandAbbreviation = "RequiredBrandAbbreviation";
            public const string InvalidBrandName = "InvalidBrandName";
            public const string InvalidBrandAbbreviation = "InvalidBrandAbbreviation";

            // Merchandise
            public const string RequiredMerchFinMapping = "RequiredSubTeamMapping";
            public const string RequiredSubBrickCode = "RequiredSubBrickCode";
            public const string InvalidMerchandiseName = "InvalidMerchandiseName";
            public const string InvalidProhibitDiscount = "InvalidProhibitDiscount";
            public const string InvalidNonMerchTrait = "InvalidNonMerchTrait";
            public const string InvalidMerchFinMapping = "InvalidSubTeamMapping";
            public const string InvalidSubBrickCode = "InvalidSubBrickCode";
            public const string InvalidDefaultTaxClass = "InvalidDefaultTaxClass";

            // National
            public const string InvalidNationalClassName = "InvalidNationalClassName";
            public const string RequiredNationalClassCode = "RequiredNationalClassCode";
            public const string InvalidNationalClassCode = "InvalidNationalClassCode";

            // Financial
            public const string InvalidFinancialName = "InvalidFinancialName";
        }

        public static class Descriptions
        {
            // Required Property Descriptions (used for any 'Required' error code).
            public static readonly string RequiredProperty = "[{PropertyName}] is required.";

            // Main HierarchyClass Properties
            public static readonly string InvalidAction = "Cannot process ActionType [{PropertyValue}]. Hierarchy Class Listener only accepts ActionTypes AddOrUpdate and Delete";
            public static readonly string InvalidId = "[{PropertyName}] has an invalid value '{PropertyValue}'. It must be a non-negative integer.";
            public static readonly string InvalidHierarchyName = "[{PropertyName}] has an invalid value '{PropertyValue}'. It must be one of the following: Brands, Merchandise, National, Financial, or Tax";

            // Hierarchy Level Names
            public static readonly string InvalidBrandHierarchyLevelName = "[{PropertyName}] has an invalid value '{PropertyValue}'. It must be 'Brand' for the Brand Hierarchy.";
            public static readonly string InvalidMerchandiseHierarchyLevelName = "[{PropertyName}] has an invalid value '{PropertyValue}'. It must be one of the following for the Merchandise Hierarchy: " + string.Join(",", HierarchyConstants.MerchandiseHierarchyLevelNames);
            public static readonly string InvalidNationalHierarchyLevelName = "[{PropertyName}] has an invalid value '{PropertyValue}'. It must be one of the following for the National Hierarchy: " + string.Join(",", HierarchyConstants.NationalHierarchyLevelNames);
            public static readonly string InvalidFinancialHierarchyLevelName = "[{PropertyName}] has an invalid value '{PropertyValue}'. It must be 'Financial' for the Financial Hierarchy.";

            // Tax
            public static readonly string RequiredTaxAbbreviation = "TaxAbbreviation trait is required.";
            public static readonly string RequiredTaxRomance = "TaxRomance trait is required.";
            public static readonly string InvalidTaxHierarchyLevelName = "TaxClass name has an invalid value '{PropertyValue}'. It must be 'Tax' for the Tax Hierarchy.";
            public static readonly string InvalidTaxClassName = "[{PropertyName}] has an invalid value '{PropertyValue}'.  It must start with a 7 digit numeric code followed by 255 characters.";
            public static readonly string InvalidTaxAbbreviation = "TaxAbbreviation trait with TraitCode " + TraitCodes.TaxAbbreviation + " has an invalid value '{PropertyValue}'. It must start with a 7 digit numeric code followed by 42 characters. Following the following regex pattern: '" + TraitPatterns.TaxAbbreviation + "'.";
            public static readonly string InvalidTaxRomance = "TaxRomance trait with TraitCode " + TraitCodes.TaxRomance + " has an invalid value '{PropertyValue}'. It must be 1 to 150 characters long and contain only the following special characters:  /\\%<>&-+=.  Following the regex pattern: '" + TraitPatterns.TaxRomance + "'.";

            // Brand
            public static readonly string InvalidBrandName = "[{PropertyName}] has an invalid value '{PropertyValue}'.  It must be a text value with no more than 35 characters and only allows letters, numbers, and the following: space, !, #, $, %, &, ', (, ), *, comma, -, ., /, :, ;, <, =, >, ?, @. Following the regex pattern: '" + HierarchyConstants.BrandNamePattern + "'.";
            public static readonly string InvalidBrandAbbreviation = "BrandAbbreviation trait with TraitCode " + TraitCodes.BrandAbbreviation + " has an invalid value '{PropertyValue}'. It must be 10 or fewer valid characters (Letters, Numbers, and Ampersands only). Following the following regex pattern: '" + TraitPatterns.BrandAbbreviation + "'.";
            public static readonly string RequiredBrandAbbreviation = "BrandAbbreviation trait is required.";

            // Merchandise
            public static readonly string RequiredMerchFinMapping = "SubTeam trait is required.";
            public static readonly string RequiredSubBrickCode = "SubBrickCode trait is required.";
            public static readonly string InvalidMerchandiseName = "The Merchandise class name has an invalid value '{PropertyValue}'. It must be less than or equal to 255 characters.";
            public static readonly string InvalidProhibitDiscount = "ProhibitDiscount trait has an invalid value '{PropertyValue}'. It must be 1 or 0.  Following the trait pattern: '" + TraitPatterns.ProhibitDiscount + "'.";
            public static readonly string InvalidNonMerchTrait = "NonMerchandise trait has an invalid value '{PropertyValue}'.  It must be one of the following:  Bottle Deposit, CRV, Coupon, Bottle Return, CRV Credit, Legacy POS Only, Blackhawk Fee, Non-Retail";
            public static readonly string InvalidSubBrickCode = "SubBrickCode trait has an invalid value '{PropertyValue}'.  It must be only numbers or letters. Following the trait pattern: '" + TraitPatterns.SubBrickCode + "'.";
            public static readonly string InvalidDefaultTaxClass = "DefaultTaxClassAssociation trait has an invalid value '{PropertyValue}'.  It must be only numbers. Following the trait pattern:  '" + TraitPatterns.MerchDefaultTaxAssociatation + "'.";
            public static readonly string InvalidMerchFinMapping = "MerchFinMapping trait has an invalid value '{PropertyValue}'. It must be only numbers or letters. Following the trait pattern: '" + TraitPatterns.MerchFinMapping + "'.";

            // National
            public static readonly string InvalidNationalClassName = "The National Class name has an invalid value '{PropertyValue}'. It must be less than or equal to 255 characters.";
            public static readonly string InvalidNationalClassCode = "NationalClassCode trait has an invalid value '{PropertyValue}'. It must be only numbers.  Following the trait pattern:  '" + TraitPatterns.NationalClassCode + "'.";
            public static readonly string RequiredNationalClassCode = "NationalClassCode trait is required.";
            public static readonly string InvalidNationalClassCodeLength = "NationalClassCode trait has an invalid value '{PropertyValue}'. It must be 5 characters only.";

            // Financial
            public static readonly string InvalidFinancialName = "Financial class name has an invalid value '{PropertyValue}'. It must be less than or equal to 255 characters.";
        }
    }
}
