namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface IHierarchyValueParser
    {
        string ParseHierarchyClassIdForContract(string hierarchyClassName, int hierarchyClassId, int hierarchyId);

        string ParseHierarchyNameForContract(string hierarchyClassName, int hierarchyClassId, int hierarchyId);
    }
}