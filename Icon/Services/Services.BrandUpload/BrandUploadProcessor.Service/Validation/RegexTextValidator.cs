using System.Text;
using System.Text.RegularExpressions;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.Validators.ItemAttributes;

namespace BrandUploadProcessor.Service.Validation
{
    public class RegexTextValidator : IRegexTextValidator
    {
        public ValidationResponse Validate(string regexPattern, string value)
        {
            var result = new ValidationResponse {IsValid = true};

            if (!Regex.IsMatch(value, regexPattern))
            {
                result.IsValid = false;
                result.Error = $"'{value}' does not meet traitPattern [ {regexPattern} ] requirements.";
            }

            return result;

        }
    }
}