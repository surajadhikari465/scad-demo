ALTER PROCEDURE [app].[AssociateMessageToQueue]
       @MessageQueueTable	nvarchar(100),
       @MessagesToUpdate	app.MessageQueueType readonly,
       @MessageHistoryId	int,
	   @MessageStatusId		int
AS
BEGIN
	
	set nocount on;

	if (@MessageQueueTable = 'MessageQueuePrice')
		begin
			update
				mq
			set
				MessageHistoryId = @MessageHistoryId,
				MessageStatusId = @MessageStatusId
			from
				(
					select
						MessageHistoryId,
						MessageStatusId
					from
						app.MessageQueuePrice		r
						join @MessagesToUpdate		m	on	r.MessageQueueId = m.MessageQueueId
				)	mq
		end

	else if (@MessageQueueTable = 'MessageQueueItemLocale')
		begin
			update
				mq
			set
				MessageHistoryId = @MessageHistoryId,
				MessageStatusId = @MessageStatusId
			from
				(
					select
						MessageHistoryId,
						MessageStatusId
					from
						app.MessageQueueItemLocale	il
						join @MessagesToUpdate		m	on	il.MessageQueueId = m.MessageQueueId
				)	mq
		end

	else if (@MessageQueueTable = 'MessageQueueProduct')
		begin
			update
				mq
			set
				MessageHistoryId = @MessageHistoryId,
				MessageStatusId = @MessageStatusId
			from
				(
					select
						MessageHistoryId,
						MessageStatusId
					from
						app.MessageQueueProduct		p
						join @MessagesToUpdate		m	on	p.MessageQueueId = m.MessageQueueId
				)	mq
		end

	else if (@MessageQueueTable = 'MessageQueueProductSelectionGroup')
		begin
			update
				mq
			set
				MessageHistoryId = @MessageHistoryId,
				MessageStatusId = @MessageStatusId
			from
				(
					select
						MessageHistoryId,
						MessageStatusId
					from
						app.MessageQueueProductSelectionGroup		p
						join @MessagesToUpdate						m	on	p.MessageQueueId = m.MessageQueueId
				)	mq
		end
    
END