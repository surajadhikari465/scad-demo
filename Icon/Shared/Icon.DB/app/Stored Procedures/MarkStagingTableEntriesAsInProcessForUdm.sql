
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-10-29
-- Description:	Called from the POS Push Controller.
--				Marks IRMAPush records as in process
--				for UDM processing.
-- =============================================

CREATE PROCEDURE [app].[MarkStagingTableEntriesAsInProcessForUdm]
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
			EsbReadyDate is not null and
			InUdmDate is null and
			UdmFailedDate is null			
		order by
			IRMAPushID
	)
	update Staging set InProcessBy = @JobInstance

END
