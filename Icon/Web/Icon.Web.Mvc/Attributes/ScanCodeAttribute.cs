using Icon.Web.Common;
using Icon.Web.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ScanCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Prevent null values when no scan code was entered.
            string scanCode = value as string ?? String.Empty;

            // Allow an empty scan code string.
            if (scanCode == String.Empty)
            {
                return ValidationResult.Success;
            }

            if (new ScanCodeValidator().Validate(scanCode))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format("Please enter {0} or fewer digits.", Constants.ScanCodeMaxLength.ToString()));
        }
    }
}