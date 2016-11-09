
namespace Icon.Web.DataAccess.Commands
{
    public class UpdateProductSelectionGroupCommand
    {
        public int ProductSelectionGroupId { get; set; }
        public string ProductSelectionGroupName { get; set; }
        public int ProductSelectionGroupTypeId { get; set; }
        public int? TraitId { get; set; }
        public string TraitValue { get; set; }
        public int? MerchandiseHierarchyClassId { get; set; }

        //Output property
        public bool ProductSelectionGroupNameChanged { get; set; }

        public bool ProductSelectionGroupTypeChanged { get; set; }
    }
}
