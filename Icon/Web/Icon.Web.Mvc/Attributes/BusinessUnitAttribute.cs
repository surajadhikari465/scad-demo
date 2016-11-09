using Icon.Web.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BusinessUnitAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (new BusinessUnitValidator().Validate(value as string ?? String.Empty))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Please enter a whole number with five or fewer digits.");
        }
    }
}