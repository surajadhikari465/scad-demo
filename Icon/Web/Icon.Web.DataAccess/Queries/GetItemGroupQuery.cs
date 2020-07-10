using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    /// <summary>
    /// Get ItemGroup Query
    /// </summary>
    public class GetItemGroupQuery : IQueryHandler<GetItemGroupParameters, IEnumerable<ItemGroupModel>>
    {
        private IDbConnection connection;

        /// <summary>
        /// Initializes an instance of GetItemGroupQuery
        /// </summary>
        /// <param name="connection">IDbConnection</param>
        public GetItemGroupQuery(IDbConnection connection)
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
        /// <returns>IEnumerable of ItemGroupModel</returns>
        public IEnumerable<ItemGroupModel> Search(GetItemGroupParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return connection.Query<ItemGroupModel>($@"
                SELECT ig.[ItemGroupId]
		                ,ig.[ItemGroupTypeId]
		                ,ig.[ItemGroupAttributesJson]
		                ,ig.[LastModifiedBy]
		                ,ig.[SysStartTimeUtc]
		                ,ig.[SysEndTimeUtc]
		                ,sc.[scanCode]
                FROM [dbo].[ItemGroup] ig
	                INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
	                INNER JOIN [dbo].[ScanCode] sc ON (sc.[ItemId] = img.[ItemId])
                WHERE [ItemGroupTypeId] = @ItemGroupTypeId",
                parameters);
        }
    }
}
