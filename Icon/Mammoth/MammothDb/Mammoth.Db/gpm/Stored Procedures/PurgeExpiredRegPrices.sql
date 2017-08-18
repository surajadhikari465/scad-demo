CREATE PROCEDURE [gpm].[PurgeExpiredRegPrices]
	@RegionCode nvarchar(2),
	@MaxDeleteCount int,
	@BatchSize int
AS 
BEGIN

	/* =========================================================
	Description:	Deletes any REG prices with an 
					StartDates less than the current day that 
					are not the most recent REG price 
					in a specified number of batches.					
	============================================================*/

	DECLARE @sql nvarchar(max) = '
		DECLARE @DefaultDeleteCount int = 100000
		SET @MaxDeleteCount = COALESCE(@MaxDeleteCount, @DefaultDeleteCount)

		DECLARE @Today datetime2(7) = CAST(SYSDATETIME() AS DATE);
		
		PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + '', '' + '' Building temp table of most recent REG prices from gpm.Price_' + @RegionCode + '''

		--Build temp table of most recent REG prices. 
		--Any price with a date less that these prices should be deleted
		IF OBJECT_ID(''tempdb..#MostRecentRegPrices '', ''U'') IS NOT NULL
			DROP TABLE #MostRecentRegPrices
		CREATE TABLE #MostRecentRegPrices 
		(
			ItemId int,
			BusinessUnitId int,
			StartDate datetime2(7)
		)
		
		CREATE CLUSTERED INDEX CIX_MostRecentRegPrices ON #MostRecentRegPrices(ItemId, BusinessUnitId, StartDate)
		
		INSERT INTO #MostRecentRegPrices
		SELECT 
			ItemId, 
			BusinessUnitId, 
			MAX(StartDate)
		FROM gpm.Price_' + @RegionCode + '
		WHERE PriceType = ''REG''
			AND StartDate < @Today
		GROUP BY ItemId, BusinessUnitId HAVING COUNT(*) > 1
		
		--Get count of expired prices to delete
		DECLARE @DeleteCount int = (
			SELECT COUNT(*)
			FROM gpm.Price_' + @RegionCode + '
			WHERE PriceType = ''REG''
				and exists
				(
					select 1
					from #MostRecentRegPrices m
					where gpm.Price_' + @RegionCode + '.ItemID = m.ItemId
						and gpm.Price_' + @RegionCode + '.BusinessUnitID = m.BusinessUnitId
						and gpm.Price_' + @RegionCode + '.StartDate < m.StartDate
				))

		IF @DeleteCount > @MaxDeleteCount
			SET @DeleteCount = @MaxDeleteCount

		DECLARE @BatchId int = 0
		DECLARE @TotalDeletedCount int = 0
		DECLARE @BatchDeletedCount int = 0

		PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + '', '' + ''Deleting '' + 
			CAST(@DeleteCount AS NVARCHAR(10)) + '' REG prices from gpm.Price_' + @RegionCode + ' in batches of '' + 
			CAST(@BatchSize AS NVARCHAR(10))

		--Delete expired prices in batches
		WHILE @TotalDeletedCount < @DeleteCount
		BEGIN
			DELETE TOP(@BatchSize) FROM gpm.Price_' + @RegionCode + '
			WHERE PriceType = ''REG''
				and exists
				(
					select 1
					from #MostRecentRegPrices m
					where gpm.Price_' + @RegionCode + '.ItemID = m.ItemId
						and gpm.Price_' + @RegionCode + '.BusinessUnitID = m.BusinessUnitId
						and gpm.Price_' + @RegionCode + '.StartDate < m.StartDate
				)

			SET @BatchDeletedCount = @@ROWCOUNT
			SET @TotalDeletedCount += @BatchDeletedCount
			SET @BatchId += 1

			PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + '', '' + ''BatchId: '' + 
				CAST(@BatchId AS NVARCHAR(10)) + '', BatchDeletedCount: '' + 
				CAST(@BatchDeletedCount AS NVARCHAR(10)) + '', BatchSize: '' + 
				CAST(@BatchSize AS NVARCHAR(10)) + '', TotalDeletedCount: '' + 
				CAST(@TotalDeletedCount AS NVARCHAR(10)) + '', DeleteCount: '' + 
				CAST(@DeleteCount AS NVARCHAR(10))
			
			IF @BatchDeletedCount = 0 AND @TotalDeletedCount < @DeleteCount
			BEGIN
				PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + '', BatchDeletedCount is 0 but TotalDeletedCount is less than DeleteCount. Breaking out of WHILE loop.''
				BREAK
			END
		END'

	EXEC sp_executesql @sql,
					   N'@MaxDeleteCount int, @BatchSize int',
					   @MaxDeleteCount,
					   @BatchSize
		
END