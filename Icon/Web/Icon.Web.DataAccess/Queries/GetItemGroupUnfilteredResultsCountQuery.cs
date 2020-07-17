using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Queries
{
	/// <summary>
	/// Get Unfiltered Results Count Query
	/// </summary>
	public class GetItemGroupUnfilteredResultsCountQuery : IQueryHandler<GetItemGroupUnfilteredResultsCountQueryParameters, int>
    {
		private IDbConnection dbConnection;

		/// <summary>
		/// Initializes an instance of GetItemGroupUnfilteredResultsCountQuery
		/// </summary>
		/// <param name="dbConnection">IDbConnection</param>
		public GetItemGroupUnfilteredResultsCountQuery(IDbConnection dbConnection)
		{
			if (dbConnection == null)
			{
				throw new ArgumentNullException(nameof(dbConnection));
			}
			this.dbConnection = dbConnection;
		}

		public string UnfilteredResultsCountQuery = @"
			WITH UnfilteredResults AS
			(
				SELECT ig.[ItemGroupId],sc.[ScanCode]		
				FROM [dbo].[ItemGroup] ig (NOLOCK)
					INNER JOIN [dbo].[ItemGroupMember] img (NOLOCK) ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
					INNER JOIN [dbo].[ScanCode] sc (NOLOCK) ON (sc.[ItemId] = img.[ItemId])
				WHERE [ItemGroupTypeId] = @ItemGroupTypeId
			)
			SELECT COUNT(*) AS [UnfilteredResultsCount] FROM UnfilteredResults";

		/// <summary>
		/// Executes the Query
		/// </summary>
		/// <param name="parameters">GetUnfilteredResultsCountQueryParameters</param>
		/// <returns></returns>
		public int Search(GetItemGroupUnfilteredResultsCountQueryParameters parameters)
        {
			// dbConnection is created as scoped, we need to clone it to allow run in parallel
			using (var connection = (IDbConnection)((ICloneable)this.dbConnection).Clone())
			{
				return connection.QueryFirst<int>(UnfilteredResultsCountQuery, parameters);
			}
		}
    }
}
