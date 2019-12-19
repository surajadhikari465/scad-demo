using System.Linq;
using Icon.Common.Models;

namespace Icon.Common.Validators.ItemAttributes
{
    public class ItemAttributesPickListValidator : AbstractItemAttributesValidator
    {
        public ItemAttributesPickListValidator(AttributeModel attribute) : base(attribute) { }

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
                if (!attribute.PickListData.Any(pl => pl.PickListValue == value))
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add($"{attribute.DisplayName} does not contain '{value}' as a possible value.");
                }
            }
            return result;
        }
    }
}