CREATE PROCEDURE [stage].[GpmInsertPriceDataConversion]
	@BatchSize int = 0
AS

IF @BatchSize = 0
	SET @BatchSize = 50000;

-- Iterate through each region returned in stage.GpmDataConversion table
DECLARE @Region nvarchar(2);
DECLARE db_cursor CURSOR FOR  
	SELECT DISTINCT Region 
	FROM [stage].[GpmDataConversion]

OPEN db_cursor   
FETCH NEXT FROM db_cursor INTO @Region   

WHILE @@FETCH_STATUS = 0   
BEGIN   
    
	DECLARE @sql nvarchar(max);

	-- Truncate gpm price table for that region
	SET @sql = N'TRUNCATE TABLE [gpm].[Price_' + @Region + '];';
	EXEC sp_executesql @sql;

	-- Insert into gpm price table from staging table for that region
	SET @sql = N'
	SET IDENTITY_INSERT [gpm].[Price_'+ @Region + '] ON
	WHILE 1 = 1
	BEGIN
		INSERT INTO [gpm].[Price_' + @Region + ']
		(
			[Region],
			[PriceID],
			[GpmID],
			[ItemID],
			[BusinessUnitID],
			[StartDate],
			[EndDate],
			[Price],
			[PercentOff],
			[PriceType],
			[PriceTypeAttribute],
			[SellableUOM],
			[CurrencyCode],
			[Multiple],
			[TagExpirationDate],
			[InsertDateUtc]
		)
		SELECT TOP(@BatchSize)
			@Region as Region,
			s.PriceID,
			s.GpmID,
			s.ItemID,
			s.BusinessUnitID,
			s.StartDate,
			s.EndDate,
			s.Price,
			s.PercentOff,
			s.PriceType,
			s.PriceTypeAttribute,
			s.SellableUOM,
			s.CurrencyCode,
			s.Multiple,
			s.NewTagExpiration,
			s.InsertDateUtc
		FROM [stage].[GpmDataConversion] s
		WHERE s.Region = @Region
			AND NOT EXISTS (
			SELECT 1
			FROM [gpm].[Price_' + @Region + ']
			WHERE PriceID = s.PriceID
				AND Region = s.Region
		)

		IF @@ROWCOUNT < @BatchSize
			BREAK
	END
	SET IDENTITY_INSERT [gpm].[Price_'+ @Region + '] OFF';

	PRINT @sql;
	EXEC sp_executesql @sql, N'@BatchSize int, @Region nvarchar(2)', @BatchSize = @BatchSize, @Region = @Region;

	FETCH NEXT FROM db_cursor INTO @Region
END  

CLOSE db_cursor   
DEALLOCATE db_cursor
GO

GRANT EXECUTE on [stage].[GpmInsertPriceDataConversion] to [MammothRole]
GO