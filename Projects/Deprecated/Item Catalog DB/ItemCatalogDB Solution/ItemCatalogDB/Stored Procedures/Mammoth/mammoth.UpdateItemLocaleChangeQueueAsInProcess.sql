IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'UpdateItemLocaleChangeQueueAsInProcess')
BEGIN
	EXEC('CREATE PROCEDURE [mammoth].[UpdateItemLocaleChangeQueueAsInProcess] AS BEGIN SET NOCOUNT ON; END')
END
GO

ALTER PROCEDURE [mammoth].[UpdateItemLocaleChangeQueueAsInProcess]
       @NumberOfRows		int,
	   @JobInstance			int
AS
BEGIN

	SET NOCOUNT ON

	-- ===============================================
	-- Update any rows that might be stuck in process
	-- ===============================================
	UPDATE [mammoth].[ItemLocaleChangeQueue]
	SET InProcessBy = NULL
	WHERE InProcessBy = @JobInstance

	-- ===============================================
	-- Update rows with job instance
	-- ===============================================
	;WITH Publish
	AS
	(
		SELECT TOP(@NumberOfRows) 
			InProcessBy
		FROM
			[mammoth].[ItemLocaleChangeQueue] WITH (ROWLOCK, READPAST, UPDLOCK)
		WHERE
			InProcessBy IS NULL
			AND ProcessFailedDate IS NULL
		ORDER BY
			InsertDate
	)

	UPDATE Publish SET InProcessBy = @JobInstance

END
GO