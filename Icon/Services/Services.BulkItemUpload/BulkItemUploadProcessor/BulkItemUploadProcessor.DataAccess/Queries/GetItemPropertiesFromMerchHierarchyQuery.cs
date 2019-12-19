using System.Data;
using System.Linq;
using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemPropertiesFromMerchHierarchyQueryHandler : IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel>
    {
        private readonly IDbConnection DbConnection;

        public GetItemPropertiesFromMerchHierarchyQueryHandler(IDbConnection connection)
        {
            this.DbConnection = connection;
        }

        public MerchDependentItemPropertiesModel Search(GetItemPropertiesFromMerchHierarchyParameters parameters)
        {
            MerchDependentItemPropertiesModel merchDependentItemPropertiesModel = DbConnection.Query<MerchDependentItemPropertiesModel>(
                "dbo.GetItemPropertiesByMerchHierarchyClassId",
                new
                {
                    merchHierarchyClassId = parameters.MerchHierarchyClassId
                },
                commandType: CommandType.StoredProcedure
            ).FirstOrDefault();
            return merchDependentItemPropertiesModel;
        }
    }
}