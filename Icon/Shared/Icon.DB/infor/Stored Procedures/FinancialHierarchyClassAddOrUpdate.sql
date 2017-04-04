CREATE PROCEDURE infor.FinancialHierarchyClassAddOrUpdate
	@hierarchyClasses infor.HierarchyClassType READONLY,
	@hierarchyClassTraits infor.HierarchyClassTraitType READONLY
	AS

BEGIN

	DECLARE @financialHierarchyId INT	= (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'Financial')
	
	-- Extract Fin (Trait Code)  for example if it is Spa Sales (3750) extract 3750, put data into temporary table
	SELECT hierarchyLevel, hc.HierarchyId as [HierarchyId], ParentHierarchyClassId, HierarchyClassName ,Substring(HierarchyClassName, len(HierarchyClassName)-4,4)  as Fin
	INTO #tmpClass
	FROM @hierarchyClasses hc
	JOIN dbo.HierarchyPrototype hp on hc.HierarchyLevelName = hp.HierarchyLevelName
	WHERE hc.HierarchyId = @financialHierarchyId
	-- update class name if we find a match 
	UPDATE dbo.HierarchyClass
	SET HierarchyClassName =  tmpClass.HierarchyClassName
	FROM #tmpClass tmpClass
	WHERE Substring(dbo.HierarchyClass.HierarchyClassName, len(dbo.HierarchyClass.HierarchyClassName)-4,4) = tmpClass.Fin

	--If not match found then insert into HierarchyClass table from #tmpClass Table
	INSERT INTO dbo.HierarchyClass( hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
	SELECT hierarchyLevel, [HierarchyId], ParentHierarchyClassId, HierarchyClassName  
	FROM #tmpClass
	WHERE #tmpClass.Fin  NOT in (SELECT Substring(HierarchyClassName, len(HierarchyClassName)-4,4)
								  FROM dbo.HierarchyClass)
	

	SELECT hct.HierarchyClassID as PassedHierarchyClassID, TraitId, TraitValue  , hc.hierarchyClassID as InsertedHierarchyClassID
	INTO #tmpTrait 
	FROM @hierarchyClassTraits hct  Inner join dbo.HierarchyClass hc
	ON  hct.HierarchyClassId = Substring(hc.HierarchyClassName, len(hc.HierarchyClassName)-4,4)
	WHERE hc.HierarchyId = @financialHierarchyId

	UPDATE hct
	SET hct.TraitValue = #tmpTrait.traitValue
	FROM dbo.HierarchyClassTrait hct
	INNER JOIN #tmpTrait 
	ON #tmpTrait.InsertedHierarchyClassID = hct.hierarchyClassID AND #tmpTrait.TraitId = hct.TraitId
	

	INSERT INTO dbo.HierarchyClassTrait (hierarchyClassID, TraitId, uomID, traitValue)
	SELECT InsertedHierarchyClassID , TraitId,null,TraitValue
	FROM #tmpTrait
	WHERE NOT EXISTS( SELECT * FROM dbo.HierarchyClassTrait  WHERE  hierarchyClassID =  #tmpTrait.InsertedHierarchyClassID AND TraitId =  #tmpTrait.TraitId  )

	
			DECLARE @finTraitId INT
			SELECT TOP 1 @finTraitId = TraitID FROM dbo.Trait WHERE TraitCode = 'FIN'
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
					null,
					PassedHierarchyClassID 
					FROM #tmpTrait
				 WHERE InsertedHierarchyClassID NOT IN(  SELECT HierarchyClassID FROM dbo.HierarchyClassTrait WHERE traitid = @FinTraitId)



	DROP TABLE #tmpTrait
	DROP TABLE #tmpClass
END

