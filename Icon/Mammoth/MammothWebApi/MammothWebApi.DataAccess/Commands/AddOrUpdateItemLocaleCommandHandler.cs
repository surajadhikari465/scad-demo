using Dapper;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdateItemLocaleCommandHandler : ICommandHandler<AddOrUpdateItemLocaleCommand>
    {
        private IDbProvider db;

        public AddOrUpdateItemLocaleCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateItemLocaleCommand data)
        {
            string sql = @"
    --declare temp table with index
    CREATE TABLE #tmpStagedItemLocale
    (
		[Region]                 NVARCHAR (2)     NOT NULL,
        [ItemID]				 INT              NOT NULL,
        [BusinessUnitID]         INT              NOT NULL,
        [Discount_Case]          BIT              NOT NULL,
        [Discount_TM]            BIT              NOT NULL,
        [Restriction_Age]        INT              NULL,
        [Restriction_Hours]      BIT              NOT NULL,
        [Authorized]             BIT              NOT NULL,
        [Discontinued]           BIT              NULL,
        [LabelTypeDesc]          NVARCHAR (4)     NULL,
        [LocalItem]              BIT              NULL,
        [OrderedByInfor]     BIT              NULL,
        [Product_Code]           NVARCHAR (15)    NULL,
        [RetailUnit]             NVARCHAR (25)    NULL,
        [Sign_Desc]              NVARCHAR (60)    NULL,
        [Locality]               NVARCHAR (50)    NULL,
        [Sign_RomanceText_Long]  NVARCHAR (300)   NULL,
        [Sign_RomanceText_Short] NVARCHAR (255)   NULL,
        [Msrp]                   SMALLMONEY       NOT NULL,
        [AltRetailUOM]			 NVARCHAR(25)	  NULL,
        [AltRetailSize]			 NUMERIC(9,4)	  NULL,
        [DefaultScanCode]		 BIT        	  NULL,
        [InsertOrUpdate]		 CHAR			  NOT NULL,
        PRIMARY KEY ([Region], [ItemID], [BusinessUnitID])
    )

    --copy staging data into temp table
    INSERT #tmpStagedItemLocale
    SELECT DISTINCT
	    stage.[Region],
	    item.[ItemID],
	    stage.[BusinessUnitID],
	    stage.[Discount_Case],
	    stage.[Discount_TM],
	    stage.[Restriction_Age],
	    stage.[Restriction_Hours],
	    stage.[Authorized],
	    stage.[Discontinued],
	    stage.[LabelTypeDesc],
	    stage.[LocalItem],  
        stage.[OrderedByInfor],
	    stage.[Product_Code],
	    stage.[RetailUnit],
	    stage.[Sign_Desc],
	    stage.[Locality],
	    stage.[Sign_RomanceText_Long],
	    stage.[Sign_RomanceText_Short],
        stage.[Msrp],
        stage.[AltRetailUOM],
        stage.[AltRetailSize],	
        stage.[DefaultScanCode],
	    CASE WHEN att.[ItemAttributeLocaleID] is null THEN 'I' ELSE 'U' END As [InsertOrUpdate]
    FROM 
	    [stage].[ItemLocale]	        AS stage
	    INNER JOIN [dbo].[Items]		AS item ON
            item.[ScanCode] = [stage].[ScanCode]
	    INNER JOIN [dbo].[Locales_{0}]   AS loc ON
            loc.[BusinessUnitID] = [stage].[BusinessUnitID]   -- (to ensure the Locale exists in Mammoth)
	    LEFT OUTER JOIN [dbo].[ItemAttributes_Locale_{0}] AS att ON
            att.[ItemID]        = item.[ItemID] AND
            att.[Region]        = loc.[Region] AND
            att.[BusinessUnitID] = stage.[BusinessUnitID]
    WHERE 
	    stage.[Region] =        @Region AND
        stage.[TransactionId] = @TransactionId
			
    DECLARE @staged_row_count int = @@ROWCOUNT;

    BEGIN TRY
        BEGIN TRAN

        --perform updates
        UPDATE [dbo].[ItemAttributes_Locale_{0}]
	        SET
			    [Discount_Case]			    = tmp.[Discount_Case],
			    [Discount_TM]				= tmp.[Discount_TM],
			    [Restriction_Age]			= tmp.[Restriction_Age],
			    [Restriction_Hours]		    = tmp.[Restriction_Hours],
			    [Authorized]				= tmp.[Authorized],
			    [Discontinued]			    = tmp.[Discontinued],
			    [LocalItem]				    = tmp.[LocalItem],
                [OrderedByInfor]        = tmp.[OrderedByInfor],
			    [LabelTypeDesc]			    = tmp.[LabelTypeDesc],
			    [Product_Code]			    = tmp.[Product_Code],
			    [RetailUnit]				= tmp.[RetailUnit],
			    [Sign_Desc]				    = tmp.[Sign_Desc],
			    [Locality]				    = tmp.[Locality],
			    [Sign_RomanceText_Long]	    = tmp.[Sign_RomanceText_Long],
			    [Sign_RomanceText_Short]    = tmp.[Sign_RomanceText_Short],
                [MSRP]                      = tmp.[Msrp],
                [AltRetailUOM]	            = tmp.[AltRetailUOM],
                [AltRetailSize]	            = tmp.[AltRetailSize],
                [DefaultScanCode]           = tmp.[DefaultScanCode],
			    [ModifiedDate]              = @Timestamp
	        FROM
		       [dbo].[ItemAttributes_Locale_{0}] att
			        JOIN #tmpStagedItemLocale tmp ON
                        tmp.[Region] = att.[Region] AND
                        tmp.[ItemID] = att.[ItemID] AND
                        tmp.[BusinessUnitID] = att.[BusinessUnitID]
	        WHERE tmp.[InsertOrUpdate]= 'U'
				
	    IF @@ROWCOUNT < @staged_row_count
	    BEGIN
		    --perform inserts
            INSERT INTO [dbo].[ItemAttributes_Locale_{0}]
            (
				[Region],
				[ItemID],
				[BusinessUnitID],
				[Discount_Case],
				[Discount_TM],
				[Restriction_Age],
				[Restriction_Hours],
				[Authorized],
				[Discontinued],
				[LocalItem],
                [OrderedByInfor],
				[LabelTypeDesc],
				[Product_Code],
				[RetailUnit],
				[Sign_Desc],
				[Locality],
				[Sign_RomanceText_Long],
				[Sign_RomanceText_Short],
				[MSRP],
                [AltRetailUOM],
                [AltRetailSize],
                [DefaultScanCode],
				[AddedDate]
            )
			SELECT 
				tmp.[Region],
				tmp.[ItemID],
				tmp.[BusinessUnitID],
				tmp.[Discount_Case],
				tmp.[Discount_TM],
				tmp.[Restriction_Age],
				tmp.[Restriction_Hours],
				tmp.[Authorized],
				tmp.[Discontinued],
				tmp.[LocalItem],
                tmp.[OrderedByInfor],
				tmp.[LabelTypeDesc],
				tmp.[Product_Code],
				tmp.[RetailUnit],
				tmp.[Sign_Desc],
				tmp.[Locality],
				tmp.[Sign_RomanceText_Long],
				tmp.[Sign_RomanceText_Short],
				tmp.[MSRP],
                tmp.[AltRetailUOM],
                tmp.[AltRetailSize],
                tmp.[DefaultScanCode],
				@Timestamp
			FROM #tmpStagedItemLocale tmp
			WHERE tmp.[InsertOrUpdate] = 'I'
	    END

        COMMIT TRAN
    END TRY
    BEGIN CATCH
	    ROLLBACK TRAN;
        IF OBJECT_ID('tempdb..#tmpStagedItemLocale') IS NOT NULL DROP TABLE #tmpStagedItemLocale;
        THROW
    END CATCH";

            int affectedRows = this.db.Connection.Execute(String.Format(sql, data.Region),
                new { Region = new DbString { Value = data.Region, Length = 2 }, Timestamp = data.Timestamp, TransactionId = data.TransactionId },
                transaction: this.db.Transaction);
            return;            
        }
    }
}
