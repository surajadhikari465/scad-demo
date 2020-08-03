using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
	/// <summary>
	/// get th ememebers of an Item group.
	/// </summary>
	public class GetItemGroupMembersQuery : IQueryHandler<GetItemGroupMembersParameters, IEnumerable<ItemGroupMember>>
    {
        private IDbConnection dbConnection;

		/// <summary>
		/// Initializes an instance of GetItemGroupMembersQuery.
		/// </summary>
		/// <param name="dbConnection"></param>
		public GetItemGroupMembersQuery(IDbConnection dbConnection)
        {
			if (dbConnection == null)
			{
				throw new ArgumentNullException(nameof(dbConnection));
			}

			this.dbConnection = dbConnection;
        }

		/// <summary>
		/// Executes teh query.
		/// </summary>
		/// <param name="parameters">Get Item Group Members Parameters.</param>
		/// <returns>Lists of ItemGroupMember.</returns>
		public IEnumerable<ItemGroupMember> Search(GetItemGroupMembersParameters parameters)
        {
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			var result = dbConnection.Query<ItemGroupMember>(@"
				SELECT ig.[ItemGroupId] as ItemGroupId
					,igm.ItemId as ItemId
					,sc.scanCode as ScanCode
					,igm.IsPrimary
					,i.ProductDescription
					,i.CustomerFriendlyDescription
					,JSON_VALUE(i.ItemAttributesJson, '$.ItemPack') as ItemPack
					,JSON_VALUE(i.ItemAttributesJson, '$.RetailSize') as RetailSize
					,JSON_VALUE(i.ItemAttributesJson, '$.UOM') as UOM
				FROM [dbo].[ItemGroup] ig
					INNER JOIN [dbo].[ItemGroupMember] igm on (igm.ItemGroupId = ig.ItemGroupId)
					INNER JOIN [dbo].ScanCode sc on (sc.itemID = igm.ItemId)
					INNER JOIN [dbo].[Item] i on (i.itemId = igm.ItemId)
				where ig.ItemGroupId = @ItemGroupId",
				parameters);
			return result;
		}
    }
}
