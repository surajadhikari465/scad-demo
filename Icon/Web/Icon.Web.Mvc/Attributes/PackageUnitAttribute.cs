using System;
using System.ComponentModel.DataAnnotations;
using Icon.Common.Validators;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PackageUnitAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (new PackageUnitValidator().Validate(value as string ?? String.Empty))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Please enter a whole number with three or fewer digits.");
        }
    }
}