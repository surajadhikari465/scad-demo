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

            //using (var reader = ObjectReader.Create(
            //    data.Prices,
            //    nameof(DbPriceModel.Region),
            //    nameof(DbPriceModel.PriceID),
            //    nameof(DbPriceModel.GpmID),
            //    nameof(DbPriceModel.ItemID),
            //    nameof(DbPriceModel.BusinessUnitID),
            //    nameof(DbPriceModel.StartDate),
            //    nameof(DbPriceModel.EndDate),
            //    nameof(DbPriceModel.Price),
            //    nameof(DbPriceModel.PriceType),
            //    nameof(DbPriceModel.PriceTypeAttribute),
            //    nameof(DbPriceModel.PriceUOM),
            //    nameof(DbPriceModel.CurrencyID),
            //    nameof(DbPriceModel.Multiple),
            //    nameof(DbPriceModel.NewTagExpiration)))
            //{
            //    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(db.Connection as SqlConnection))
            //    {
            //        sqlBulkCopy.DestinationTableName = "gpm.Price_" + region;
            //        sqlBulkCopy.WriteToServer(reader);
            //    }
            //}
        }
    }
}
