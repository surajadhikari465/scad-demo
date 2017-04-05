using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammoth.Common.DataAccess.DbProviders;
using Dapper;
using Icon.Infor.Listeners.Price.DataAccess.Models;

namespace Icon.Infor.Listeners.Price.DataAccess.Commands
{
    public class DeletePricesCommandHandler : ICommandHandler<DeletePricesCommand>
    {
        private IDbProvider dbProvider;

        public DeletePricesCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public void Execute(DeletePricesCommand data)
        {
            if(!data.Prices.Any())
            {
                return;
            }

            var gpmIds = data.Prices.Select(p => p.GpmID);

            foreach (string region in data.Prices.Select(p => p.Region))
            {
                string sql = $@"DELETE gpm.Price_{region}
                                OUTPUT
                                    deleted.Region              as Region,
	                                deleted.PriceID             as PriceID,
	                                deleted.GpmID               as GpmID,
	                                deleted.ItemID              as ItemID,
	                                deleted.BusinessUnitID      as BusinessUnitID,
	                                deleted.StartDate           as StartDate,
	                                deleted.EndDate             as EndDate,
	                                deleted.Price               as Price,
	                                deleted.PriceType           as PriceType,
	                                deleted.PriceTypeAttribute  as PriceTypeAttribute,
	                                deleted.PriceUOM            as PriceUOM,
	                                deleted.CurrencyID          as CurrencyID,
	                                deleted.Multiple            as Multiple,
	                                deleted.NewTagExpiration    as NewTagExpiration,
	                                deleted.AddedDate           as AddedDate,
                                    @Now                        as DeleteDate
                                INTO gpm.DeletedPrices
                                WHERE GpmID in @GpmIDs";

                int affectedRows = dbProvider.Connection.Execute(sql,
                    new { GpmIDs = gpmIds, Now = DateTime.Now },
                    this.dbProvider.Transaction);
            }
        }
    }
}
