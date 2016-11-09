CREATE PROCEDURE [app].[RefreshBrands]
	@ids app.IntList readonly
AS
BEGIN
	DECLARE @taskName nvarchar(20) = 'RefreshBrands',
			@brandsHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands'),
			@brandEventTypeId int = (select EventId from EventType where EventName = 'Brand Name Update')

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for Brands...';

	DECLARE @hierarchyMessageTypeId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy'), 
			@readyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'), 
			@messageActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate'),
			@brandsHierarchyLevelName nvarchar(16) = (select hierarchyLevelName from HierarchyPrototype where hierarchyID = @brandsHierarchyId),
			@brandsItemsAttached bit = (select itemsAttached from HierarchyPrototype where hierarchyID = @brandsHierarchyId),
			@brandsHierarchyLevel int = (select hierarchyLevel from HierarchyPrototype where hierarchyID = @brandsHierarchyId),
			@brandsParentClassId int = null
			
	INSERT INTO app.MessageQueueHierarchy
	SELECT	MessageTypeId			= @hierarchyMessageTypeId,
			MessageStatusId			= @readyStatusId,
			MessageHistoryId		= null,
			MessageActionId			= @messageActionId,
			InsertDate				= sysdatetime(),
			HierarchyId				= @brandsHierarchyId,
			HierarchyName			= 'Brands',
			HierarchyLevelName		= @brandsHierarchyLevelName,
			ItemsAttached			= @brandsItemsAttached,
			HierarchyClassId		= hc.HierarchyClassId,
			HierarchyClassName		= hc.HierarchyClassName,
			HierarchyLevel			= @brandsHierarchyLevel,
			HierarchyParentClassId	= @brandsParentClassId,
			null,
			null
	FROM @ids ids
	JOIN HierarchyClass hc on ids.I = hc.hierarchyClassID
	WHERE hc.hierarchyID = @brandsHierarchyId

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding global events for Brands...';

	INSERT INTO app.EventQueue(EventId, EventMessage, EventReferenceId, RegionCode)
	SELECT @brandEventTypeId,
		hc.hierarchyClassName,
		hc.hierarchyClassId,
		r.RegionCode
	FROM @ids ids
	JOIN HierarchyClass hc on ids.I = hc.hierarchyClassID
	CROSS APPLY Regions r

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully refreshed Brands...';
END