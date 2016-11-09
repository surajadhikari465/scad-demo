CREATE PROCEDURE [mammoth].[BulkInsertProductMammothEvents]
	@eventMessageList mammoth.EventMessageType READONLY,
	@eventTypeId int

AS
	DECLARE @distinctMessages mammoth.EventMessageType			
	
	--Generate ProductAdd events for scan codes
	INSERT @distinctMessages 
	SELECT DISTINCT EventMessage from @eventMessageList

	INSERT INTO mammoth.EventQueue (EventTypeId
           ,EventReferenceId
           ,EventMessage
           ,InsertDate
           ,ProcessedFailedDate
           ,InProcessBy
           ,NumberOfRetry)
	SELECT @eventTypeId, sc.itemID, sc.scanCode, sysdatetime(), null, null, null
	  FROM @distinctMessages dsc
	  JOIN ScanCode sc 
	    ON dsc.EventMessage = sc.scanCode