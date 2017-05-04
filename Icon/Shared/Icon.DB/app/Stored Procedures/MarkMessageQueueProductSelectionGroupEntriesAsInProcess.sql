create procedure app.MarkMessageQueueProductSelectionGroupEntriesAsInProcess
       @LookAhead				int, 
       @JobInstance				int,
	   @BusinessUnit			int
as
begin
	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')
	DECLARE @CurrentMessagesInProcess int
	DECLARE @NumberOfRows INT

	SET @CurrentMessagesInProcess = (SELECT COUNT(*) FROM app.MessageQueueProductSelectionGroup (NOLOCK) WHERE InProcessBy = @JobInstance)

	IF( @CurrentMessagesInProcess < @LookAhead)
	BEGIN
		SET @NumberOfRows = @LookAhead - @CurrentMessagesInProcess
	
		;with MessageQueue as
		(
			select 
				top(@NumberOfRows) InProcessBy 
			from 
				app.MessageQueueProductSelectionGroup with (rowlock, readpast, updlock)
			where 
				MessageStatusId = @ReadyStatus and 
				InProcessBy is null 
			order by 
				MessageQueueId
		)
		update MessageQueue set InProcessBy = @JobInstance
	end
end