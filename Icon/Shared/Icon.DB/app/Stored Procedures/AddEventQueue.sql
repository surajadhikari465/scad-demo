CREATE PROCEDURE [app].[AddEventQueue]
	@EventName NVARCHAR(255), 
    @EventQueueEntries [app].[EventQueueEntriesType] READONLY,
	@RegionList [app].[RegionAbbrType] READONLY

AS
BEGIN
    SET NOCOUNT ON;

	BEGIN TRY;

		BEGIN TRANSACTION;

		----------------------------------------------
		-- Verify the EventType.EventName
		----------------------------------------------
		DECLARE @EventId INT;
		SELECT 	@EventId = (SELECT [EventId] FROM [app].[EventType] WHERE [EventName] = @EventName);

		IF @EventId IS NULL
		BEGIN
			-- RAISERROR with severity 11-19 will cause execution to jump to the CATCH block.
			RAISERROR('No EventName found matching the @EventName ''%s''!', 16, 1, @EventName);
		END

		----------------------------------------------
		-- Queue the Event
		----------------------------------------------
		DECLARE @eventCount INT, @regionCount INT
		SELECT @eventCount = (SELECT COUNT(*) FROM @EventQueueEntries)
		SELECT @regionCount = (SELECT COUNT(*) FROM @RegionList)

		IF (@eventCount > 0 AND @regionCount > 0) -- EVENTS AND REGIONS TO QUEUE UP
		BEGIN

			INSERT INTO [app].[EventQueue] ([EventId], [EventMessage], [EventReferenceId], [RegionCode])
			SELECT @EventId, e.EventMessage, e.EventReferenceId, r.RegionAbbr
			FROM @EventQueueEntries e 
			CROSS APPLY @RegionList r

		END
		ELSE IF (@eventCount > 0 AND @regionCount = 0)-- EVENTS, BUT NO REGIONS --> USE THE NULL REGION
		BEGIN

			INSERT INTO [app].[EventQueue] ([EventId], [EventMessage], [EventReferenceId], [RegionCode])
			SELECT @EventId, e.EventMessage, e.EventReferenceId, NULL
			FROM @EventQueueEntries e 

		END
		ELSE IF (@eventCount = 0 AND @regionCount > 0)-- REGIONS, BUT NO EVENTS
		BEGIN

			INSERT INTO [app].[EventQueue] ([EventId], [EventMessage], [EventReferenceId], [RegionCode])
			SELECT @EventId, NULL, NULL, r.RegionAbbr
			FROM @RegionList r

		END
		-- ELSE -- NO EVENTS AND NO REGIONS DO NOTHING

		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH

		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		SET NOCOUNT OFF;

		RAISERROR (@ErrorMessage, -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH;

    SET NOCOUNT OFF;
END;