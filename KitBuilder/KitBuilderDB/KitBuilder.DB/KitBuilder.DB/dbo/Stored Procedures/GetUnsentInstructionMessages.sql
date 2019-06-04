CREATE PROCEDURE [dbo].[GetUnsentInstructionMessages]
	@batchSize  int = 0
AS
BEGIN
	SET NOCOUNT ON

	UPDATE top(@batchSize) ma
	   SET Status = 'PR'
	OUTPUT inserted.InstructionListQueueID, inserted.KeyID, inserted.Status,inserted.MessageTimestampUtc, inserted.InsertDateUtc
	  FROM dbo.InstructionListQueue ma WITH (UPDLOCK, READPAST)  
	 WHERE Status = 'U'
  
	SET NOCOUNT OFF
END