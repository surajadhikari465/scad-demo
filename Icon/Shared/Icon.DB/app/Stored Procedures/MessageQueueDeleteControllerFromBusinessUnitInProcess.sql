-- =============================================
-- Author:		Cake
-- Create date: 2016-01-19
-- Description:	Removes a controller instance ID from
--				the API controller tracking table.
-- =============================================

CREATE PROCEDURE app.MessageQueueDeleteControllerFromBusinessUnitInProcess
	@InstanceId int,
	@MessageTypeId int
AS
BEGIN
	set nocount on

    delete from app.MessageQueueBusinessUnitInProcess 
	where ControllerInstanceId = @InstanceId 
		and ControllerTypeId = @MessageTypeId
END
GO
