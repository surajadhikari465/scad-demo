using System.Data;
using System.Linq;
using Dapper;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Commands
{
    public class SaveErrorsCommandHandler : ICommandHandler<SaveErrorsCommand>
    {
        private readonly IDbConnection dbConnection;

        public SaveErrorsCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
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

            // todo: update this query to work with brands uploads
            string sql =
                "INSERT INTO BulkUploadErrors (BulkUploadId, RowId, Message) VALUES (@BulkUploadId, @RowId, @Message)";
            dbConnection.Execute(sql, dataToInsert);

        }
    }
}