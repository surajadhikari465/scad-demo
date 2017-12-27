using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdateItemLocaleSupplierCommandHandler : ICommandHandler<AddOrUpdateItemLocaleSupplierCommand>
    {
        private IDbProvider db;

        public AddOrUpdateItemLocaleSupplierCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateItemLocaleSupplierCommand data)
        {
            string sql = @"
    --declare temp table with index
    CREATE TABLE #tmpStagedItemLocaleSupplier
    (
		[Region]                 NVARCHAR (2)     NOT NULL,
        [ItemID]				 INT              NOT NULL,
        [BusinessUnitID]         INT              NOT NULL,
	    [SupplierName]		     NVARCHAR(255)	  NULL,
	    [SupplierItemId]	     NVARCHAR(20)	  NULL,
	    [SupplierCaseSize]	     INT		      NULL,
	    [IrmaVendorKey]		     NVARCHAR(10)	  NULL,
        [InsertOrUpdate]		 CHAR			  NOT NULL,
        PRIMARY KEY ([Region], [ItemID], [BusinessUnitID])
    )

    --copy staging data into temp table
    INSERT #tmpStagedItemLocaleSupplier
    SELECT DISTINCT
	    stage.[Region],
	    item.[ItemID],
	    stage.[BusinessUnitID],
        stage.[SupplierName],		
        stage.[SupplierItemId],	
        stage.[SupplierCaseSize],	
        stage.[IrmaVendorKey],		
	    CASE WHEN sup.ItemLocaleSupplierID is null THEN 'I' ELSE 'U' END As [InsertOrUpdate]
    FROM 
	    [stage].[ItemLocaleSupplier]	AS stage
	    INNER JOIN [dbo].[Items]		AS item ON item.[ScanCode] = [stage].[ScanCode]
	    INNER JOIN [dbo].[Locales_{0}]  AS loc ON loc.[BusinessUnitID] = [stage].[BusinessUnitID] -- (to ensure the Locale exists in Mammoth)
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

        --perform updates
        UPDATE [dbo].[ItemLocale_Supplier_{0}]
	        SET
			    [SupplierName]			    = tmp.[SupplierName],
			    [SupplierItemId]			= tmp.[SupplierItemId],
			    [SupplierCaseSize]			= tmp.[SupplierCaseSize],
			    [IrmaVendorKey]		        = tmp.[IrmaVendorKey],
			    [ModifiedDateUtc]           = @Timestamp
	        FROM
		       [dbo].[ItemLocale_Supplier_{0}] sup
			        JOIN #tmpStagedItemLocaleSupplier tmp ON
                        tmp.[Region] = sup.[Region] AND
                        tmp.[ItemID] = sup.[ItemID] AND
                        tmp.[BusinessUnitID] = sup.[BusinessUnitID]
	        WHERE tmp.[InsertOrUpdate]= 'U'
				
	    IF @@ROWCOUNT < @staged_row_count
	    BEGIN
		    --perform inserts
            INSERT INTO [dbo].[ItemLocale_Supplier_{0}]
            (
				[Region],
				[ItemID],
				[BusinessUnitID],
				[SupplierName],
                [SupplierItemId],	
                [SupplierCaseSize],
                [IrmaVendorKey],
				[AddedDateUtc]
            )
			SELECT 
				tmp.[Region],
				tmp.[ItemID],
				tmp.[BusinessUnitID],
				tmp.[SupplierName],	
				tmp.[SupplierItemId],	
				tmp.[SupplierCaseSize],
				tmp.[IrmaVendorKey],
				@Timestamp
			FROM #tmpStagedItemLocaleSupplier tmp
			WHERE tmp.[InsertOrUpdate] = 'I'
	    END

        COMMIT TRAN
    END TRY
    BEGIN CATCH
	    ROLLBACK TRAN;
        IF OBJECT_ID('tempdb..#tmpStagedItemLocaleSupplier') IS NOT NULL DROP TABLE #tmpStagedItemLocaleSupplier;
        THROW
    END CATCH";

            int affectedRows = this.db.Connection.Execute(String.Format(sql, data.Region),
                new { Region = new DbString { Value = data.Region, Length = 2 }, Timestamp = data.Timestamp, TransactionId = data.TransactionId },
                transaction: this.db.Transaction);
        }
    }
}
