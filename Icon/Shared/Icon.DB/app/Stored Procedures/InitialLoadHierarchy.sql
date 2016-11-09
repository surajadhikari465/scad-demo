
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-07-07
-- Description:	Used for initial load of the R10 
--				hierarchies.  Parameter @Level accepts
--				the numeric hierarchy level value.  
--				Parameter @HierarchyName takes the
--				hierarchy's name as represented in
--				the dbo.Hierarchy table.
-- =============================================

CREATE PROCEDURE [app].[InitialLoadHierarchy]
	@HierarchyName	nvarchar(32),
	@Level			int
AS
BEGIN
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
		case
			when @HierarchyName = 'Financial' then substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
			else cast(hc.hierarchyClassID as nvarchar(32)) 
		end,
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
		h.hierarchyName = @HierarchyName
		and hc.hierarchyLevel = @Level
END
GO
