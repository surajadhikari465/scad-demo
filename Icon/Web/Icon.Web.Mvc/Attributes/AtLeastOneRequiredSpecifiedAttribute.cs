using Icon.Web.Mvc.Attributes;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AtLeastOneRequiredSpecifiedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var type = value.GetType();

            if (AreValuesSpecified(value, type))
                return ValidationResult.Success;
            else
                return new ValidationResult("Please enter at least one search term.");
        }

        private static bool AreValuesSpecified(object value, Type type)
        {
            foreach (var property in type.GetProperties().Where(p => p.IsDefined(typeof(AtLeastOneRequiredPropertyAttribute), false)))
            {
                if (property.GetValue(value) != null)
                {
                    return true;
                }
            }

            var subProperties = type.GetProperties()
                .Where(p => p.PropertyType.IsClass 
                    && p.PropertyType != typeof(string) 
                    && !p.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)));

            foreach (var subProperty in subProperties)
            {
                var subPropertyValue = subProperty.GetValue(value);
                if (subPropertyValue != null)
                {
                    if (AreValuesSpecified(subPropertyValue, subProperty.PropertyType))
                        return true;
                }
            }
            return false;
        }
    }
}