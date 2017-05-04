create procedure app.AssociateMessageToMessageQueueProduct
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
				app.MessageQueueProduct		p
				join @MessagesToUpdate		m	on	p.MessageQueueId = m.MessageQueueId
		)	mq
end