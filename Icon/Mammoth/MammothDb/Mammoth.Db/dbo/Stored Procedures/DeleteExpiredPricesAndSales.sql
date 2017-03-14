CREATE PROCEDURE dbo.DeleteExpiredPricesAndSales
	@MaxDeleteCount int,
	@BatchSize int
AS
BEGIN
	/* =============================================
	Author:			Matt Scherping, Blake Jones
	Create date:	2017-03-10
	Description:	Deletes any expired REG and TPR prices
					in batches.					
	=============================================*/	
	DECLARE @DefaultMaxDeleteCount int = 100000
	SET @MaxDeleteCount = COALESCE(@MaxDeleteCount, @DefaultMaxDeleteCount)

	DECLARE @DefaultBatchSize int = 20000
	SET @BatchSize = COALESCE(@BatchSize, @DefaultBatchSize)

	EXEC dbo.DeleteExpiredPrices @RegionCode = 'FL', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'MA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'MW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'NA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'NC', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'NE', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'PN', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'RM', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'SO', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'SP', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'SW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'UK', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize

	EXEC dbo.DeleteExpiredSales @RegionCode = 'FL', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'MA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'MW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'NA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'NC', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'NE', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'PN', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'RM', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'SO', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'SP', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'SW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC dbo.DeleteExpiredSales @RegionCode = 'UK', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
END