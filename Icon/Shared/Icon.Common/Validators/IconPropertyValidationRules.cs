using System;
using System.Text.RegularExpressions;
using Icon.Framework;

namespace Icon.Common.Validators
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
                case Constants.ValidatorPropertyNames.ProductDescription:
                    return new IconPropertyValidationRules(TraitPatterns.ProductDescription, Constants.ValidatorErrorMessages.ProductDescriptionError);

                case Constants.ValidatorPropertyNames.PosDescription:
                    return new IconPropertyValidationRules(TraitPatterns.PosDescription, Constants.ValidatorErrorMessages.PosDescriptionError);

                case Constants.ValidatorPropertyNames.BrandName:
                    return new IconPropertyValidationRules(Constants.CustomValidationPatterns.BrandNamePattern, Constants.ValidatorErrorMessages.BrandNameError);

                case Constants.ValidatorPropertyNames.BrandAbbreviation:
                    return new IconPropertyValidationRules(TraitPatterns.BrandAbbreviation, Constants.ValidatorErrorMessages.BrandAbbreviationError);

                case Constants.ValidatorPropertyNames.TaxAbbreviation:
                    return new IconPropertyValidationRules(TraitPatterns.TaxAbbreviation, Constants.ValidatorErrorMessages.TaxAbbreviationError);

                case Constants.ValidatorPropertyNames.TaxRomance:
                    return new IconPropertyValidationRules(TraitPatterns.TaxRomance, Constants.ValidatorErrorMessages.TaxRomanceError);

                case Constants.ValidatorPropertyNames.MaxPlus:
                    return new IconPropertyValidationRules(Constants.CustomValidationPatterns.MaxPluPattern, Constants.ValidatorErrorMessages.MaxPlusError);

                case Constants.ValidatorPropertyNames.CertificationAgencyName:
                    return new IconPropertyValidationRules(Constants.CustomValidationPatterns.AgencyNamePattern, Constants.ValidatorErrorMessages.AgencyNameError);

                case Constants.ValidatorPropertyNames.Notes:
                    return new IconPropertyValidationRules(TraitPatterns.Notes, Constants.ValidatorErrorMessages.NotesError);
                    
                case Constants.ValidatorPropertyNames.CreatedDate:
                case Constants.ValidatorPropertyNames.ModifiedDate:
                    return new IconPropertyValidationRules(Constants.CustomValidationPatterns.DateFormatPattern, Constants.ValidatorErrorMessages.DateFormatError);

                case Constants.ValidatorPropertyNames.ModifiedUser:
                    return new IconPropertyValidationRules(Constants.CustomValidationPatterns.UserFromatPattern, Constants.ValidatorErrorMessages.UserFormatError);

                case Constants.ValidatorPropertyNames.RetailSize:
                    return new IconPropertyValidationRules(Constants.CustomValidationPatterns.RetailSizeFormatPattern, Constants.ValidatorErrorMessages.RetailSizeError);

                case Constants.ValidatorPropertyNames.AlcoholByVolume:
                    return new IconPropertyValidationRules(TraitPatterns.AlcoholByVolume, Constants.ValidatorErrorMessages.AlcoholByVolumeError);

                case Constants.ValidatorPropertyNames.DrainedWeight:
                    return new IconPropertyValidationRules(TraitPatterns.DrainedWeight, Constants.ValidatorErrorMessages.DrainedWeightError);
              
                case Constants.ValidatorPropertyNames.MainProductName:
                    return new IconPropertyValidationRules(TraitPatterns.MainProductName, Constants.ValidatorErrorMessages.MainProductNameError);

                case Constants.ValidatorPropertyNames.ProductFlavorType:
                    return new IconPropertyValidationRules(TraitPatterns.ProductFlavorType, Constants.ValidatorErrorMessages.ProductFlavorTypeError);

                default:
                    throw new ArgumentException($"Unknown property: {propertyToValidate}");
            }
        }
    }
}
