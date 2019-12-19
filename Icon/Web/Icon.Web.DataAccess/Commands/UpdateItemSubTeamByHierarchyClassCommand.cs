namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemSubTeamByHierarchyClassCommand
    {
        public int HierarchyClassId { get; set; }
        public int SubTeamHierarchyClassId { get; set; }
        public string UserName { get; set; }
        public string ModifiedDateTimeUtc { get; set; }
    }
}
