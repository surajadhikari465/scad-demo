CREATE PROCEDURE amz.GetMessagesWithError
(	   @RunAsOfDate DateTime,
       @CheckMessagesBeforeThisNumberOfMinutes INT
)

AS
BEGIN
       DECLARE @DBNAME VARCHAR(200)
       DECLARE @CurrentDateTime DateTime = GETDATE()
	   DECLARE @GetOneYearBeforeDate DateTime=   (SELECT DATEADD(year, -1, GETDATE()))

	   IF( @RunAsOfDate < @GetOneYearBeforeDate)
	   SET @RunAsOfDate = GETDATE()

	   DECLARE @DateToRunFor Date = (SELECT dateadd(day,datediff(day,1,@RunAsOfDate),0))
	   DECLARE @DateToRunUpTo Date = (SELECT CONVERT(date, @RunAsOfDate))

	   SET NOCOUNT ON

	   IF OBJECT_ID('tempdb..##tmpOrderInfoAmazon') IS NOT NULL
	    DROP TABLE ##tmpOrderInfoAmazon

       SELECT QueueID AS 'Queue ID', 
              et.EventTypeCode AS 'Event Type Code',
              Substring(ErrorDescription, 0 ,50)  AS 'Error Description',
			  LastProcessedTime AS 'Time Of Failure'
       INTO ##tmpOrderInfoAmazon
       FROM [amz].[InventoryQueue] q
       INNER JOIN amz.EventType et ON q.EventTypeID = et.EventTypeID
       WHERE ((q.Status in ('F')  AND q.ProcessTimes = 6)
               OR ( q.Status in ('I', 'U')))
               AND DateDiff(mi, q.InsertDate, @CurrentDateTime )> @CheckMessagesBeforeThisNumberOfMinutes
			   AND  q.InsertDate > @DateToRunFor
			   AND  q.InsertDate < @DateToRunUpTo

       UNION
       
       SELECT QueueID, 
              et.EventTypeCode,
              Substring(ErrorDescription, 0 ,50) AS ErrorDescription,
			  LastProcessedTime AS 'Time Of Failure'
       FROM [amz].[ReceiptQueue] q
       INNER JOIN amz.EventType et ON q.EventTypeID = et.EventTypeID
       WHERE ((q.Status in ('F')  AND q.ProcessTimes = 6)
               OR ( q.Status in ('I', 'U')))
               AND DateDiff(mi, q.InsertDate, @CurrentDateTime )> @CheckMessagesBeforeThisNumberOfMinutes
			   AND  q.InsertDate > @DateToRunFor
			   AND  q.InsertDate < @DateToRunUpTo
       UNION
       
       SELECT QueueID, 
              et.EventTypeCode,
              Substring(ErrorDescription, 0 ,50) AS ErrorDescription,
			  LastProcessedTime AS 'Time Of Failure'
       FROM [amz].[OrderQueue] q
       INNER JOIN amz.EventType et ON q.EventTypeID = et.EventTypeID
       WHERE ((q.Status in ('F')  AND q.ProcessTimes = 6)
               OR ( q.Status in ('I', 'U')))
               AND DateDiff(mi, q.InsertDate, @CurrentDateTime )> @CheckMessagesBeforeThisNumberOfMinutes
			   AND  q.InsertDate > @DateToRunFor
			   AND  q.InsertDate < @DateToRunUpTo
       UNION

       SELECT QueueID, 
              q.EventType,
              Substring(ErrorDescription, 0 ,50) AS ErrorDescription,
			  LastProcessedTime AS 'Time Of Failure'
       FROM [amz].[MessageArchive] q
       WHERE ((q.Status in ('F')  AND q.ProcessTimes = 6)
               OR ( q.Status in ('I', 'U')))
               AND DateDiff(mi, q.InsertDate, @CurrentDateTime )> @CheckMessagesBeforeThisNumberOfMinutes
			   AND  q.InsertDate > @DateToRunFor
			   AND  q.InsertDate < @DateToRunUpTo

       ORDER BY EventTypeCode

       UPDATE ##tmpOrderInfoAmazon
       SET [Error Description]= 'Error Occured'
       WHERE [Error Description] IS NULL OR [Error Description] = ''

	   SET NOCOUNT OFF
END