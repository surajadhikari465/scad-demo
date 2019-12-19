namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemProhibitDiscountByHierarchyClassCommand
    {
        public int HierarchyClassId { get; set; }
        public string ProhibitDiscount { get; set; }
        public string UserName { get; set; }
        public string ModifiedDateTimeUtc { get; set; }
    }
}