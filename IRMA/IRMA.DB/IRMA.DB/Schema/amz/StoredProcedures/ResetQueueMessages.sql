--Used by Mammoth/WebSupport
CREATE PROCEDURE [amz].[ResetQueueMessages]
  @action NVARCHAR(25),
  @queue NVARCHAR(25),
  @maxRecords int = 50001,
  @userName NVARCHAR(200) = NULL,
  @messageType NVARCHAR(50) = NULL,
  @eventType NVARCHAR(25) = NULL,
  @status NVARCHAR(1) = NULL,
  @storeBU INT = NULL,
  @keyID INT = NULL,
  @secondaryKeyID INT = NULL, 
  @startDatetime DATETIME = NULL,
  @endDatetime DATETIME = NULL,
  @IDs dbo.IntType READONLY
AS
BEGIN
  SET NOCOUNT ON
  
  DECLARE @sql varchar(max);

  IF(@action = 'Get') --Get queue list
    BEGIN
      IF(@queue = 'ArchivedMessage')
       BEGIN
           SELECT TOP(@maxRecords) MessageArchiveID ArchiveID, BusinessUnitID StoreBU, EventType [Event], KeyID, SecondaryKeyID,
                CASE Status WHEN 'F' THEN 'Failed' WHEN 'P' THEN 'Processed' ELSE 'Unprocessed' END Status, 
				CONVERT(VARCHAR, InsertDate, 120) Insert_Date, ResetBy Reset_By
           FROM amz.MessageArchive
          WHERE (@keyID IS NULL OR KeyID = @keyID) 
		    AND BusinessUnitID = ISNULL(@storeBU, BusinessUnitID)
			AND EventType = ISNULL(@eventType, EventType)
			AND EventType LIKE (CASE @messageType
								WHEN 'Inventory' THEN 'INV_%'
								WHEN 'PurchaseOrder' THEN 'PO_%'
								WHEN 'Receipt' THEN 'RCPT_%'
								WHEN 'TransferOrder' THEN 'TSF_%'
								ELSE EventType
								END)
		    AND (@startDatetime IS NULL OR InsertDate >= @startDatetime)
			AND (@endDatetime IS NULL OR  InsertDate <= @endDatetime)
			AND Status = IsNull(@status, Status)
       END

      ELSE IF(RIGHT(@queue, 6) = 'Events')
       BEGIN
         SELECT TOP(@maxRecords) MessageArchiveEventID ArchiveID, EventTypeCode [Event], KeyID, SecondaryKeyID, 
              '' Status, CONVERT(VARCHAR, InsertDate, 120) Insert_Date, '' Reset_By
           FROM amz.MessageArchiveEvent
		  WHERE (@keyID IS NULL OR KeyID = @keyID)  
			AND (@secondaryKeyID IS NULL OR SecondaryKeyID = @secondaryKeyID) 
			AND (@startDatetime IS NULL OR InsertDate >= @startDatetime)
			AND (@endDatetime IS NULL OR  InsertDate <= @endDatetime)
			AND EventTypeCode = ISNULL(@eventType, EventTypeCode)
		    AND MessageType like (LEFT(@queue, 5) +'%')
	   END
      SET NOCOUNT OFF;
      RETURN;
    END
  
  
  IF(@action = 'Reset') --Reset queue message(s)
    BEGIN
      IF(NOT Exists(select 1 from @IDs)) RETURN;

      IF(OBJECT_ID('tempdb..#tempIDs') is not null) DROP TABLE #tempIDs; --Needed for dynamic SQL below
      SELECT [Key] AS ID INTO #tempIDs FROM @IDs GROUP BY [Key];
  
      IF(@queue = 'ArchivedMessage')
       BEGIN
         UPDATE A SET ProcessTimes = 0, Status = 'U', LastReprocessID = NULL, LastReprocess = NULL, ResetBy = @userName
         FROM amz.MessageArchive A
         INNER JOIN #tempIDs B ON B.ID = A.MessageArchiveID;
       END

      ELSE IF(RIGHT(@queue, 6) = 'Events')
       BEGIN
		 
		 SELECT @sql = CASE @queue
					WHEN 'InventoryEvents'
						THEN 'InventoryQueue '
					WHEN 'PurchaseOrderEvents'
						THEN 'OrderQueue '
					WHEN 'ReceiptEvents'
						THEN 'ReceiptQueue '
					ELSE
						'TransferQueue '
				END
		 
		 SET @sql = 'INSERT INTO amz.' + @sql +
			' (EventTypeCode, MessageType, KeyID, SecondaryKeyID, InsertDate, MessageTimestampUtc) ' +
					'SELECT EventTypeCode, MessageType, KeyID, SecondaryKeyID, InsertDate, MessageTimestampUtc
					   FROM amz.MessageArchiveEvent A
				 INNER JOIN #tempIDs B ON B.ID = A.MessageArchiveEventID'	
		 
         EXEC(@sql);
      END

      IF(OBJECT_ID('tempdb..#tempIDs') is not null) DROP TABLE #tempIDs;
  END
  
  SET NOCOUNT OFF;
END
GO

GRANT EXECUTE ON OBJECT::amz.ResetQueueMessages to MammothRole

GO