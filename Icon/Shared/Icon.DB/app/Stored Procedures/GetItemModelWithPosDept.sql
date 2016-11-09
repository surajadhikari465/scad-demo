

create PROCEDURE [app].[GetItemModelWithPosDept]

	@ScanCodes app.ScanCodeListType Readonly
AS
BEGIN
	SET NOCOUNT ON;
	
	--=======================================================
	-- Declare Variables
	--=======================================================
	declare @posDept int;
	declare @finId int;
	declare @merchFin int;
	declare @validationDate int;
	declare @merchId int;

	-- traits
	set @posDept = (select traitID from Trait where traitCode = 'PDN');
	set @merchFin = (select traitID from Trait where traitCode = 'MFM');
	set @validationDate = (select traitID from Trait where traitcode = 'Validation Date');
	
	-- hierarchies
	set @finId = (select hierarchyID from Hierarchy where hierarchyName = 'Financial');
	set @merchId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise');

	--=======================================================
	-- Setup CTEs
	--=======================================================
	WITH 
	ItemTrait_CTE (traitID, traitValue, itemID)
	AS
	(
		SELECT it.traitID, it.traitValue, it.itemID
		FROM ItemTrait it
		WHERE it.localeID = 1
	),
	Merch_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		FROM 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @merchId
	),
	MerchSubTeam_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID, subTeamName)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			ItemHierarchyClass			ihc
			JOIN HierarchyClass			hc	on	ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @merchFin
		WHERE
			hc.hierarchyID = @merchId
	),
	Fin_CTE (hierarchyClassID, hierarchyClassName, hierarchyID, posDeptNo)
	AS
	(
		SELECT hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			
			 HierarchyClass			hc	
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @posDept
		WHERE
			hc.hierarchyID = @finId
	)

	--=======================================================
	-- Main
	--=======================================================
	SELECT
		sc.itemID				as ItemId,
		vdat.traitValue			as ValidationDate,
		sc.scanCode				as ScanCode,
		sct.scanCodeTypeDesc	as ScanCodeType,
		hc.hierarchyClassName as SubTeamName,
		hc.hierarchyClassID	as SubTeamNo		 
		--fin.posDeptNo		as PosDeptNo
	FROM
		@ScanCodes			codes
		JOIN ScanCode		sc		on	codes.ScanCode = sc.scanCode
		JOIN ScanCodeType	sct		on	sc.scanCodeTypeID = sct.scanCodeTypeID
		LEFT JOIN ItemTrait_CTE	vdat	on	sc.itemID	= vdat.itemID
									and vdat.traitID = @validationDate
		JOIN Merch_CTE	merch	on	sc.itemID = merch.itemID
		JOIN MerchSubTeam_CTE	merchFin		on	sc.itemID = merchFin.itemID
		JOIN HierarchyClass hc on merchFin.subTeamName = hc.hierarchyClassName
		JOIN Fin_CTE fin on hc.hierarchyClassID = fin.hierarchyClassID

END

GO