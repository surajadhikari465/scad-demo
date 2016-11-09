create PROCEDURE [app].[GetItemSubTeamModel]
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
	declare @productDescription int;
	declare @posDescription int;
	declare @packageUnit int;
	declare @foodStamp int;
	declare @tare int;
	declare @brandId int;
	declare @taxId int;
	declare @taxAbbrevation int;
	declare @nonAlignedSubTeam int;

	-- traits
	set @posDept = (select traitID from Trait where traitCode = 'PDN');
	set @merchFin = (select traitID from Trait where traitCode = 'MFM');
	set @validationDate = (select traitID from Trait where traitcode = 'Validation Date');
	set @productDescription = (select traitID from Trait where traitDesc = 'Product Description');
	set @posDescription = (select traitID from Trait where traitDesc = 'POS Description');
	set @packageUnit = (select traitID from Trait where traitDesc = 'Package Unit');
	set @foodStamp = (select traitID from Trait where traitDesc = 'Food Stamp Eligible');
	set @tare = (select traitID from Trait where traitDesc = 'POS Scale Tare');
	SET @taxAbbrevation = (select traitID from Trait where traitDesc = 'Tax Abbreviation');
	set @nonAlignedSubTeam = (select traitID from Trait where traitCode = 'NAS');
	
	-- hierarchies
	set @finId = (select hierarchyID from Hierarchy where hierarchyName = 'Financial');
	set @merchId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise');
	set @brandId = (select hierarchyID from Hierarchy where hierarchyName = 'Brands');
	set @taxId = (select hierarchyID from Hierarchy where hierarchyName = 'Tax');

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
	),
	Brand_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		FROM 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @brandId
	),
	Tax_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID, taxAbbreviation)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			ItemHierarchyClass			ihc
			JOIN HierarchyClass			hc	on	ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @taxAbbrevation
		WHERE
			hc.hierarchyID = @taxId
	),
	Fin_Event_CTE (hierarchyClassID, hierarchyClassName, hierarchyID, disableSubTeamEvents)
	AS
	(
		SELECT hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue
		FROM 
			
			 HierarchyClass			hc	
			JOIN HierarchyClassTrait	hct on	hc.hierarchyClassID = hct.hierarchyClassID
											and hct.traitID = @nonAlignedSubTeam
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
		prd.traitValue			as ProductDescription,
		pos.traitValue			as PosDescription,
		pack.traitValue			as PackageUnit,
		fs.traitValue			as FoodStampEligible,
		tare.traitValue			as Tare,
		brand.hierarchyClassID	as BrandId,
		CASE 
			WHEN LEN(brand.hierarchyClassName) > 25 THEN SUBSTRING(brand.hierarchyClassName, 1, 25)
			ELSE brand.hierarchyClassName 
		END						as BrandName,
		tax.taxAbbreviation		as TaxClassName,
		hc.hierarchyClassName as SubTeamName,
		hc.hierarchyClassID	as SubTeamNo,
		cast(coalesce(nullif(fin.posDeptNo,''), '-1') AS int) as DeptNo,
		cast(coalesce(nullif(finEvent.disableSubTeamEvents,'0'), '0') AS bit) as SubTeamNotAligned
	FROM
		@ScanCodes			codes
		JOIN ScanCode		sc		on	codes.ScanCode = sc.scanCode
		JOIN ScanCodeType	sct		on	sc.scanCodeTypeID = sct.scanCodeTypeID
		LEFT JOIN ItemTrait_CTE	vdat	on	sc.itemID	= vdat.itemID
									and vdat.traitID = @validationDate
									JOIN ItemTrait_CTE	prd		on	sc.itemID = prd.itemID
									and prd.traitID = @productDescription
		JOIN ItemTrait_CTE	pos		on	sc.itemID = pos.itemID
									and pos.traitID = @posDescription
		JOIN ItemTrait_CTE	pack	on	sc.itemID = pack.itemID
									and pack.traitID = @packageUnit
		JOIN ItemTrait_CTE	fs		on	sc.itemID = fs.itemID
									and fs.traitID = @foodStamp
		JOIN ItemTrait_CTE	tare	on	sc.itemID = tare.itemID
									and tare.traitID = @tare
		LEFT JOIN Brand_CTE	brand	on	sc.itemID = brand.itemID
		LEFT JOIN Tax_CTE	tax		on	sc.itemID = tax.itemID	
		JOIN MerchSubTeam_CTE	merchFin		on	sc.itemID = merchFin.itemID
		JOIN HierarchyClass hc on merchFin.subTeamName = hc.hierarchyClassName
		LEFT JOIN Fin_CTE fin on hc.hierarchyClassID = fin.hierarchyClassID
		LEFT JOIN Fin_Event_CTE finEvent on hc.hierarchyClassID = finEvent.hierarchyClassID

END
GO