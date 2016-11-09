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

	declare @ReadyMessageStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')

	;with MessageHistory as
	(
		select top(@NumberOfRows) 
			InProcessBy
		from
			app.MessageHistory mh with (rowlock, readpast, updlock)
		where
			mh.MessageStatusId = @ReadyMessageStatus and
			mh.MessageTypeId = @MessageTypeId and
			mh.InProcessBy is null
		order by
			mh.MessageHistoryId
	)
	update MessageHistory set InProcessBy = @JobInstance

END
