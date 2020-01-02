DECLARE @manufactureHierarchyId INT = (
		SELECT HIERARCHYID
		FROM dbo.Hierarchy
		WHERE hierarchyName = 'Manufacturer'
		)
	,@brandsHierarchyId INT = (
		SELECT HIERARCHYID
		FROM dbo.Hierarchy
		WHERE hierarchyName = 'Brands'
		)
DECLARE @HierarchyMessageId INT = (
		SELECT MessageTypeId
		FROM app.MessageType
		WHERE MessageTypeName = 'Hierarchy'
		)
	,@ReadyStatusId INT = (
		SELECT MessageStatusId
		FROM app.MessageStatus
		WHERE MessageStatusName = 'Ready'
		)
	,@ActionId INT = (
		SELECT MessageActionId
		FROM app.MessageAction
		WHERE MessageActionName = 'AddOrUpdate'
		)
	,@zipCodeId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'ZIP'
		)
	,@arCustomerId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'ARC'
		)
	,@nccId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'NCC'
		)
	,@brandAbbreviationId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'BA'
		)
	,@designationId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'GRD'
		)
	,@localityId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'LCL'
		)
	,@brandsHierarchyLevelName NVARCHAR(16) = (
		SELECT hierarchyLevelName
		FROM HierarchyPrototype
		WHERE HIERARCHYID = @brandsHierarchyId
		)
	,@brandsItemsAttached BIT = (
		SELECT itemsAttached
		FROM HierarchyPrototype
		WHERE HIERARCHYID = @brandsHierarchyId
		)
	,@brandsHierarchyLevel INT = (
		SELECT hierarchyLevel
		FROM HierarchyPrototype
		WHERE HIERARCHYID = @brandsHierarchyId
		)
	,@parentCompanyId INT = (
		SELECT traitID
		FROM dbo.Trait
		WHERE traitCode = 'PCO'
		);

INSERT INTO app.MessageQueueHierarchy (
	MessageTypeId
	,MessageStatusId
	,MessageActionId
	,HIERARCHYID
	,HierarchyName
	,HierarchyLevelName
	,ItemsAttached
	,HierarchyClassId
	,HierarchyClassName
	,HierarchyLevel
	,HierarchyParentClassId
	,ZipCode
	,ArCustomerId
	)
SELECT @HierarchyMessageId
	,@ReadyStatusId
	,@ActionId
	,h.HIERARCHYID
	,h.hierarchyName
	,hp.hierarchyLevelName
	,hp.itemsAttached
	,cast(hc.hierarchyClassID AS NVARCHAR(32))
	,hc.hierarchyClassName
	,hc.hierarchyLevel
	,hc.hierarchyParentClassID
	,zc.TraitValue AS ZipCode
	,arc.TraitValue AS ArCustomerId
FROM HierarchyClass hc
JOIN Hierarchy h ON hc.HIERARCHYID = h.HIERARCHYID
JOIN HierarchyPrototype hp ON hc.HIERARCHYID = hp.HIERARCHYID
	AND hc.hierarchyLevel = hp.hierarchyLevel
LEFT JOIN dbo.HierarchyClassTrait zc ON zc.hierarchyClassID = hc.HierarchyClassId
	AND zc.traitID = @zipCodeId
LEFT JOIN dbo.HierarchyClassTrait arc ON arc.hierarchyClassID = hc.HierarchyClassId
	AND arc.traitID = @arCustomerId
WHERE hc.HIERARCHYID = @ManufactureHierarchyId

INSERT INTO app.MessageQueueHierarchy (
	MessageTypeId
	,MessageStatusId
	,MessageActionId
	,HIERARCHYID
	,HierarchyName
	,HierarchyLevelName
	,ItemsAttached
	,HierarchyClassId
	,HierarchyClassName
	,HierarchyLevel
	,HierarchyParentClassId
	,NationalClassCode
	,BrandAbbreviation
	,ZipCode
	,Designation
	,Locality
	,ParentCompany
	)
SELECT @HierarchyMessageId
	,@readyStatusId
	,@ActionId
	,@brandsHierarchyId
	,'Brands'
	,@brandsHierarchyLevelName
	,@brandsItemsAttached
	,hc.HierarchyClassId
	,hc.HierarchyClassName
	,@brandsHierarchyLevel
	,NULL
	,hct.TraitValue AS NationalClassCode
	,ba.TraitValue AS BrandAbbreviation
	,zc.TraitValue AS ZipCode
	,ds.TraitValue AS Designation
	,lo.TraitValue AS Locality
	,pa.TraitValue AS ParentCompany
FROM HierarchyClass hc
LEFT JOIN HierarchyClassTrait hct ON hct.hierarchyClassID = hc.HierarchyClassId
	AND hct.traitID = @nccId
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
WHERE hc.HIERARCHYID = @brandsHierarchyId