-- =============================================
-- Author:		Min Zhao
-- Create date: 2014-10-15
-- Description:	Bulk insert EventQueue entities.
-- =============================================
CREATE PROCEDURE [app].[InsertEventQueueEntries]
	@EventQueueEntries app.EventQueueType readonly
AS
BEGIN	
	insert into [app].[EventQueue]
        ([EventId]
		,[EventMessage]
        ,[EventReferenceId]
		,[RegionCode]
		,[InsertDate]) 
	select 
		[EventId],
		[EventMessage],
		[EventReferenceId],
		[RegionCode], 
		GetDate() 
	from @EventQueueEntries
END

GO
