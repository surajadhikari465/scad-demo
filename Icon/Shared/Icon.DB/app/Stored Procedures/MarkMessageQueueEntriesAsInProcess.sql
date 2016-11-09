
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-21
-- Description:	Used in the API Controller.  Marks
--				a set of queued messages as in 
--				process for a particular controller.
-- =============================================

CREATE PROCEDURE [app].[MarkMessageQueueEntriesAsInProcess]
       @MessageQueueTable		nvarchar(100),
       @NumberOfRows			int, 
       @JobInstance				int,
	   @BusinessUnit			int
AS
BEGIN

	set nocount on;

	declare @ReadyStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')

	if (@MessageQueueTable = 'MessageQueuePrice')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueuePrice mq with (rowlock, readpast, updlock)					
				where 
					MessageStatusId = @ReadyStatus and 
					BusinessUnit_ID = @BusinessUnit and
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end

	else if (@MessageQueueTable = 'MessageQueueItemLocale')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueueItemLocale mq with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					BusinessUnit_ID = @BusinessUnit and
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end

	else if (@MessageQueueTable = 'MessageQueueProduct')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueueProduct	with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end

	else if (@MessageQueueTable = 'MessageQueueHierarchy')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueueHierarchy with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end

	else if (@MessageQueueTable = 'MessageQueueLocale')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueueLocale with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance
			
		end

	else if (@MessageQueueTable = 'MessageQueueProductSelectionGroup')
		begin

			;with MessageQueue as
			(
				select 
					top(@NumberOfRows) InProcessBy 
				from 
					app.MessageQueueProductSelectionGroup with (rowlock, readpast, updlock)
				where 
					MessageStatusId = @ReadyStatus and 
					InProcessBy is null 
				order by 
					MessageQueueId
			)
			update MessageQueue set InProcessBy = @JobInstance

		end

END
