using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemTypeByHierarchyClassCommandHandler : ICommandHandler<UpdateItemTypeByHierarchyClassCommand>
    {
        IDbConnection db;

        public UpdateItemTypeByHierarchyClassCommandHandler(IDbConnection db)
        {
            this.db = db;
        }

        public void Execute(UpdateItemTypeByHierarchyClassCommand data)
        {
            this.db.Execute("dbo.UpdateItemTypeByHierarchyClassId",
                new
                {
                    hierarchyClassId = data.HierarchyClassId,
                    itemTypeId = data.ItemTypeId,
                    userName = data.UserName,
                    modifiedDateTimeUtc = data.ModifiedDateTimeUtc
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
