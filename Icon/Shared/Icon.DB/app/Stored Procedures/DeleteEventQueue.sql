-- =============================================
-- Author:		Benjamin Sims
-- Create date: 2014-10-20
-- Description:	Deletes app.EventQueue Rows
--				(called by Global Controller)
-- =============================================

CREATE PROCEDURE [app].[DeleteEventQueue]
	@EventsToDelete app.EventQueueIdType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

	BEGIN TRY
		BEGIN TRAN
		
		--=======================================================
		-- Delete rows
		--=======================================================
		DELETE eq
		FROM
			app.EventQueue				eq
			INNER JOIN @EventsToDelete	e	on eq.QueueId = e.QueueId

		COMMIT TRAN
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRAN
		THROW;
	END CATCH
END
GO