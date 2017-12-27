using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammoth.Common.DataAccess.DbProviders;
using Dapper;

namespace MammothWebApi.DataAccess.Commands
{
    public class DeleteItemLocaleSupplierCommandHandler : ICommandHandler<DeleteItemLocaleSupplierCommand>
    {
        private IDbProvider db;

        public DeleteItemLocaleSupplierCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(DeleteItemLocaleSupplierCommand data)
        {

            string sql = @"
    --declare temp table with index
    CREATE TABLE #tmpStagedItemLocaleSupplierDelete
    (
		[Region]                 NVARCHAR (2)     NOT NULL,
        [ItemID]				 INT              NOT NULL,
        [BusinessUnitID]         INT              NOT NULL,
        PRIMARY KEY ([Region], [ItemID], [BusinessUnitID])
    )

    --copy staging data into temp table
    INSERT #tmpStagedItemLocaleSupplierDelete
    SELECT DISTINCT
	    stage.[Region],
	    item.[ItemID],
	    stage.[BusinessUnitID]
    FROM 
	    [stage].[ItemLocaleSupplierDelete]	AS stage
	    INNER JOIN [dbo].[Items]		    AS item ON item.[ScanCode] = [stage].[ScanCode]
	    INNER JOIN [dbo].[Locales_{0}]      AS loc ON loc.[BusinessUnitID] = [stage].[BusinessUnitID] -- (to ensure the Locale exists in Mammoth)
	    LEFT OUTER JOIN [dbo].[ItemLocale_Supplier_{0}] AS sup ON
            sup.[ItemID]        = item.[ItemID] AND
            sup.[Region]        = loc.[Region] AND
            sup.[BusinessUnitID] = stage.[BusinessUnitID]
    WHERE 
	    stage.[Region] =        @Region AND
        stage.[TransactionId] = @TransactionId
			
    DECLARE @staged_row_count int = @@ROWCOUNT;

    BEGIN TRY
        BEGIN TRAN

        --perform delete
        DELETE sup
        FROM [dbo].[ItemLocale_Supplier_{0}] sup
        JOIN #tmpStagedItemLocaleSupplierDelete tmp ON
            tmp.[Region] =          sup.[Region] AND
            tmp.[ItemID] =          sup.[ItemID] AND
            tmp.[BusinessUnitID] =  sup.[BusinessUnitID]

        COMMIT TRAN
    END TRY
    BEGIN CATCH
	    ROLLBACK TRAN;
        IF OBJECT_ID('tempdb..#tmpStagedItemLocaleSupplierDelete') IS NOT NULL DROP TABLE #tmpStagedItemLocaleSupplierDelete;
        THROW
    END CATCH";

            int affectedRows = this.db.Connection.Execute(string.Format(sql, data.Region),
                new { Region = new DbString { Value = data.Region, Length = 2 }, Timestamp = data.Timestamp, TransactionId = data.TransactionId },
                transaction: this.db.Transaction);
        }
    }
}
