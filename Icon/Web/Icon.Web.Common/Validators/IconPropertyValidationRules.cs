using Icon.Framework;
using System;
using System.Text.RegularExpressions;

namespace Icon.Web.Common.Validators
{
    public class IconPropertyValidationRules
    {
        public string ErrorMessage { get; set; }
        public string ExpressionToValidate { get; set; }

        public IconPropertyValidationRules(string expressionToValidate, string errorMessage)
        {
            ExpressionToValidate = expressionToValidate;
            ErrorMessage = errorMessage;
        }

        public virtual bool IsValid(string propertyValue, object dataContext)
        {
            if (propertyValue != null && Regex.IsMatch(propertyValue, ExpressionToValidate))
            {
                return DoAddtionalValidation(propertyValue, dataContext);
            }

            return false;
        }

        public virtual bool DoAddtionalValidation(string propertyValue, object dataContext)
        {
            // Do additional validation and set the appropriate error message if needed in derived classes.
            return true;
        }

        public static IconPropertyValidationRules GetIconPropertyValidationRules(string propertyToValidate)
        {
            switch (propertyToValidate)
            {
                case ValidatorPropertyNames.ProductDescription:
                    return new IconPropertyValidationRules(TraitPatterns.ProductDescription, ValidatorErrorMessages.ProductDescriptionError);

                case ValidatorPropertyNames.PosDescription:
                    return new IconPropertyValidationRules(TraitPatterns.PosDescription, ValidatorErrorMessages.PosDescriptionError);

                case ValidatorPropertyNames.BrandName:
                    return new IconPropertyValidationRules(CustomValidationPatterns.BrandNamePattern, ValidatorErrorMessages.BrandNameError);

                case ValidatorPropertyNames.BrandAbbreviation:
                    return new IconPropertyValidationRules(TraitPatterns.BrandAbbreviation, ValidatorErrorMessages.BrandAbbreviationError);

                case ValidatorPropertyNames.TaxAbbreviation:
                    return new IconPropertyValidationRules(TraitPatterns.TaxAbbreviation, ValidatorErrorMessages.TaxAbbreviationError);

                case ValidatorPropertyNames.TaxRomance:
                    return new IconPropertyValidationRules(TraitPatterns.TaxRomance, ValidatorErrorMessages.TaxRomanceError);

                case ValidatorPropertyNames.MaxPlus:
                    return new IconPropertyValidationRules(CustomValidationPatterns.MaxPluPattern, ValidatorErrorMessages.MaxPlusError);

                case ValidatorPropertyNames.CertificationAgencyName:
                    return new IconPropertyValidationRules(CustomValidationPatterns.AgencyNamePattern, ValidatorErrorMessages.AgencyNameError);

                case ValidatorPropertyNames.Notes:
                    return new IconPropertyValidationRules(TraitPatterns.Notes, ValidatorErrorMessages.NotesError);
                    
                case ValidatorPropertyNames.CreatedDate:
                case ValidatorPropertyNames.ModifiedDate:
                    return new IconPropertyValidationRules(CustomValidationPatterns.DateFormatPattern, ValidatorErrorMessages.DateFormatError);

                case ValidatorPropertyNames.ModifiedUser:
                    return new IconPropertyValidationRules(CustomValidationPatterns.UserFromatPattern, ValidatorErrorMessages.UserFormatError);

                case ValidatorPropertyNames.RetailSize:
                    return new IconPropertyValidationRules(CustomValidationPatterns.RetailSizeFormatPattern, ValidatorErrorMessages.RetailSizeError);

                case ValidatorPropertyNames.AlcoholByVolume:
                    return new IconPropertyValidationRules(TraitPatterns.AlcoholByVolume, ValidatorErrorMessages.AlcoholByVolumeError);

                case ValidatorPropertyNames.DrainedWeightUom:
                    return new IconPropertyValidationRules(TraitPatterns.DrainedWeightUom, ValidatorErrorMessages.DrainedWeightUomError);

                case ValidatorPropertyNames.DrainedWeight:
                    return new IconPropertyValidationRules(TraitPatterns.DrainedWeight, ValidatorErrorMessages.DrainedWeightError);

                case ValidatorPropertyNames.FairTradeCertified:
                    return new IconPropertyValidationRules(TraitPatterns.FairTradeCertified, ValidatorErrorMessages.FairTradeCertifiedError);

                case ValidatorPropertyNames.MainProductName:
                    return new IconPropertyValidationRules(TraitPatterns.MainProductName, ValidatorErrorMessages.MainProductNameError);

                case ValidatorPropertyNames.ProductFlavorType:
                    return new IconPropertyValidationRules(TraitPatterns.ProductFlavorType, ValidatorErrorMessages.ProductFlavorTypeError);

                default:
                    throw new ArgumentException(String.Format("Unknown property: {0}", propertyToValidate));
            }
        }
    }
}
