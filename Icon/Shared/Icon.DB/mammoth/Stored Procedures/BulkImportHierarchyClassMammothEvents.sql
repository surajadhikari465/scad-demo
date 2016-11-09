CREATE PROCEDURE [mammoth].[BulkImportHierarchyClassMammothEvents]
	@hierarchyClassList mammoth.BulkImportedHierarchyClassType READONLY,
	@hierarchyID int,
	@hierarchyLevel int,
	@eventMessage varchar(100)

AS
	DECLARE @distinctNames mammoth.BulkImportedHierarchyClassType			
	
	--Generate ProductAdd events for scan codes
	INSERT @distinctNames (HierarchyClassId, HierarchyClassName, MammothEventTypeId)
	SELECT DISTINCT HierarchyClassId, HierarchyClassName, MammothEventTypeId from @hierarchyClassList

	--Generate new hierarchy class add events
	INSERT INTO mammoth.EventQueue (EventTypeId
           ,EventReferenceId
           ,EventMessage
           ,InsertDate
           ,ProcessedFailedDate
           ,InProcessBy
           ,NumberOfRetry)
	SELECT MammothEventTypeId, dsc.hierarchyClassID, @eventMessage, sysdatetime(), null, null, null
	  FROM @distinctNames dsc
     WHERE dsc.HierarchyClassId is not null 
	   AND dsc.HierarchyClassId > 0
	
	--Generate hierarchy class update events
	INSERT INTO mammoth.EventQueue (EventTypeId
           ,EventReferenceId
           ,EventMessage
           ,InsertDate
           ,ProcessedFailedDate
           ,InProcessBy
           ,NumberOfRetry)
	SELECT MammothEventTypeId, hc.hierarchyClassID, @eventMessage, sysdatetime(), null, null, null
	  FROM @distinctNames dsc
	  JOIN HierarchyClass hc on dsc.HierarchyClassName = hc.hierarchyClassName
     WHERE hc.hierarchyID = @hierarchyID
	   AND hc.hierarchyLevel = @hierarchyLevel
	   AND (dsc.HierarchyClassId is null or dsc.HierarchyClassId = 0)