using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetMerchandiseHierarchyClassTraitsQuery : IQueryHandler<GetMerchandiseHierarchyClassTraitsParameters, IEnumerable<MerchandiseHierarchyClassTrait>>
    {
        private IDbConnection dbConnection;

        public GetMerchandiseHierarchyClassTraitsQuery(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<MerchandiseHierarchyClassTrait> Search(GetMerchandiseHierarchyClassTraitsParameters parameters)
        {
            return dbConnection.Query<MerchandiseHierarchyClassTrait>(@"SELECT 
            hc.hierarchyClassID AS HierarchyClassId,
            hctmfm.traitValue AS FinancialHierarchyClassId,
            hctnm.traitValue AS ItemType,
            CAST(hctprh.traitValue as bit) AS ProhibitDiscount 
            FROM dbo.HierarchyClass hc
		    INNER JOIN dbo.HierarchyClass parent on parent.hierarchyClassID = hc.hierarchyParentClassID
            LEFT JOIN dbo.HierarchyClassTrait hctmfm ON hctmfm.traitID = (SELECT traitId FROM dbo.trait WHERE traitCode='MFM') AND hctmfm.hierarchyClassID = hc.hierarchyClassID
            LEFT JOIN dbo.HierarchyClassTrait hctnm ON hctnm.traitID = (SELECT traitId FROM dbo.trait WHERE traitCode='NM') AND hctnm.hierarchyClassID = hc.hierarchyClassID
            LEFT JOIN dbo.HierarchyClassTrait hctprh ON hctprh.traitID = (SELECT traitId FROM dbo.trait WHERE traitCode='PRH') AND hctprh.hierarchyClassID = parent.hierarchyClassID
            JOIN dbo.Hierarchy h ON h.hierarchyID = hc.hierarchyID
            WHERE h.hierarchyName ='Merchandise' AND hc.HierarchyLevel = 5");
        }
    }
}
