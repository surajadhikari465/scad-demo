CREATE PROCEDURE [app].[GenerateItemUpdateMessagesByHierarchyClass]
	@hierarchyClassID int
AS

-- ***************************************************************
-- Get list of Items associated to the sub-brick @hierarchyClassID
-- We need to filter out Coupon Item Types
-- ***************************************************************

DECLARE @itemAssociations app.UpdatedItemIDsType

INSERT INTO @itemAssociations
SELECT
	ihc.itemID			as itemId
FROM
	ItemHierarchyClass	ihc
	JOIN Item			i	on ihc.itemID = i.itemID
	JOIN ItemType		t	on i.itemTypeID = t.itemTypeID
WHERE
	ihc.hierarchyClassID = @hierarchyClassID
	AND t.itemTypeCode <> 'CPN'
	AND t.itemTypeCode <> 'NRT'

EXEC app.GenerateItemUpdateMessages @updatedItemIDs = @itemAssociations
GO
