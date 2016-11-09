-- ===============================================================
-- Create date: 2014-08-22
-- Description:	
--		Updates the itemTypeID for every item associated to the 
--		hierarchyClassID that is supplied.
-- ===============================================================

CREATE PROCEDURE [app].[UpdateItemTypeByHierarchyClass]
	@hierarchyClassID int,
	@itemTypeCode nvarchar(3),
	@userName nvarchar(255)
AS

-- ***************************************************************
-- Get itemTypeID based on @itemTypeCode
-- ***************************************************************
DECLARE @itemTypeId int;
SET @itemTypeId = (SELECT itemTypeID FROM ItemType t WHERE t.itemTypeCode = @itemTypeCode);

-- ***************************************************************
-- Get list of Items that are associated to the @hierarchyClassID
-- ***************************************************************
DECLARE @itemAssociations TABLE
(
	itemID int primary key
)

INSERT INTO @itemAssociations
SELECT
	ihc.itemID
FROM
	ItemHierarchyClass ihc
WHERE
	ihc.hierarchyClassID = @hierarchyClassID

-- ***************************************************************
-- Update ItemTypes, Modified Date Trait, and Modified User Trait
-- ***************************************************************
UPDATE Item
SET
	itemTypeID = @itemTypeId
FROM
	Item i
	JOIN @itemAssociations ia on i.itemID = ia.itemID


DECLARE @now nvarchar(27);
SET @now = CONVERT(nvarchar(27), SYSDATETIME(), 121);

UPDATE ItemTrait
SET
	traitValue = @now
FROM
	ItemTrait				it
	JOIN @itemAssociations	ia	on it.itemID = ia.itemID
	JOIN Trait				t	on it.traitID = t.traitID
WHERE
	t.traitCode = 'MOD' -- Modified Date

UPDATE ItemTrait
SET
	traitValue = @userName
FROM
	ItemTrait				it
	JOIN @itemAssociations	ia	on it.itemID = ia.itemID
	JOIN Trait				t	on it.traitID = t.traitID
WHERE
	t.traitCode = 'USR' -- Modified User
GO
