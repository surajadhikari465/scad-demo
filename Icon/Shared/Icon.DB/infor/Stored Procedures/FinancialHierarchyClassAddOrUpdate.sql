CREATE PROCEDURE infor.FinancialHierarchyClassAddOrUpdate
	@hierarchyClasses infor.HierarchyClassType READONLY,
	@hierarchyClassTraits infor.HierarchyClassTraitType READONLY
	AS
BEGIN

	DECLARE @financialHierarchyId INT = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial')
	DECLARE @finTraitId INT = (SELECT TOP 1 TraitID FROM dbo.Trait WHERE TraitCode = 'FIN')

	SELECT hierarchyLevel, 
		   hc.HierarchyId as [HierarchyId], 
		   ParentHierarchyClassId, 
		   HierarchyClassName,
		   hct.hierarchyClassId AS HierarchyClassId,
		   hc.SequenceId,
		   hc.InforMessageId
	INTO #tmpFinancialClass
	FROM @hierarchyClasses hc
	JOIN dbo.HierarchyPrototype hp ON hc.HierarchyLevelName = hp.HierarchyLevelName
	LEFT JOIN dbo.HierarchyClassTrait hct ON CAST(hc.HierarchyClassId AS NVARCHAR(255)) = hct.traitValue 
											 AND hct.traitID = @finTraitId
	WHERE hc.HierarchyId = @financialHierarchyId

	-- Update class name if we find a match based on HierarchyClassId
	UPDATE dbo.HierarchyClass
	SET HierarchyClassName =  tmpFinancialClass.HierarchyClassName
	FROM #tmpFinancialClass tmpFinancialClass
	WHERE dbo.HierarchyClass.hierarchyClassID = tmpFinancialClass.HierarchyClassId
	     AND tmpFinancialClass.HierarchyClassId IS NOT NULL

	--If not match found then insert into HierarchyClass table from #tmpFinancialClass Table
	INSERT INTO dbo.HierarchyClass
			(
			  hierarchyLevel, 
			  hierarchyID, 
			  hierarchyParentClassID, 
			  hierarchyClassName
			)
	SELECT hierarchyLevel, 
		   [HierarchyId],
		   ParentHierarchyClassId,
		   HierarchyClassName  
	FROM #tmpFinancialClass
	WHERE #tmpFinancialClass.HierarchyClassId IS NULL

	SELECT hct.HierarchyClassID AS PassedHierarchyClassID, 
		   TraitId,
		   TraitValue, 
		   hc.hierarchyClassID AS InsertedHierarchyClassID
	INTO #tmpTrait 
	FROM @hierarchyClassTraits hct  
	INNER JOIN dbo.HierarchyClass hc
		ON hct.HierarchyClassId = [dbo].[fn_ExtractPsSubTeamNumberFromSubTeamName](hc.HierarchyClassName)
	WHERE hc.HierarchyId = @financialHierarchyId

	UPDATE hct
	SET hct.TraitValue = #tmpTrait.traitValue
	FROM dbo.HierarchyClassTrait hct
	INNER JOIN #tmpTrait 
	ON #tmpTrait.InsertedHierarchyClassID = hct.hierarchyClassID 
	AND #tmpTrait.TraitId = hct.TraitId
	
	INSERT INTO dbo.HierarchyClassTrait 
	(
		hierarchyClassID, 
		TraitId, 
		uomID, 
		traitValue
	)
	SELECT InsertedHierarchyClassID , 
		   TraitId,	
		   NULL,
		   TraitValue
	FROM #tmpTrait
	WHERE NOT EXISTS (SELECT * FROM dbo.HierarchyClassTrait WHERE hierarchyClassID = #tmpTrait.InsertedHierarchyClassID AND TraitId = #tmpTrait.TraitId)

	-- insert only if record does not exist for Trait code fin
	INSERT INTO dbo.HierarchyClassTrait
	(
		 [traitID]
		,[hierarchyClassID]
		,[uomID]
		,[traitValue]
	)
	SELECT  DISTINCT 
			@finTraitId,
			InsertedHierarchyClassID ,
			NULL,
			PassedHierarchyClassID 
	FROM #tmpTrait
	WHERE InsertedHierarchyClassID NOT IN (SELECT HierarchyClassID FROM dbo.HierarchyClassTrait WHERE traitid = @FinTraitId)

	INSERT INTO infor.HierarchyClassSequence(HierarchyClassID, InforMessageId, SequenceID)
	SELECT 
		hct.HierarchyClassId,
		tmp.InforMessageId,
		tmp.SequenceId
	FROM #tmpFinancialClass tmp
	JOIN HierarchyClassTrait hct on tmp.HierarchyClassId = hct.traitValue
		AND hct.traitID = @FinTraitId
	WHERE tmp.SequenceId IS NOT NULL
		AND hct.HierarchyClassID NOT IN
		(
			SELECT HierarchyClassId FROM infor.HierarchyClassSequence
		)

	UPDATE hcs
	SET SequenceID = tmp.SequenceId,
		ModifiedDateUtc = SYSUTCDATETIME(),
		InforMessageId = tmp.InforMessageId
	FROM #tmpFinancialClass tmp
	JOIN HierarchyClassTrait hct on tmp.HierarchyClassId = hct.traitValue
		AND hct.traitID = @FinTraitId
	JOIN infor.HierarchyClassSequence hcs ON hct.HierarchyClassId = hcs.HierarchyClassID
	WHERE tmp.SequenceId IS NOT NULL
	
	DROP TABLE #tmpTrait
	DROP TABLE #tmpFinancialClass
END