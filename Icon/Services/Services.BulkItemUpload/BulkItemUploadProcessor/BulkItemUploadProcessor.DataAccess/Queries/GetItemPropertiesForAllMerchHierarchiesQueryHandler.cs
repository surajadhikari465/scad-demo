using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Data;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemPropertiesForAllMerchHierarchiesQueryHandler : IQueryHandler<
        EmptyQueryParameters<IEnumerable<MerchPropertiesModel>>, IEnumerable<MerchPropertiesModel>>
    {
        private readonly IDbConnection DbConnection;

        public GetItemPropertiesForAllMerchHierarchiesQueryHandler(IDbConnection dbConnection)
        {
            this.DbConnection = dbConnection;
        }

        public IEnumerable<MerchPropertiesModel> Search(EmptyQueryParameters<IEnumerable<MerchPropertiesModel>> parameters)
        {
            var sql = @"[dbo].[GetItemPropertiesForAllMerchHierarchies]";
            var results = DbConnection.Query<MerchPropertiesModel>(sql);

            return results;
        }
    }
}