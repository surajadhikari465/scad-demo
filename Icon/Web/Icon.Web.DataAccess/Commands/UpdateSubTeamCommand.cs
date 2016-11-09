
using Icon.Framework;
namespace Icon.Web.DataAccess.Commands
{
    public class UpdateSubTeamCommand : HierarchyClassCommand
    {
        public string SubTeamName { get; set; }
        public string PeopleSoftNumber { get; set; }

        //Output parameters
        public HierarchyClass UpdatedHierarchyClass { get; set; }
        public bool PeopleSoftChanged { get; set; }
    }
}
