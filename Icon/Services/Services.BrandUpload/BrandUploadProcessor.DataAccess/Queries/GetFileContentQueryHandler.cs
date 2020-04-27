using System.Data;
using BrandUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Queries
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
            var query = "select FileContent from  BulkUploadData where BulkUploadId = @BulkUploadId";
            var fileContent = Connection.ExecuteScalar<byte[]>(query, new { parameters.BulkUploadId });
            return new GetFileContentResults { Data = fileContent };
        }
    }
}