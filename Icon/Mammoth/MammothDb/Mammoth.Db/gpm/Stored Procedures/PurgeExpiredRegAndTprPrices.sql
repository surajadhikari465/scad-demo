CREATE PROCEDURE [gpm].[PurgeExpiredRegAndTprPrices]
	@MaxDeleteCount int,
	@BatchSize int
AS
BEGIN
	/* ======================================================
	Description:	Deletes any expired REG and TPR prices
					in batches.					
	=======================================================*/	
	DECLARE @DefaultMaxDeleteCount int = 100000
	SET @MaxDeleteCount = COALESCE(@MaxDeleteCount, @DefaultMaxDeleteCount)

	DECLARE @DefaultBatchSize int = 20000
	SET @BatchSize = COALESCE(@BatchSize, @DefaultBatchSize)

	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'FL', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'MA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'MW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'NA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'NC', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'NE', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'PN', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'RM', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'SO', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'SP', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'SW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'TS', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredRegPrices @RegionCode = 'UK', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize

	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'FL', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'MA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'MW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'NA', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'NC', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'NE', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'PN', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'RM', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'SO', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'SP', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'SW', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'TS', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
	EXEC gpm.PurgeExpiredTprPrices @RegionCode = 'UK', @MaxDeleteCount = @MaxDeleteCount, @BatchSize = @BatchSize
END