CREATE PROCEDURE [app].[GenerateItemUpdateMessagesByHierarchyClass]
	@hierarchyClassID int
AS

-- ***************************************************************
-- Get list of Items associated to the sub-brick @hierarchyClassID
-- We need to filter out Coupon Item Types
-- ***************************************************************

DECLARE @itemAssociations esb.MessageQueueItemIdsType
DECLARE @now DATETIME2(7) = SYSUTCDATETIME()

INSERT INTO @itemAssociations
SELECT
	ihc.itemID			AS itemId,
	@now				AS EsbReadyDateTimeUtc,
	@now				AS InsertDateUtc
FROM
	ItemHierarchyClass	ihc
	JOIN Item			i	ON ihc.itemID = i.itemID
	JOIN ItemType		t	ON i.itemTypeID = t.itemTypeID
WHERE
	ihc.hierarchyClassID = @hierarchyClassID
	AND t.itemTypeCode <> 'CPN'

EXEC esb.AddMessageQueueItem @MessageQueueItems = @itemAssociations
GO
