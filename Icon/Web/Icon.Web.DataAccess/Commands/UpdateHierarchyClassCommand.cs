using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateHierarchyClassCommand
    {
        public HierarchyClass UpdatedHierarchyClass { get; set; }
        public string TaxAbbreviation { get; set; }
        public string GlAccount { get; set; }
        public int SubTeamHierarchyClassId { get; set; }

        // 'Output' property to be set in the CommandHandler.
        // Determines whether or not a hierarchy message should be generated.
        public bool ClassNameChanged { get; set; }
    }
}
