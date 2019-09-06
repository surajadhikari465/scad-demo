CREATE PROCEDURE [infor].[HierarchyClassGenerateMessages]
  @hierarchyClasses infor.HierarchyClassType READONLY,
  @hierarchyClassTraits infor.HierarchyClassTraitType READONLY
AS
BEGIN
	--Generate Hierarchy Class messages to the ESB
	DECLARE @brandHierarchyId INT = (SELECT HierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Brands'),
          @merchHierarchyId INT = (SELECT HierarchyID	FROM dbo.Hierarchy WHERE hierarchyName = 'Merchandise'),
          @financialHierarchyId INT = (SELECT HierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial'),
          @nationalHierarchyId INT = (SELECT HierarchyID FROM dbo.Hierarchy	WHERE hierarchyName = 'National'),
          @readyMessageStatusId INT = (SELECT MessageStatusId	FROM app.MessageStatus WHERE MessageStatusName = 'Ready'),
          @hierarchyMessageTypeId INT = (SELECT MessageTypeId	FROM app.MessageType WHERE MessageTypeName = 'Hierarchy'),
          @deleteMessageActionId INT = (SELECT MessageActionId FROM app.MessageAction WHERE MessageActionName = 'Delete'),
          @nccId INT = (SELECT traitID FROM dbo.Trait	WHERE traitCode = 'NCC'),
		  @brandAbbreviationId INT = (SELECT traitID FROM dbo.Trait	WHERE traitCode = 'BA'),
		  @zipCodeId INT = (SELECT traitID FROM dbo.Trait	WHERE traitCode = 'ZIP'),
		  @designationId INT = (SELECT traitID FROM dbo.Trait	WHERE traitCode = 'GRD'),
		  @localityId INT = (SELECT traitID FROM dbo.Trait	WHERE traitCode = 'LCL');

  --Directed to us by the DBAs: we should dump any table type data into a tempdb table.
  SELECT *
  INTO #hierarchyClasses
  FROM @hierarchyClasses;

  SELECT *
  INTO #hierarchyClassesTraits
  FROM @hierarchyClassTraits;

	INSERT INTO app.MessageQueueHierarchy(
		MessageTypeId
		,MessageStatusId
		,MessageActionId
		,HierarchyID
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
		,Locality)
	SELECT @hierarchyMessageTypeId
		,@readyMessageStatusId
		,hc.ActionId
		,hc.HierarchyID
		,h.hierarchyName
		,hp.hierarchyLevelName
		,hp.itemsAttached
		,CASE WHEN hc.HierarchyID = @financialHierarchyId THEN SUBSTRING(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4) ELSE hc.HierarchyClassId END HierarchyClassId
		,hc.HierarchyClassName
		,hp.hierarchyLevel
		,hc.ParentHierarchyClassId
		,hct.TraitValue AS NationalClassCode
		,ba.TraitValue AS BrandAbbreviation
		,zc.TraitValue AS ZipCode
		,ds.TraitValue AS Designation
		,lo.TraitValue AS Locality
	FROM #hierarchyClasses hc
	JOIN dbo.Hierarchy h ON hc.HierarchyID = h.HierarchyID
	JOIN dbo.HierarchyPrototype hp ON hp.HierarchyID = hc.HierarchyID AND hp.hierarchyLevelName = hc.hierarchyLevelName
    LEFT JOIN #hierarchyClassesTraits hct ON hct.hierarchyClassID = hc.HierarchyClassId AND hct.traitID = @nccId
    LEFT JOIN dbo.HierarchyClassTrait ba ON ba.hierarchyClassID = hc.HierarchyClassId AND ba.traitID = @brandAbbreviationId
    LEFT JOIN dbo.HierarchyClassTrait zc ON zc.hierarchyClassID = hc.HierarchyClassId AND zc.traitID = @zipCodeId
    LEFT JOIN dbo.HierarchyClassTrait ds ON ds.hierarchyClassID = hc.HierarchyClassId AND ds.traitID = @designationId
    LEFT JOIN dbo.HierarchyClassTrait lo ON lo.hierarchyClassID = hc.HierarchyClassId AND lo.traitID = @localityId
	WHERE h.HierarchyID IN(
			@brandHierarchyId
			,@merchHierarchyId
			,@financialHierarchyId
			,@nationalHierarchyId);
END