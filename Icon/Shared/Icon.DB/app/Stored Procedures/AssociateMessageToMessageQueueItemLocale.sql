create procedure app.AssociateMessageToMessageQueueItemLocale
	@MessagesToUpdate app.MessageQueueType readonly,
	@MessageHistoryId int,
	@MessageStatusId int
as
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
				app.MessageQueueItemLocale	i
				join @MessagesToUpdate		m	on	i.MessageQueueId = m.MessageQueueId
		)	mq
end