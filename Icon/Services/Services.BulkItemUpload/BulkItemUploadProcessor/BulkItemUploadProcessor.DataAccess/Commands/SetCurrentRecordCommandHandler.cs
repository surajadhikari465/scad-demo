using System.Data;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetCurrentRecordCommandHandler : ICommandHandler<SetCurrentRecordCommand>
    {
        private readonly IDbConnection DbConnection;

        public SetCurrentRecordCommandHandler(IDbConnection connection)
        {
            DbConnection = connection;
        }

        public void Execute(SetCurrentRecordCommand data)
        {
            var query = "update BulkItemUpload set CurrentRow = @CurrentRecord where BulkItemUploadId = @BulkItemUploadId";
            DbConnection.Execute(query, new { data.BulkItemUploadId, data.CurrentRecord });
        }
    }
}