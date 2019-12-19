CREATE PROCEDURE [dbo].[UpdateItemSubTeamByHierarchyClassId]
	@hierarchyClassID int,
	@subTeamHierarchyClassId int,
	@userName nvarchar(255),
	@modifiedDateTimeUtc nvarchar(30)
AS

-- ***************************************************************
-- Get list of Items that are associated to the @hierarchyClassID
-- ***************************************************************
DECLARE @financialHierarchyId int = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Financial');

CREATE TABLE #itemAssociations (itemID int PRIMARY KEY);

INSERT INTO #itemAssociations (itemID)
SELECT
	ihc.itemID as itemID
FROM
	ItemHierarchyClass ihc
WHERE
	ihc.hierarchyClassID = @hierarchyClassID

UPDATE ihc
SET ihc.hierarchyClassID = @subTeamHierarchyClassId
FROM ItemHierarchyClass ihc
JOIN #itemAssociations ia on ihc.itemID = ia.itemID
JOIN HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
	AND hc.hierarchyID = @financialHierarchyId

-- ***************************************************************
-- Update ItemTypes, Modified Date Trait, and Modified User Trait
-- ***************************************************************
UPDATE Item
SET
	ItemAttributesJson = JSON_MODIFY(JSON_MODIFY(i.ItemAttributesJson,'$."ModifiedDateTimeUtc"',@modifiedDateTimeUtc),'$."ModifiedBy"',@userName)
FROM
	Item i
	JOIN #itemAssociations ia on i.itemID = ia.itemID

