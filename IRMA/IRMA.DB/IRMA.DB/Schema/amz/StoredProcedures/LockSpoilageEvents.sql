CREATE PROCEDURE amz.LockInventorySpoilage
@NumberOfEntriesToProcess AS INT,
@Instance INT,
@ProcessDelayTimeInMinutes INT,
@WaitTimeForRecordsProcessedThreetimes  INT,
@WaitTimeForRecordsProcessedFourtimes  INT,
@WaitTimeForRecordsProcessedFivetimes  INT
AS
BEGIN

DECLARE @CurrentDateTime DateTime = GetDate()
DECLARE @InventorySpoilageEventID INT

SET @InventorySpoilageEventID = (SELECT EventTypeId from amz.EventType WHERE EventTypeCode = 'INV_ADJ')

UPDATE iq
	SET iq.InProcessBy = NULL,
		iq.Status = 'F',
		iq.ProcessTimes =  ISNULL(iq.ProcessTimes, 0) + 1,
		iq.LastProcessedTime = ISNULL(iq.LastProcessedTime, @CurrentDateTime)
	FROM [amz].[InventoryQueue] iq WITH (ROWLOCK, UPDLOCK, READPAST)
	WHERE iq.InProcessBy = @Instance

;WITH eventQueue_cte
	AS
	(
		SELECT TOP(@NumberOfEntriesToProcess)
				iq.QueueID,
				iq.InProcessBy
		FROM 
			[amz].[InventoryQueue]		iq	WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
		WHERE 
			iq.InProcessBy IS NULL  AND iq.Status IN ('U', 'F')
			AND  ((ISNULL(ProcessTimes, 0) <= 2 AND DATEDIFF(minute, iq.InsertDate, @CurrentDateTime) >= @ProcessDelayTimeInMinutes)
                OR  (ProcessTimes = 3 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedThreetimes) 
                OR  (ProcessTimes = 4 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFourtimes)
                OR  (ProcessTimes = 5 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFivetimes))

	)

	-- =====================================================
	-- Update rows if there are any to update
	-- =====================================================
	UPDATE iq
	SET  InProcessBy = @Instance,
	     Status = 'I'
	FROM  [amz].[InventoryQueue] iq
	JOIN eventQueue_cte eq ON iq.QueueID = eq.QueueID
	WHERE 
			iq.InProcessBy IS NULL  AND iq.Status IN ('U', 'F') 
	
END

GO

GRANT EXECUTE ON OBJECT::amz.LockInventorySpoilage to TibcoDataWriter

GO