CREATE PROCEDURE [mammoth].[InsertMammothEvent]
    @eventTypeId int,
	@eventReferenceId int,
	@eventMessage nvarchar(100) = null

AS
	--Generate Mammoth events
	INSERT INTO mammoth.EventQueue (EventTypeId
           ,EventReferenceId
           ,EventMessage
           ,InsertDate
           ,ProcessedFailedDate
           ,InProcessBy
           ,NumberOfRetry)
	SELECT @eventTypeId, @eventReferenceId, @eventMessage, sysdatetime(), null, null, null