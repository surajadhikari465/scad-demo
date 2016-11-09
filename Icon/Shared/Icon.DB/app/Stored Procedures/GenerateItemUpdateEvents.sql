CREATE procedure [app].[GenerateItemUpdateEvents] 
	@updatedItemIDs app.UpdatedItemIDsType READONLY,
	@eventTypeName nvarchar(20) = 'Item Update'
as
	DECLARE @distinctItemIDs app.UpdatedItemIDsType,			
			@itemUpdateEventType int,
			@validationDateTraitID int,
			@itemUpdateSetting int,			
			@validatedItemIDs app.UpdatedItemIDsType;
			

	SET @itemUpdateEventType = (select EventId from app.EventType where EventName = @eventTypeName)
	SET @validationDateTraitID = (select traitID from Trait where traitCode = 'VAL')
	
	--Generate Item Update events for validated scan codes
	INSERT @distinctItemIDs 
	SELECT DISTINCT itemID from @updatedItemIDs

	INSERT INTO app.EventQueue
	SELECT @itemUpdateEventType, sc.scanCode, dii.itemID, iis.regionCode, sysdatetime(), null, null
	FROM @distinctItemIDs dii
	JOIN ScanCode sc
		ON sc.itemID = dii.itemID
	JOIN app.IRMAItemSubscription iis
		ON sc.scanCode = iis.identifier AND iis.deleteDate is NULL
	JOIN Item i
		ON sc.itemID = i.itemID
	JOIN ItemTrait it
		ON i.itemID = it.itemID
		AND it.traitID = @validationDateTraitID
