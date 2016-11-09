-- Generates Item update events for all the items those associated with passed in hierachy class

create Procedure [app].[GenerateItemUpdateEventsForHierarchy]
 @hierarchyID int
	
AS
	DECLARE
		@itemUpdateEventType int,
		@validationDateTraitID int
			
	SET @itemUpdateEventType = (select EventId from app.EventType where EventName = 'Item Update')
	SET @validationDateTraitID = (select traitID from Trait where traitCode = 'VAL')
 
 	
	--Insert Events
	INSERT INTO app.EventQueue
	SELECT @itemUpdateEventType, sc.scanCode, sc.itemID, iis.regionCode, sysdatetime(), null, null
	FROM ScanCode sc	
	JOIN ItemHierarchyClass ihc 
		ON sc.itemID = ihc.itemID
		AND ihc.hierarchyClassID = @hierarchyID
	JOIN app.IRMAItemSubscription iis
		ON sc.scanCode = iis.identifier AND iis.deleteDate is NULL	
	JOIN ItemTrait it
		ON sc.itemID = it.itemID
		AND it.traitID = @validationDateTraitID

GO