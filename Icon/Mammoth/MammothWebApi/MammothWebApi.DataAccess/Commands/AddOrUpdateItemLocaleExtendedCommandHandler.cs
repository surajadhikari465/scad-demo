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
            // if there is an existing Extended Attribute record and the matching staged record has a Value then UPDATE [U]
            // if there is an existing Extended Attribute record and the matching staged record has no Value (null) then DELETE [D]
            // if there is no exising Extended Attribute record matching the staged record then INSERT [I]
            string sql = @" 
    
	    --declare temp table with index
	    CREATE TABLE #tmpStagedItemLocaleExt
	    (
            [Region]				NVARCHAR(2)		NOT NULL,
		    [ItemID]				INT				NOT NULL,
		    [BusinessUnitID]		INT				NOT NULL,
	        [LocaleID]				INT				NOT NULL,
	        [AttributeId]			INT				NOT NULL,
	        [AttributeValue]		NVARCHAR(MAX)	NULL,
	        [ExistingId]			INT				NULL,
	        [ExistingValue]			NVARCHAR(MAX)	NULL,
		    [Operation]				CHAR			NULL,
            PRIMARY KEY ([Region], [ItemID], [LocaleID], [AttributeId])
	    )

        --copy staging data into temp table
        INSERT #tmpStagedItemLocaleExt
        SELECT DISTINCT
		    stg.[Region],
		    item.[ItemID],
		    stg.[BusinessUnitID],
		    loc.[LocaleID],
		    stg.[AttributeId],
		    stg.[AttributeValue],
		    ext.[AttributeID] AS ExistingId,
		    ext.[AttributeValue] AS ExistingValue,
            CASE 
                WHEN stg.[AttributeValue] is not null AND ext.[AttributeValue] is null THEN 'I'
                WHEN stg.[AttributeValue] is null AND ext.[AttributeValue] is not null THEN 'D'
                WHEN stg.[AttributeValue] is not null AND ext.[AttributeValue] is not null THEN 'U'
            END AS [Operation]
      FROM 
            [stage].[ItemLocaleExtended] AS stg
		    INNER JOIN [dbo].[Locales_{0}] AS loc ON
                loc.[BusinessUnitID] = stg.[BusinessUnitID]
            INNER JOIN [dbo].[Items] AS item ON
                item.[ScanCode]   = stg.[ScanCode] 
		    LEFT OUTER JOIN [dbo].[ItemAttributes_Locale_{0}_Ext] AS ext ON
                ext.[Region]   = stg.[Region] AND
                ext.[ItemID]   = item.[ItemID] AND
                ext.[LocaleID]   = loc.[LocaleID] AND
                ext.[AttributeID]  = stg.[AttributeId]
        WHERE 
            stg.[TransactionId] = @TransactionId AND 
            stg.[Region] = @Region 

        BEGIN TRY
            BEGIN TRAN

            --1/3 update existing records where matching staged data has a new extended attribute value
            UPDATE ext
            SET AttributeValue	= tmp.AttributeValue, ModifiedDate	= @Timestamp			
            FROM [dbo].ItemAttributes_Locale_{0}_Ext ext
	            JOIN #tmpStagedItemLocaleExt AS tmp ON 
				    tmp.[Region] = ext.[Region] AND  
				    tmp.[ItemID] = ext.[ItemID] AND 
				    tmp.[LocaleID] = ext.[LocaleID] AND 
				    tmp.[AttributeID] = ext.[AttributeID] 
            WHERE tmp.[Operation] = 'U'

            --2/3 delete existing records where matching staged data has a NULL extended attribute value
            DELETE ext
            FROM [dbo].ItemAttributes_Locale_{0}_Ext ext
			    JOIN #tmpStagedItemLocaleExt tmp ON 
				    tmp.[Region] = ext.[Region] AND  
				    tmp.[ItemID] = ext.[ItemID] AND 
				    tmp.[LocaleID] = ext.[LocaleID] AND 
				    tmp.[AttributeID] = ext.[AttributeID]
            WHERE tmp.[Operation] = 'D'

            --3/3 insert new records when nothing matches the staged data
            INSERT INTO [dbo].ItemAttributes_Locale_{0}_Ext
            (
			    [ItemID],
			    [LocaleID],
			    [AttributeID],
			    [AttributeValue],
			    [AddedDate]
		    )
            SELECT 
			    tmp.[ItemID],
			    tmp.[LocaleID],
			    tmp.[AttributeID],
			    tmp.[AttributeValue],
			    @Timestamp
            FROM #tmpStagedItemLocaleExt tmp		
            WHERE tmp.[Operation]='I'
            COMMIT TRAN
        END TRY
        BEGIN CATCH
	        ROLLBACK TRAN;
            IF OBJECT_ID('tempdb..#tmpStagedItemLocale') IS NOT NULL DROP TABLE #tmpStagedItemLocale;
            THROW
        END CATCH
            ";

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
