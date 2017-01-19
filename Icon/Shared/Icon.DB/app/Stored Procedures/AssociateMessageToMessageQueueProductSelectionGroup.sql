create procedure app.AssociateMessageToMessageQueueProductSelectionGroup
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
				app.MessageQueueProductSelectionGroup		g
				join @MessagesToUpdate						m	on	g.MessageQueueId = m.MessageQueueId
		)	mq
end