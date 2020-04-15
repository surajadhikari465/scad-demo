using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Icon.Common.DataAccess;
using MoreLinq;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class SaveItemErrorsCommandHandler : ICommandHandler<SaveErrorsCommand>
    {
        private readonly IDbConnection DbConnection;

        public SaveItemErrorsCommandHandler(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void Execute(SaveErrorsCommand data)
        {
            var dataToInsert = from error in data.ErrorList
                select new
                {
                    data.BulkUploadId,
                    data.RowId,
                    Message = error
                };

            string sql =
                @"INSERT INTO BulkUploadErrors (BulkUploadId, RowId, Message) VALUES (@BulkUploadId, @RowId, @Message)";
            DbConnection.Execute(sql, dataToInsert);

        }
    }
}