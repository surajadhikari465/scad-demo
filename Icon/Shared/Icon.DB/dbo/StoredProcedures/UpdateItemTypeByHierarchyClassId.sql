CREATE PROCEDURE [dbo].[UpdateItemTypeByHierarchyClassId]
	@hierarchyClassID int,
	@itemTypeId nvarchar(3),
	@userName nvarchar(255),
	@modifiedDateTimeUtc nvarchar(30)
AS

-- ***************************************************************
-- Get list of Items that are associated to the @hierarchyClassID
-- ***************************************************************
CREATE TABLE #itemAssociations (itemID int PRIMARY KEY);

INSERT INTO #itemAssociations (itemID)
SELECT
	ihc.itemID as itemID
FROM
	ItemHierarchyClass ihc
WHERE
	ihc.hierarchyClassID = @hierarchyClassID

-- ***************************************************************
-- Update ItemTypes, Modified Date Trait, and Modified User Trait
-- ***************************************************************
UPDATE Item
SET
	ItemTypeId = @itemTypeId,
	ItemAttributesJson = JSON_MODIFY(JSON_MODIFY(i.ItemAttributesJson,'$."ModifiedDateTimeUtc"',@modifiedDateTimeUtc),'$."ModifiedBy"',@userName)
FROM
	Item i
	JOIN #itemAssociations ia on i.itemID = ia.itemID