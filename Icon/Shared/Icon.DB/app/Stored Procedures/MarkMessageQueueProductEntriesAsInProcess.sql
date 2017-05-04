create procedure app.MarkMessageQueueProductEntriesAsInProcess
       @LookAhead				int, 
       @JobInstance				int,
	   @BusinessUnit			int
as
begin
	set nocount on;
	DECLARE @CurrentMessagesInProcess int
	DECLARE @NumberOfRows INT

	declare @ReadyStatus int = (select MessageStatusId from app.MessageStatus (NOLOCK) where MessageStatusName = 'Ready')
	SET @CurrentMessagesInProcess = (SELECT COUNT(*) FROM app.MessageQueueProduct WHERE InProcessBy = @JobInstance)


	IF( @CurrentMessagesInProcess < @LookAhead)
	BEGIN
		SET @NumberOfRows = @LookAhead - @CurrentMessagesInProcess

		;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueueProduct	with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance

	END 
end