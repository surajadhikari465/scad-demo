CREATE PROCEDURE [app].[RefreshBrands] @ids app.IntList readonly
AS
BEGIN
	DECLARE @taskName NVARCHAR(20) = 'RefreshBrands'
		,@brandsHierarchyId INT = (
			SELECT HIERARCHYID
			FROM Hierarchy
			WHERE hierarchyName = 'Brands'
			)
		,@brandEventTypeId INT = (
			SELECT EventId
			FROM EventType
			WHERE EventName = 'Brand Name Update'
			)

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for Brands...';

	DECLARE @hierarchyMessageTypeId INT = (
			SELECT MessageTypeId
			FROM app.MessageType
			WHERE MessageTypeName = 'Hierarchy'
			)
		,@readyStatusId INT = (
			SELECT MessageStatusId
			FROM app.MessageStatus
			WHERE MessageStatusName = 'Ready'
			)
		,@messageActionId INT = (
			SELECT MessageActionId
			FROM app.MessageAction
			WHERE MessageActionName = 'AddOrUpdate'
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
		,@brandsParentClassId INT = NULL
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
		,@zipCodeId INT = (
			SELECT traitID
			FROM dbo.Trait
			WHERE traitCode = 'ZIP'
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
		,NationalClassCode
		,BrandAbbreviation
		,ZipCode
		,Designation
		,Locality
		,ParentCompany
		)
	SELECT @hierarchyMessageTypeId
		,@readyStatusId
		,@messageActionId
		,@brandsHierarchyId
		,'Brands'
		,@brandsHierarchyLevelName
		,@brandsItemsAttached
		,hc.HierarchyClassId
		,hc.HierarchyClassName
		,@brandsHierarchyLevel
		,@brandsParentClassId
		,hct.TraitValue AS NationalClassCode
		,ba.TraitValue AS BrandAbbreviation
		,zc.TraitValue AS ZipCode
		,ds.TraitValue AS Designation
		,lo.TraitValue AS Locality
		,pa.TraitValue AS ParentCompany
	FROM @ids ids
	INNER JOIN HierarchyClass hc ON ids.I = hc.hierarchyClassID
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

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding global events for Brands...';

	INSERT INTO app.EventQueue (
		EventId
		,EventMessage
		,EventReferenceId
		,RegionCode
		)
	SELECT @brandEventTypeId
		,hc.hierarchyClassName
		,hc.hierarchyClassId
		,r.RegionCode
	FROM @ids ids
	INNER JOIN HierarchyClass hc ON ids.I = hc.hierarchyClassID
	CROSS APPLY Regions r

	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Successfully refreshed Brands...';
END