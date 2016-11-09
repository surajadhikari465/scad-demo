
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-21
-- Description:	Used in the API Controller.  Updates
--				the ProcessedDate column for a given
--				set of queued messages.
-- =============================================

CREATE PROCEDURE [app].[UpdateMessageQueueProcessedDate]
       @MessageQueueTable	nvarchar(100),
       @MessagesToUpdate	app.MessageQueueType readonly,
       @ProcessedDate		datetime2
AS
BEGIN
	
	set nocount on;

	if (@MessageQueueTable = 'MessageQueuePrice')
		begin
			update
				mq
			set
				InProcessBy		= null,
				ProcessedDate	= @ProcessedDate
			from
				(
					select
						InProcessBy,
						ProcessedDate
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
				InProcessBy		= null,
				ProcessedDate	= @ProcessedDate
			from
				(
					select
						InProcessBy,
						ProcessedDate
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
				InProcessBy		= null,
				ProcessedDate	= @ProcessedDate
			from
				(
					select
						InProcessBy,
						ProcessedDate
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
				InProcessBy		= null,
				ProcessedDate	= @ProcessedDate
			from
				(
					select
						InProcessBy,
						ProcessedDate
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
				InProcessBy		= null,
				ProcessedDate	= @ProcessedDate
			from
				(
					select
						InProcessBy,
						ProcessedDate
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
				InProcessBy		= null,
				ProcessedDate	= @ProcessedDate
			from
				(
					select
						InProcessBy,
						ProcessedDate
					from
						app.MessageQueueProductSelectionGroup	p
						join @MessagesToUpdate					m	on	p.MessageQueueId = m.MessageQueueId
				)	mq
		end
    
END
GO
