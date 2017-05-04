create procedure app.MarkMessageQueuePriceEntriesAsInProcess
       @LookAhead				int, 
       @JobInstance				int,
	   @BusinessUnit			int
as
begin
	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')
	DECLARE @CurrentMessagesInProcess int
	DECLARE @NumberOfRows INT

	SET @CurrentMessagesInProcess = (SELECT COUNT(*) FROM app.MessageQueuePrice (NOLOCK) WHERE InProcessBy = @JobInstance)

	IF( @CurrentMessagesInProcess < @LookAhead)
	BEGIN
		SET @NumberOfRows = @LookAhead - @CurrentMessagesInProcess
		;with MessageQueue as
		(
			select 
				top(@NumberOfRows) InProcessBy 
			from 
				app.MessageQueuePrice mq with (rowlock, readpast, updlock)					
			where 
				MessageStatusId = @ReadyStatus and 
				BusinessUnit_ID = @BusinessUnit and
				InProcessBy is null 
			order by 
				MessageQueueId
		)
		update MessageQueue set InProcessBy = @JobInstance
END
end