-- =============================================
-- Author:		Cake
-- Create date: 2016-01-19
-- Description:	Returns the BusinessUnitId that should be
--				processed next in the given MessageQueue table.
-- =============================================

CREATE PROCEDURE [app].[MessageQueueGetBusinessUnitToProcess]
	@MessageQueueTable		nvarchar(100),
	@InstanceId				int
AS
BEGIN
	set nocount on

	declare @NextAvailableBusinessUnit int

	if (@MessageQueueTable = 'MessageQueuePrice')
		begin
			select top (1)
				@NextAvailableBusinessUnit = q.BusinessUnit_ID
			from 
				app.MessageQueuePrice q
				join app.MessageQueueBusinessUnitInProcess p on q.BusinessUnit_ID = p.BusinessUnitInProcess
			where q.InProcessBy = @InstanceId
				and p.ControllerInstanceId = @InstanceId

			if @NextAvailableBusinessUnit is null
				begin
					select top(1)
						@NextAvailableBusinessUnit = q.BusinessUnit_ID
					from
						app.MessageQueuePrice q
						left join app.MessageQueueBusinessUnitInProcess p on q.BusinessUnit_ID = p.BusinessUnitInProcess
					where
						p.BusinessUnitInProcess is null and
						q.InProcessBy is null and
						q.MessageStatusId = 1
					order by
						q.MessageQueueId
				end
			
			if @NextAvailableBusinessUnit is null
				begin
					select top(1)
						q.BusinessUnit_ID
					from
						app.MessageQueuePrice q
					where
						q.InProcessBy is null and
						q.MessageStatusId = 1
					order by
						q.MessageQueueId
				end
			else
				begin
					select @NextAvailableBusinessUnit
				end
		end

	if (@MessageQueueTable = 'MessageQueueItemLocale')
		begin
			select top (1)
				@NextAvailableBusinessUnit = q.BusinessUnit_ID
			from 
				app.MessageQueueItemLocale q
				join app.MessageQueueBusinessUnitInProcess p on q.BusinessUnit_ID = p.BusinessUnitInProcess
			where q.InProcessBy = @InstanceId
				and p.ControllerInstanceId = @InstanceId

			if @NextAvailableBusinessUnit is null
				begin
					select top(1)
						@NextAvailableBusinessUnit = q.BusinessUnit_ID
					from
						app.MessageQueueItemLocale q
						left join app.MessageQueueBusinessUnitInProcess p on q.BusinessUnit_ID = p.BusinessUnitInProcess
					where
						p.BusinessUnitInProcess is null and
						q.InProcessBy is null and
						q.MessageStatusId = 1
					order by
						q.MessageQueueId
				end
			
			if @NextAvailableBusinessUnit is null
				begin
					select top(1)
						q.BusinessUnit_ID
					from
						app.MessageQueueItemLocale q
					where
						q.InProcessBy is null and
						q.MessageStatusId = 1
					order by
						q.MessageQueueId
				end
			else
				begin
					select @NextAvailableBusinessUnit
				end
		end
END
