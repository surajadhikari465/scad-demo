namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemTypeByHierarchyClassCommand
    {
        public int HierarchyClassId { get; set; }
        public int ItemTypeId { get; set; }
        public string UserName { get; set; }
        public string ModifiedDateTimeUtc { get; set; }
    }
}
