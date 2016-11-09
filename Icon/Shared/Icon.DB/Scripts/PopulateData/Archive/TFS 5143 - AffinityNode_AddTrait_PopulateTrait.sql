-- ==================================================================
-- Date:		11/10/2014
-- TFS:			5143
-- Description:	This will add the 'Affinity' Trait to the 
--				HierarchyClassTrait for the HierarchyClass Sub-bricks
--				that are children of the 'Affinity Rewards' Brick.
--				For current simplicity, this will not be added as
--				a HierarchyClassTrait for all HierarchyClasses.
-- ==================================================================

-- ==================================================================
-- Variables
-- ==================================================================
DECLARE @hierarchyTraitGroupId int;
DECLARE @affinityTraitId int;

SET @hierarchyTraitGroupId = (SELECT traitGroupID FROM TraitGroup WHERE traitGroupDesc = 'Hierarchy Class Traits');
SET @affinityTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Affinity');

-- ==================================================================
-- Add Trait if it doesn't exist yet
-- ==================================================================
IF @affinityTraitId IS NULL
BEGIN
	SET IDENTITY_INSERT dbo.Trait ON

	SET @affinityTraitId = (SELECT max(traitID) FROM Trait) + 1;
	INSERT INTO Trait (traitID, traitCode, traitPattern, traitDesc, traitGroupID)
	VALUES (@affinityTraitId, 'AFF', '1', 'Affinity', @hierarchyTraitGroupId)

	SET IDENTITY_INSERT dbo.Trait OFF
END

-- ==================================================================
-- Add HierarchyClassTrait to the affinity sub-bricks (children of 'Affinity Rewards' Brick)
-- ==================================================================
INSERT INTO HierarchyClassTrait (hierarchyClassID, traitID, traitValue)
SELECT
	hc.hierarchyClassID		as hierarchyClassID,
	@affinityTraitId		as traitID,
	'1'						as traitValue
FROM
	HierarchyClass		hc
	JOIN Hierarchy		h	on hc.hierarchyID = h.hierarchyID
	JOIN HierarchyClass brk on hc.hierarchyParentClassID = brk.hierarchyClassID
WHERE
	brk.hierarchyClassName = 'Affinity Rewards'
	AND h.hierarchyName = 'Merchandise'
	AND hc.hierarchyLevel = 5