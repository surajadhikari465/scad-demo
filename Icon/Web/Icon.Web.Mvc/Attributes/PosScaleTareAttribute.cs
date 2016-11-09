using Icon.Web.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PosScaleTareAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (new PosScaleTareValidator().Validate(value as string ?? String.Empty))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Please enter a value between 0 and 9.999.");
        }
    }
}