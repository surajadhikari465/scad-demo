ALTER PROCEDURE [app].[MarkMessageQueueEntriesAsInProcess]
       @MessageQueueTable		nvarchar(100),
       @NumberOfRows			int, 
       @JobInstance				nvarchar(30),
	   @ConfiguredBusinessUnits	app.BusinessUnitType readonly
AS
BEGIN

	set nocount on;

	set transaction isolation level serializable

	-- Concurrency logic:
	-- 1) First, un-mark any rows that may have been previously marked by the instance but remain unfinished.
	-- 2) Mark as in process any rows that are in Ready status and within the provided limit.

	declare @ReadyStatus int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')

	if (@MessageQueueTable = 'MessageQueuePrice')
		begin
			update app.MessageQueuePrice set InProcessBy = null where InProcessBy = @JobInstance

			begin transaction

				;with MessageQueue as
				(
					select 
						top(@NumberOfRows) InProcessBy 
					from 
						app.MessageQueuePrice			mq
						join @ConfiguredBusinessUnits	bu	on	mq.BusinessUnit_ID = bu.BusinessUnitId 
					where 
						MessageStatusId = @ReadyStatus and 
						InProcessBy is null 
					order by 
						MessageQueueId
				)
				update MessageQueue set InProcessBy = @JobInstance 

			commit transaction
		end

	else if (@MessageQueueTable = 'MessageQueueItemLocale')
		begin
			update app.MessageQueueItemLocale set InProcessBy = null where InProcessBy = @JobInstance
				
			begin transaction

				;with MessageQueue as
				(
					select 
						top(@NumberOfRows) InProcessBy 
					from 
						app.MessageQueueItemLocale		mq
						join @ConfiguredBusinessUnits	bu	on	mq.BusinessUnit_ID = bu.BusinessUnitId 
					where 
						MessageStatusId = @ReadyStatus and 
						InProcessBy is null 
					order by 
						MessageQueueId
				)
				update MessageQueue set InProcessBy = @JobInstance 

			commit transaction
		end

	else if (@MessageQueueTable = 'MessageQueueProduct')
		begin
			update app.MessageQueueProduct set InProcessBy = null where InProcessBy = @JobInstance
				
			begin transaction

				;with MessageQueue as
				(
					select 
						top(@NumberOfRows) InProcessBy 
					from 
						app.MessageQueueProduct
					where 
						MessageStatusId = @ReadyStatus and 
						InProcessBy is null 
					order by 
						MessageQueueId
				)
				update MessageQueue set InProcessBy = @JobInstance 

			commit transaction
		end

	else if (@MessageQueueTable = 'MessageQueueProductSelectionGroup')
		begin
			update app.MessageQueueProductSelectionGroup set InProcessBy = null where InProcessBy = @JobInstance
				
			begin transaction

				;with MessageQueue as
				(
					select 
						top(@NumberOfRows) InProcessBy 
					from 
						app.MessageQueueProductSelectionGroup
					where 
						MessageStatusId = @ReadyStatus and 
						InProcessBy is null 
					order by 
						MessageQueueId
				)
				update MessageQueue set InProcessBy = @JobInstance 

			commit transaction
		end

END
