--Move Prices from MA -> MW
DECLARE @SourceRegion VARCHAR(2) = 'MA'
DECLARE @MoveToRegion VARCHAR(2) = 'MW'
DECLARE @query NVARCHAR(max);

SET @query = 'BEGIN TRY
BEGIN TRANSACTION

ALTER TABLE  gpm.Price_' + @SourceRegion + ' NOCHECK CONSTRAINT ALL
ALTER TABLE gpm.Price_' + @MoveToRegion + ' NOCHECK CONSTRAINT ALL

	INSERT INTO gpm.Price_' + @MoveToRegion + ' (
		[Region]
		,[GpmID]
		,[ItemID]
		,[BusinessUnitID]
		,[StartDate]
		,[EndDate]
		,[Price]
		,[PercentOff]
		,[PriceType]
		,[PriceTypeAttribute]
		,[SellableUOM]
		,[CurrencyCode]
		,[Multiple]
		,[TagExpirationDate]
		,[InsertDateUtc]
		,[ModifiedDateUtc]
		)
	SELECT' + ' ''' + @MoveToRegion + '''' + '
		,[GpmID]
		,[ItemID]
		,[BusinessUnitID]
		,[StartDate]
		,[EndDate]
		,[Price]
		,[PercentOff]
		,[PriceType]
		,[PriceTypeAttribute]
		,[SellableUOM]
		,[CurrencyCode]
		,[Multiple]
		,[TagExpirationDate]
		,[InsertDateUtc]
		,[ModifiedDateUtc]
	FROM gpm.Price_' + '' + @SourceRegion + '' + '
	WHERE BusinessUnitID in (SELECT BusinessUnitId
FROM locale
WHERE StoreName in (''Cedar Center'',''Rocky River'', ''Orange Village'', ''Toledo'', ''Akron''))

	DELETE gpm.Price_' + @SourceRegion + 
	'
	WHERE BusinessUnitID in (SELECT BusinessUnitId
FROM locale
WHERE StoreName in (''Cedar Center'',''Rocky River'', ''Orange Village'', ''Toledo'', ''Akron''))

	ALTER TABLE  gpm.Price_' + @SourceRegion + ' CHECK  CONSTRAINT ALL
	ALTER TABLE gpm.Price_' + @MoveToRegion + ' CHECK  CONSTRAINT ALL

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
		ROLLBACK TRANSACTION

	    DECLARE @ErrorMessage NVARCHAR(4000);
DECLARE @ErrorSeverity INT;
DECLARE @ErrorState INT;
	  
	  SELECT 
    @ErrorMessage = ERROR_MESSAGE(),
    @ErrorSeverity = ERROR_SEVERITY(),
    @ErrorState = ERROR_STATE();

-- Use RAISERROR inside the CATCH block to return error
-- information about the original error that caused
-- execution to jump to the CATCH block.

RAISERROR (@ErrorMessage, -- Message text.
           @ErrorSeverity, -- Severity.
           @ErrorState -- State.
           );  
END CATCH'

EXEC sp_executesql @query;