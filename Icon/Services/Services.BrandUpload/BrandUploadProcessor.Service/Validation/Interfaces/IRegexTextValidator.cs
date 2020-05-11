using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Validation.Interfaces
{
    public interface IRegexTextValidator
    {
        ValidationResponse Validate(AttributeColumn attributeColumn, string value);
    }
}