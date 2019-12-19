using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemsByIdSearchQuery : IQueryHandler<GetItemsByIdSearchParameters, GetItemsResult>
    {
        private readonly IDbProvider db;

        public GetItemsByIdSearchQuery(IDbProvider db)
        {
            this.db = db;
        }

        public GetItemsResult Search(GetItemsByIdSearchParameters parameters)
        {
            var dt = new DataTable("dbo.GetItemsById");
            dt.Columns.Add("ItemId", typeof(string));
            parameters.ItemIds.ForEach(id => dt.Rows.Add(id));

            var queryResultsSets = db.Connection.Query<ItemDbModel>(
                @"dbo.GetItemsByIdsSearch",
                param: new { ItemIds = dt },
                commandType: CommandType.StoredProcedure,
                transaction: db.Transaction);

            return new GetItemsResult
            {
                Items = queryResultsSets.ToList(),
                TotalRecordsCount = queryResultsSets.ToList().Count
            };
        }
    }
}
