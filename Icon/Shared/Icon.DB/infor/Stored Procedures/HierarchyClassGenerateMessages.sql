CREATE PROCEDURE [infor].[HierarchyClassGenerateMessages]
	@hierarchyClasses infor.HierarchyClassType READONLY
AS
BEGIN
	--Generate Hierarchy Class messages to the ESB
  DECLARE @brandHierarchyId INT       = (select hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Brands'),
          @merchHierarchyId INT       = (select hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Merchandise'),
          @financialHierarchyId INT		= (select hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial'),
          @nationalHierarchyId INT    = (select hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'National'),
          @readyMessageStatusId INT   = (select MessageStatusId FROM app.MessageStatus WHERE MessageStatusName = 'Ready'),
          @hierarchyMessageTypeId INT	= (select MessageTypeId FROM app.MessageType WHERE MessageTypeName = 'Hierarchy'),
          @deleteMessageActionId INT  = (select MessageActionId FROM app.MessageAction WHERE MessageActionName = 'Delete')

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
                                        HierarchyParentClassId,
                                        NationalClassCode)
	  select @hierarchyMessageTypeId,
           @readyMessageStatusId,
           hc.ActionId,
           hc.HierarchyId,
           h.hierarchyName,
           hp.hierarchyLevelName,
           hp.itemsAttached,
           case when hc.HierarchyId = @financialHierarchyId then SUBSTRING(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
						    else hc.HierarchyClassId end HierarchyClassId,
           hc.HierarchyClassName,
           hp.hierarchyLevel,
           hc.ParentHierarchyClassId,
           case when Isnull(hc.hierarchyID, 0) = @nationalHierarchyId then D.traitValue else null end NationalClassCode
	  from @hierarchyClasses hc
		join dbo.Hierarchy h on hc.HierarchyId = h.hierarchyID
		join dbo.HierarchyPrototype hp on hp.hierarchyID = hc.HierarchyId and hp.hierarchyLevelName = hc.hierarchyLevelName
    left join dbo.HierarchyClassTrait D on D.hierarchyClassID = hc.HierarchyClassId
	WHERE h.HierarchyId in (@brandHierarchyId, @merchHierarchyId, @financialHierarchyId, @nationalHierarchyId)
END
GO