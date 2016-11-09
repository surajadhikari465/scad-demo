using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class DeletePricesCommandHandler : ICommandHandler<DeletePricesCommand>
    {
        private IDbProvider db;

        public DeletePricesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(DeletePricesCommand data)
        {
            string sql = @" DELETE p
                            FROM
	                            dbo.Price_{0}			p
	                            JOIN Items				i	on	p.ItemID = i.ItemID
	                            JOIN stage.Price	    sp	on	p.BusinessUnitID = sp.BusinessUnitId
									                            AND i.ScanCode = sp.ScanCode
									                            AND p.StartDate = sp.StartDate
                                                                AND (p.EndDate = sp.EndDate
                                                                    OR sp.EndDate IS NULL)
									                            AND p.Price = sp.Price
                                                                AND p.PriceType = sp.PriceType
                            WHERE
	                            sp.Region = @Region
	                            AND sp.TransactionId = @TransactionId;";

            sql = String.Format(sql, data.Region);
            int affectedRows = this.db.Connection
                .Execute(sql, new { TransactionId = data.TransactionId, Region = data.Region }, this.db.Transaction);
        }
    }
}
