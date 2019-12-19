using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemSubTeamByHierarchyClassCommandHandler : ICommandHandler<UpdateItemSubTeamByHierarchyClassCommand>
    {
        IDbConnection db;

        public UpdateItemSubTeamByHierarchyClassCommandHandler(IDbConnection db)
        {
            this.db = db;
        }

        public void Execute(UpdateItemSubTeamByHierarchyClassCommand data)
        {
            this.db.Execute("dbo.UpdateItemSubTeamByHierarchyClassId",
                new
                {
                    hierarchyClassId = data.HierarchyClassId,
                    subTeamHierarchyClassId = data.SubTeamHierarchyClassId,
                    userName = data.UserName,
                    modifiedDateTimeUtc = data.ModifiedDateTimeUtc
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
