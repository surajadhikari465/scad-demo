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
	/// Returns the number of in itemGroup total records that meet the filter
	/// </summary>
    public class GetItemGroupFilteredResultsCountQuery : IQueryHandler<GetItemGroupFilteredResultsCountQueryParameters, int>
    {
        private IDbConnection dbConnection;

		private string FilteredResultsCountQuery = @"			
				WITH FilteredResults AS
				(
					SELECT ig.[ItemGroupId]
					FROM [dbo].[ItemGroup] ig (NOLOCK)
						INNER JOIN [dbo].[ItemGroupMember] img (NOLOCK) ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
						INNER JOIN [dbo].[ScanCode] sc (NOLOCK) ON (sc.[ItemId] = img.[ItemId])
					WHERE [ItemGroupTypeId] = @ItemGroupTypeId
						AND (FREETEXT(KeyWords, @SearchTerm))
				)
				SELECT COUNT(*) AS [FilteredResultsCount] FROM FilteredResults";

		/// <summary>
		/// Initializes an instance of the class
		/// </summary>
		/// <param name="dbConnection"></param>
		public GetItemGroupFilteredResultsCountQuery(IDbConnection dbConnection)
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
		public int Search(GetItemGroupFilteredResultsCountQueryParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			// dbConnection is created as scoped, we need to clone it to allow run in parallel
			using (var connection = (IDbConnection)((ICloneable)this.dbConnection).Clone())
			{
				var result = connection.QueryFirst<int>(FilteredResultsCountQuery, parameters, commandTimeout: 120);
				return result;
			}
		}
	}
}
