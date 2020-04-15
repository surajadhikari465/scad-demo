using System.Data;
using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetFileContentQueryHandler : IQueryHandler<GetFileContentParameters, GetFileContentResults>
    {
        private readonly IDbConnection Connection;

        public GetFileContentQueryHandler(IDbConnection connection)
        {
            Connection = connection;
        }

        public GetFileContentResults Search(GetFileContentParameters parameters)
        {
            var query = @"SELECT FileContent
                        FROM BulkUploadData
                        WHERE BulkUploadId = @BulkUploadId";

            var fileContent = Connection.ExecuteScalar<byte[]>(query, new { parameters.BulkUploadId });
            return new GetFileContentResults { Data = fileContent};
        }
    }
}