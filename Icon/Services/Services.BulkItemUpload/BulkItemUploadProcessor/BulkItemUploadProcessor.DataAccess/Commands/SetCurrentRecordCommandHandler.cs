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
            var query = @"UPDATE BulkUpload
                        SET CurrentRow = @CurrentRecord
                        WHERE BulkUploadId = @BulkUploadId";
            DbConnection.Execute(query, new { data.BulkUploadId, data.CurrentRecord });
        }
    }
}