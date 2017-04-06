using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.DataAccess.Queries
{
    public class GetDeletedPricesByGpmIdsQuery : IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>>
    {
        private IDbProvider db;

        public GetDeletedPricesByGpmIdsQuery(IDbProvider db)
        {
            this.db = db;
        }

        public IEnumerable<DeletedPriceModel> Search(GetDeletedPricesByGpmIdsParameters parameters)
        {
            List<DeletedPriceModel> deletedPrices = new List<DeletedPriceModel>();

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
                                AddedDate,
                                DeleteDate
                            FROM gpm.DeletedPrices
                            WHERE GpmID IN @GpmIDs";

            IEnumerable<DeletedPriceModel> priceSet = this.db.Connection
                .Query<DeletedPriceModel>(
                    sql, 
                    new { GpmIDs = parameters.PriceIds },
                    this.db.Transaction);
            deletedPrices.AddRange(priceSet);

            return deletedPrices;
        }
    }
}
