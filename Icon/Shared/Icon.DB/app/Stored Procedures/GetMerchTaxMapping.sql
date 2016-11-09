CREATE PROCEDURE [app].[GetMerchTaxMapping]
	@merchandiseHierarcyClassID int = 0
AS
BEGIN
	SET NOCOUNT ON

	DECLARE
		@affinityTraitID int,
		@merchTaxdeTriatId int,
		@taxHierarchyID int,
		@merchandiseHierarchyID int,
		@merchandiseTraitID int,
		@browsingHierarchyID int,
		@taxRomanceTraitID int,
		@subBrickLevel int

	SET @affinityTraitID = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'AFF')
	SET @merchandiseTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MFM')
	SET @merchTaxdeTriatId = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MDT')
	SET @taxHierarchyID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax')
	SET @merchandiseHierarchyID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
	SET @taxRomanceTraitID = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'TRM')
	SET @subBrickLevel = (SELECT hierarchyLevel from HierarchyPrototype where hierarchyLevelName = 'Sub Brick')

	;WITH	
	Tax_CTE (hierarchyClassID, hierarchyClassName, hierarchyId, taxRomance)
	AS
	(
		SELECT 
			hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue as taxRomance
		FROM 
			HierarchyClass hc
			JOIN HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @taxRomanceTraitID
		WHERE
			hc.hierarchyID = @taxHierarchyID
	),

	firstHierarchy as
	(
		select 
			hc.hierarchyID as HierarchyID, 
			hc.hierarchyClassID,
			hc.hierarchyClassName,
			hc.hierarchyParentClassID,
			hc.hierarchyLevel,
			cast('' as nvarchar(max)) as ParentNames
		from 
			[dbo].[HierarchyClass] hc		
		where 
			hierarchyParentClassID is null and hierarchyID = @merchandiseHierarchyID
	  
		union all

		select 
			hc1.hierarchyID as HierarchyID, 
			hc1.hierarchyClassID,
			hc1.hierarchyClassName,
			hc1.hierarchyParentClassID,
			hc1.hierarchyLevel,
			firstHierarchy.ParentNames + '|' + firstHierarchy.hierarchyClassName
		from
			[dbo].[HierarchyClass] as hc1         
			INNER JOIN firstHierarchy on firstHierarchy.hierarchyClassID = hc1.hierarchyParentClassID					 
		where  
			hc1.hierarchyID = @merchandiseHierarchyID
	), 
	
	merchandiseHierarchy as 
	(
		select 
			hierarchyID,
			hierarchyClassID,
			hierarchyClassName,
			hierarchyParentClassID,
			hierarchyLevel,
			stuff(ParentNames, 1, 1, '') as ParentNames
		from 
			firstHierarchy
		where 
			hierarchyLevel = @subBrickLevel
	)

	SELECT	
		merchHc.hierarchyClassID as MerchandiseHierarchyClassId,
		merchHc.hierarchyClassName as MerchandiseHierarchyName,
		taxHc.hierarchyClassID as TaxHierarchyClassId,
		taxHc.hierarchyClassName as TaxHierarchyName,			
		stuff(ParentNames, CHARINDEX('|', ParentNames), len(ParentNames) - CHARINDEX('|', ParentNames) + 1, '') + '|' + merchHc.hierarchyClassName + case when hct.traitValue is null then '' else ': ' + hct.traitValue end as MerchandiseHierarchyClassLineage,
		taxHc.taxRomance as TaxHierarchyClassLineage
	FROM 
		merchandiseHierarchy merchHc
		INNER JOIN dbo.HierarchyClassTrait merchHct on merchHc.hierarchyClassID = merchHct.hierarchyClassID and merchHct.traitID = @merchTaxdeTriatId		
		INNER JOIN Tax_CTE taxHc on  merchHct.traitValue = taxHc.hierarchyClassID
		LEFT JOIN HierarchyClassTrait hct on merchHc.hierarchyClassID = hct.hierarchyClassID and hct.traitID  = @merchandiseTraitID
	WHERE 
		merchHc.hierarchyLevel = @subBrickLevel
		AND merchHc.hierarchyID = @merchandiseHierarchyID
		AND (@merchandiseHierarcyClassID = 0 OR (@merchandiseHierarcyClassID <> 0 and @merchandiseHierarcyClassID = merchHc.hierarchyClassID))
		AND NOT EXISTS (	
							select 1
							from HierarchyClassTrait hft
							where merchHc.hierarchyClassID = hft.hierarchyClassID and hft.traitID  = @affinityTraitID
						)						
END
