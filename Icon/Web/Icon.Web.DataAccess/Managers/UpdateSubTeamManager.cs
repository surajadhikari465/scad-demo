namespace Icon.Web.DataAccess.Managers
{
    public class UpdateSubTeamManager
    {
        public int HierarchyClassId { get; set; }
        public int HierarchyId { get; set; }
        public int HierarchyLevel { get; set; }
        public int? HierarchyParentClassId { get; set; }
        public string SubTeamName { get; set; }
        public string PeopleSoftNumber { get; set; }
        public string PosDeptNumber { get; set; }
        public string TeamNumber { get; set; }
        public string TeamName { get; set; }
        public bool NonAlignedSubteam { get; set; }
    }
}
