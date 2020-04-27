namespace BrandUploadProcessor.Service.Validation.Interfaces
{
    public interface IRegexTextValidator
    {
        ValidationResponse Validate(string regex, string value);
    }
}