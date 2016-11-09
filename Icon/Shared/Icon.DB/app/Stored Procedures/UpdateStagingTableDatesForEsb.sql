
-- =============================================
-- Author:		Kyle Milner
-- Create date: 10/29/2014
-- Description:	Called from the POS Push Controller.
--				Sets the EsbReadyDate or the EsbReadyFailedDate
--				for a group of IRMAPush records.
-- =============================================

CREATE PROCEDURE [app].[UpdateStagingTableDatesForEsb]
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
					EsbReadyDate,
					InProcessBy
				from
					@RecordsToUpdate	r
					join app.IRMAPush	ip	on	r.IrmaPushId = ip.IRMAPushID
				where
					EsbReadyFailedDate is null
			)

			update Staging set EsbReadyDate = @Date, InProcessBy = null
		end

	else
		begin
			;with Staging as
			(
				select
					EsbReadyFailedDate,
					InProcessBy
				from
					@RecordsToUpdate	r
					join app.IRMAPush	ip	on	r.IrmaPushId = ip.IRMAPushID
			)

			update Staging set EsbReadyFailedDate = @Date, InProcessBy = null
		end

END
GO
