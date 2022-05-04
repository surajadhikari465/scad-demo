CREATE PROCEDURE [esb].[MarkMessageQueueItemLocaleEntriesAsInProcess]
    @NumberOfRows			int, 
    @JobInstance			int
AS
BEGIN

	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'Ready')
	declare @SentToEsb int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'SentToEsb')
	declare @SentToActiveMq int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'SentToActiveMq')

	;with MessageQueue as
	(
		select 
			top(@NumberOfRows) InProcessBy 
		from 
			esb.MessageQueueItemLocale mq with (rowlock, readpast, updlock)
		where 
			( MessageStatusId = @ReadyStatus or
			MessageStatusId = @SentToEsb or
			MessageStatusId = @SentToActiveMq ) and
			InProcessBy is null 
		order by 
			MessageQueueId
	)
	update MessageQueue set InProcessBy = @JobInstance
END