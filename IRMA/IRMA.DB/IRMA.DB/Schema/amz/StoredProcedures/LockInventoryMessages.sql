CREATE PROCEDURE [amz].[LockInventoryMessages]
@NumberOfMessagesToProcess AS INT,
@Instance INT,
@ProcessDelayTimeInMinutes INT,
@WaitTimeForRecordsProcessedThreetimes INT,
@WaitTimeForRecordsProcessedFourtimes  INT,
@WaitTimeForRecordsProcessedFivetimes  INT
AS
BEGIN

DECLARE @CurrentDateTime DateTime = GetDate()

UPDATE m
	SET LastReprocessID = NULL,
		Status = 'F',
		ProcessTimes =  ISNULL(ProcessTimes, 0) + 1,
		LastProcessedTime = ISNULL(LastProcessedTime, @CurrentDateTime)
	FROM [amz].[MessageArchive] m WITH (ROWLOCK, UPDLOCK, READPAST)
	WHERE LastReprocessID = @Instance
      AND Status = 'I'

;WITH message_cte
	AS
	(
		SELECT TOP(@NumberOfMessagesToProcess)
				MessageArchiveID
		FROM 
			[amz].[MessageArchive]		WITH (ROWLOCK, UPDLOCK, READPAST) -- force row locking for concurrency purposes
		WHERE 
			LastReprocessID IS NULL  AND Status IN ('U', 'F')
			AND  ((ISNULL(ProcessTimes, 0) <= 2 AND DATEDIFF(minute, InsertDate, @CurrentDateTime) >= @ProcessDelayTimeInMinutes)
                OR  (ProcessTimes = 3 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedThreetimes) 
                OR  (ProcessTimes = 4 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFourtimes)
                OR  (ProcessTimes = 5 AND  DATEDIFF(minute, LastProcessedTime , @CurrentDateTime) >= @WaitTimeForRecordsProcessedFivetimes))

	)

	-- =====================================================
	-- Update rows if there are any to update
	-- =====================================================
	UPDATE ma
	SET  ma.LastReprocessID = @Instance,
	     ma.Status = 'I'
	FROM  [amz].[MessageArchive] ma
	JOIN  message_cte mc on ma.MessageArchiveID = mc.MessageArchiveID
	WHERE 
		  ma.LastReprocessID IS NULL  
	  AND Status IN ('U', 'F') 
	
END
GO

GRANT EXECUTE ON OBJECT::amz.LockInventoryMessages to TibcoDataWriter

GO