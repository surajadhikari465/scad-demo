using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Mappers
{
    public interface IAttributeErrorMessageMapper
    {
        string Map(AttributeColumn attributeColumn, string value);
    }
}