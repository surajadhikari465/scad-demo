create procedure app.AssociateMessageToMessageQueuePrice
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
				app.MessageQueuePrice		r
				join @MessagesToUpdate		m	on	r.MessageQueueId = m.MessageQueueId
		)	mq
end