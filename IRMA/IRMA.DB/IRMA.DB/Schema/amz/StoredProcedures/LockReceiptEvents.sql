CREATE PROCEDURE amz.LockReceiptEvents
@NumberOfOrdersToProcess AS INT,
@Instance INT,
@ProcessDelayTimeInMinutes INT,
@WaitTimeForRecordsProcessedThreetimes  INT,
@WaitTimeForRecordsProcessedFourtimes  INT,
@WaitTimeForRecordsProcessedFivetimes  INT
AS
BEGIN

DECLARE @CurrentDateTime DateTime = GetDate()
DECLARE @ReceiptCreateEventID INT
DECLARE @ReceiptModifiedEventId INT


SET @ReceiptCreateEventID = (SELECT EventTypeId from amz.EventType WHERE EventTypeCode = 'RCPT_CRE')
SET @ReceiptModifiedEventId = (SELECT EventTypeId from amz.EventType WHERE EventTypeCode = 'RCPT_MOD')

UPDATE rq
	SET	rq.InProcessBy = NULL, 
		rq.Status = 'F',
		rq.ProcessTimes = ISNULL(rq.ProcessTimes, 0) + 1,
		rq.LastProcessedTime = ISNULL(rq.LastProcessedTime, @CurrentDateTime)
	FROM [amz].[ReceiptQueue] rq WITH (ROWLOCK, UPDLOCK, READPAST)
	WHERE rq.InProcessBy = @Instance

;WITH eventQueue_cte
	AS
	(
		SELECT TOP(@NumberOfOrdersToProcess)
				rq.KeyID,
				rq.InProcessBy,
				MIN(IsNull(ProcessTimes,0)) as ProcessTimesMax,
				MIN(LastProcessedTime) as LastProcessedTime

		FROM 
			[amz].[ReceiptQueue]		rq	WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
			JOIN amz.EventType			et	on rq.EventTypeID = et.EventTypeID
		WHERE 
			rq.InProcessBy IS NULL  AND rq.Status IN ('U', 'F') AND DATEDIFF(minute, rq.InsertDate, @CurrentDateTime) >= @ProcessDelayTimeInMinutes
		GROUP BY KeyID, InProcessBy
		HAVING ( MIN(ISNull(ProcessTimes,0)) <= 2 OR  
			   ( MIN(ProcessTimes) = 3 AND  DATEDIFF(minute, MIN(LastProcessedTime) , @CurrentDateTime) >= @WaitTimeForRecordsProcessedThreetimes) OR
			    (MIN(ProcessTimes) = 4 AND  DATEDIFF(minute, MIN(LastProcessedTime) , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFourtimes) OR
				(MIN(ProcessTimes) = 5 AND  DATEDIFF(minute, MIN(LastProcessedTime) , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFivetimes))

		ORDER BY
			Max(ProcessTimes) Desc

	)

	-- =====================================================
	-- Update rows if there are any to update
	-- =====================================================
	UPDATE rq
	SET  InProcessBy = @Instance,
	     Status = 'I'
	FROM  [amz].[ReceiptQueue] rq
	JOIN eventQueue_cte eq ON rq.KeyID = eq.KeyID
	WHERE 
			rq.InProcessBy IS NULL  AND rq.Status IN ('U', 'F') 
	
	-- change status for reduntant events

	UPDATE rq
	SET  rq.InProcessBy = NULL,
		 rq.Status = 'R'
	FROM  [amz].[ReceiptQueue] rq
	WHERE InProcessBy = @Instance  
		AND rq.Status IN ('I') 
		AND rq.EventTypeID = @ReceiptModifiedEventId  
	    AND EXISTS( SELECT 1 FROM [amz].[ReceiptQueue] re
				WHERE re.KeyID = rq.KeyID and re.SecondaryKeyID = rq.SecondaryKeyID
				AND re.EventTypeID = @ReceiptCreateEventID
				AND re.Status NOT IN ('P', 'F'))
	
END

GO

GRANT EXECUTE ON OBJECT::amz.LockReceiptEvents to TibcoDataWriter

GO