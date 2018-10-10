use Icon
go

declare @rowCount int
declare @financialHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Financial')
declare @financialHierarchyClassLevel int = (select hierarchyLevel from HierarchyPrototype where hierarchyID = @financialHierarchyId)
declare @posDepartmentNumberTraitId int = (select traitID from Trait where traitCode = 'PDN')
declare @teamNumberTraitId int = (select traitID from Trait where traitCode = 'NUM')
declare @teamNameTraitId int = (select traitID from Trait where traitCode = 'NAM')
declare @financialHierarchyCodeTraitId int = (select traitID from Trait where traitCode = 'FIN')
declare @hierarchyClassId int
declare @newSubTeamName nvarchar(64)
declare @newSubTeamHierarchyClassId int
declare @posDepartmentNumber nvarchar(32)
declare @financialHierarchyCode nvarchar(64)
declare @teamNumber nvarchar(32)
declare @teamName nvarchar(64)
declare @HierarchyMessageTypeId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy')
declare @ReadyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready')
declare	@ActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate')
declare @minInforFinancialHierarchyClassId int = 1000000
declare @maxInforFinancialHierarchyClassId int = 1999999


/* ------------------------------------------
	Insert the new Seasonal Goods subteam.

*/ ------------------------------------------
BEGIN
	set @newSubTeamName = 'Seasonal Goods (7100)'
	set @posDepartmentNumber = '222'
	set @teamNumber = '140'
	set @teamName = 'Specialty'
	set @financialHierarchyCode = '7100'
	set @hierarchyClassId = 
		(
			select 
				CASE 
					WHEN max(hierarchyClassID) IS NULL THEN @minInforFinancialHierarchyClassId
					ELSE max(hierarchyClassID) + 1
				END
			from HierarchyClass hc 
			where hc.hierarchyID = @financialHierarchyId
				and hc.hierarchyClassID between @minInforFinancialHierarchyClassId and @maxInforFinancialHierarchyClassId
		)
	
	set identity_insert HierarchyClass on

	insert into
		HierarchyClass(hierarchyClassID, hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
	values
		(@hierarchyClassID, @FinancialHierarchyClassLevel, @financialHierarchyId, null, @newSubTeamName)

	set @rowCount = @@ROWCOUNT
	print 'Inserted ' + CONVERT(nvarchar(8), @rowCount) + ' HierarchyClass record for the Seasonal Goods subteam.'

	set identity_insert HierarchyClass off

	set @newSubTeamHierarchyClassId = SCOPE_IDENTITY()

	insert into
		HierarchyClassTrait
	values
		(@posDepartmentNumberTraitId, @newSubTeamHierarchyClassId, null, @posDepartmentNumber),
		(@financialHierarchyCodeTraitId, @newSubTeamHierarchyClassId, null, @financialHierarchyCode),
		(@teamNumberTraitId, @newSubTeamHierarchyClassId, null, @teamNumber),
		(@teamNameTraitId, @newSubTeamHierarchyClassId, null, @teamName)

	set @rowCount = @@ROWCOUNT
	print 'Inserted ' + CONVERT(nvarchar(8), @rowCount) + ' HierarchyClassTrait records for the Seasonal Goods subteam.'
END


/* ------------------------------------------
	Queue up Seasonal Goods message for ESB.

*/ ------------------------------------------
BEGIN
	insert into app.MessageQueueHierarchy(MessageTypeId,
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
	select @HierarchyMessageTypeId,
		     @ReadyStatusId,
		     @ActionId,
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
    where h.hierarchyName = 'Financial'
		  and hc.hierarchyLevel = @financialHierarchyClassLevel
		  and hc.hierarchyClassName = @newSubTeamName

	set @rowCount = @@ROWCOUNT
	print 'Queued ' + CONVERT(nvarchar(8), @rowCount) + ' hierarchy messages for the Seasonal Goods subteam.'
END