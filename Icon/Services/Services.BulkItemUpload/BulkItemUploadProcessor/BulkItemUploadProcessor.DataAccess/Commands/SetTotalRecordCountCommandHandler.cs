using System.Data;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetTotalRecordCountCommandHandler : ICommandHandler<SetTotalRecordCountCommand>
    {
        private readonly IDbConnection DbConnection;

        public SetTotalRecordCountCommandHandler(IDbConnection connection)
        {
            DbConnection = connection;
        }

        public void Execute(SetTotalRecordCountCommand data)
        {
            var query = "update BulkItemUpload set TotalRows = @TotalRecordCount where BulkItemUploadId = @BulkItemUploadId";
            DbConnection.Execute(query, new {data.BulkItemUploadId,data.TotalRecordCount});
        }
    }
}