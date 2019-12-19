CREATE PROCEDURE [infor].[FinalizeNewItemEvents]
	@queueIds infor.NewItemEventType READONLY,
	@instanceId int,
	@errorOccurred bit
AS
BEGIN
	IF @errorOccurred = 1
	BEGIN
		UPDATE dbo.IconItemChangeQueue
		SET ProcessFailedDate = GETDATE(),
			InProcessBy = NULL
		WHERE InProcessBy = @instanceId
			AND QID IN
			(
				SELECT QueueId FROM @queueIds
			)
	END
	ELSE 
	BEGIN 
		DELETE dbo.IconItemChangeQueue
		WHERE InProcessBy = @instanceId
			AND QID IN
			(
				SELECT QueueId FROM @queueIds
			)
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[infor].[FinalizeNewItemEvents] TO [IConInterface]
    AS [dbo];

