CREATE PROCEDURE [amz].[GetUnsentInStockMessages]
	@batchSize  int = 0
AS
BEGIN
	SET NOCOUNT ON

	UPDATE top(@batchSize) ma
	   SET Status = 'P'
	OUTPUT inserted.MessageArchiveID, inserted.EventType, inserted.BusinessUnitID, inserted.MessageNumber, inserted.Message
	  FROM amz.MessageArchive ma WITH (UPDLOCK, READPAST)  
	 WHERE Status = 'U'
  
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[amz].[GetUnsentInStockMessages] TO [TibcoDataWriter], [IconInterface]
    AS [dbo];
