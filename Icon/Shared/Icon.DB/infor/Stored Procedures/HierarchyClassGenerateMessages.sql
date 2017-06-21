CREATE PROCEDURE [infor].[HierarchyClassGenerateMessages]
	@hierarchyClasses infor.HierarchyClassType READONLY
AS
BEGIN
	--Generate Hierarchy Class messages to the ESB
	DECLARE @brandHierarchyId INT			= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Brands'),
			@merchHierarchyId INT			= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Merchandise'),
			@financialHierarchyId INT		= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial'),
			@readyMessageStatusId INT		= (SELECT MessageStatusId FROM app.MessageStatus WHERE MessageStatusName = 'Ready'),
			@hierarchyMessageTypeId INT		= (SELECT MessageTypeId FROM app.MessageType WHERE MessageTypeName = 'Hierarchy'),
			@deleteMessageActionId INT		= (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'Delete')

	INSERT INTO
		app.MessageQueueHierarchy
	SELECT
		MessageTypeId			= @hierarchyMessageTypeId,
		MessageStatusId			= @readyMessageStatusId,
		MessageHistoryId		= null,
		MessageActionId			= hc.ActionId,
		InsertDate				= sysdatetime(),
		HierarchyId				= hc.HierarchyId,
		HierarchyName			= h.hierarchyName,
		HierarchyLevelName		= hp.hierarchyLevelName,
		ItemsAttached			= hp.itemsAttached,
		HierarchyClassId		= CASE 
										WHEN hc.HierarchyId = @financialHierarchyId THEN SUBSTRING(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
										ELSE hc.HierarchyClassId
								  END,
		HierarchyClassName		= hc.HierarchyClassName,
		HierarchyLevel			= hp.hierarchyLevel,
		HierarchyParentClassId	= hc.ParentHierarchyClassId,
		null,
		null
	FROM
		@hierarchyClasses hc
		JOIN dbo.Hierarchy h on hc.HierarchyId = h.hierarchyID
		JOIN dbo.HierarchyPrototype hp on hp.hierarchyID = hc.HierarchyId
			and hp.hierarchyLevelName = hc.hierarchyLevelName
	WHERE h.HierarchyId in (@brandHierarchyId, @merchHierarchyId, @financialHierarchyId)
END
GO