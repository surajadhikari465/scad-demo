using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using MoreLinq;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetItemNutritionAttributesByItemIdQueryHandler : IQueryHandler<GetItemNutritionAttributesByItemIdQuery, IEnumerable<ItemNutritionAttributes>>
    {
        private IDbProvider db;

        public GetItemNutritionAttributesByItemIdQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<ItemNutritionAttributes> Search(GetItemNutritionAttributesByItemIdQuery parameters)
        {
            var Values = parameters.ItemIds.Select(v => new { Value = v }).ToDataTable(); 
            IEnumerable < ItemNutritionAttributes > itemNutritionAttributes = this.db.Connection.Query<ItemNutritionAttributes>(
                "[dbo].[GetItemNutritionAttributes]",
                new
                {
                    ItemIds = Values
                },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);

            return itemNutritionAttributes;
        }
    }
}