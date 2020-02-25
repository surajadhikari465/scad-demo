CREATE PROCEDURE [app].[RefreshNationalHierarchyClasses]
	@classIds app.IntList readonly,
	@regions app.RegionAbbrType readonly 
AS
BEGIN
	DECLARE @taskName NVARCHAR(20) = 'RefreshNationalClasses'

	SELECT *
	INTO #ids
	FROM @classIds;

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for National Classes...';

	DECLARE @nationalEventTypeId INT = (
			SELECT EventId
			FROM app.EventType
			WHERE EventName = 'National Class Update'
			)
		,@hierarchyMessageTypeId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Hierarchy'
			)
		,@readyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready'
			)
		,@messageActionId INT = (
			SELECT MessageActionId
			FROM app.MessageAction
			WHERE MessageActionName = 'AddOrUpdate'
			)
		,@nccId INT = (
			SELECT TraitID
			FROM dbo.Trait
			WHERE traitCode = 'NCC'
			)

	SELECT RegionAbbr
		INTO #tempRegions
	FROM @regions

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
		,NationalClassCode
		)
	SELECT @hierarchyMessageTypeId
		,@readyStatusId
		,@messageActionId
		,h.HIERARCHYID
		,h.hierarchyName
		,hp.hierarchyLevelName
		,hp.itemsAttached
		,cast(hc.hierarchyClassID AS NVARCHAR(32))
		,hc.hierarchyClassName
		,hc.hierarchyLevel
		,hc.hierarchyParentClassID
		,hct.traitValue
	FROM #ids ids
	JOIN HierarchyClass hc ON ids.I = hc.hierarchyClassID
	JOIN Hierarchy h ON hc.HIERARCHYID = h.HIERARCHYID
	JOIN HierarchyPrototype hp ON hc.HIERARCHYID = hp.HIERARCHYID
		AND hc.hierarchyLevel = hp.hierarchyLevel
	JOIN HierarchyClassTrait hct ON hc.hierarchyClassID = hct.hierarchyClassID
		AND hct.traitID = @nccId

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully added National Hierarchy Classes to MessageQueueHierarchy...';

	INSERT INTO app.EventQueue (
		EventId
		,EventMessage
		,EventReferenceId
		,RegionCode
		)
	SELECT @nationalEventTypeId
		,hc.hierarchyClassName
		,hc.hierarchyClassId
		,r.RegionAbbr
	FROM #ids ids
	JOIN HierarchyClass hc ON ids.I = hc.hierarchyClassID
	CROSS APPLY #tempRegions r

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully added National Hierarchy Classes to EventQueue...';
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully refreshed National Hierarchy Classes...';
END
GO

