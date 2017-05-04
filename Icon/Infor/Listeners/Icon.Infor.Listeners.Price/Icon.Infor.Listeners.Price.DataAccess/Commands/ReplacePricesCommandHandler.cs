using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;

namespace Icon.Infor.Listeners.Price.DataAccess.Commands
{
    public class ReplacePricesCommandHandler : ICommandHandler<ReplacePricesCommand>
    {
        private IDbProvider db;

        public ReplacePricesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(ReplacePricesCommand data)
        {
            foreach (var price in data.Prices)
            {
                string region = price.Region;
                string sql = $@"DELETE FROM gpm.Price_{region}
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
                                        SYSDATETIME()               as DeleteDate
	                                INTO gpm.DeletedPrices
	                            WHERE GpmID = @ReplaceGpmID

	                            INSERT INTO gpm.Price_{region}
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
		                            SYSDATETIME()
	                            )";

                int affectedRows = this.db.Connection
                    .Execute(
                        sql,
                        new
                        {
                            ReplaceGpmID = price.ReplaceGpmId,
                            Region = price.Region,
                            GpmID = price.GpmID,
                            ItemID = price.ItemID,
                            BusinessUnitID = price.BusinessUnitID,
                            StartDate = price.StartDate,
                            EndDate = price.EndDate,
                            Price = price.Price,
                            PriceType = price.PriceType,
                            PriceTypeAttribute = price.PriceTypeAttribute,
                            PriceUOM = price.PriceUOM,
                            CurrencyID = price.CurrencyID,
                            Multiple = price.Multiple,
                            NewTagExpiration = price.NewTagExpiration
                        },
                        this.db.Transaction);
            }
        }
    }
}
