using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Linq;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemPropertiesFromMerchHierarchyQuery : IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel>
    {
        private IDbConnection connection;

        public GetItemPropertiesFromMerchHierarchyQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public MerchDependentItemPropertiesModel Search(GetItemPropertiesFromMerchHierarchyParameters parameters)
        {
            MerchDependentItemPropertiesModel merchDependentItemPropertiesModel = connection.Query<MerchDependentItemPropertiesModel>(
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