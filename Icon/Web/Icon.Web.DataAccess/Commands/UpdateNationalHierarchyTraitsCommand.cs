using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateNationalHierarchyTraitsCommand
    {
        public HierarchyClass NationalHierarchy { get; set; }
        public string TraitCode { get; set; }
        public string TraitValue { get; set; }
    }
}
