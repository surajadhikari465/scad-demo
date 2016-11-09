CREATE PROCEDURE [app].[GetCertificationAgencies]
AS
BEGIN
-- ==================================================
-- Variables
-- ==================================================
DECLARE @agencyHierarchyId int;
DECLARE @glutenFreeTraitId int;
DECLARE @kosherTraitId int;
DECLARE @nonGMOTraitId int;
DECLARE @organicTraitId int;
DECLARE @veganTraitId int;
DECLARE @defaultCerficationAgencyTraitId int;
DECLARE @organicTraitValue varchar(50);

SET @agencyHierarchyId = (SELECT hierarchyID FROM Hierarchy WHERE hierarchyName = 'Certification Agency Management');
SET @glutenFreeTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Gluten Free');
SET @kosherTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Kosher');
SET @nonGMOTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Non GMO');
SET @organicTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Organic');
SET @veganTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Vegan');
SET @defaultCerficationAgencyTraitId = (SELECT traitID FROM Trait WHERE traitDesc = 'Default Certification Agency');
SET @organicTraitValue = 'Organic'

-- ==================================================
-- Main
-- ==================================================
SELECT
	hc.hierarchyClassID			as HierarchyClassID,
	hc.hierarchyClassName		as HierarchyClassName,
	hc.hierarchyID				as HierarchyID,
	hc.hierarchyLevel			as HierarchyLevel,
	hc.hierarchyParentClassId	as HierarchyParentClassId,
	ISNULL(hctg.traitValue,0)	as GlutenFree,
	ISNULL(hctk.traitValue,0)	as Kosher,
	ISNULL(hctn.traitValue,0)	as NonGMO,
	ISNULL(hcto.traitValue,0)	as Organic,
	ISNULL(hctv.traitValue,0)	as Vegan,
	CASE 
	WHEN htdo.hierarchyClassID is null THEN '0'
	ELSE '1'
	END as DefaultOrganic

	
FROM
	HierarchyClass hc
	LEFT JOIN HierarchyClassTrait hctg on hc.hierarchyClassID = hctg.hierarchyClassID
										 AND hctg.traitID = @glutenFreeTraitId
	LEFT JOIN HierarchyClassTrait hctk on hc.hierarchyClassID = hctk.hierarchyClassID
										 AND hctk.traitID = @kosherTraitId
	LEFT JOIN HierarchyClassTrait hctn on hc.hierarchyClassID = hctn.hierarchyClassID
										 AND hctn.traitID = @nonGMOTraitId
	LEFT JOIN HierarchyClassTrait hcto on hc.hierarchyClassID = hcto.hierarchyClassID
										 AND hcto.traitID = @organicTraitId
	LEFT JOIN HierarchyClassTrait hctv on hc.hierarchyClassID = hctv.hierarchyClassID
										 AND hctv.traitID = @veganTraitId
	LEFT JOIN HierarchyClassTrait htdo on hc.hierarchyClassID = htdo.hierarchyClassID
										 AND htdo.traitID = @defaultCerficationAgencyTraitId
										 AND htdo.traitValue = @organicTraitValue

WHERE
	hc.hierarchyID = @agencyHierarchyId
ORDER BY
	hc.hierarchyClassName
END