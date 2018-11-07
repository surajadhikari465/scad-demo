

-- =============================================
-- Author:		Kyle Milner
-- Create date: 11/02/2014
-- Description:	Called from the POS Push Controller.
--				Marks IConPOSPushPublish records as
--				in process for the given controller.
-- =============================================

CREATE PROCEDURE [dbo].[MarkPublishTableEntriesAsInProcess]
       @NumberOfRows		int,
	   @JobInstance			int
AS
BEGIN

	set nocount on

	;with Publish as
	(
		select top(@NumberOfRows) 
			InProcessBy
		from
			IConPOSPushPublish with (rowlock, readpast, updlock)
		where
			InProcessBy is null and
			ProcessedDate is null and
			ProcessingFailedDate is null
		order by
			IConPOSPushPublishID
	)
	update Publish set InProcessBy = @JobInstance

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarkPublishTableEntriesAsInProcess] TO [IConInterface]
    AS [dbo];

