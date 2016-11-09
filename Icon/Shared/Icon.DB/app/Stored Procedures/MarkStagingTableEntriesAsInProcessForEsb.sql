
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-29
-- Description:	Called from the POS Push Controller.
--				Marks IRMAPush records as in process
--				for ESB processing.
-- =============================================

CREATE PROCEDURE [app].[MarkStagingTableEntriesAsInProcessForEsb]
       @NumberOfRows		int,
	   @JobInstance			int
AS
BEGIN

	set nocount on;

	;with Staging as
	(
		select top(@NumberOfRows) 
			InProcessBy
		from
			app.IRMAPush with (rowlock, readpast, updlock)
		where
			InProcessBy is null and
			EsbReadyDate is null and
			EsbReadyFailedDate is null
		order by
			IRMAPushID
	)
	update Staging set InProcessBy = @JobInstance

END
