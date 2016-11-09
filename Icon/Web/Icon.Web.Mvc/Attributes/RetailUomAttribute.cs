using Icon.Framework;
using Icon.Web.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

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