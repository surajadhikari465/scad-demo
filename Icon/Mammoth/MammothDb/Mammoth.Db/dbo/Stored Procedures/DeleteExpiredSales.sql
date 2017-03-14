CREATE PROCEDURE dbo.DeleteExpiredSales
	@RegionCode nvarchar(2),
	@MaxDeleteCount int,
	@BatchSize int
AS 
BEGIN
	/* =============================================
	Author:			Matt Scherping, Blake Jones
	Create date:	2017-03-10
	Description:	Deletes any non REG prices that 
					have an EndDate less than today 
					for a given region.
	=============================================*/

	DECLARE @sql nvarchar(max) = '
		DECLARE @DefaultDeleteCount int = 100000
		SET @MaxDeleteCount = COALESCE(@MaxDeleteCount, @DefaultDeleteCount)

		DECLARE @Today datetime2 = CONVERT(date, GETDATE())

		--Get count of expired prices to delete
		DECLARE @DeleteCount int = (
			SELECT COUNT(*)
			FROM Price_' + @RegionCode + '
			WHERE PriceType <> ''REG'' 
				AND EndDate < @Today)

		IF @DeleteCount > @MaxDeleteCount
			SET @DeleteCount = @MaxDeleteCount

		DECLARE @TotalDeletedCount int = 0
		DECLARE @BatchDeletedCount int = 0
		DECLARE @BatchId int = 0

		PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + '', '' + ''Deleting '' + CAST(@DeleteCount AS NVARCHAR(10)) + '' TPR prices from Price_' + @RegionCode + ' in batches of '' + CAST(@BatchSize AS NVARCHAR(10))

		--Delete expired prices in batches
		WHILE @TotalDeletedCount < @DeleteCount
		BEGIN
			DELETE TOP(@BatchSize) Price_' + @RegionCode +'
			WHERE PriceType <> ''REG'' 
				AND EndDate < @Today
			
			SET @BatchDeletedCount = @@ROWCOUNT
			SET @TotalDeletedCount += @BatchDeletedCount
			SET @BatchId += 1

			PRINT CONVERT(nvarchar, SYSDATETIME(), 121) + '', '' + ''BatchId: '' + CAST(@BatchId AS NVARCHAR(10)) + '', BatchDeletedCount: '' + CAST(@BatchDeletedCount AS NVARCHAR(10)) + '', BatchSize: '' + CAST(@BatchSize AS NVARCHAR(10)) + '', TotalDeletedCount: '' + CAST(@TotalDeletedCount AS NVARCHAR(10)) + '', DeleteCount: '' + CAST(@DeleteCount AS NVARCHAR(10))
			
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