--Used by Mammoth/WebSupport
CREATE PROCEDURE amz.ResetQueueMessages
  @action NVARCHAR(25),
  @queue NVARCHAR(25),
  @maxRecords int = 10000,
	@userName NVARCHAR(200) = NULL,
  @status NVARCHAR(1) = NULL,
  @keyID INT = NULL,
  @IDs dbo.IntType READONLY
AS
BEGIN
  SET NOCOUNT ON
  
  DECLARE @sql varchar(max);

  IF(@action = 'Get') --Get queue list
    BEGIN
      IF(@queue = 'Archive')
       BEGIN
         IF(@keyID IS NOT NULL)
           SELECT TOP(@maxRecords) MessageArchiveID QueueID,  B.EventTypeDescription [Event], KeyID,
                CASE Status WHEN 'F' THEN 'Failed' WHEN 'P' THEN 'Processed' ELSE 'Unprocessed' END Status,
                CONVERT(VARCHAR, A.InsertDate, 120) Insert_Date, ResetBy Reset_By
           FROM amz.MessageArchive A
           INNER JOIN amz.EventType B on B.EventTypeCode = A.EventType
           WHERE A.KeyID = @keyID
         ELSE
           SELECT TOP(@maxRecords) MessageArchiveID QueueID,  B.EventTypeDescription [Event], KeyID,
                  CASE Status WHEN 'F' THEN 'Failed' WHEN 'P' THEN 'Processed' ELSE 'Unprocessed' END Status,
                  CONVERT(VARCHAR, A.InsertDate, 120) Insert_Date, ResetBy Reset_By
           FROM amz.MessageArchive A
           INNER JOIN amz.EventType B on B.EventTypeCode = A.EventType
           WHERE ((Status = IsNull(@status, Status) AND Status <> 'F') OR
                 (Status = IsNull(@status, Status) AND Status = 'F' AND IsNull(ProcessTimes, 0) >= 6))
       END

      ELSE IF(@queue IN('Inventory', 'Order', 'Receipt'))
       BEGIN
         SET @sql = 'DECLARE @status NVARCHAR(1) = ' + CASE WHEN @status IS NULL THEN 'NULL' ELSE ('''' + @status + '''') END + ';
                       SELECT TOP(' + CAST(@maxRecords as varchar(25)) + ') QueueID, B.EventTypeDescription [Event], KeyID,
                              CASE Status WHEN ''F'' THEN ''Failed'' WHEN ''P'' THEN ''Processed'' ELSE ''Unprocessed'' END Status,
                              CONVERT(VARCHAR, A.InsertDate, 120) Insert_Date, ResetBy Reset_By
                       FROM amz.' + @queue + 'Queue A
                       INNER JOIN amz.EventType B on B.EventTypeID = A.EventTypeID';


         IF(@keyID IS NOT NULL)
           SET @sql = @sql + ' WHERE A.KeyID = ' + cast(@keyID as nvarchar(20)); 
         ELSE
           SET @sql = @sql + ' WHERE (Status = IsNull(@status, Status) AND Status <> ''F'') OR
                                     (Status = IsNull(@status, Status) AND Status = ''F'' AND IsNull(ProcessTimes, 0) >= 6);';
        EXEC(@sql);
      END

      SET NOCOUNT OFF;
      RETURN;
    END
  
  
  IF(@action = 'Reset') --Reset queue message(s)
    BEGIN
      IF(NOT Exists(select 1 from @IDs)) RETURN;

      IF(OBJECT_ID('tempdb..#tempIDs') is not null) DROP TABLE #tempIDs; --Needed for dynamic SQL below
      SELECT [Key] AS ID INTO #tempIDs FROM @IDs GROUP BY [Key];
  
      IF(@queue = 'Archive')
       BEGIN
         UPDATE A SET ProcessTimes = 0, Status = 'U', LastReprocessID = NULL, LastReprocess = NULL, ResetBy = @userName
         FROM amz.MessageArchive A
         INNER JOIN #tempIDs B ON B.ID = A.MessageArchiveID;
       END

      ELSE IF(@queue IN('Inventory', 'Order', 'Receipt'))
       BEGIN
         SET @sql = 'UPDATE A SET ProcessTimes = 0, Status = ''U'', InProcessBy = NULL, ResetBy = ''' + @userName + '''
                     FROM amz.' + @queue + 'Queue A
                     INNER JOIN #tempIDs B ON B.ID = A.QueueID;';
         EXEC(@sql);
      END

      IF(OBJECT_ID('tempdb..#tempIDs') is not null) DROP TABLE #tempIDs;
  END
  
  SET NOCOUNT OFF;
END