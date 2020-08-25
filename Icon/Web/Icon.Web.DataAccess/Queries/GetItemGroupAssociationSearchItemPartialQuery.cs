using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// Returns the items with an specific scan code prefix, not associated with an Item Group.
    /// </summary>
    public class GetItemGroupAssociationSearchItemPartialQuery : IQueryHandler<GetItemGroupAssociationSearchItemPartialParameters, IEnumerable<ItemGroupAssociationItemModel>>
    {
        private IDbConnection dbConnection;

        private const string ItemCountQuery = @"
			SELECT [ItemGroupId], count([ItemId]) as ItemCount
			  FROM [dbo].[ItemGroupMember]
			  WHERE [ItemGroupId] in @ItemGroupIds
			  GROUP BY [ItemGroupId]";

        private const string ItemSelectionQuery = @"
                SELECT TOP (@MaxResultSize) 
	                i.[ItemId] as ItemId
	                ,sc.[scanCode] as ScanCode
	                ,i.[CustomerFriendlyDescription] AS CustomerFriendlyDescription
	                ,ig.[ItemGroupId] AS ItemGroupId
	                ,CASE WHEN @ItemGroupTypeId = 1 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SKUDescription') ELSE NULL END AS SKUDescription
	                ,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription') ELSE NULL END AS PriceLineDescription
					,igm.IsPrimary
                FROM [dbo].[Item] i
	                INNER JOIN [dbo].[ScanCode] sc on (sc.itemID = i.ItemId)
	                INNER JOIN [dbo].[ItemGroupMember] igm on (igm.ItemId = i.ItemId)
	                INNER JOIN [dbo].[ItemGroup] ig on (igm.ItemGroupId = ig.ItemGroupId)
                WHERE sc.[scanCode] like @ScanCodePrefix
		                AND ig.[ItemGroupTypeId] =  @ItemGroupTypeId
                ORDER by sc.[scanCode] ASC;";

        /// <summary>
        /// Initializes an instance of (GetItemGroupAssociationSearchItemPartialQuery)
        /// </summary>
        /// <param name="dbConnection">IDbConnection.</param>
        public GetItemGroupAssociationSearchItemPartialQuery (IDbConnection dbConnection)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            this.dbConnection = dbConnection;
        }

        /// <summary>
        /// Search items with an specific scan code prefix, not associated with an Item Group. 
        /// </summary>
        /// <param name="parameters">GetItemGroupAssociationSearchItemPartialParameters</param>
        /// <returns>List of Item data.</returns>
        public IEnumerable<ItemGroupAssociationItemModel> Search(GetItemGroupAssociationSearchItemPartialParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var itemsSet = this.dbConnection.Query<ItemGroupAssociationItemModel>(ItemSelectionQuery, 
                parameters);

            var itemGroupIds = itemsSet.Select(ig => ig.ItemGroupId).ToList();
            var itemCounts = (this.dbConnection.Query<ItemGroupModel>(ItemCountQuery, new { ItemGroupIds = itemGroupIds }))
                                .ToDictionary(ic => ic.ItemGroupId);

            // Update the ItemCount in the item set.
            foreach (var item in itemsSet)
            {
                item.ItemGroupItemCount = itemCounts[item.ItemGroupId]?.ItemCount;
            }

            return itemsSet;
        }
    }
}
