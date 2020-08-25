using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
	/// <summary>
	/// Returns the a GetItemGroupById
	/// </summary>
	public class GetItemGroupByIdQuery : IQueryHandler<GetItemGroupByIdParameters, ItemGroupModel>
    {
        private IDbConnection dbConnection;

		/// <summary>
		/// Initializes an instance of the class
		/// </summary>
		/// <param name="dbConnection"></param>
		public GetItemGroupByIdQuery(IDbConnection dbConnection)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection));
            }

            this.dbConnection = dbConnection;
        }

		/// <summary>
		/// Run the search
		/// </summary>
		/// <param name="parameters">GetItemGroupFilteredResultsCountQueryParameters</param>
		/// <returns></returns>
		public ItemGroupModel Search(GetItemGroupByIdParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}


			var result = dbConnection.QueryFirstOrDefault<ItemGroupModel>(@"
			SELECT ig.[ItemGroupId]
						,ig.[ItemGroupTypeId]
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SKUDescription') AS SKUDescription
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription') AS PriceLineDescription
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineSize') AS PriceLineSize
						,JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM') AS PriceLineUOM		
						,ig.[ItemGroupAttributesJson]
						,sc.[ScanCode]		
				FROM [dbo].[ItemGroup] ig
					LEFT JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
					LEFT JOIN [dbo].[ScanCode] sc ON (sc.[ItemId] = img.[ItemId])
				WHERE ig.[ItemGroupId] = @ItemGroupId", 
				parameters);

			return result;
		}
	}
}
