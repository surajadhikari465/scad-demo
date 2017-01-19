create procedure app.MarkMessageQueueItemLocaleEntriesAsInProcess
       @NumberOfRows			int, 
       @JobInstance				int,
	   @BusinessUnit			int
as
begin
	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')
	
	;with MessageQueue as
	(
		select 
			top(@NumberOfRows) InProcessBy 
		from 
			app.MessageQueueItemLocale mq with (rowlock, readpast, updlock)
		where 
			MessageStatusId = @ReadyStatus and 
			BusinessUnit_ID = @BusinessUnit and
			InProcessBy is null 
		order by 
			MessageQueueId
	)
	update MessageQueue set InProcessBy = @JobInstance
end