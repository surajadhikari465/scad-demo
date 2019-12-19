using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateItemProhibitDiscountByHierarchyClassCommandHandler : ICommandHandler<UpdateItemProhibitDiscountByHierarchyClassCommand>
    {
        IDbConnection db;

        public UpdateItemProhibitDiscountByHierarchyClassCommandHandler(IDbConnection db)
        {
            this.db = db;
        }

        public void Execute(UpdateItemProhibitDiscountByHierarchyClassCommand data)
        {
            this.db.Execute("dbo.UpdateItemProhibitDiscountByHierarchyClassId",
                new
                {
                    hierarchyClassId = data.HierarchyClassId,
                    prohibitDiscount = data.ProhibitDiscount,
                    userName = data.UserName,
                    modifiedDateTimeUtc = data.ModifiedDateTimeUtc
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
