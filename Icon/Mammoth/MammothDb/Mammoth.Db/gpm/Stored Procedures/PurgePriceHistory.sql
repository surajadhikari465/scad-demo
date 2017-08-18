CREATE PROCEDURE [gpm].[PurgePriceHistory]
	@MaxDeleteCount int,
	@BatchSize int,
	@Retention_Days int
AS
	DECLARE @UpdateCt_Batch INT
    DECLARE @UpdateCt_Total INT
    DECLARE @BatchID INT
    DECLARE @TotalCt INT
    DECLARE @Now DATE
    DECLARE @Then DATE
    DECLARE @Rowset INT
    DECLARE @BatchSize_Fallback INT
    DECLARE @RowCt_Total INT

	SET @BatchID = 1;
	SET @UpdateCt_Total = 0

	SET @Now = GETDATE()
	SET @Then = DATEADD(DAY, -@Retention_Days, @Now)

	SELECT @Rowset = COUNT(*) FROM [gpm].[PriceHistory] WHERE PriceHistoryInsertDateUtc < @Then

	PRINT 'Row count: ' + CAST(@Rowset AS VARCHAR(18))

	SELECT @BatchSize = COALESCE(@BatchSize, @BatchSize_Fallback)

	SELECT @TotalCt = CASE 
		WHEN @Rowset >= @MaxDeleteCount THEN @MaxDeleteCount
		WHEN @Rowset < @MaxDeleteCount THEN @Rowset
	END

	PRINT 'Rows to delete this run: ' + CAST(@TotalCt AS VARCHAR(18))

	-- delete up to @TotalCt rows each night, in batches of @BatchSize
	WHILE @UpdateCt_Total < @TotalCt BEGIN
		SET @UpdateCt_Batch = 0
		BEGIN TRAN
			BEGIN TRY

				DELETE TOP (@BatchSize) FROM [gpm].[PriceHistory] WHERE PriceHistoryInsertDateUtc < @Then
				SELECT @UpdateCt_Batch = @@ROWCOUNT	

			COMMIT TRAN

				SET @UpdateCt_Total += @UpdateCt_Batch 
				PRINT 'Batch ' + CAST(@BatchID AS VARCHAR(4))
				PRINT '...row count (batch): ' + CAST(@UpdateCt_Batch AS VARCHAR(5))
				PRINT '...row count (total): ' + CAST(@UpdateCt_Total AS VARCHAR(10)) + '.'

			END TRY
			BEGIN CATCH
				PRINT 'Batch ' + CAST(@BatchID AS VARCHAR(4)) + ': Delete failed!'
				ROLLBACK TRAN	
			END CATCH
		SET @BatchID += 1
	END

