
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-21
-- Description:	Used in the API Controller to
--				associate a newly created Message
--				to the individual queue entries
--				that compose the message.
-- =============================================

CREATE PROCEDURE [app].[AssociateMessageToQueue]
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
						app.MessageQueueItemLocale	i
						join @MessagesToUpdate		m	on	i.MessageQueueId = m.MessageQueueId
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

	else if (@MessageQueueTable = 'MessageQueueHierarchy')
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

	else if (@MessageQueueTable = 'MessageQueueLocale')
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
						app.MessageQueueProductSelectionGroup		g
						join @MessagesToUpdate						m	on	g.MessageQueueId = m.MessageQueueId
				)	mq
		end
    
END
GO
