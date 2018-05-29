using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class DeleteItemLocalePriceCommandHandler : ICommandHandler<DeleteItemLocalePriceCommand>
    {
        private IDbProvider db;

        public DeleteItemLocalePriceCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(DeleteItemLocalePriceCommand data)
        {
            string sql = @" DELETE p
                            FROM
	                            dbo.Price_{0} p
                                JOIN Items i on	p.ItemID = i.ItemID
                            WHERE
	                            p.Region = @Region
                                AND p.BusinessUnitID = @BusinessUnitId
	                            AND i.ScanCode = @ScanCode;";

            sql = String.Format(sql, data.Region);
            int affectedRows = this.db.Connection
                .Execute(sql, new { Region = data.Region, BusinessUnitID = data.BusinessUnitId, ScanCode = data.ScanCode }, this.db.Transaction);
        }
    }
}