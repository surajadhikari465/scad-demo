using Icon.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using Icon.Common.Validators;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RetailUomAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (new RetailUomValidator().Validate(value as string ?? String.Empty))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format("UOM should be one of the following: {0}.", String.Join(", ", UomCodes.ByName.Values))); 
        }
    }
}