using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Icon.Common.Models;

namespace Icon.Common.Validators.ItemAttributes
{
    public class ItemAttributesTextValidator : AbstractItemAttributesValidator
    {
        public ItemAttributesTextValidator(AttributeModel attribute) : base(attribute) { }

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
                if (value.Length > attribute.MaxLengthAllowed)
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add($"{attribute.DisplayName} has a max length of {attribute.MaxLengthAllowed}.");
                }
                if (attribute.CharacterSetRegexPattern != null)
                {
                    if (!Regex.IsMatch(value, attribute.CharacterSetRegexPattern))
                    {
                        result.IsValid = false;
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append($"{attribute.DisplayName} does not meet character set restrictions. {attribute.DisplayName} only accepts ")
                            .Append(string.Join(", ", attribute.CharacterSets.Select(cs => cs.CharacterSetModel.Name)));
                        if (!string.IsNullOrWhiteSpace(attribute.SpecialCharactersAllowed))
                        {
                            stringBuilder.Append($" and allows the following special characters: {attribute.SpecialCharactersAllowed}.");
                        }
                        else
                        {
                            stringBuilder.Append(".");
                        }
                        result.ErrorMessages.Add(stringBuilder.ToString());
                    }
                }
            }
            return result;
        }
    }
}