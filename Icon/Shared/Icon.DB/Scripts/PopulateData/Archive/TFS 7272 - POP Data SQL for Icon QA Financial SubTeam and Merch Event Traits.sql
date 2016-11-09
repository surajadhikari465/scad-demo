
BEGIN

-- ====================================================
-- Declare Internal Variables
-- ====================================================
DECLARE @merchFinTraitID int;
DECLARE	@disableeventTraitID int;
DECLARE @merchandiseClassID int;
declare @finanncialClassID int;

-- Locale

-- Traits
SET @merchFinTraitID					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MFM');
SET @disableeventTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DEG');

-- Hierarchy

SET @merchandiseClassID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
SET @finanncialClassID	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Financial')

--Insert disable traits for fin hiearchy
insert into HierarchyClassTrait

SELECT @disableeventTraitID, mrc.hierarchyClassID, null, 1
	FROM 		
		 HierarchyClass			mrc		 
	WHERE
		mrc.hierarchyID = @finanncialClassID and mrc.hierarchyClassName 
		in  ( 
		'No Subteam (0000)',
		'Customer Service Non Margin (7000)',
		'Affinity (4551)')
		and  not exists
		(select 1 from HierarchyClassTrait hct where mrc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @disableeventTraitID)


-- ====================================================
-- Setup CTEs for HierarchyClass-FIN associations
-- ====================================================
;with
Merch_CTE (hierarchyClassID, hierarchyClassName, hierarchyID)
AS
(
	SELECT mrc.hierarchyClassID, mrc.hierarchyClassName, mrc.hierarchyID
	FROM 		
		 HierarchyClass			mrc
	WHERE
		mrc.hierarchyID = @merchandiseClassID and mrc.hierarchyLevel = 5
),


Fin_CTE (hierarchyClassID, hierarchyClassName, hierarchyID)
AS
(
	SELECT mrc.hierarchyClassID, mrc.hierarchyClassName, mrc.hierarchyID
	FROM 		
		 HierarchyClass			mrc
		 join HierarchyClassTrait hct on mrc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @disableeventTraitID
	WHERE
		mrc.hierarchyID = @finanncialClassID 
)


-- ====================================================
-- Main Query
-- ====================================================

insert into HierarchyClassTrait

SELECT
	@disableeventTraitID,
	merch.hierarchyClassID		as MerchandiseHierarchyClassId,
	null,
	1
	
FROM
Merch_CTE merch
join HierarchyClassTrait mhct on merch.hierarchyClassID = mhct.hierarchyClassID and mhct.traitID = @merchFinTraitID
join Fin_CTE fin on mhct.traitValue = fin.hierarchyClassName
where
not exists
		(select 1 from HierarchyClassTrait hct where merch.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @disableeventTraitID)
								
end;