CREATE PROCEDURE [app].[UpdateTaxHierarchyClass] 
	@TaxHierarchyClasses app.TaxHierarchyClassUpdateType READONLY,  
	@Regions app.RegionAbbrType READONLY,
	@GenerateGlobalEvents bit
AS
BEGIN

	SET NOCOUNT ON;
	
	BEGIN TRY;

			BEGIN TRANSACTION;

			DECLARE @IdToNameMap as TABLE
			(
				hierarchyClassID int,
				cchHierarchyClassID nvarchar(7),
				oldHierarchyClassName nvarchar(255),
				newHierarchyClassName nvarchar(255)
			);

			DECLARE @UpdatedTaxHierarchyClasses as Table
			(
				hierarchyClassID int,
				cchHierarchyClassID nvarchar(7),
				hierarchyClassName nvarchar(255)
			);

			DECLARE @NewTaxHierarchyClasses AS TABLE
			(
				hierarchyClassID int,
				cchHierarchyClassID nvarchar(7),
				hierarchyClassName nvarchar(255),
				insertTaxAbbr bit
			);

			DECLARE @TaxHierarchyID int;
			SET @TaxHierarchyID = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Tax');

			----------------------------------------------------------------------
			-- Create a mapping between the Tax IDs and the HierarchyClass IDs
			----------------------------------------------------------------------
			INSERT	@IdToNameMap (hierarchyClassID, cchHierarchyClassID, oldHierarchyClassName, newHierarchyClassName)
			SELECT	hc.hierarchyClassID,
					thc.HierarchyClassID,
					hc.HierarchyClassName,
					thc.HierarchyClassName
			FROM	(SELECT hierarchyClassID, hierarchyClassName 
					 FROM dbo.HierarchyClass 
					 WHERE hierarchyId = @TaxHierarchyID) AS hc
			INNER JOIN @TaxHierarchyClasses thc ON hc.hierarchyClassName LIKE thc.HierarchyClassID + '%';

			----------------------------------------------------------------------
			-- Update current Tax HierarchyClasses
			----------------------------------------------------------------------
			INSERT @UpdatedTaxHierarchyClasses (hierarchyClassID, cchHierarchyClassID, hierarchyClassName)
			SELECT itnm.hierarchyClassID,
				   itnm.cchHierarchyClassID,
				   itnm.newHierarchyClassName
			FROM @IdToNameMap itnm
			WHERE itnm.oldHierarchyClassName <> itnm.newHierarchyClassName;
			
			UPDATE dbo.HierarchyClass
			SET hierarchyClassName = uthc.hierarchyClassName
			FROM dbo.HierarchyClass hc
			INNER JOIN @UpdatedTaxHierarchyClasses uthc
			ON hc.hierarchyClassID = uthc.hierarchyClassID;

			----------------------------------------------------------------------
			-- Add new Tax HierarchyClasses
			----------------------------------------------------------------------
			INSERT dbo.HierarchyClass ([hierarchyLevel], [hierarchyID], [hierarchyParentClassID], [hierarchyClassName])
				OUTPUT INSERTED.hierarchyClassID, NULL, INSERTED.hierarchyClassName, CASE WHEN LEN(INSERTED.hierarchyClassName) <= 50 THEN 1 ELSE 0 END
					INTO @NewTaxHierarchyClasses (hierarchyClassID, cchHierarchyClassID, hierarchyClassName, insertTaxAbbr)
			SELECT	1,
					@TaxHierarchyID,
					NULL,
					thc.hierarchyClassName 
			FROM @TaxHierarchyClasses thc
			WHERE thc.HierarchyClassName NOT IN (SELECT hierarchyClassName FROM @UpdatedTaxHierarchyClasses)
			AND thc.HierarchyClassName NOT IN (SELECT hierarchyClassName FROM dbo.[HierarchyClass]);

			-- Get the CCH hierarchyClassID to be used as the iCon.app.EventQueue.EventMessage
			UPDATE @NewTaxHierarchyClasses
			SET cchHierarchyClassID = thc.HierarchyClassID
			FROM @NewTaxHierarchyClasses nthc
			INNER JOIN @TaxHierarchyClasses thc ON nthc.hierarchyClassName = thc.HierarchyClassName

			--Insert the Tax abbreviation if the new tax hierarchyClassName is <= 50 characters
			DECLARE @taxAbbrTraitID INT
			SELECT @taxAbbrTraitID = (SELECT traitID from [dbo].[Trait] WHERE [TraitDesc] = 'Tax Abbreviation')
			
			INSERT INTO dbo.HierarchyClassTrait (traitID, hierarchyClassID, uomID, traitValue)
			SELECT @taxAbbrTraitID, nthc.hierarchyClassID, null, nthc.hierarchyClassName
			FROM @NewTaxHierarchyClasses nthc
			WHERE nthc.insertTaxAbbr = 1
			AND NOT EXISTS (SELECT 1 FROM dbo.hierarchyClassTrait hct WHERE nthc.hierarchyClassID = hct.hierarchyClassID  and hct.traitID = @taxAbbrTraitID)

			---insert tax romance if not exists for new tax classes			
			DECLARE @taxRomanceTraitID INT
			SELECT @taxRomanceTraitID = (SELECT traitID from [dbo].[Trait] WHERE TraitCode = 'TRM')
			
			INSERT INTO dbo.HierarchyClassTrait (traitID, hierarchyClassID, uomID, traitValue)
			SELECT @taxRomanceTraitID, nthc.hierarchyClassID, null, nthc.hierarchyClassName
			FROM @NewTaxHierarchyClasses nthc
			WHERE nthc.insertTaxAbbr = 1
			AND NOT EXISTS (SELECT 1 FROM dbo.hierarchyClassTrait hct WHERE nthc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @taxRomanceTraitID)

			----------------------------------------------------------------------
			-- Add New Tax HierarchyClass events in EventQueue
			----------------------------------------------------------------------
			IF (@GenerateGlobalEvents = 1)
			BEGIN		
				DECLARE @NewTaxEventQueueEntries app.EventQueueEntriesType;
				INSERT @NewTaxEventQueueEntries (EventMessage, EventReferenceId)
				SELECT cchHierarchyClassID, hierarchyClassID
				FROM @NewTaxHierarchyClasses
				WHERE insertTaxAbbr = 1;
			
				IF ((SELECT COUNT(*) FROM @NewTaxHierarchyClasses) > 0)
				BEGIN
					EXEC app.AddEventQueue 
						@EventName = 'New Tax Hierarchy',
						@EventQueueEntries  = @NewTaxEventQueueEntries,
						@RegionList = @Regions;
				END
			END
			
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