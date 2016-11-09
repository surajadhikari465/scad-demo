using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.DataAccess.Queries
{
    /// <summary>
    /// This query will retrieve every price 
    /// </summary>
    public class GetPricesByScanCodeAndStoreQueryHandler : IQueryHandler<GetPricesByScanCodeAndStoreQuery, List<ItemPriceModel>>
    {
        private IDbProvider db;

        public GetPricesByScanCodeAndStoreQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public List<ItemPriceModel> Search(GetPricesByScanCodeAndStoreQuery parameters)
        {
            string querySql = @"
            SELECT
                @Region as Region,
                i.ScanCode,
                p.BusinessUnitID,
                p.StartDate,
                p.EndDate,
                p.Price,
                p.PriceType,
                p.PriceUOM,
                c.CurrencyCode,
                p.Multiple
            FROM
                dbo.Price_{0} p
                JOIN Items i on p.ItemID = i.ItemID
                JOIN Currency c on p.CurrencyID = c.CurrencyID
            WHERE
                p.BusinessUnitId IN @BusinessUnitIds
                AND i.ScanCode IN @ScanCodes";

            var sql = string.Format(querySql, parameters.Region);
            var prices = this.db.Connection.Query<ItemPriceModel>(
                sql,
                new
                {
                    parameters.Region,
                    parameters.BusinessUnitIds,
                    parameters.ScanCodes
                },
                this.db.Transaction);

            return prices.ToList();
        }
    }
}
