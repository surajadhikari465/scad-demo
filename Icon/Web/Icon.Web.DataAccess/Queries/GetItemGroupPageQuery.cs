using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// Get ItemGroup Query
    /// </summary>
    public class GetItemGroupPageQuery : IQueryHandler<GetItemGroupParameters, List<ItemGroupModel>>
    {
		private IDbConnection dbConnection;

		private const string FilteredQuery = @"
			WITH UnfilteredResults AS
			(
				SELECT ig.[ItemGroupId]
						,ig.[ItemGroupTypeId]
						,CASE WHEN @ItemGroupTypeId = 1 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SkuDescription') ELSE NULL END AS SKUDescription
						,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription') ELSE NULL END AS PriceLineDescription
						,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineSize') ELSE NULL END AS PriceLineSize
						,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM') ELSE NULL END AS PriceLineUOM		
						,sc.[ScanCode]		
				FROM [dbo].[ItemGroup] ig
					INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
					INNER JOIN [dbo].[ScanCode] sc ON (sc.[ItemId] = img.[ItemId])
				WHERE [ItemGroupTypeId] = @ItemGroupTypeId
			),
			FilteredResults AS
			(
				SELECT [ItemGroupId]
						,[ItemGroupTypeId]
						,[SKUDescription]
						,[PriceLineDescription]
						,[PriceLineSize]
						,[PriceLineUOM]
						,[ScanCode]		
				FROM UnfilteredResults
				WHERE [ItemGroupId] LIKE @searchTerm
						OR (@ItemGroupTypeId = 1 AND SKUDescription LIKE @SearchTerm)
						OR (@ItemGroupTypeId = 2 AND PriceLineDescription LIKE @SearchTerm)
						OR (@ItemGroupTypeId = 2 AND PriceLineSize LIKE @SearchTerm)
						OR (@ItemGroupTypeId = 2 AND PriceLineUOM LIKE @SearchTerm)
						OR scanCode LIKE @SearchTerm	
			),
			PagedResults AS 
			(
				SELECT [ItemGroupId],[ItemGroupTypeId],[SKUDescription],[PriceLineDescription],[PriceLineSize],[PriceLineUOM],[ScanCode]
				FROM FilteredResults				
				ORDER BY [ItemGroupId] DESC -- REPLACE IN CODE
				OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY
			)
			SELECT [ItemGroupId]
				,[ItemGroupTypeId]
				,[SKUDescription]
				,[PriceLineDescription]
				,[PriceLineSize]
				,[PriceLineUOM]
				,[ScanCode]
			FROM PagedResults;
			";

		private const string UnfilteredQuery = @"
			WITH UnfilteredResults AS
			(
				SELECT ig.[ItemGroupId]
						,ig.[ItemGroupTypeId]
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SkuDescription') as SKUDescription
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription') as PriceLineDescription
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineRetailSize') as PriceLineSize
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM') as PriceLineUOM		
						,sc.[ScanCode]		
				FROM [dbo].[ItemGroup] ig
					INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
					INNER JOIN [dbo].[ScanCode] sc ON (sc.[ItemId] = img.[ItemId])
				WHERE [ItemGroupTypeId] = @ItemGroupTypeId
			),
			PagedResults AS 
			(
				SELECT [ItemGroupId],[ItemGroupTypeId],[SKUDescription],[PriceLineDescription],[PriceLineSize],[PriceLineUOM],[ScanCode]
				FROM UnfilteredResults
				ORDER BY [ItemGroupId] DESC -- REPLACE IN CODE
				OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY
			)
			SELECT [ItemGroupId]
				,[ItemGroupTypeId]
				,[SKUDescription]
				,[PriceLineDescription]
				,[PriceLineSize]
				,[PriceLineUOM]
				,[ScanCode]
			FROM PagedResults;
			";

		private const string FilteredQueryOrderByItemCount = @"
			WITH UnfilteredResults AS
			(
				SELECT ig.[ItemGroupId]
						,ig.[ItemGroupTypeId]
						,CASE WHEN @ItemGroupTypeId = 1 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SkuDescription') ELSE NULL END AS SKUDescription
						,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription') ELSE NULL END AS PriceLineDescription
						,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineSize') ELSE NULL END AS PriceLineSize
						,CASE WHEN @ItemGroupTypeId = 2 THEN JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM') ELSE NULL END AS PriceLineUOM							,sc.[ScanCode]		
				FROM [dbo].[ItemGroup] ig (NOLOCK)
					INNER JOIN [dbo].[ItemGroupMember] img (NOLOCK) ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
					INNER JOIN [dbo].[ScanCode] sc (NOLOCK) ON (sc.[ItemId] = img.[ItemId])
				WHERE [ItemGroupTypeId] = @ItemGroupTypeId
			),
			FilteredResults AS
			(
				SELECT [ItemGroupId]
						,[ItemGroupTypeId]
						,[SKUDescription]
						,[PriceLineDescription]
						,[PriceLineSize]
						,[PriceLineUOM]
						,[ScanCode]		
				FROM UnfilteredResults
				WHERE [ItemGroupId] LIKE @searchTerm
						OR (@ItemGroupTypeId = 1 AND SKUDescription LIKE @SearchTerm)
						OR (@ItemGroupTypeId = 2 AND PriceLineDescription LIKE @SearchTerm)
						OR (@ItemGroupTypeId = 2 AND PriceLineSize LIKE @SearchTerm)
						OR (@ItemGroupTypeId = 2 AND PriceLineUOM LIKE @SearchTerm)
						OR scanCode LIKE @searchTerm	
			),
			ItemCount as 
			(
				SELECT [ItemGroupId], count([ItemId]) as ItemCount
				  FROM [dbo].[ItemGroupMember]
				  GROUP BY [ItemGroupId]
			),
			PagedResults AS 
			(
				SELECT fr.[ItemGroupId],[ItemGroupTypeId],[SKUDescription],[PriceLineDescription],[PriceLineSize],[PriceLineUOM],[ScanCode], ic.[ItemCount]
				FROM FilteredResults fr
					INNER JOIN ItemCount ic ON (fr.[ItemGroupId] = ic.[ItemGroupId])		
				ORDER BY [ItemCount] DESC --REPLACE in CODE
				OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY
			)
			SELECT [ItemGroupId]
				,[ItemGroupTypeId]
				,[SKUDescription]
				,[PriceLineDescription]
				,[PriceLineSize]
				,[PriceLineUOM]
				,[ScanCode]
				,[ItemCount]
			FROM PagedResults;";

		private const string UnfilteredQueryOrderByItemCount = @"
			WITH UnfilteredResults AS
			(
				SELECT ig.[ItemGroupId]
						,ig.[ItemGroupTypeId]
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SkuDescription') as SKUDescription
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription') as PriceLineDescription
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineRetailSize') as PriceLineSize
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM') as PriceLineUOM		
						,sc.[ScanCode]		
				FROM [dbo].[ItemGroup] ig
					INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
					INNER JOIN [dbo].[ScanCode] sc ON (sc.[ItemId] = img.[ItemId])
				WHERE [ItemGroupTypeId] = @ItemGroupTypeId
			),
			ItemCount as 
			(
				SELECT [ItemGroupId], count([ItemId]) as ItemCount
				  FROM [dbo].[ItemGroupMember]
				  GROUP BY [ItemGroupId]
			),
			PagedResults AS 
			(
				SELECT fr.[ItemGroupId],[ItemGroupTypeId],[SKUDescription],[PriceLineDescription],[PriceLineSize],[PriceLineUOM],[ScanCode], ic.[ItemCount]
				FROM UnfilteredResults fr
					INNER JOIN ItemCount ic ON (fr.[ItemGroupId] = ic.[ItemGroupId])	
				ORDER BY [ItemCount] DESC --REPLACE in CODE
				OFFSET @RowsOffset ROWS FETCH NEXT @PageSize ROWS ONLY
			)
			SELECT [ItemGroupId]
				,[ItemGroupTypeId]
				,[SKUDescription]
				,[PriceLineDescription]
				,[PriceLineSize]
				,[PriceLineUOM]
				,[ScanCode]
				,[ItemCount]
			FROM PagedResults;";

		private const string ItemCountQuery = @"
			SELECT [ItemGroupId], count([ItemId]) as ItemCount
			  FROM [dbo].[ItemGroupMember]
			  WHERE [ItemGroupId] in @ItemGroupIds
			  GROUP BY [ItemGroupId]";

        /// <summary>
        /// Initializes an instance of GetItemGroupQuery
        /// </summary>
        /// <param name="dbConnection">IDbConnection</param>
        public GetItemGroupPageQuery(IDbConnection dbConnection)
        {
			if (dbConnection == null)
			{
				throw new ArgumentNullException(nameof(dbConnection));
			}
			
			this.dbConnection = dbConnection;
		}

		/// <summary>
		/// Execute the query
		/// </summary>
		/// <param name="parameters">GetItemGroupParameters</param>
		/// <returns></returns>
		public List<ItemGroupModel> Search(GetItemGroupParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			// dbConnection is created as scoped, we need to clone it to allow run in parallel
			using (var connection = (IDbConnection)((ICloneable)this.dbConnection).Clone())
			{
				List<ItemGroupModel> itemGroupPage = null;
				// generate the effective query
				if (parameters.SortColumn != ItemGroupColumns.ItemCount)
				{ 
					string baseQuery = String.IsNullOrWhiteSpace(parameters.SearchTerm) ?
											UnfilteredQuery 
											: FilteredQuery;
					var sortOrderStament = (parameters.SortOrder == SortOrder.Ascending) ? "ASC" : "DESC";
					string sortStament = $"ORDER BY {parameters.SortColumn} {sortOrderStament}";
					string effectiveQuery = baseQuery.Replace("ORDER BY [ItemGroupId] DESC -- REPLACE IN CODE", sortStament);
					
					// Get the itemgroup page
					itemGroupPage = (connection.Query<ItemGroupModel>(effectiveQuery, parameters, commandTimeout: 600))

								.ToList();

					// Get the Item Count for each itemgroup in the page
					var itemGroupIds = itemGroupPage.Select(ig => ig.ItemGroupId).ToList();
					var itemCounts = (connection.Query<ItemGroupModel>(ItemCountQuery, new { ItemGroupIds = itemGroupIds }))
										.ToDictionary(ic => ic.ItemGroupId);

					// Update the ItemCount in itemGroupPage
					foreach (var itemGroup in itemGroupPage)
					{
						itemGroup.ItemCount = itemCounts[itemGroup.ItemGroupId]?.ItemCount;
					}
				}
				else
				{
					string baseQuery = String.IsNullOrWhiteSpace(parameters.SearchTerm) ?
										UnfilteredQueryOrderByItemCount 
										: FilteredQueryOrderByItemCount;
					var sortOrderStament = (parameters.SortOrder == SortOrder.Ascending) ? "ASC" : "DESC";
					string effectiveQuery = baseQuery.Replace("[ItemCount] DESC --REPLACE in CODE", $"[ItemCount] {sortOrderStament}");

					// Get the itemgroup page
					itemGroupPage = (connection.Query<ItemGroupModel>(effectiveQuery, parameters, commandTimeout: 240))
								.ToList();
				}


				return itemGroupPage;
			}
		}
	}
}
