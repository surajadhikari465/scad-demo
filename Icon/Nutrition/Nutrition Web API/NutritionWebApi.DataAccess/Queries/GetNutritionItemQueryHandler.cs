using Dapper;
using NutritionWebApi.Common;
using NutritionWebApi.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace NutritionWebApi.DataAccess.Queries
{
    public class GetNutritionItemQueryHandler : IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>>
    {
        public IDbConnectionProvider DbConnectionProvider { get; set; }

        public GetNutritionItemQueryHandler(IDbConnectionProvider DbProvider)
        {
            this.DbConnectionProvider = DbProvider;
        }

        public List<NutritionItemModel> Search(GetNutritionItemQuery parameters)
        {
            List<NutritionItemModel> nutritionItems = new List<NutritionItemModel>();

            string sql = "select * from nutrition.ItemNutrition";

            if (!String.IsNullOrEmpty(parameters.Plu))
            {
                sql = sql + " where Plu = '" + parameters.Plu + "'";
            }

            nutritionItems = this.DbConnectionProvider.Connection.Query<NutritionItemModel>(sql, commandType: CommandType.Text).AsList();

            return nutritionItems;
        }
    }
}
