use Icon
go

declare @rowCount int
declare @financialHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Financial')
declare @financialHierarchyClassLevel int = (select hierarchyLevel from HierarchyPrototype where hierarchyID = @financialHierarchyId)
declare @newSubTeamName nvarchar(64)
declare @newSubteamHierarchyClassId int
declare @HierarchyMessageId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy')
declare	@ReadyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')
declare @AddOrUpdateActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate')
declare	@DeleteActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'Delete')


/* ------------------------------------------
	Queue up a message and remove the
	Seasonal Goods subteam.

*/ ------------------------------------------
BEGIN
	set @newSubTeamName = 'Seasonal Goods (7100)'
	set @newSubteamHierarchyClassId = (select hierarchyClassID from HierarchyClass where hierarchyClassName = @newSubTeamName and hierarchyID = @financialHierarchyId)

	insert into 
		app.MessageQueueHierarchy
	select
		@HierarchyMessageId,
		@ReadyStatusId,
		null,
		@DeleteActionId,
		sysdatetime(),
		h.hierarchyID,
		h.hierarchyName,
		hp.hierarchyLevelName,
		hp.itemsAttached,
		substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4),
		hc.hierarchyClassName,
		hc.hierarchyLevel,
		hc.hierarchyParentClassID,
		null,
		null
	from
		Hierarchy h
		join HierarchyClass hc on h.hierarchyID = hc.hierarchyID
		join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID and hc.hierarchyLevel = hp.hierarchyLevel
    where
		h.hierarchyName = 'Financial'
		and hc.hierarchyLevel = @financialHierarchyClassLevel
		and hc.hierarchyClassName = @newSubTeamName

	set @rowCount = @@ROWCOUNT
	print 'Queued ' + CONVERT(nvarchar(8), @rowCount) + ' hierarchy Delete message for the Seasonal Good subteam.'
	

	delete from HierarchyClassTrait where hierarchyClassID = @newSubteamHierarchyClassId
		
	set @rowCount = @@ROWCOUNT
	print 'Deleted ' + CONVERT(nvarchar(8), @rowCount) + ' HierarchyClassTrait records for the Seasonal Good subteam.'


	delete from HierarchyClass where hierarchyClassID = @newSubteamHierarchyClassId

	set @rowCount = @@ROWCOUNT
	print 'Deleted ' + CONVERT(nvarchar(8), @rowCount) + ' HierarchyClass record for the Seasonal Good subteam.'								
END