DECLARE @BusinessUnitId INT
DECLARE @MoveToRegion VARCHAR(2) = 'NC'
DECLARE @SourceRegion VARCHAR(2) = 'RM'
DECLARE @StoreNameToMove VARCHAR(200) = 'Boise'

SELECT @BusinessUnitId = BusinessUnitId
FROM locale
WHERE StoreName = @StoreNameToMove

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
	SELECT' + ' '''+ @MoveToRegion +''''+  '
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
	FROM gpm.Price_' + ''+ @SourceRegion +''+ '
	WHERE BusinessUnitID = @BusinessUnitId

	DELETE gpm.Price_' + @SourceRegion + '
	WHERE BusinessUnitID = @BusinessUnitId

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

EXEC sp_executesql @query, N'@BusinessUnitId int', @BusinessUnitId = @BusinessUnitId;