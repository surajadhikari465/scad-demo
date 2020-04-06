-- set variables
DECLARE @manufactureHierarchyId INT = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Manufacturer');
DECLARE @messageTypeId INT = (SELECT MessageTypeId FROM app.MessageType WHERE MessageTypeName = 'Hierarchy');
DECLARE @messageStatusId INT = (SELECT MessageStatusId FROM app.MessageStatus WHERE MessageStatusName = 'Ready');
DECLARE @messageActionId INT = (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'AddOrUpdate');
DECLARE @zipCodeId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'ZIP'); -- Zip Code
DECLARE @arCustomerId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'ARC'); -- AR Customer Id

-- insert into queue table
INSERT INTO app.MessageQueueHierarchy
(
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
	HierarchyParentClassId,
	ZipCode,
	ArCustomerId
)
SELECT
	@messageTypeId,
	@messageStatusId,
	@messageActionId,
	h.hierarchyID,
	h.hierarchyName,
	hp.hierarchyLevelName,
	hp.itemsAttached,
	hc.hierarchyClassID,
	hc.hierarchyClassName,		
	hc.hierarchyLevel,
	hc.hierarchyParentClassID,
	zc.TraitValue AS ZipCode,
	arc.TraitValue AS ArCustomerId
FROM Hierarchy h
INNER JOIN HierarchyClass hc on h.hierarchyID = hc.hierarchyID
INNER JOIN HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID
	AND hc.hierarchyLevel = hp.hierarchyLevel
LEFT JOIN dbo.HierarchyClassTrait zc ON zc.hierarchyClassID = hc.HierarchyClassId
	AND zc.traitID = @zipCodeId
LEFT JOIN dbo.HierarchyClassTrait arc ON arc.hierarchyClassID = hc.HierarchyClassId
	AND arc.traitID = @arCustomerId
WHERE hc.hierarchyID = @manufactureHierarchyId