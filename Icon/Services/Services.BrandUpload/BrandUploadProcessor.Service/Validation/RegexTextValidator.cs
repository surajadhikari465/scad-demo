using System.Text.RegularExpressions;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Mappers;
using BrandUploadProcessor.Service.Validation.Interfaces;

namespace BrandUploadProcessor.Service.Validation
{
    public class RegexTextValidator : IRegexTextValidator
    {
        private readonly IAttributeErrorMessageMapper errorMessageMapper;

        public RegexTextValidator(IAttributeErrorMessageMapper errorMessageMapper)
        {
            this.errorMessageMapper = errorMessageMapper;
        }

        public ValidationResponse Validate(AttributeColumn attributeColumn, string value)
        {
            if (Regex.IsMatch(value, attributeColumn.RegexPattern)) 
                return new ValidationResponse { IsValid = true };

            return new ValidationResponse {IsValid = false, Error = errorMessageMapper.Map(attributeColumn, value)};

        }
    }
}