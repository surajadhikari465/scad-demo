using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddNationalHierarchyCommand
    {
        public HierarchyClass NationalHierarchy { get; set; }
        public string NationalClassCode { get; set; }
        public string UserName { get; set; }
    }
}
