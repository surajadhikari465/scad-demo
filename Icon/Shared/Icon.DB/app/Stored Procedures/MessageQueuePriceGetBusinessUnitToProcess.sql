create procedure app.MessageQueuePriceGetBusinessUnitToProcess
	@InstanceId	int
as
begin
	set nocount on

	declare @NextAvailableBusinessUnit int

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
