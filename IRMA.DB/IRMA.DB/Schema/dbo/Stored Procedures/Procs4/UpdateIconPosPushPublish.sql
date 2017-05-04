

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE [dbo].[UpdateIconPosPushPublish]
	@IconPublishEventList	dbo.IconPosPublishEventIdType	readonly,
	@ControllerInstance		varchar(32),
	@ProcessedDate			datetime2(7)
AS

BEGIN
	update
		IConPOSPushPublish
	set
		InProcessBy = @ControllerInstance,
		ProcessedDate = @ProcessedDate
	from
		@IconPublishEventList el
		join IConPOSPushPublish ippp on el.IconPosPushPublishId = ippp.IConPOSPushPublishID
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateIconPosPushPublish] TO [IConInterface]
    AS [dbo];

