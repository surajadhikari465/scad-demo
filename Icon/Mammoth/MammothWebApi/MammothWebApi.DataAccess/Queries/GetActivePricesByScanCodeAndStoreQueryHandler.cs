using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace MammothWebApi.DataAccess.Queries
{
    public class GetActivePricesByScanCodeAndStoreQueryHandler : IQueryHandler<GetActivePricesByScanCodeAndStoreQuery, List<ItemPriceModel>>
    {
        private IDbProvider db;

        public GetActivePricesByScanCodeAndStoreQueryHandler(IDbProvider db)
        {
            this.db = db;
        }

        public List<ItemPriceModel> Search(GetActivePricesByScanCodeAndStoreQuery parameters)
        {
            string querySql = @"
            SELECT ScanCode, BusinessUnitID
            INTO #storeScanCodes
            FROM @StoreScanCodes
            
            DECLARE @Today DATETIME2(7) = CAST(GETDATE() AS DATE)

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
                #storeScanCodes ssc
                JOIN Items i on ssc.ScanCode = i.ScanCode
                JOIN dbo.Price_{0} p ON i.ItemID = p.ItemID
                    AND ssc.BusinessUnitId = p.BusinessUnitID
                JOIN Currency c on p.CurrencyID = c.CurrencyID
            WHERE p.StartDate <= @Today
                AND ISNULL(p.EndDate, @Today) >= @Today";

            var sql = string.Format(querySql, parameters.Region);
            var prices = this.db.Connection.Query<ItemPriceModel>(
                sql,
                new
                {
                    parameters.Region,
                    StoreScanCodes = parameters.StoreScanCodes.ToDataTable().AsTableValuedParameter("ScanCodeBusinessUnitIdType")
                },
                this.db.Transaction);

            return prices.ToList();
        }
    }
}
