using System.Collections.Generic;
using System.Data;
using System.Linq;
using BrandUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Queries
{
    public class GetBrandsQueryHandler : IQueryHandler<EmptyQueryParameters<List<BrandModel>>, List<BrandModel>>
    {
        private readonly IDbConnection dbConnection;

        public GetBrandsQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public List<BrandModel> Search(EmptyQueryParameters<List<BrandModel>> paremeters)
        {
            return dbConnection.Query<BrandModel>(
                @"select hc.HierarchyClassId BrandId, HierarchyClassName BrandName, hct.traitvalue BrandAbbreviation
                        from HierarchyClass hc 
                        inner join Hierarchy h on hc.hierarchyID = h.hierarchyID
                        inner join HierarchyClassTrait hct on hc.hierarchyclassid = hct.hierarchyclassid
                        inner join Trait t on hct.traitid = t.traitid
                         where h.HierarchyName = 'Brands' and t.traitCode = 'BA'").ToList();
        }
    }
}