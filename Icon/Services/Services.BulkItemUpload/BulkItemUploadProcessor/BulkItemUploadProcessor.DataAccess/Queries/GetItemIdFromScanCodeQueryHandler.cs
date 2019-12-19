using System.Data;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemIdFromScanCodeQueryHandler : IQueryHandler<GetItemIdFromScanCodeParameters, int?>
    {
        private readonly IDbConnection DbConnection;

        public GetItemIdFromScanCodeQueryHandler(IDbConnection connection)
        {
            this.DbConnection = connection;
        }

        public int? Search(GetItemIdFromScanCodeParameters parameters)
        {
            int? scanCodeModel = DbConnection.QueryFirstOrDefault<int?>(
                "select ItemId from dbo.Scancode where Scancode = @ScanCode",
                new
                {
                    parameters.ScanCode
                },
                commandType: CommandType.Text
            );
            return scanCodeModel;
        }
    }
}