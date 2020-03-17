DECLARE @scriptKey VARCHAR(128) = '32769_UpdateSubTeamName'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	
	DECLARE @hierarchyClassId INT = (SELECT TOP 1 hierarchyClassID FROM dbo.HierarchyClass WHERE hierarchyClassName = 'Prep Foods Production (4850)' AND hierarchyID = 5),
		@messageTypeId int   = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy'),
		@messageStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'),
		@messageActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate')
		
	UPDATE dbo.HierarchyClass
	SET hierarchyClassName = 'Misc. Third Party Vendors - PFDS (4850)'
	WHERE hierarchyClassID = @hierarchyClassId

	INSERT INTO app.MessageQueueHierarchy(
		MessageTypeId,
		MessageStatusId,
		MessageActionId,
		HierarchyId,
		HierarchyName,
		HierarchyLevelName,
		ItemsAttached,
		HierarchyClassId,
		HierarchyClassName,
		HierarchyLevel,
		HierarchyParentClassId)
	SELECT @messageTypeId,
		@messageStatusId,
		@messageActionId,
		h.hierarchyID,
		h.hierarchyName,
		hp.hierarchyLevelName,
		hp.itemsAttached,
		substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4),
		hc.hierarchyClassName,		
		hc.hierarchyLevel,
		hc.hierarchyParentClassID
	from Hierarchy h
	join HierarchyClass hc on h.hierarchyID = hc.hierarchyID
	join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID and hc.hierarchyLevel = hp.hierarchyLevel
	where hc.hierarchyClassID = @hierarchyClassId

	INSERT INTO app.PostDeploymentScriptHistory(ScriptKey, RunTime) VALUES (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO