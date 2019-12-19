using System;
using System.Linq;
using Icon.Common.Models;

namespace Icon.Common.Validators.ItemAttributes
{
    public class ItemAttributesNumericItemValidator : AbstractItemAttributesValidator
    {
        private decimal attributeMinimumNumber;
        private decimal attributeMaximumNumber;
        private int attributeNumberOfDecimals;

        public ItemAttributesNumericItemValidator(AttributeModel attribute) : base(attribute)
        {
            if(!decimal.TryParse(attribute.MinimumNumber, out attributeMinimumNumber))
            {
                throw new InvalidOperationException($"Unable to create validator for attribute '{attribute.AttributeName}' because Minimum Number is not set and is required for number attributes.");
            }
            if (!decimal.TryParse(attribute.MaximumNumber, out attributeMaximumNumber))
            {
                throw new InvalidOperationException($"Unable to create validator for attribute '{attribute.AttributeName}' because Maximum Number is not set and is required for number attributes.");
            }
            if (!int.TryParse(attribute.NumberOfDecimals, out attributeNumberOfDecimals))
            {
                throw new InvalidOperationException($"Unable to create validator for attribute '{attribute.AttributeName}' because Number of Decimals is not set and is required for number attributes.");
            }
        }

        public override ItemAttributesValidationResult Validate(string value)
        {
            var result = new ItemAttributesValidationResult { IsValid = true };

            if (attribute.IsRequired && string.IsNullOrWhiteSpace(value))
            {
                result.IsValid = false;
                result.ErrorMessages.Add($"{attribute.DisplayName} is required.");
            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (decimal.TryParse(value, out decimal parseValue))
                {
                    if (parseValue < attributeMinimumNumber)
                    {
                        result.IsValid = false;
                        result.ErrorMessages.Add($"{attribute.DisplayName} must be greater than or equal to {attribute.MinimumNumber}.");
                    }
                    else if (parseValue > attributeMaximumNumber)
                    {
                        result.IsValid = false;
                        result.ErrorMessages.Add($"{attribute.DisplayName} must be less than or equal to {attribute.MaximumNumber}.");
                    }
                    if (value.Contains('.'))
                    {
                        var numberOfDecimals = value.Split('.')[1].Length;
                        if (numberOfDecimals > attributeNumberOfDecimals)
                        {
                            result.IsValid = false;
                            result.ErrorMessages.Add($"{attribute.DisplayName} must have at most {attribute.NumberOfDecimals} number of decimals.");
                        }
                    }
                }
                else
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add($"{attribute.DisplayName} is not a valid number.");
                }
            }
            return result;
        }
    }
}