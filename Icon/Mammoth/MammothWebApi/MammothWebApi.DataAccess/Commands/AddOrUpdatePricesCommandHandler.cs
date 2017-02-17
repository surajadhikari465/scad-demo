using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdatePricesCommandHandler : ICommandHandler<AddOrUpdatePricesCommand>
    {
        private IDbProvider db;

        public AddOrUpdatePricesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdatePricesCommand data)
        {
            string mergeSql = @"MERGE
	                                dbo.Price_{0} WITH (updlock, rowlock) p
                                USING
	                                (
		                                SELECT
	                                        i.ItemID,
	                                        sp.BusinessUnitId,
                                            sp.Multiple,
	                                        sp.Price,
	                                        sp.PriceType,
	                                        sp.StartDate,
	                                        sp.EndDate,
                                            sp.PriceUom,
                                            c.CurrencyID
                                        FROM
	                                        stage.Price	        sp
	                                        JOIN Items			i	on sp.ScanCode = i.ScanCode
                                            JOIN Currency       c   on sp.CurrencyCode = c.CurrencyCode
                                        WHERE
	                                        sp.TransactionId = @TransactionId
                                            AND sp.Region = @Region
	                                ) staged
                                ON
	                                p.ItemID = staged.ItemID
	                                AND p.BusinessUnitID = staged.BusinessUnitID
                                    AND p.StartDate = staged.StartDate
                                    AND p.PriceType = staged.PriceType
                                WHEN MATCHED THEN
	                                UPDATE 
                                    SET 
                                        p.Multiple = staged.Multiple,
                                        p.Price = staged.Price,
                                        p.EndDate = staged.EndDate,
                                        p.PriceUom = staged.PriceUom,
                                        p.CurrencyID = staged.CurrencyID,
                                        p.ModifiedDate = @Timestamp
                                WHEN NOT MATCHED THEN
	                                INSERT
	                                (
		                                ItemID,
		                                BusinessUnitID,
		                                StartDate,
                                        EndDate,
		                                Price,
                                        PriceType,
                                        PriceUom,
                                        Multiple,
                                        CurrencyID,
                                        AddedDate
	                                )
	                                VALUES
	                                (
		                                staged.ItemID,
		                                staged.BusinessUnitId,
		                                staged.StartDate,
                                        staged.EndDate,
		                                staged.Price,
                                        staged.PriceType,
                                        staged.PriceUom,
                                        staged.Multiple,
                                        staged.CurrencyID,
                                        @Timestamp
	                                );";

            // format sql specific to regional Price_XX table
            string sql = String.Format(mergeSql, data.Region);
            int affectedRows = this.db.Connection
                .Execute(
                    sql, 
                    new
                    {
                        Timestamp = data.Timestamp,
                        Region = new DbString { Value = data.Region, Length = 2 },
                        TransactionId = data.TransactionId
                    },
                    transaction: this.db.Transaction);
        }
    }
}
