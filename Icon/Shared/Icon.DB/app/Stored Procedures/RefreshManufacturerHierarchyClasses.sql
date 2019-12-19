CREATE PROCEDURE [app].[RefreshManufacturerHierarchyClasses] @ids app.IntList readonly
AS
BEGIN
	DECLARE @taskName NVARCHAR(35) = 'RefreshManufacturerHierarchyClasses'

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for Manufacturer Hierarchy Classes...';

	DECLARE @HierarchyMessageId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Hierarchy'
			)
		,@ReadyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready'
			)
		,@ActionId INT = (
			SELECT MessageActionId
			FROM app.MessageAction
			WHERE MessageActionName = 'AddOrUpdate'
			)
		,@zipCodeId INT = (
			SELECT traitID
			FROM dbo.Trait
			WHERE traitCode = 'ZIP'
			)
		,@arCustomerId INT = (
			SELECT traitID
			FROM dbo.Trait
			WHERE traitCode = 'ARC'
			)

	INSERT INTO app.MessageQueueHierarchy (
		MessageTypeId
		,MessageStatusId
		,MessageActionId
		,HIERARCHYID
		,HierarchyName
		,HierarchyLevelName
		,ItemsAttached
		,HierarchyClassId
		,HierarchyClassName
		,HierarchyLevel
		,HierarchyParentClassId
		,ZipCode
		,ArCustomerId
		)
	SELECT @HierarchyMessageId
		,@ReadyStatusId
		,@ActionId
		,h.HIERARCHYID
		,h.hierarchyName
		,hp.hierarchyLevelName
		,hp.itemsAttached
		,cast(hc.hierarchyClassID AS NVARCHAR(32))
		,hc.hierarchyClassName
		,hc.hierarchyLevel
		,hc.hierarchyParentClassID
		,zc.TraitValue AS ZipCode
		,arc.TraitValue AS ArCustomerId
	FROM @ids ids
	JOIN HierarchyClass hc ON ids.I = hc.hierarchyClassID
	JOIN Hierarchy h ON hc.HIERARCHYID = h.HIERARCHYID
	JOIN HierarchyPrototype hp ON hc.HIERARCHYID = hp.HIERARCHYID
		AND hc.hierarchyLevel = hp.hierarchyLevel
	LEFT JOIN dbo.HierarchyClassTrait zc ON zc.hierarchyClassID = hc.HierarchyClassId
		AND zc.traitID = @zipCodeId
	LEFT JOIN dbo.HierarchyClassTrait arc ON arc.hierarchyClassID = hc.HierarchyClassId
		AND arc.traitID = @arCustomerId

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully refreshed Manufacturer Hierarchy Classes...';
END