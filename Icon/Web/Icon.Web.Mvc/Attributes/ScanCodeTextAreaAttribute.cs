using System;
using System.ComponentModel.DataAnnotations;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ScanCodeTextAreaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // The TextArea returns all of the user's input as one concatenated string.
            string scanCodes = value as string;

            if (String.IsNullOrWhiteSpace(scanCodes))
            {
                return new ValidationResult("Please enter at least one scan code.");
            }

            return ValidationResult.Success;
        }
    }
}