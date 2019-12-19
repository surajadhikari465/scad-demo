using System;
using System.ComponentModel.DataAnnotations;
using Icon.Common.Validators;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IconPropertyValidationAttribute : ValidationAttribute
    {
        private string propertyToValidate;

        public bool CanBeNullOrEmpty { get; set; }

        public IconPropertyValidationAttribute(string propertyToValidate)
        {
            this.propertyToValidate = propertyToValidate;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // The TextArea returns all of the user's input as one concatenated string.
            string propertyValue = value as string;

            if (string.IsNullOrEmpty(propertyValue) && !CanBeNullOrEmpty)
            {
                return new ValidationResult("Value cannot be null.");
            }
            else if(!string.IsNullOrEmpty(propertyValue))
            {
                IconPropertyValidationRules validationRules = IconPropertyValidationRules.GetIconPropertyValidationRules(propertyToValidate);

                if (!validationRules.IsValid(propertyValue, validationContext.ObjectInstance))
                {
                    return new ValidationResult(validationRules.ErrorMessage);
                }
                
            }
            return ValidationResult.Success;
        }
    }
}