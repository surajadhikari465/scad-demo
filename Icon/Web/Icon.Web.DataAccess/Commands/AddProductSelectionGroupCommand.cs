
namespace Icon.Web.DataAccess.Commands
{
    public class AddProductSelectionGroupCommand
    {
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }

        // Output property
        public int ProductSelectionGroupId { get; set; }
    }
}
