using Icon.Web.Mvc.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Icon.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PluCategoryStartAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pluCategoryViewModel = validationContext.ObjectInstance as BarcodeTypeViewModel;

            if (pluCategoryViewModel != null && value != null && pluCategoryViewModel.EndRange != null)
            {
                long beginRange = Int64.Parse(value.ToString());
                long endRange = Int64.Parse(pluCategoryViewModel.EndRange);
                if (beginRange < endRange)
                {
                    if (value.ToString().Length > 6)
                    {
                        beginRange = Int64.Parse(value.ToString().Substring(0, 6));
                    }
                    if (pluCategoryViewModel.EndRange.Length > 6)
                    {
                        endRange = Int64.Parse(pluCategoryViewModel.EndRange.Substring(0, 6));
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    return new ValidationResult("PLU Category Start value must be less than End value.");
                }
            }           
            return ValidationResult.Success;     
        }
    }
}