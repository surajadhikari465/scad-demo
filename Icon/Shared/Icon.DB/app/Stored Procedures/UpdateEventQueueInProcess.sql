-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2014-10-20
-- Description:	Updates app.EventQueue.InProcessBy
--				column, and returns the updated rows
-- =============================================

CREATE PROCEDURE [app].[UpdateEventQueueInProcess]
	@RegisteredEventNames app.EventNameType READONLY,
	@MaxRows int,
	@Instance nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	-- =====================================================
	-- Update any rows that were marked InProcess but weren't successfully processed by that instance
	-- =====================================================
	UPDATE eq
	SET eq.InProcessBy = NULL 
	FROM app.EventQueue eq WITH (ROWLOCK, UPDLOCK, READPAST)
	WHERE eq.InProcessBy = @Instance

	-- =====================================================
	-- Update and then Query rows
	-- =====================================================
	;WITH eventQueue_cte
	AS
	(
		SELECT TOP(@MaxRows)
			eq.InProcessBy
		FROM 
			app.EventQueue				eq	WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
			JOIN app.EventType			et	on eq.EventId = et.EventId
			JOIN @RegisteredEventNames	en	on et.EventName = en.EventName
		WHERE 
			eq.InProcessBy IS NULL
			AND eq.ProcessFailedDate IS NULL
		ORDER BY
			eq.InsertDate ASC
	)

	-- =====================================================
	-- Update rows if there are any to update
	-- =====================================================
	UPDATE eventQueue_cte SET InProcessBy = @Instance

	-- =====================================================
	-- Return Rows that were updated
	-- =====================================================
	SELECT TOP(@MaxRows)
		q.QueueId				as QueueId,
		q.EventId				as EventId,
		q.EventMessage			as EventMessage,
		q.EventReferenceId		as EventReferenceId,
		q.RegionCode			as RegionCode,
		q.InsertDate			as InsertDate,
		q.ProcessFailedDate		as ProcessFailedDate,
		q.InProcessBy			as InProcessBy,
		et.EventName			as EventName
	FROM
		app.EventQueue		q
		JOIN app.EventType	et on q.EventId = et.EventId
	WHERE
		q.InProcessBy = @Instance
		AND q.ProcessFailedDate IS NULL
END
GO