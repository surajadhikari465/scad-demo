using Dapper;
using Icon.Common.DataAccess;
using System.Data;

namespace Icon.Web.DataAccess.Commands
{
    public class AddMessageQueueItemByHierarchyClassIdCommadHandler : ICommandHandler<AddMessageQueueItemByHierarchyClassIdCommand>
    {
        private readonly IDbConnection db;

        public AddMessageQueueItemByHierarchyClassIdCommadHandler(IDbConnection db)
        {
            this.db = db;
        }

        public void Execute(AddMessageQueueItemByHierarchyClassIdCommand data)
        {
            this.db.Execute("app.GenerateItemUpdateMessagesByHierarchyClass", new { hierarchyClassID = data.HierarchyClassId }, commandType: CommandType.StoredProcedure);
        }
    }
}
