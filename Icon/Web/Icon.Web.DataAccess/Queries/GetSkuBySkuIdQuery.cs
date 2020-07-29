using Icon.Common.DataAccess;
using System.Linq;
using Dapper;
using System.Data;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetSkuBySkuIdQuery : IQueryHandler<GetSkuBySkuIdParameters, SkuModel>
    {
        private IDbConnection connection;


        public GetSkuBySkuIdQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public SkuModel Search(GetSkuBySkuIdParameters parameters)
        {
            return connection.Query<SkuModel>(@"
                            WITH ItemGroupData
                            AS (
	                            SELECT ig.ItemGroupId as SkuId
		                            ,JSON_VALUE(ItemGroupAttributesJson, '$.SkuDescription') AS SkuDescription
		                            ,ig.LastModifiedBy AS LastModifiedBy
		                            ,ig.SysStartTimeUtc AS LastModifiedDate
		                            ,s.scanCode as PrimaryItemUpc
	                            FROM itemgroup ig
	                            LEFT JOIN [dbo].[ItemGroupMember] igm ON ig.ItemGroupId = igm.ItemGroupId
		                            AND igm.IsPrimary = 1
	                            LEFT JOIN scancode s ON s.itemid = igm.itemid
	                            WHERE ig.itemgroupid = @itemgroupid
	                            )
	                            ,ItemCount
                            AS (
	                            SELECT [ItemGroupId]
		                            ,count([ItemId]) AS CountOfItems
	                            FROM [dbo].[ItemGroupMember]
	                            WHERE itemgroupid = @itemgroupid
	                            GROUP BY [ItemGroupId]
	                            )
	                            ,ItemHistory
                            AS (
	                            SELECT TOP 1 [ItemGroupId]
		                            ,SysStartTimeUTC AS CreatedDate
		                            ,LastModifiedBy AS CreatedBy
	                            FROM [dbo].[ItemGroupHistory]
	                            WHERE itemgroupid = @itemgroupid
	                            ORDER BY SysStartTimeUTC ASC
	                            )
                            SELECT ig.SkuId
	                            ,ig.SkuDescription
	                            ,ig.LastModifiedBy
	                            ,ig.LastModifiedDate
	                            ,ig.PrimaryItemUpc
	                            ,ic.CountOfItems
	                            ,ih.CreatedBy
	                            ,ih.CreatedDate
                            FROM ItemGroupData ig
                            LEFT JOIN ItemHistory ih ON (ig.SkuId = ih.[ItemGroupId])
                            LEFT JOIN ItemCount ic ON (ig.SkuId = ic.[ItemGroupId])",
                                new { itemgroupid = parameters.SkuId }).FirstOrDefault();
        }
    }
}
