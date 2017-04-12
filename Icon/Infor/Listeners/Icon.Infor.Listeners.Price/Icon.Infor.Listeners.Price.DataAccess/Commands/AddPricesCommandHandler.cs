using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System.Linq;

namespace Icon.Infor.Listeners.Price.DataAccess.Commands
{
    public class AddPricesCommandHandler : ICommandHandler<AddPricesCommand>
    {
        private IDbProvider db;

        public AddPricesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddPricesCommand data)
        {
            if (!data.Prices.Any())
            {
                return;
            }

            foreach (var price in data.Prices)
            {
                string sql = $@"INSERT INTO gpm.Price_{price.Region}
                            (
	                            Region,
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
                            )
                            VALUES
                            (
	                            @Region,
	                            @GpmID,
	                            @ItemID,
	                            @BusinessUnitID,
	                            @StartDate,
	                            @EndDate,
	                            @Price,
	                            @PriceType,
	                            @PriceTypeAttribute,
	                            @PriceUOM,
	                            @CurrencyID,
	                            @Multiple,
	                            @NewTagExpiration,
	                            @AddedDate
                            )";
                this.db.Connection.Execute(sql, price, this.db.Transaction);
            }
        }
    }
}
