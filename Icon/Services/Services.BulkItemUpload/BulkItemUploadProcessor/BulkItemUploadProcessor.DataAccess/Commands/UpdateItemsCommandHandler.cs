using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using System.Data;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class UpdateItemsCommandHandler : ICommandHandler<UpdateItemsCommand>
    {
        private readonly IDbConnection connection;

        public UpdateItemsCommandHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public void Execute(UpdateItemsCommand data)
        {
            connection.Execute(
                "dbo.UpdateItems",
                new
                {
                    Items = data.Items.ToDataTable().AsTableValuedParameter("dbo.UpdateItemsType")
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
