using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemColumnOrderQuery : IQueryHandler<EmptyQueryParameters<List<ItemColumnOrderModel>>, List<ItemColumnOrderModel>>
    {
        private readonly IDbConnection dbConnection;

        public GetItemColumnOrderQuery(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public List<ItemColumnOrderModel> Search(EmptyQueryParameters<List<ItemColumnOrderModel>> parameters)
        {
            var sql = @"
                SELECT ColumnType
	                ,ReferenceId
	                ,h.HierarchyName ReferenceName
	                ,d.DisplayOrder
                FROM dbo.ItemColumnDisplayOrder d
                INNER JOIN dbo.Hierarchy h ON d.ReferenceId = h.HIERARCHYID
                WHERE d.ColumnType = 'Hierarchy'

                UNION ALL

                SELECT ColumnType
	                ,ReferenceId
	                ,a.AttributeName ReferenceName
	                ,d.DisplayOrder
                FROM dbo.ItemColumnDisplayOrder d
                INNER JOIN dbo.Attributes a ON d.ReferenceId = a.AttributeId
                WHERE d.ColumnType = 'Attribute'
                ORDER BY DisplayOrder
            ";

            var data = dbConnection.Query<ItemColumnOrderModel>(sql);
            return data.ToList();
        }
    }
}