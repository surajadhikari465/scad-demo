-- =============================================
-- Author:		Kyle Milner
-- Create date: 2014-12-29
-- Description:	Accepts a collection of scan codes
--				and returns item data for those
--				found in the database.
-- =============================================

CREATE PROCEDURE [app].[GetItemsByBulkScanCodeSearch]
	@SearchScanCodes app.ScanCodeListType readonly
AS
BEGIN
	
	set nocount on;

	declare
		@productDescriptionTraitId	int = (select traitID from Trait where traitCode = 'PRD'),
		@posDescriptionTraitId		int = (select traitID from Trait where traitCode = 'POS'),
		@packageUnitTraitId			int = (select traitID from Trait where traitCode = 'PKG'),
		@foodStampEligibleTraitId	int = (select traitID from Trait where traitCode = 'FSE'),
		@departmentSaleTraitId		int = (select traitID from Trait where traitCode = 'DPT'),
		@hiddenItemTraitId			int = (select traitID from Trait where traitCode = 'HID'),
		@validationDateTraitId		int = (select traitID from Trait where traitCode = 'VAL'),
		@retailSizeId				int = (select traitID from Trait where traitCode = 'RSZ'),
		@retailUomId				int = (select traitID from Trait where traitCode = 'RUM'),
		@deliverySystemId			int = (select traitID from Trait where traitCode = 'DS'),
		@scaleTareId				int = (select traitID from Trait where traitCode = 'SCT'),
		@alcoholByVolumeTraitId		int = (select traitID from Trait where traitCode = 'ABV'),
		@caseineFreeTraitId			int = (select traitID from Trait where traitCode = 'CF'),
		@fairTradeCertifiedTraitId	int = (select traitID from Trait where traitCode = 'FTC'),
		@drainedWeightTraitId		int = (select traitID from Trait where traitCode = 'DW'),
		@draintedWeightUomTraitId	int = (select traitID from Trait where traitCode = 'DWU'),
		@hempTraitId				int = (select traitID from Trait where traitCode = 'HEM'),
		@localeLoanProducerTraitId	int = (select traitID from Trait where traitCode = 'LLP'),
		@mainProductNameTraitId		int = (select traitID from Trait where traitCode = 'MPN'),
		@nutritionRequiredTraitId	int = (select traitID from Trait where traitCode = 'NR'),
		@organicPersonalCareTraitId	int = (select traitID from Trait where traitCode = 'OPC'),
		@paleoTraitId				int = (select traitID from Trait where traitCode = 'PLO'),
		@productFlavorTypeTraitId	int = (select traitID from Trait where traitCode = 'PFT'),
		@chainLocaleId				int = (select localeID from Locale where localeName = 'Whole Foods'),
		@brandHierarchyID			int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands'),
		@merchandiseClassID			int = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise'),
		@taxClassID					int = (select hierarchyID from Hierarchy where hierarchyName = 'Tax'),
		@nationalHierarchyID		int = (SELECT h.hierarchyID from Hierarchy h where h.hierarchyName = 'National'),
		@notesTraitId				int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NTS'),
		@certificationAgencyHierarchyID int = (SELECT h.hierarchyID from Hierarchy h where h.hierarchyName = 'Certification Agency Management'),
		@createdDateTraitID		    int	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'INS'),
		@modifiedDateTraitID		int	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MOD'),
		@modifiedUserTraitID		int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'USR');


	;WITH
	Brand (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		select ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		from 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
		where
			hc.hierarchyID = @brandHierarchyId
	),

	Tax (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		select ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		from 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
		where
			hc.hierarchyID = @taxClassId
	),

	Merchandise (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		select ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		from 
			ItemHierarchyClass			ihc
			JOIN HierarchyClass			hc	on	ihc.hierarchyClassID = hc.hierarchyClassID
		where
			hc.hierarchyID = @merchandiseClassId
	),

	National_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		FROM 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @nationalHierarchyID
	)	

    select
		i.itemID					as ItemId,
		sc.scanCode					as ScanCode,
		brand.hierarchyClassName	as BrandName,
		brand.hierarchyClassID		as BrandHierarchyClassId,
		prd.traitValue				as ProductDescription,
		pos.traitValue				as PosDescription,
		pkg.traitValue				as PackageUnit,
		fse.traitValue				as FoodStampEligible,
		sct.traitValue				as PosScaleTare,
		rsz.traitValue				as RetailSize,
		rum.traitValue				as RetailUom,	
		ds.traitValue				as DeliverySystem,	
		merch.hierarchyClassName	as MerchandiseHierarchyName,
		merch.hierarchyClassID		as MerchandiseHierarchyClassId,
		tax.hierarchyClassName		as TaxHierarchyName,
		tax.hierarchyClassID		as TaxHierarchyClassId,
		val.traitValue				as ValidatedDate,
		dpt.traitValue				as DepartmentSale,
		hid.traitValue				as HiddenItem,
		note.traitValue				as Notes,
		nat.hierarchyClassName		as NationalHierarchyClassName,
		nat.hierarchyClassID		as NationalHierarchyClassId,		
		abv.traitValue				as AlcoholByVolume,
		cf.traitValue				as CaseinFree,
		dw.traitValue				as DrainedWeight,
		dwu.traitValue				as DrainedWeightUom,
		ftc.traitValue				as FairTradeCertified,
		hem.traitValue				as Hemp,
		llp.traitValue				as LocalLoanProducer,
		mpn.traitValue				as MainProductName,
		nr.traitValue				as NutritionRequired,
		opc.traitValue				as OrganicPersonalCare,
		plo.traitValue				as Paleo,
		pft.traitValue				as ProductFlavorType,
		isa.AnimalWelfareRatingId	as AnimalWelfareRatingId,
		isa.Biodynamic				as Biodynamic,
		isa.CheeseMilkTypeId		as CheeseMilkTypeId,
		isa.CheeseRaw				as CheeseRaw,
		isa.EcoScaleRatingId		as EcoScaleRatingId,
		isa.GlutenFreeAgencyName	as GlutenFreeAgencyName,
		isa.KosherAgencyName		as KosherAgencyName,
		isa.Msc						as Msc,
		isa.NonGmoAgencyName		as NonGmoAgencyName,
		isa.OrganicAgencyName		as OrganicAgencyName,
		isa.PremiumBodyCare			as PremiumBodyCare,
		isa.SeafoodFreshOrFrozenId	as SeafoodFreshOrFrozenId,
		isa.SeafoodCatchTypeId		as SeafoodCatchTypeId,
		isa.VeganAgencyName			as VeganAgencyName,
		isa.Vegetarian			    as Vegetarian,
		isa.WholeTrade				as WholeTrade,
		isa.GrassFed				as GrassFed,
		isa.PastureRaised			as PastureRaised,
		isa.FreeRange				as FreeRange,
		isa.DryAged					as DryAged,
		isa.AirChilled				as AirChilled,
		isa.MadeinHouse				as MadeinHouse,
		crdate.traitValue			as CreatedDate,
		moddate.traitValue			as LastModifiedDate,
		modusr.traitValue			as LastModifiedUser
	from
		@SearchScanCodes		ssc
		join ScanCode			sc		on	ssc.ScanCode = sc.scanCode
		join Item				i		on	sc.itemID = i.itemID
		join ItemType			it		on	i.itemTypeID = it.itemTypeID
		left join ItemTrait		prd		on	i.itemID = prd.itemID
											and prd.traitID = @productDescriptionTraitID
											and prd.localeID = @chainLocaleId
		left join ItemTrait		pos		on	i.itemID = pos.itemID
											and pos.traitID = @posDescriptionTraitId
											and pos.localeID = @chainLocaleId
		left join ItemTrait		pkg		on	i.itemID = pkg.itemID
											and pkg.traitID = @packageUnitTraitId
											and pkg.localeID = @chainLocaleId
		left join ItemTrait		fse		on	i.itemID = fse.itemID
											and fse.traitID = @foodStampEligibleTraitId
											and fse.localeID = @chainLocaleId
		left join ItemTrait		sct		on	i.itemID = sct.itemID
											and sct.traitID = @scaleTareId
											and sct.localeID = @chainLocaleId
		left join ItemTrait		rsz		on	i.itemID = rsz.itemID
											and rsz.traitID = @retailSizeId
											and rsz.localeID = @chainLocaleId
		left join ItemTrait		rum		on	i.itemID = rum.itemID
											and rum.traitID = @retailUomId
											and rum.localeID = @chainLocaleId
		left join ItemTrait		ds		on	i.itemID = ds.itemID
											and ds.traitID = @deliverySystemId
											and ds.localeID = @chainLocaleId
		left join ItemTrait		val		on	i.itemID = val.itemID
											and val.traitID = @validationDateTraitId
											and val.localeID = @chainLocaleId
		left join ItemTrait		dpt		on	i.itemID = dpt.itemID
											and dpt.traitID = @departmentSaleTraitId
											and dpt.localeID = @chainLocaleId
		left join ItemTrait		hid		on	i.itemID = hid.itemID
											and hid.traitID = @hiddenItemTraitId
											and hid.localeID = @chainLocaleId
		left join ItemTrait	note		on	i.itemID = note.itemID
											and note.traitID = @notesTraitId
											and note.localeID = @chainLocaleId
		LEFT JOIN ItemTrait	crdate		on	i.itemID = crdate.itemID
											and crdate.traitID = @createdDateTraitID
											and crdate.localeID = @chainLocaleId
		LEFT JOIN ItemTrait	moddate		on	i.itemID = moddate.itemID
											and moddate.traitID = @modifiedDateTraitID
											and moddate.localeID = @chainLocaleId
		LEFT JOIN ItemTrait	modusr		on	i.itemID = modusr.itemID
											and modusr.traitID = @modifiedUserTraitID
											and modusr.localeID = @chainLocaleId
		LEFT JOIN ItemTrait abv			on	i.itemID = abv.itemID
											and abv.traitID = @alcoholByVolumeTraitId
											and abv.localeID = @chainLocaleId
		LEFT JOIN ItemTrait cf			on	i.itemID = cf.itemID
											and cf.traitID = @caseineFreeTraitId
											and cf.localeID = @chainLocaleId
		LEFT JOIN ItemTrait ftc			on	i.itemID = ftc.itemID
											and ftc.traitID = @fairTradeCertifiedTraitId
											and ftc.localeID = @chainLocaleId
		LEFT JOIN ItemTrait hem			on	i.itemID = hem.itemID
											and hem.traitID = @hempTraitId
											and hem.localeID = @chainLocaleId
		LEFT JOIN ItemTrait opc			on	i.itemID = opc.itemID
											and opc.traitID = @organicPersonalCareTraitId
											and opc.localeID = @chainLocaleId
		LEFT JOIN ItemTrait nr			on	i.itemID = nr.itemID
											and nr.traitID = @nutritionRequiredTraitId
											and nr.localeID = @chainLocaleId
		LEFT JOIN ItemTrait dw			on	i.itemID = dw.itemID
											and dw.traitID = @drainedWeightTraitId
											and dw.localeID = @chainLocaleId
		LEFT JOIN ItemTrait dwu			on	i.itemID = dwu.itemID
											and dwu.traitID = @draintedWeightUomTraitId
											and dwu.localeID = @chainLocaleId
		LEFT JOIN ItemTrait mpn			on	i.itemID = mpn.itemID
											and mpn.traitID = @mainProductNameTraitId
											and mpn.localeID = @chainLocaleId
		LEFT JOIN ItemTrait pft			on	i.itemID = pft.itemID
											and pft.traitID = @productFlavorTypeTraitId
											and pft.localeID = @chainLocaleId
		LEFT JOIN ItemTrait plo			on	i.itemID = plo.itemID
											and plo.traitID = @paleoTraitId
											and plo.localeID = @chainLocaleId
		LEFT JOIN ItemTrait llp			on	i.itemID = llp.itemID
											and llp.traitID = @localeLoanProducerTraitId
											and llp.localeID = @chainLocaleId
		left join Brand			brand	on	i.itemID = brand.itemID
		left join Tax			tax		on	i.itemID = tax.itemID
		left join Merchandise	merch	on	i.itemID = merch.itemID
		LEFT JOIN National_CTE nat		on i.itemID = nat.itemID		
		LEFT JOIN ItemSignAttribute isa		on	i.itemID = isa.ItemID
END