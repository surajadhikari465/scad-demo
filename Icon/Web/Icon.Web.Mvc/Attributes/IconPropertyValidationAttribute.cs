using System;
using System.ComponentModel.DataAnnotations;
using Icon.Web.Common.Validators;
using Icon.Web.Common;
using System.Text.RegularExpressions;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IconPropertyValidationAttribute : ValidationAttribute
    {

        private string propertyToValidate;

        public bool CanBeNullOrEmprty { get; set; }
        public IconPropertyValidationAttribute(string propertyToValidate)
        {
            this.propertyToValidate = propertyToValidate;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // The TextArea returns all of the user's input as one concatenated string.
            string propertyValue = value as string;

            if (String.IsNullOrEmpty(propertyValue) && !CanBeNullOrEmprty)
            {
                return new ValidationResult("Value cannot be null.");
            }
            else if(!String.IsNullOrEmpty(propertyValue))
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