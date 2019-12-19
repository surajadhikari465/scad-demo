using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WFM.OutOfStock.API.Domain;

namespace WFM.OutOfStock.API.Validation
{
    public sealed class RegionCodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var regionCode = value as string;
            if (regionCode == null || string.IsNullOrWhiteSpace(regionCode))
            {
                return new ValidationResult("Region code provided must not be empty or whitespace");
            }

            if (!Constants.ValidRegionCodes.Contains(regionCode))
            {
                return new ValidationResult("Region code provided must exist in the collection of predetermined codes");
            }

            return ValidationResult.Success;
        }
    }
}