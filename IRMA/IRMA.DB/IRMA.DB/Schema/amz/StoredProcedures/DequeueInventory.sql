CREATE PROCEDURE [amz].[DequeueInventory]
	@maxRecords int = 0,
	@lastRunDateTime datetime = NULL,
	@affectedRowCount int OUTPUT
AS
BEGIN

	SET NOCOUNT ON

	IF @maxRecords = 0
		SET @maxRecords = 100;

	IF @lastRunDateTime IS NULL
		SET @lastRunDateTime = DATEADD(minute, -5, GETDATE());

	WITH QueueCte
	AS (
		SELECT TOP (@maxRecords) *
		FROM amz.InventoryQueue WITH (ROWLOCK,READPAST)
		WHERE InsertDate < @lastRunDateTime
		ORDER BY QueueID
		)
	DELETE
	FROM QueueCte
	OUTPUT
		deleted.QueueID,
		deleted.EventTypeCode,
		deleted.MessageType,
		deleted.KeyID,
		deleted.SecondaryKeyID,
		deleted.InsertDate,
		deleted.MessageTimestampUtc;

	SELECT @affectedRowCount = @@ROWCOUNT;

	SET NOCOUNT OFF
END
GO

GRANT EXEC on [amz].[DequeueInventory] to [TibcoDataWriter]
GO