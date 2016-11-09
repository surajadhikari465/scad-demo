
-- =============================================
-- Author:		Kyle Milner
-- Create date: 10/29/2014
-- Description:	Called from the POS Push Controller.
--				Sets the InUdmDate or the UdmFailedDate
--				for a group of IRMAPush records.
-- =============================================

CREATE PROCEDURE [app].[UpdateStagingTableDatesForUdm]
       @ProcessedSuccessfully	bit,
	   @RecordsToUpdate			app.IrmaPushIdType readonly,
       @Date					datetime2
AS
BEGIN

	set nocount on;

	if (@ProcessedSuccessfully = 1)
		begin
			;with Staging as
			(
				select
					InUdmDate,
					InProcessBy
				from
					@RecordsToUpdate	r
					join app.IRMAPush	ip	on	r.IrmaPushId = ip.IRMAPushID
				where
					UdmFailedDate is null
			)

			update Staging set InUdmDate = @Date, InProcessBy = null
		end

	else
		begin
			;with Staging as
			(
				select
					UdmFailedDate,
					InProcessBy
				from
					@RecordsToUpdate	r
					join app.IRMAPush	ip	on	r.IrmaPushId = ip.IRMAPushID
			)

			update Staging set UdmFailedDate = @Date, InProcessBy = null
		end

END
GO
