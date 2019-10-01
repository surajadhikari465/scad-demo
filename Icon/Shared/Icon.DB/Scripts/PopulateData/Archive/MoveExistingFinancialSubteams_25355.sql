SET NOCOUNT ON;
DECLARE @scriptKey VARCHAR(128) = 'MoveExistingFinancialSubteams_25355';

IF (NOT EXISTS (SELECT *FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	DECLARE @hierarchyId INT = (
			SELECT [HierarchyId]
			FROM Hierarchy
			WHERE HierarchyName = 'Financial');--5

	DECLARE @NumTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NUM');--TeamNumber: 63

	DECLARE @NamTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NAM') --TeamName: 64

	DECLARE @subteamIDs AS TABLE(hierarchyClassId INT);

	DECLARE @financialHierarchyClassLevel INT = (
		SELECT hierarchyLevel
		FROM HierarchyPrototype
		WHERE HIERARCHYID = @hierarchyId);

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

  INSERT INTO @subteamIDs VALUES(84348), (84349);

  BEGIN TRY
		BEGIN TRANSACTION;

	  UPDATE hct
	  SET traitValue = '220'
	  FROM HierarchyClassTrait hct
	  JOIN @subteamIDs id ON id.hierarchyClassId = hct.hierarchyClassID
	  WHERE hct.traitID = @NumTraitId;

	  INSERT INTO HierarchyClassTrait (
		  traitID
		  ,hierarchyClassID
		  ,uomID
		  ,traitValue)
	  SELECT @NumTraitId
		  ,id.hierarchyClassId
		  ,NULL
		  ,'220'
	  FROM @subteamIDs id
	  LEFT JOIN HierarchyClassTrait hct ON id.hierarchyClassId = hct.hierarchyClassID
		  AND hct.traitID = @NumTraitId
	  WHERE hct.traitID IS NULL;


	  UPDATE hct
	  SET traitValue = 'Bakery'
	  FROM HierarchyClassTrait hct
	  JOIN @subteamIDs id ON id.hierarchyClassId = hct.hierarchyClassID
	  WHERE hct.traitID = @NamTraitId;

	  INSERT INTO HierarchyClassTrait (
		  traitID
		  ,hierarchyClassID
		  ,uomID
		  ,traitValue)
	  SELECT @NamTraitId
		  ,id.hierarchyClassId
		  ,NULL
		  ,'Bakery'
	  FROM @subteamIDs id
	  LEFT JOIN HierarchyClassTrait hct ON id.hierarchyClassId = hct.hierarchyClassID
		  AND hct.traitID = @NamTraitId
	  WHERE hct.traitID IS NULL;

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
		  AND hc.hierarchyClassID IN(84348, 84349);

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