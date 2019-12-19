CREATE PROCEDURE app.GenerateItemUpdateEvents 
	@ItemIds app.IntList READONLY,
	@EventTypeName NVARCHAR(20) = 'Item Update'
AS
DECLARE @distinctItemIds app.IntList,
	@itemUpdateEventTypeId INT

SET @itemUpdateEventTypeId = (
		SELECT EventId
		FROM app.EventType
		WHERE EventName = @EventTypeName
		)

--Generate Item Update events
INSERT @distinctItemIds
SELECT DISTINCT I
FROM @ItemIds

INSERT INTO app.EventQueue (
	EventId,
	EventMessage,
	EventReferenceId,
	RegionCode,
	InsertDate,
	ProcessFailedDate,
	InProcessBy
	)
SELECT @itemUpdateEventTypeId,
	sc.scanCode,
	sc.itemID,
	iis.regionCode,
	SYSDATETIME(),
	NULL,
	NULL
FROM @distinctItemIds dii
INNER JOIN ScanCode sc ON sc.itemID = dii.I
INNER JOIN app.IRMAItemSubscription iis ON sc.scanCode = iis.identifier
	AND iis.deleteDate IS NULL
