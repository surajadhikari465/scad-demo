-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-03-02
-- Description:	Called from the API Controller.
--				Used for concurrency control when
--				processing the MessageHistory table.
-- =============================================

CREATE PROCEDURE [app].[MarkUnsentMessagesAsInProcess]
	@NumberOfRows	int,
	@MessageTypeId	int,
	@JobInstance	int
AS
BEGIN
	
	set nocount on

	declare @SentToEsbStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'SentToEsb');
	declare @SentToActiveMqStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'SentToActiveMq');
	declare @ReadyMessageStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')

	;with MessageHistory as
	(
		select top(@NumberOfRows) 
			InProcessBy
		from
			app.MessageHistory mh with (rowlock, readpast, updlock)
		where
			(mh.MessageStatusId = @ReadyMessageStatus or 
			mh.MessageStatusId = @SentToEsbStatus or 
			mh.MessageStatusId = @SentToActiveMqStatus) and
			mh.MessageTypeId = @MessageTypeId and
			mh.InProcessBy is null
		order by
			mh.MessageHistoryId
	)
	update MessageHistory set InProcessBy = @JobInstance

END
