-- set variables
DECLARE @financialHierarchyId INT = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial');
DECLARE @messageTypeId INT = (SELECT MessageTypeId FROM app.MessageType WHERE MessageTypeName = 'Hierarchy');
DECLARE @messageStatusId INT = (SELECT MessageStatusId FROM app.MessageStatus WHERE MessageStatusName = 'Ready');
DECLARE @messageActionId INT = (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'AddOrUpdate');

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
    HierarchyParentClassId
)
SELECT
	@messageTypeId,
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
join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID
	and hc.hierarchyLevel = hp.hierarchyLevel
where h.hierarchyID = @financialHierarchyId