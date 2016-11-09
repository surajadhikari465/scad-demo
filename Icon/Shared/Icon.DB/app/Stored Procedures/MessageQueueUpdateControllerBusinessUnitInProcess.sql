-- =============================================
-- Author:		Cake
-- Create date: 2016-01-19
-- Description:	Updates the API controller tracking table
--				with an instance ID and the business unit
--				in process.
-- =============================================

CREATE PROCEDURE app.MessageQueueUpdateControllerBusinessUnitInProcess
	@InstanceId int,
	@BusinessUnit int,
	@MessageTypeId int
AS
BEGIN
	set nocount on

    merge
		app.MessageQueueBusinessUnitInProcess with (updlock, rowlock) bu
	using
		(select @InstanceId, @MessageTypeId) as source(InstanceId, MessageTypeId)
	on
		bu.ControllerInstanceId = source.InstanceId
		and bu.ControllerTypeId = source.MessageTypeId
	when matched then
		update set bu.BusinessUnitInProcess = @BusinessUnit
	when not matched then
		insert (ControllerInstanceId, BusinessUnitInProcess, ControllerTypeId)
		values (@InstanceId, @BusinessUnit, @MessageTypeId)
	;
END
GO
