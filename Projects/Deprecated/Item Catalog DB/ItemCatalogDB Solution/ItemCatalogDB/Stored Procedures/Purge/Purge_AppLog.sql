IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.Purge_AppLog'))
   exec('CREATE PROCEDURE [dbo].[Purge_AppLog] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[Purge_AppLog] (@QC BIT = 1, @BatchSize INT) AS BEGIN

    DECLARE @UpdateCt_Batch INT
    DECLARE @UpdateCt_Total INT
    DECLARE @BatchID INT
    DECLARE @TotalCt INT
    DECLARE @Now DATE
    DECLARE @Then DATE
    DECLARE @Rowset INT
    DECLARE @BatchSize_Fallback INT
    DECLARE @Retention_Days INT
    DECLARE @RowCt_Total INT
    
	-- For debug
	--DECLARE @QC BIT
	
	SET @BatchSize_Fallback =  50000; -- batch size
    SET @RowCt_Total = 100000; -- how many rows for the entire job
	SET @Retention_Days = 14;
	SET @BatchID = 1;
	SET @UpdateCt_Total = 0

	SET @Now = DATEADD(d,0,DATEDIFF(d,0,GETDATE()))
	SET @Then = DATEADD(DAY, -@Retention_Days, @Now)

	SELECT @Rowset = COUNT(*) FROM [dbo].[AppLog] WHERE InsertDate < @Then

	PRINT 'Row count: ' + CAST(@Rowset AS VARCHAR(18))

	SELECT @BatchSize = COALESCE(@BatchSize, @BatchSize_Fallback)

	SELECT @TotalCt = CASE 
		WHEN @Rowset >= @RowCt_Total THEN @RowCt_Total
		WHEN @Rowset < @RowCt_Total THEN @Rowset
	END

	PRINT 'Rows to delete this run: ' + CAST(@TotalCt AS VARCHAR(18))

	-- delete up to @TotalCt rows each night, in batches of @BatchSize

	WHILE @UpdateCt_Total < @TotalCt BEGIN
		SET @UpdateCt_Batch = 0
		BEGIN TRAN
			BEGIN TRY
				IF (@QC = 1) BEGIN
					SELECT TOP (@BatchSize) * FROM [dbo].[AppLog] WHERE InsertDate < @Then
					SELECT @UpdateCt_Batch = @@ROWCOUNT	
				END

				IF (@QC = 0) BEGIN
					DELETE TOP (@BatchSize) FROM [dbo].[AppLog] WHERE InsertDate < @Then
					SELECT @UpdateCt_Batch = @@ROWCOUNT	
				END

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
		WAITFOR DELAY '00:00:05'
	END
END
GO