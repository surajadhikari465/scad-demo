CREATE PROCEDURE infor.FailUnprocessedEvents
	@instanceId int
AS
BEGIN
	UPDATE dbo.IconItemChangeQueue
	SET ProcessFailedDate = GETDATE(),
		InProcessBy = NULL
	WHERE InProcessBy = @instanceId
		AND ProcessFailedDate IS NULL
END