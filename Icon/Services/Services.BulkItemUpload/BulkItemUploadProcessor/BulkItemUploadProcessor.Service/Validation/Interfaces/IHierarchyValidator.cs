namespace BulkItemUploadProcessor.Service.Validation.Interfaces
{
    public interface IHierarchyValidator
    {
        ValidationResponse Validate(string hierarchyName, string value, bool allowEmptyString = false, bool allowRemove = false);
    }
}