using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdateItemLocaleExtendedCommandHandler : ICommandHandler<AddOrUpdateItemLocaleExtendedCommand>
    {
        private IDbProvider db;

        public AddOrUpdateItemLocaleExtendedCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateItemLocaleExtendedCommand data)
        {
            string sql = @" MERGE dbo.ItemAttributes_Locale_{0}_Ext WITH (updlock, rowlock) ile
                            USING
                            (
	                            SELECT
		                            s.Region		as Region,
		                            i.ItemID		as ItemID,
		                            l.LocaleID		as LocaleID,
		                            s.AttributeId	as AttributeID,
		                            s.AttributeValue as AttributeValue
	                            FROM
		                            stage.ItemLocaleExtended	s
		                            JOIN dbo.Items				i on s.ScanCode = i.ScanCode
		                            JOIN dbo.Locales_{0}		l on s.BusinessUnitId = l.BusinessUnitID
                                WHERE
                                    s.TransactionId = @TransactionId
                                    AND s.Region = @Region
                            )	stage
                            ON
	                            ile.ItemID = stage.ItemID
	                            AND ile.LocaleID = stage.LocaleID
	                            AND ile.AttributeID = stage.AttributeID
                            WHEN MATCHED AND stage.AttributeValue IS NULL THEN
                                DELETE
                            WHEN MATCHED AND stage.AttributeValue IS NOT NULL THEN
	                            UPDATE
	                            SET
		                            ile.AttributeValue = stage.AttributeValue,
		                            ile.ModifiedDate = @Timestamp
                            WHEN NOT MATCHED AND stage.AttributeValue IS NOT NULL THEN
	                            INSERT
	                            (
		                            ItemID,
		                            LocaleID,
		                            AttributeID,
		                            AttributeValue,
		                            AddedDate
	                            )
	                            VALUES
	                            (
		                            stage.ItemID,
		                            stage.LocaleID,
		                            stage.AttributeID,
		                            stage.AttributeValue,
		                            @Timestamp
	                            );";

            sql = String.Format(sql, data.Region);
            int affectedRows = this.db.Connection.Execute(sql,
                new
                {
                    Timestamp = data.Timestamp,
                    Region = new DbString { Value = data.Region, Length = 2 },
                    TransactionId = data.TransactionId
                },
                this.db.Transaction);
        }
    }
}
