using Icon.Web.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PosDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (new PosDescriptionValidator().Validate(value as string ?? String.Empty))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Please enter 25 or fewer characters.");           
        }
    }
}