using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PluAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Prevent null values when no plu was entered.
            string plu = value as string ?? String.Empty;

            if (!Regex.IsMatch(plu, "^[0-9]+$") && plu != String.Empty)
            {
                return new ValidationResult("Please enter numeric values only.");
            }

            // Plu must be less than 6 digits or exactly 11 digits
            if (plu.Length > 6 && plu.Length != 11)
            {
                return new ValidationResult("Regional PLU length does not match national PLU format.");
            }

            return ValidationResult.Success;
        }
    }
}