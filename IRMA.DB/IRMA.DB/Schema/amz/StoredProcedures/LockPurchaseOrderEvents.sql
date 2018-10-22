CREATE PROCEDURE amz.LockPurchaseOrderEvents
@NumberOfOrdersToProcess AS INT,
@Instance INT,
@ProcessDelayTimeInMinutes INT,
@WaitTimeForRecordsProcessedThreetimes  INT,
@WaitTimeForRecordsProcessedFourtimes  INT,
@WaitTimeForRecordsProcessedFivetimes  INT
AS
BEGIN

DECLARE @CurrentDateTime DateTime = GetDate()

UPDATE rq
       SET rq.InProcessBy = NULL 
       FROM amz.OrderQueue rq WITH (ROWLOCK, UPDLOCK, READPAST)
       WHERE rq.InProcessBy = @Instance

;WITH eventQueue_cte
       AS
       (
             SELECT TOP(@NumberOfOrdersToProcess)
                           rq.KeyID,
                           rq.InProcessBy
             FROM 
                    [amz].[OrderQueue]       rq     WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
                    JOIN amz.EventType       et     on rq.EventTypeID = et.EventTypeID
             WHERE 
                    rq.InProcessBy IS NULL
               AND  et.EventTypeCode like 'PO%'
			   AND  rq.Status IN ('U', 'E')
               AND  ((ISNULL(ProcessTimes, 0) <= 2 AND DATEDIFF(minute, rq.InsertDate, @CurrentDateTime) >= @ProcessDelayTimeInMinutes)
                OR  (ProcessTimes = 3 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedThreetimes) 
                OR  (ProcessTimes = 4 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFourtimes)
                OR  (ProcessTimes = 5 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFivetimes)))

       -- =====================================================
       -- Update rows if there are any to update
       -- =====================================================
       UPDATE rq
       SET  InProcessBy = @Instance, Status = 'I'
       FROM  [amz].[OrderQueue] rq
       JOIN eventQueue_cte eq ON rq.KeyID = eq.KeyID

END
GO

GRANT EXECUTE ON OBJECT::amz.LockPurchaseOrderEvents to TibcoDataWriter

GO