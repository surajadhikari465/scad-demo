CREATE PROCEDURE [esb].[MarkMessageQueueEntriesAsInProcess]
	@MessageQueueTable		nvarchar(100),
    @NumberOfRows			int, 
    @JobInstance			int
AS
BEGIN

	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'Ready')

	if (@MessageQueueTable = 'MessageQueuePrice')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					esb.MessageQueuePrice			mq	with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end

	else if (@MessageQueueTable = 'MessageQueueItemLocale')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					esb.MessageQueueItemLocale		mq	with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end
END