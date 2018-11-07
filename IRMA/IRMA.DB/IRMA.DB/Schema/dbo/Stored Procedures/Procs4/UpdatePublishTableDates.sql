
CREATE PROCEDURE [dbo].[UpdatePublishTableDates]
       @ProcessedSuccessfully	bit,
	   @RecordsToUpdate			dbo.IconPosPublishEventIdType readonly,
       @Date					datetime2
AS
BEGIN

	set nocount on;

	if (@ProcessedSuccessfully = 1)
		begin
			;with Publish as
			(
				select
					ProcessedDate,
					InProcessBy
				from
					@RecordsToUpdate		r
					join IConPOSPushPublish	pub	on	r.IconPosPushPublishId = pub.IConPOSPushPublishID
				where
					ProcessingFailedDate is null
			)

			update Publish set ProcessedDate = @Date, InProcessBy = null
		end

	else
		begin
			;with Publish as
			(
				select
					ProcessingFailedDate,
					InProcessBy
				from
					@RecordsToUpdate		r
					join IConPOSPushPublish	pub	on	r.IconPosPushPublishId = pub.IConPOSPushPublishID
			)

			update Publish set ProcessingFailedDate = @Date, InProcessBy = null
		end

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdatePublishTableDates] TO [IConInterface]
    AS [dbo];

