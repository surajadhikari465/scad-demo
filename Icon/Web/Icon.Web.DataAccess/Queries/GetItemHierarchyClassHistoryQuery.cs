using Dapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemHierarchyClassHistoryQuery : IQueryHandler<GetItemHierarchyClassHistoryParameters, ItemHierarchyClassHistoryAllModel>
    {
        private IDbConnection dbConnection;

        public GetItemHierarchyClassHistoryQuery(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public ItemHierarchyClassHistoryAllModel Search(GetItemHierarchyClassHistoryParameters parameters)
        {
            string sql = @"
                SELECT ihc.ItemId,
	            ihc.hierarchyClassId,
                ihc.localeId,
                ihc.SysStartTimeUtc,
                ihc.SysEndTimeUtc,
	            hc.hierarchyClassName,
	            hc.hierarchyId,
	            h.hierarchyName,
	            (SELECT HierarchyLineage FROM {0}HierarchyView WHERE  hierarchyClassID = hc.hierarchyClassID) as hierarchyLineage
            FROM dbo.ItemHierarchyClass FOR SYSTEM_TIME ALL ihc
            JOIN dbo.HierarchyClass hc on hc.hierarchyClassId = ihc.hierarchyClassId
            JOIN dbo.Hierarchy h on h.hierarchyID = hc.hierarchyID
            WHERE ItemId = @itemId AND h.hierarchyID = @hierarchyId
            ORDER BY SysStartTimeUtc
            OPTION(RECOMPILE);";
                
            ItemHierarchyClassHistoryAllModel response = new ItemHierarchyClassHistoryAllModel();
            response.MerchHierarchy = this.dbConnection.Query<ItemHierarchyClassHistoryModel>(string.Format(sql, "Merchandise"), new { itemId = parameters.ItemId, hierarchyId = Hierarchies.Merchandise}).ToList();
            response.BrandHierarchy= this.dbConnection.Query<ItemHierarchyClassHistoryModel>(string.Format(sql, "Brand"), new { itemId = parameters.ItemId, hierarchyId = Hierarchies.Brands }).ToList();
            response.TaxHierarchy = this.dbConnection.Query<ItemHierarchyClassHistoryModel>(string.Format(sql, "Tax"), new { itemId = parameters.ItemId, hierarchyId = Hierarchies.Tax }).ToList();
            response.NationalHierarchy= this.dbConnection.Query<ItemHierarchyClassHistoryModel>(string.Format(sql, "NationalClass"), new { itemId = parameters.ItemId, hierarchyId = Hierarchies.National }).ToList();
            response.FinancialHierarchy = this.dbConnection.Query<ItemHierarchyClassHistoryModel>(string.Format(sql, "Financial"), new { itemId = parameters.ItemId, hierarchyId = Hierarchies.Financial }).ToList();
            response.ManufacturerHierarchy = this.dbConnection.Query<ItemHierarchyClassHistoryModel>(string.Format(sql, "Manufacturer"), new { itemId = parameters.ItemId, hierarchyId = Hierarchies.Manufacturer }).ToList();
            return response;
        }
    }
}