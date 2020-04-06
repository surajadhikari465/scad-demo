-- set variables
DECLARE @brandHierarchyId INT = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Brands');
DECLARE @messageTypeId INT = (SELECT MessageTypeId FROM app.MessageType WHERE MessageTypeName = 'Hierarchy');
DECLARE @messageStatusId INT = (SELECT MessageStatusId FROM app.MessageStatus WHERE MessageStatusName = 'Ready');
DECLARE @messageActionId INT = (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'AddOrUpdate');
DECLARE @zipCodeId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'ZIP'); -- Zip Code
DECLARE @brandAbbreviationId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'BA'); -- Brand Abbreviation
DECLARE @designationId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'GRD'); -- Designation
DECLARE @localityId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'LCL'); -- Locality
DECLARE @parentCompanyId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'PCO'); -- Parent Company

-- insert into queue table
INSERT INTO app.MessageQueueHierarchy
(
	MessageTypeId,
	MessageStatusId,
	MessageActionId,
	HierarchyId,
	HierarchyName,
	HierarchyLevelName,
	ItemsAttached,
	HierarchyClassId,
	HierarchyClassName,
	HierarchyLevel,
	HierarchyParentClassId,
	BrandAbbreviation,
	ZipCode,
	Designation,
	Locality,
	ParentCompany
)
SELECT
	@messageTypeId,
	@messageStatusId,
	@messageActionId,
	h.hierarchyID,
	h.hierarchyName,
	hp.hierarchyLevelName,
	hp.itemsAttached,
	hc.hierarchyClassID,
	hc.hierarchyClassName,		
	hc.hierarchyLevel,
	hc.hierarchyParentClassID,
	ba.TraitValue AS BrandAbbreviation,
	zc.TraitValue AS ZipCode,
	ds.TraitValue AS Designation,
	lo.TraitValue AS Locality,
	pa.TraitValue AS ParentCompany
FROM Hierarchy h
INNER JOIN HierarchyClass hc on h.hierarchyID = hc.hierarchyID
INNER JOIN HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID
	and hc.hierarchyLevel = hp.hierarchyLevel
LEFT JOIN dbo.HierarchyClassTrait ba ON ba.hierarchyClassID = hc.HierarchyClassId
	AND ba.traitID = @brandAbbreviationId
LEFT JOIN dbo.HierarchyClassTrait zc ON zc.hierarchyClassID = hc.HierarchyClassId
	AND zc.traitID = @zipCodeId
LEFT JOIN dbo.HierarchyClassTrait ds ON ds.hierarchyClassID = hc.HierarchyClassId
	AND ds.traitID = @designationId
LEFT JOIN dbo.HierarchyClassTrait lo ON lo.hierarchyClassID = hc.HierarchyClassId
	AND lo.traitID = @localityId
LEFT JOIN dbo.HierarchyClassTrait pa ON pa.hierarchyClassID = hc.HierarchyClassId
	AND pa.traitID = @parentCompanyId
WHERE hc.hierarchyID = @brandHierarchyId