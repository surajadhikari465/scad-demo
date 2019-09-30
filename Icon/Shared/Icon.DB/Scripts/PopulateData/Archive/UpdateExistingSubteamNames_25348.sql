SET NOCOUNT ON;

DECLARE @scriptKey VARCHAR(128) = 'UpdateExistingSubteamNames_25348';

IF (NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	DECLARE @hierarchyId INT = (
			SELECT [HierarchyId]
			FROM Hierarchy
			WHERE HierarchyName = 'Financial');--5

	DECLARE @subteamIDs AS TABLE(hierarchyClassId INT, traitValue VARCHAR(255));

	DECLARE @financialHierarchyClassLevel INT = (
		SELECT hierarchyLevel
		FROM HierarchyPrototype
		WHERE [hierarchyID] = @hierarchyId);

	DECLARE @HierarchyMessageTypeId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Hierarchy');

	DECLARE @ReadyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready');

	DECLARE @ActionId INT = (
			SELECT MessageActionId
			FROM app.MessageAction
			WHERE MessageActionName = 'AddOrUpdate');

	BEGIN TRY
		BEGIN TRANSACTION

		UPDATE HierarchyClass
		SET hierarchyClassName = 'Bin Bulk (1400)'
		WHERE hierarchyClassID = 84228;

		UPDATE HierarchyClass
		SET hierarchyClassName = 'Baking, Meals, Essentials (1000)'
		WHERE hierarchyClassID = 84225;

		INSERT INTO @subteamIDs
		SELECT hierarchyClassID, traitValue
		FROM HierarchyClassTrait
		WHERE traitValue IN('1000', '1400');

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

		INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
			VALUES (@scriptKey , GETDATE());

		COMMIT TRANSACTION;

		PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Completed: ' + @scriptKey;
	END TRY
	BEGIN CATCH
		PRINT 'Error:';

		SELECT ERROR_NUMBER() AS ErrorNumber
			,ERROR_SEVERITY() AS ErrorSeverity
			,ERROR_STATE() AS ErrorState
			,ERROR_PROCEDURE() AS ErrorProcedure
			,ERROR_LINE() AS ErrorLine
			,ERROR_MESSAGE() AS ErrorMessage;

		ROLLBACK TRANSACTION;
	END CATCH
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Update data already applied: ' + @scriptKey;
END
GO