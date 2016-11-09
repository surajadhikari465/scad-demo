using Icon.Web.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValueRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string castValue;

            if (value == null)
            {
                castValue = String.Empty;
            }
            else if (value.GetType() == typeof(DateTime))
            {
                castValue = value.ToString();
            }
            else
            {
                castValue = value as string;
            }
            
            if (new ValueRequiredValidator().Validate(castValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Please enter a value.");
        }
    }
}