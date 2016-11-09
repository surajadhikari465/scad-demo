create PROCEDURE [app].[GetAllHierarchyLineage]
@includeBrowsingHierarchy bit = 0,
@includeNationalHierarchy bit = 0,
@includeFinancialHierarchy bit = 0
As

BEGIN

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	Declare 
	@affinityTraitID int,
	@brandHierarchyID int,
	@taxHierarchyID int,
	@merchandiseHierarchyID int,
	@merchandiseTraitID int,
	@browsingHierarchyID int,
	@taxRomanceTraitID int,
	@nationalHierarchyID int,
	@nationalClassCodeTraitID int,
	@financialHierarchyID int

	SET @affinityTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'AFF')
	SET @merchandiseTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MFM')
	SET @brandHierarchyID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Brands')
	SET @taxHierarchyID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax')
	SET @merchandiseHierarchyID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
	SeT @browsingHierarchyID = (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Browsing')
	SET @taxRomanceTraitID = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'TRM')
	SET @nationalHierarchyID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'National')
	SET @nationalClassCodeTraitID = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NCC')
	SET @financialHierarchyID = (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Financial')

	--Brand
	select  hc.hierarchyID as HierarchyId,  hc.hierarchyClassID as HierarchyClassId, hc.hierarchyClassName  as HierarchyClassName, hc.hierarchyLevel as HierarchyLevel, hc.hierarchyClassName as HierarchyLineage, hc.hierarchyParentClassID as HierarchyParentClassId
	from HierarchyClass hc	
	where hierarchyID = @brandHierarchyID
		order by hierarchyClassName;

	--Tax
	select hc.hierarchyID as HierarchyId,  hc.hierarchyClassID as HierarchyClassId, hc.hierarchyClassName  as HierarchyClassName, hc.hierarchyLevel as HierarchyLevel, 
	hct.traitValue as HierarchyLineage, hc.hierarchyParentClassID as HierarchyParentClassId
	from HierarchyClass hc
	INNER JOIN HierarchyClassTrait hct on
			hc.hierarchyClassID = hct.hierarchyClassID
			and hct.traitID  = @taxRomanceTraitID
	where hierarchyID = @taxHierarchyID
	
	order by hierarchyClassName;

	--Merch
	with firstHierarchy as
	(
		select hc.hierarchyID as HierarchyID, hc.hierarchyClassID,
			 hc.hierarchyClassName,
			 hc.hierarchyParentClassID,
			 hc.hierarchyLevel,
			 cast('' as nvarchar(max)) as ParentNames
		from [dbo].[HierarchyClass] hc		
		where hierarchyParentClassID is null and hierarchyID = @merchandiseHierarchyID
	  
		union all
		select hc1.hierarchyID as HierarchyID, 
		 hc1.hierarchyClassID,
			 hc1.hierarchyClassName,
			 hc1.hierarchyParentClassID,
			 hc1.hierarchyLevel,
			 firstHierarchy.ParentNames + '|' + firstHierarchy.hierarchyClassName
		from [dbo].[HierarchyClass] as hc1         
		INNER JOIN firstHierarchy on
			firstHierarchy.hierarchyClassID = hc1.hierarchyParentClassID					 
		 where  hc1.hierarchyID = @merchandiseHierarchyID
	), 
	
	merchandiseHierarchy as 
	(
		select 
		HierarchyID,
		hierarchyClassID,
		hierarchyClassName,
		hierarchyParentClassID,
		hierarchyLevel,
		stuff(ParentNames, 1, 1, '') as ParentNames
		from firstHierarchy
		where hierarchyLevel = 5 
	)


	select c.hierarchyID as HierarchyId, c.hierarchyClassID as HierarchyClassId,
			c.hierarchyClassName as HierarchyClassName,
			c.hierarchyLevel as HierarchyLevel,
		    stuff(ParentNames, CHARINDEX('|', ParentNames), len(ParentNames) - CHARINDEX('|', ParentNames) + 1, '') + '|' + c.hierarchyClassName + case when hct.traitValue is null then '' else ': ' + hct.traitValue end
		     as HierarchyLineage,
		   c.hierarchyParentClassID as HierarchyParentClassId
	from merchandiseHierarchy c	
	left JOIN HierarchyClassTrait hct on
			c.hierarchyClassID = hct.hierarchyClassID
			and hct.traitID  = @merchandiseTraitID
	where c.hierarchyLevel = 5 and
			not exists (	select 1
							 from HierarchyClassTrait hft
							 where c.hierarchyClassID = hft.hierarchyClassID and hft.traitID  = @affinityTraitID
						)
	order by HierarchyLineage

	----Browsing
	if @includeBrowsingHierarchy = 1 
	begin
		with browsingHierarchy as
		(
			select hc.hierarchyID as HierarchyID, hc.hierarchyClassID,
				 hc.hierarchyClassName,
				 hc.hierarchyParentClassID,
				 hc.hierarchyLevel,
				 cast('' as nvarchar(max)) as ParentNames
			from [dbo].[HierarchyClass] hc		
			where hierarchyParentClassID is null and hierarchyID = @browsingHierarchyID
	  
			union all
			select hc1.hierarchyID as HierarchyID, 
			 hc1.hierarchyClassID,
				 hc1.hierarchyClassName,
				 hc1.hierarchyParentClassID,
				 hc1.hierarchyLevel,
				 browsingHierarchy.ParentNames + '|' + browsingHierarchy.hierarchyClassName
			from [dbo].[HierarchyClass] as hc1         
			INNER JOIN browsingHierarchy on
				browsingHierarchy.hierarchyClassID = hc1.hierarchyParentClassID					 
			 where  hc1.hierarchyID = @browsingHierarchyID
		)


		select bh.hierarchyID as HierarchyId, bh.hierarchyClassID as HierarchyClassId,
				bh.hierarchyClassName as HierarchyClassName,
				bh.hierarchyLevel as HierarchyLevel,
			   stuff(ParentNames, 1, 1, '') + '|' + bh.hierarchyClassName
				 as HierarchyLineage,
			   bh.hierarchyParentClassID as HierarchyParentClassId
		from browsingHierarchy bh		
		where bh.hierarchyLevel = 2
		order by hierarchyClassName
	end;

	--National class
	if @includeNationalHierarchy = 1
	begin
		
		select hc.hierarchyID as HierarchyId,  hc.hierarchyClassID as HierarchyClassId, hc.hierarchyClassName  as HierarchyClassName, hc.hierarchyLevel as HierarchyLevel, 
		hc.hierarchyClassName + ': ' + hct.traitValue as HierarchyLineage, hc.hierarchyParentClassID as HierarchyParentClassId
		from HierarchyClass hc
		INNER JOIN HierarchyClassTrait hct on
				hc.hierarchyClassID = hct.hierarchyClassID
				and hct.traitID  = @nationalClassCodeTraitID
		where hierarchyID = @nationalHierarchyID	
		order by hierarchyClassName;
	end;

	--Fin class
	if @includeFinancialHierarchy = 1
	select hc.hierarchyID as HierarchyId,  hc.hierarchyClassID as HierarchyClassId, hc.hierarchyClassName  as HierarchyClassName, hc.hierarchyLevel as HierarchyLevel, 
	hc.hierarchyClassName as HierarchyLineage, hc.hierarchyParentClassID as HierarchyParentClassId
	from HierarchyClass hc
	where hierarchyID = @financialHierarchyID
	
	order by hierarchyClassName;


end;