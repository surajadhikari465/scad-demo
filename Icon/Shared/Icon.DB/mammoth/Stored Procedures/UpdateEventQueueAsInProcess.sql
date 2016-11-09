CREATE PROCEDURE [mammoth].[UpdateEventQueueAsInProcess]
	@Instance int,
	@NumberOfRows int,
	@EventTypeIds app.IntList READONLY
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Update any rows that were marked InProcess but weren't successfully processed by that instance
	-- =====================================================
	UPDATE eq
	SET eq.InProcessBy = NULL 
	FROM mammoth.EventQueue eq WITH (ROWLOCK, UPDLOCK, READPAST)
	WHERE eq.InProcessBy = @Instance

	-- =====================================================
	-- Update and then Query rows
	-- =====================================================
	;WITH eventQueue_cte
	AS
	(
		SELECT TOP(@NumberOfRows)
			eq.InProcessBy
		FROM 
			mammoth.EventQueue	eq WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
			join @EventTypeIds	et on eq.EventTypeId = et.I	
		WHERE 
			eq.InProcessBy IS NULL
			AND (eq.ProcessedFailedDate IS NULL
				OR (eq.ProcessedFailedDate IS NOT NULL
					AND eq.NumberOfRetry < 3))
		ORDER BY
			eq.InsertDate ASC
	)

	-- =====================================================
	-- Update rows if there are any to update
	-- =====================================================
	UPDATE eventQueue_cte SET InProcessBy = @Instance
END