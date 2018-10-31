CREATE PROCEDURE amz.LockTransferOrderEvents
@NumberOfOrdersToProcess AS INT,
@Instance INT,
@ProcessDelayTimeInMinutes INT,
@WaitTimeForRecordsProcessedThreetimes  INT,
@WaitTimeForRecordsProcessedFourtimes  INT,
@WaitTimeForRecordsProcessedFivetimes  INT
AS
BEGIN

DECLARE @CurrentDateTime DateTime = GetDate()

UPDATE q
       SET	q.InProcessBy = NULL, 
			q.Status = 'F',
			q.ProcessTimes = ISNULL(q.ProcessTimes, 0) + 1,
			q.LastProcessedTime = ISNULL(q.LastProcessedTime, @CurrentDateTime)
       FROM amz.OrderQueue q WITH (ROWLOCK, UPDLOCK, READPAST)
       WHERE q.InProcessBy = @Instance

;WITH eventQueue_cte
       AS
       (
             SELECT TOP(@NumberOfOrdersToProcess)
                           q.QueueID,
                           q.InProcessBy
             FROM 
                    [amz].[OrderQueue]       q     WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
                    JOIN amz.EventType       et     on q.EventTypeID = et.EventTypeID
             WHERE 
                    q.InProcessBy IS NULL
               AND  et.EventTypeCode like 'TSF%'
			   AND  q.Status IN ('U', 'F')
               AND  ((ISNULL(ProcessTimes, 0) <= 2 AND DATEDIFF(minute, q.InsertDate, @CurrentDateTime) >= @ProcessDelayTimeInMinutes)
                OR  (ProcessTimes = 3 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedThreetimes) 
                OR  (ProcessTimes = 4 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFourtimes)
                OR  (ProcessTimes = 5 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFivetimes)))

       -- =====================================================
       -- Update rows if there are any to update
       -- =====================================================
       UPDATE q
       SET  InProcessBy = @Instance, Status = 'I'
       FROM  [amz].[OrderQueue] q
       JOIN eventQueue_cte eq ON q.QueueID = eq.QueueID

END
GO

GRANT EXECUTE ON OBJECT::amz.LockTransferOrderEvents to TibcoDataWriter

GO