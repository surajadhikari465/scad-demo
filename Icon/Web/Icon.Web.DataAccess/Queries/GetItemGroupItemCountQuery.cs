using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// Get ItemGroup Item Count Query
    /// </summary>
    public class GetItemGroupItemCountQuery : IQueryHandler<GetItemGroupItemCountParameters, IEnumerable<SkuItemCountModel>>
    {
        private IDbConnection connection;

        /// <summary>
        /// Initializes an instance of GetItemGroupQuery
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        public GetItemGroupItemCountQuery(IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            this.connection = connection;
        }

        /// <summary>
        /// Search/ Execute the query
        /// </summary>
        /// <param name="parameters">GetItemGroupParameters</param>
        /// <returns>IEnumerable of SkuItemCountModel</returns>
        public IEnumerable<SkuItemCountModel> Search(GetItemGroupItemCountParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return connection.Query<SkuItemCountModel>($@"
                    SELECT ig.[ItemGroupId]
		                    ,COUNT(img.[ItemId]) AS CountOfItems
                    FROM [dbo].[ItemGroup] ig
	                    INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId)
                    WHERE [ItemGroupTypeId] = @ItemGroupTypeId
                    GROUP by ig.[ItemGroupId];",
                parameters);
        }
    }
}
