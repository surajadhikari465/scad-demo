using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOneRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var typeInfo = value.GetType();

            var properties = typeInfo.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(value, null) != null)
                {
                    // One of the search terms had a value, so we can stop here and return a successful validation.
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Please enter at least one search term.");
        }
    }
}