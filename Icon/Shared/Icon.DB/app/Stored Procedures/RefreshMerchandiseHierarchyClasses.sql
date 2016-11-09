CREATE PROCEDURE [app].[RefreshMerchandiseHierarchyClasses]
	@ids app.IntList readonly
AS
BEGIN
	DECLARE @taskName nvarchar(20) = 'RefreshMerchandiseHierarchyClasses'

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for Merchandise Hierarchy Classes...';

	declare
		@HierarchyMessageId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy'),
		@ReadyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'),
		@ActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate')
		
	insert into 
		app.MessageQueueHierarchy
	select
		@HierarchyMessageId,
		@ReadyStatusId,
		null,
		@ActionId,
		sysdatetime(),
		h.hierarchyID,
		h.hierarchyName,
		hp.hierarchyLevelName,
		hp.itemsAttached,
		cast(hc.hierarchyClassID as nvarchar(32)),
		hc.hierarchyClassName,		
		hc.hierarchyLevel,
		hc.hierarchyParentClassID,
		null,
		null
	from
		@ids ids
		join HierarchyClass hc on ids.I = hc.hierarchyClassID
		join Hierarchy h on hc.hierarchyID = h.hierarchyID
		join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID and hc.hierarchyLevel = hp.hierarchyLevel

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully refreshed Merchandise Hierarchy Classes...';
END