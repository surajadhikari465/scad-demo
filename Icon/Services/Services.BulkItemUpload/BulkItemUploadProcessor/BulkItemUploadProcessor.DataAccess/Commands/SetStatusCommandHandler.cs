using System.Data;
using System.Linq;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SetStatusCommandHandler : ICommandHandler<SetStatusCommand>
    {
        private readonly IDbConnection DbConnection;

        public SetStatusCommandHandler(IDbConnection connection)
        {
            DbConnection = connection;
        }

        public void Execute(SetStatusCommand data)
        {
            var query = @"UPDATE BulkUpload
                        SET StatusId = @StatusId
                            ,Message = @Message
                            ,PercentageProcessed = @PercentageProcessed
                        WHERE BulkUploadId = @BulkUploadId";
            DbConnection.Execute(query,
                new
                {
                    StatusId = data.FileStatus, data.BulkUploadId,
                    Message = data.Message.Substring(0, data.Message.Length <= 500 ? data.Message.Length : 500),
                    PercentageProcessed = data.PercentageProcessed
                });
        }
    }
}