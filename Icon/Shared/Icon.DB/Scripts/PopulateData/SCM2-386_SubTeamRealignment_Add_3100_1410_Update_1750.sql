SET NOCOUNT ON;

DECLARE @scriptKey VARCHAR(128) = 'SCM2-386_SubTeamRealignment_Add_3100_1410_Update_1750';

IF (NOT EXISTS (SELECT *FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	DECLARE @Error VARCHAR(2500) = NULL
	    ,@SubteamId INT
		,@HierarchyClassId INT
		,@HierarchyClassName NVARCHAR(255)
		,@PdnTraitValue NVARCHAR(255)
		,@NumTraitValue NVARCHAR(255)
		,@NamTraitValue NVARCHAR(255)
		,@FinTraitValue NVARCHAR(255);

	DECLARE @hierarchyId INT = (
			SELECT [HierarchyId]
			FROM Hierarchy
			WHERE HierarchyName = 'Financial'
			);--5
	DECLARE @pdnTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'PDN'
			);--PosDepartmentNumber: 62
	DECLARE @NumTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NUM'
			);--TeamNumber: 63
	DECLARE @NamTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NAM'
			) --TeamName: 64
	DECLARE @FinTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'FIN'
			);--Financial Hierarchy Code: 53
	DECLARE @subteamIDs AS TABLE (
		 hierarchyClassId INT
		,traitValue VARCHAR(255)
		);

	DECLARE @financialHierarchyClassLevel INT = (
		SELECT hierarchyLevel
		FROM HierarchyPrototype
		WHERE HIERARCHYID = @hierarchyId
		);
	DECLARE @HierarchyMessageTypeId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Hierarchy'
			);
	DECLARE @ReadyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready'
			);
	DECLARE @ActionId INT = (
			SELECT MessageActionId
			FROM app.MessageAction
			WHERE MessageActionName = 'AddOrUpdate'
			);

	UPDATE HierarchyClass
		SET hierarchyClassName = 'Misc. Third Party Vendors - PROD (1750)'
		WHERE hierarchyLevel = 1
			AND [HierarchyId] = @hierarchyId
			AND hierarchyClassName = 'Value Add (1750)';

	INSERT INTO @subteamIDs
	SELECT hierarchyClassID, traitValue
	FROM HierarchyClassTrait
	WHERE traitValue = '1750';

	-- Send out message for SubTeam rename
	INSERT INTO app.MessageQueueHierarchy (
		MessageTypeId
		,MessageStatusId
		,MessageHistoryId
		,MessageActionId
		,InsertDate
		,[HierarchyId]
		,HierarchyName
		,HierarchyLevelName
		,ItemsAttached
		,HierarchyClassId
		,HierarchyClassName
		,HierarchyLevel
		,HierarchyParentClassId
		,InProcessBy
		,ProcessedDate
		,NationalClassCode)
	SELECT @HierarchyMessageTypeId
		,@ReadyStatusId
		,NULL
		,@ActionId
		,GetDate()
		,h.hierarchyID
		,h.hierarchyName
		,hp.hierarchyLevelName
		,hp.itemsAttached
		,substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
		,hc.hierarchyClassName
		,hc.hierarchyLevel
		,hc.hierarchyParentClassID
		,NULL
		,NULL
		,NULL
	FROM Hierarchy h
	JOIN HierarchyClass hc ON h.hierarchyID = hc.hierarchyID
	JOIN HierarchyPrototype hp ON hc.hierarchyID = hp.hierarchyID
		AND hc.hierarchyLevel = hp.hierarchyLevel
	WHERE h.hierarchyID = @hierarchyId
		AND hc.hierarchyLevel = @financialHierarchyClassLevel
		AND hc.hierarchyClassID IN(SELECT hierarchyClassId from @subteamIDs);

	IF (OBJECT_ID('tempdb..#Subteams') IS NOT NULL)
	DROP TABLE #Subteams;
    BEGIN
	  CREATE TABLE #Subteams (
		SubteamId INT NOT NULL IDENTITY(1, 1)
		,HierarchyClassId INT
		,HierarchyClassName NVARCHAR(255)
		,PdnTraitValue NVARCHAR(255)
		,NumTraitValue NVARCHAR(255)
		,NamTraitValue NVARCHAR(255)
		,FinTraitValue NVARCHAR(255)
		);
	  INSERT INTO HierarchyClass (
		hierarchyLevel,
		hierarchyID,
		hierarchyParentClassID,
		hierarchyClassName
		)
		VALUES (
		1,
		@hierarchyId,
		NULL,
		'Misc. Third Party Vendors - WB (3100)'
		);
	  -- Capture the HierarchyClassID using SCOPE_IDENTITY() and pass into the insert statements into HierarchyClassTrait
	  SELECT @HierarchyClassId = SCOPE_IDENTITY();

	  INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue
		)
	  VALUES (
		@HierarchyClassId
		,'Misc. Third Party Vendors - WB (3100)'
		,'241'
		,'200'
		,'Whole Body'
		,'3100'
		);

	  INSERT INTO HierarchyClass (
		hierarchyLevel,
		hierarchyID,
		hierarchyParentClassID,
		hierarchyClassName
		)
		VALUES (
		1,
		@hierarchyId,
		NULL,
		'Misc. Third Party Vendors - GROC (1410)'
		);
	  -- Capture the HierarchyClassID using SCOPE_IDENTITY() and pass into the insert statements into HierarchyClassTrait
	  SELECT @HierarchyClassID = SCOPE_IDENTITY();

	  INSERT INTO #Subteams (
		HierarchyClassId
		,HierarchyClassName
		,PdnTraitValue
		,NumTraitValue
		,NamTraitValue
		,FinTraitValue
		)
	  VALUES (
		@HierarchyClassId
		,'Misc. Third Party Vendors - GROC (1410)'
		,'261'
		,'100'
		,'Grocery'
		,'1410'
		);
    END

    DECLARE cur CURSOR FOR SELECT 
        SubteamId
        ,HierarchyClassId
        ,HierarchyClassName
        ,PdnTraitValue
        ,NumTraitValue
        ,NamTraitValue
        ,FinTraitValue
    FROM #Subteams
    ORDER BY SubteamId;

    OPEN cur;

    FETCH NEXT FROM cur
    INTO @SubteamId
        ,@HierarchyClassId
        ,@HierarchyClassName
        ,@PdnTraitValue
        ,@NumTraitValue
        ,@NamTraitValue
        ,@FinTraitValue;

	WHILE @@Fetch_Status = 0
	BEGIN
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO dbo.HierarchyClassTrait (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@PdnTraitId
					,@HierarchyClassId
					,NULL
					,@PdnTraitValue
					);

				INSERT INTO dbo.HierarchyClassTrait (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@NumTraitId
					,@HierarchyClassId
					,NULL
					,@NumTraitValue
					);

				INSERT INTO [dbo].[HierarchyClassTrait] (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@FinTraitId
					,@HierarchyClassId
					,NULL
					,@FinTraitValue
					);

				INSERT INTO dbo.HierarchyClassTrait (
					traitID
					,hierarchyClassID
					,uomID
					,traitValue
					)
				VALUES (
					@NamTraitId
					,@HierarchyClassId
					,NULL
					,@NamTraitValue
					);

				INSERT INTO app.MessageQueueHierarchy (
					MessageTypeId
					,MessageStatusId
					,MessageHistoryId
					,MessageActionId
					,InsertDate
					,[HierarchyId]
					,HierarchyName
					,HierarchyLevelName
					,ItemsAttached
					,HierarchyClassId
					,HierarchyClassName
					,HierarchyLevel
					,HierarchyParentClassId
					,InProcessBy
					,ProcessedDate
					,NationalClassCode
					)
				SELECT @HierarchyMessageTypeId
					,@ReadyStatusId
					,NULL
					,@ActionId
					,GetDate()
					,h.HIERARCHYID
					,h.hierarchyName
					,hp.hierarchyLevelName
					,hp.itemsAttached
					,substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
					,hc.hierarchyClassName
					,hc.hierarchyLevel
					,hc.hierarchyParentClassID
					,NULL
					,NULL
					,NULL
				FROM Hierarchy h
				JOIN HierarchyClass hc ON h.HIERARCHYID = hc.HIERARCHYID
				JOIN HierarchyPrototype hp ON hc.HIERARCHYID = hp.HIERARCHYID
					AND hc.hierarchyLevel = hp.hierarchyLevel
				WHERE h.HIERARCHYID = @hierarchyId
					AND hc.hierarchyLevel = @financialHierarchyClassLevel
					AND hc.hierarchyClassName = @HierarchyClassName;

				COMMIT TRANSACTION;

				PRINT 'Hierarchy Class: ' + @HierarchyClassName + ' added.'
			END TRY

			BEGIN CATCH
				IF @@TRANCOUNT > 0
				BEGIN
					PRINT 'Error:';
					SET @Error = ERROR_MESSAGE();

					SELECT ERROR_NUMBER() AS ErrorNumber
						,ERROR_SEVERITY() AS ErrorSeverity
						,ERROR_STATE() AS ErrorState
						,ERROR_PROCEDURE() AS ErrorProcedure
						,ERROR_LINE() AS ErrorLine
						,@Error AS ErrorMessage;

					ROLLBACK TRANSACTION;
				END
			END CATCH;
		END

		FETCH NEXT FROM cur
		INTO @SubteamId
			,@HierarchyClassId
			,@HierarchyClassName
			,@PdnTraitValue
			,@NumTraitValue
			,@NamTraitValue
			,@FinTraitValue;
	END

	CLOSE cur;

	DEALLOCATE cur;

	IF(@Error IS NULL)
	    BEGIN
		    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
			VALUES (@scriptKey , GETDATE());

		    PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Completed: ' + @scriptKey;
	    END
	ELSE
	    BEGIN
		    PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Failed: ' + @scriptKey + ' Error = ' + @Error;
	    END	
    END
ELSE
    BEGIN
	    PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey;
    END
SET NOCOUNT OFF;
GO