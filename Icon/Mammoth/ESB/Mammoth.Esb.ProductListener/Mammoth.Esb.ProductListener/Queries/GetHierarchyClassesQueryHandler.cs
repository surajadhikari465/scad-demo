using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Queries
{
    public class GetHierarchyClassesQueryHandler : IQueryHandler<GetHierarchyClassesParameters, List<HierarchyClassModel>>
    {
        private IDbProvider dbProvider;
        
        public GetHierarchyClassesQueryHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        } 

        public List<HierarchyClassModel> Search(GetHierarchyClassesParameters parameters)
        {
            return dbProvider.Connection.Query<HierarchyClassModel>(
                @"SELECT HierarchyClassID as HierarchyClassId, 
                         HierarchyClassName
                  FROM HierarchyClass
                  WHERE HierarchyID = @HierarchyId",
                new { HierarchyId = parameters.HierarchyId },
                dbProvider.Transaction).ToList();
        }
    }
}