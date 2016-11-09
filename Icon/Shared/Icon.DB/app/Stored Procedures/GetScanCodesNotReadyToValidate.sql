
CREATE PROCEDURE [app].[GetScanCodesNotReadyToValidate]
	@Items app.ItemCanonicalDataType READONLY
AS
BEGIN
	DECLARE @brandHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands'),
			@merchHierarchyId int= (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise'),
			@taxHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Tax'),
			@productDescriptionTraitId int = (select traitID from Trait where traitCode = 'PRD'),
			@posDescriptionTraitId int = (select traitID from Trait where traitCode = 'POS'),
			@packageUnitTraitId int = (select traitID from Trait where traitCode = 'PKG'),
			@foodStampEligibleTraitId int = (select traitID from Trait where traitCode = 'FSE'),
			@posScaleTareTraitId int = (select traitID from Trait where traitCode = 'SCT'),
			@globalLocaleId int = (select localeID from Locale where localeName = 'Whole Foods'),
			@nationalHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'National');

	WITH
	Brand_CTE (itemID)
	AS
	(
		select itemID 
		from
		ItemHierarchyClass brandihc
		join HierarchyClass brandhc on brandihc.hierarchyClassID = brandhc.hierarchyClassID
			and brandhc.hierarchyID = @brandHierarchyId
	),
	Merch_CTE (itemID)
	AS
	(
		select itemID 
		from
		ItemHierarchyClass merchihc
		join HierarchyClass merchhc on merchihc.hierarchyClassID = merchhc.hierarchyClassID
			and merchhc.hierarchyID = @merchHierarchyId
	),
	Tax_CTE (itemID)
	AS
	(
		select itemID 
		from 
		ItemHierarchyClass taxihc
		join HierarchyClass taxhc on taxihc.hierarchyClassID = taxhc.hierarchyClassID
			and taxhc.hierarchyID = @taxHierarchyId		
	),
	Item_CTE (ScanCode, ProductDescription, PosDescription, PackageUnit, FoodStampEligible, PosScaleTare, BrandId, MerchId, TaxId, NatId)
	AS
	(
		select i.ScanCode,
				COALESCE(NULLIF(i.[Product Description], ''), NULLIF(prd.traitValue, '')) as ProductDescription,
				COALESCE(NULLIF(i.[POS Description], ''), NULLIF(pos.traitValue, '')) as PosDescription,
				COALESCE(NULLIF(i.[Package Unit], ''), NULLIF(pkg.traitValue, '')) as PackageUnit,
				COALESCE(NULLIF(i.[Food Stamp Eligible], ''), NULLIF(fse.traitValue, '')) as FoodStampEligible,
				COALESCE(NULLIF(i.[POS Scale Tare], ''), NULLIF(sct.traitValue, '')) as PosScaleTare,
				COALESCE(NULLIF(i.[Brand ID], ''), brandihc.hierarchyClassID) as BrandId,
				COALESCE(NULLIF(i.[Merchandise Hierarchy ID], ''), merchihc.hierarchyClassID) as MerchId,
				COALESCE(NULLIF(i.[Tax Class ID], ''), taxihc.hierarchyClassID) as TaxId,
				COALESCE(NULLIF(i.[National Class ID], ''), natihc.hierarchyClassID) as NatId
		from @Items i
		join ScanCode sc on i.ScanCode = sc.scanCode
		left join (ItemHierarchyClass brandihc
			join HierarchyClass brandhc on brandihc.hierarchyClassID = brandhc.hierarchyClassID
				and brandhc.hierarchyID = @brandHierarchyId) on brandihc.itemID = sc.itemID
		left join (ItemHierarchyClass taxihc
				join HierarchyClass taxhc on taxihc.hierarchyClassID = taxhc.hierarchyClassID
					and taxhc.hierarchyID = @taxHierarchyId) on taxihc.itemID = sc.itemID
		left join (ItemHierarchyClass merchihc 
				join HierarchyClass merchhc on merchihc.hierarchyClassID = merchhc.hierarchyClassID
					and merchhc.hierarchyID = @merchHierarchyId) on sc.itemID = merchihc.itemID
		left join (ItemHierarchyClass natihc 
				join HierarchyClass nathc on natihc.hierarchyClassID = nathc.hierarchyClassID
					and nathc.hierarchyID = @nationalHierarchyId) on sc.itemID = natihc.itemID
		left join ItemTrait prd on sc.itemID = prd.itemID 
			and prd.traitID = @productDescriptionTraitId
			and prd.localeID = @globalLocaleId
		left join ItemTrait pos on sc.itemID = pos.itemID
			and pos.traitID = @posDescriptionTraitId
			and pos.localeID = @globalLocaleId
		left join ItemTrait pkg on sc.itemID = pkg.itemID
			and pkg.traitID = @packageUnitTraitId
			and pkg.localeID = @globalLocaleId
		left join ItemTrait fse on sc.itemID = fse.itemID
			and fse.traitID = @foodStampEligibleTraitId
			and fse.localeID = @globalLocaleId
		left join ItemTrait sct on sc.itemID = sct.itemID
			and sct.traitID = @posScaleTareTraitId
			and sct.localeID = @globalLocaleId
	)

--Select scan codes where the imported data joined with the current item's data does not have all of the canonical properties present to validate an item
	select i.ScanCode
	from Item_CTE as i
	WHERE i.ProductDescription IS NULL
		OR i.PosDescription IS NULL
		OR i.PackageUnit IS NULL
		OR i.FoodStampEligible IS NULL
		OR i.PosScaleTare IS NULL
		OR i.BrandId IS NULL
		OR i.MerchId IS NULL
		OR i.TaxId IS NULL
		OR i.NatId IS NULL
END
GO
