CREATE PROCEDURE [esb].[MarkMessageQueuePriceEntriesAsInProcess]
    @NumberOfRows			int, 
    @JobInstance			int
AS
BEGIN

	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from esb.MessageStatus where MessageStatusName = 'Ready')

	;with MessageQueue as
	(
		select 
			top(@NumberOfRows) InProcessBy 
		from 
			esb.MessageQueuePrice mq with (rowlock, readpast, updlock)
		where 
			MessageStatusId = @ReadyStatus and 
			InProcessBy is null 
		order by 
			MessageQueueId
	)
	update MessageQueue set InProcessBy = @JobInstance
END