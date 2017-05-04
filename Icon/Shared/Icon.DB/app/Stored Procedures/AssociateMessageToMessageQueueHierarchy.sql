create procedure app.AssociateMessageToMessageQueueHierarchy
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
				app.MessageQueueHierarchy	h
				join @MessagesToUpdate		m	on	h.MessageQueueId = m.MessageQueueId
		)	mq
end