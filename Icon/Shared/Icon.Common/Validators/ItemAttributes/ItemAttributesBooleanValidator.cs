using Icon.Common.Models;

namespace Icon.Common.Validators.ItemAttributes
{
    public class ItemAttributesBooleanValidator : AbstractItemAttributesValidator
    {
        public ItemAttributesBooleanValidator(AttributeModel attribute) : base(attribute) { }

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
                if (!bool.TryParse(value, out _))
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add($"{attribute.DisplayName} contains an invalid value. {attribute.DisplayName} can only be true or false.");
                }
            }
            return result;
        }
    }
}