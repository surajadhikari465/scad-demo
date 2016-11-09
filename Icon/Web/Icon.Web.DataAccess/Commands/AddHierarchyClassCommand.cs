using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddHierarchyClassCommand
    {
        public HierarchyClass NewHierarchyClass { get; set; }
        public string TaxAbbreviation { get; set; }
        public string GlAccount { get; set; }
        public int SubTeamHierarchyClassId { get; set; }
        public string NonMerchandiseTrait { get; set; }
        public string PosDeptNumber { get; set; }
        public string TeamNumber { get; set; }
        public string TeamName { get; set; }
        public string NationalClassCode { get; set; }
        public string SubBrickCode { get; set; }
    }
}
