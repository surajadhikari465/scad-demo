DECLARE @SourceRegion VARCHAR(2) = 'RM'
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
WHERE StoreName in (''Regency'',''Lincoln''))

	DELETE gpm.Price_' + @SourceRegion + 
	'
	WHERE BusinessUnitID in (SELECT BusinessUnitId
FROM locale
WHERE StoreName in (''Regency'',''Lincoln''))

	ALTER TABLE  gpm.Price_' + @SourceRegion + ' CHECK  CONSTRAINT ALL
	ALTER TABLE gpm.Price_' + @MoveToRegion + ' CHECK  CONSTRAINT ALL

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
		ROLLBACK TRANSACTION

	    SELECT   
        ERROR_NUMBER() AS ErrorNumber  
       ,ERROR_MESSAGE() AS ErrorMessage;  
END CATCH'

EXEC sp_executesql @query;