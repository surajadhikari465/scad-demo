CREATE PROCEDURE [esb].[AddMessageQueueItem] @MessageQueueItems esb.MessageQueueItemIdsType readonly
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#itemIDs') IS NOT NULL)
		DROP TABLE #itemIDs;

	CREATE TABLE #itemIDs (
		ItemId INT NOT NULL
		,EsbReadyDateTimeUtc DATETIME2(7) NOT NULL
		,InsertDateUtc DATETIME2(7) NOT NULL
		)

	INSERT INTO #itemIDs
	SELECT ItemId
		,EsbReadyDateTimeUtc
		,InsertDateUtc
	FROM @MessageQueueItems

	INSERT INTO esb.MessageQueueItem (
		EsbReadyDateTimeUtc
		,ItemId
		,InsertDateUtc
		)
	SELECT EsbReadyDateTimeUtc
		,#itemIDs.ItemID
		,InsertDateUtc
	FROM #itemIDs
	INNER JOIN Item i ON #itemIDs.itemID = i.itemID
	INNER JOIN ItemType t ON i.itemTypeID = t.itemTypeID
	WHERE t.itemTypeCode <> 'CPN'
END