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
            var query = "select FileContent from  BulkItemUploadData where BulkItemUploadId = @BulkItemUploadId";
            var fileContent = Connection.ExecuteScalar<byte[]>(query, new { parameters.BulkItemUploadId });
            return new GetFileContentResults { Data = fileContent};
        }
    }
}