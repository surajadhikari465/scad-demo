using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.Price.DataAccess.Queries
{
    public class GetPricesByGpmIdsQuery : IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>>
    {
        private IDbProvider db;

        public GetPricesByGpmIdsQuery(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<DbPriceModel> Search(GetPricesByGpmIdsParameters parameters)
        {
            List<DbPriceModel> prices = new List<DbPriceModel>();
            foreach (var region in parameters.Prices.Select(p => p.Region))
            {
                string sql = $@"SELECT
                                    Region,
                                    PriceID,
                                    GpmID,
                                    ItemID,
                                    BusinessUnitID,
                                    StartDate,
                                    EndDate,
                                    Price,
                                    PriceType,
                                    PriceTypeAttribute,
                                    PriceUOM,
                                    CurrencyID,
                                    Multiple,
                                    NewTagExpiration,
                                    AddedDate
                                FROM gpm.Price_{region}
                                WHERE GpmID IN @GpmIDs";

                IEnumerable<DbPriceModel> priceSet = this.db.Connection
                    .Query<DbPriceModel>(sql, new { GpmIDs = parameters.Prices.Select(p => p.GpmID) }, this.db.Transaction);
                prices.AddRange(priceSet);
            }

            return prices;
        }
    }
}
