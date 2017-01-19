create procedure app.AssociateMessageToMessageQueueLocale
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
				app.MessageQueueLocale		l
				join @MessagesToUpdate		m	on	l.MessageQueueId = m.MessageQueueId
		)	mq
end