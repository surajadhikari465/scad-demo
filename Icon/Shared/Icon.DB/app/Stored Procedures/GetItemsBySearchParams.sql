CREATE PROCEDURE [app].[GetItemsBySearchParams]
	@scanCode varchar(15) = null, 
	@merchandiseHierarchy varchar(255) = null, 
	@taxRomanceName varchar(255) = null , 
	@brandName varchar(255) = null, 
	@productDescription varchar(255) = null,
	@retailSize varchar(255) = null,
	@retailUom varchar(255) = null,
	@deliverySystem varchar(255) = null,
	@posDescription varchar(255) = null, 
	@pkgUnit varchar(255) = null,
	@posScaleTare varchar(255) = null,
	@foodStampEligible varchar(3) = null,
	@departmentSale varchar(3) = null,
	@itemStatus varchar(30) = null,
	@partialScanCodeSearch bit = 0,
	@partialBrandName bit = 0,
	@hiddenItemStatus varchar(10) = null,
	@nationalHierarchy varchar(255)= null,	
	@cf varchar(255)= null,
	@ftc varchar(255)= null,
	@hem varchar(255)= null,
	@opc varchar(255)= null,
	@nr varchar(255)= null,
	@dw varchar(255)= null,
	@dwu varchar(255)= null,
	@abv varchar(255)= null,
	@mpn varchar(255)= null,
	@pft varchar(255)= null,
	@plo varchar(255)= null,
	@llp varchar(255)= null,
	@animalWelfareRatingId int = null,
	@biodynamic varchar(3) = null,
	@milkTypeId int = null,
	@cheeseRaw varchar(3) = null,
	@ecoScaleRatingId int = null,
	@glutenFreeAgency varchar(255) = null,
	@kosherAgency varchar(255) = null,
	@isMsc varchar(3) = null,
	@nonGmoAgency varchar(255) = null,
	@organicAgency varchar(255) = null,
	@premiumBodyCare varchar(3) = null,
	@seafoodFreshOrFrozenId int = null,
	@seafoodCatchTypeId int = null,
	@veganAgency varchar(255) = null,
	@vegetarian varchar(3) = null,
	@wholeTrade varchar(3) = null,
	@notes varchar(255)  = null,
	@isGrassFed varchar(3) = null,
	@isPastureRaised varchar(3) = null,
	@isFreeRange varchar(3) = null,
	@isDryAged varchar(3) = null,
	@isAirChilled varchar(3) = null,
	@isMadeInHouse varchar(3) = null,
	@createDate varchar(255) = null,
	@modifiedDate varchar(255) = null,
	@modifiedUser varchar(255) = null,
	@pageSize int = null,
	@pageIndex int = null,
	@sortColumn nvarchar(255) = null,
	@sortOrder nvarchar(4) = null,
	@getOnlyCount bit = 0
	WITH RECOMPILE
AS
BEGIN

	-- ====================================================
	-- Declare Internal Variables
	-- ====================================================
	DECLARE @validationStatus int;
	DECLARE	@localeID int;
	DECLARE	@productDescriptionTraitID int;
	DECLARE	@posTraitID int;
	DECLARE @packageUnitTraitID int;
	DECLARE @foodStampEligibleTraitID int;
	DECLARE @departmentSaleTraitID int;
	DECLARE @brandHierarchyID int;
	DECLARE @scaleTareID int;
	DECLARE @merchandiseClassID int;	
	DECLARE @taxClassID int;
	DECLARE @validationDateTraitID int;
	DECLARE @readyMessageStatusID int;
	DECLARE @stagedMessageStatusID int;	
	DECLARE @merchFinMappingTraitID int;
	DECLARE @prohibitDiscountTraitID int;
	DECLARE @retailSizeID int;
	DECLARE @retailUomID int;
	DECLARE @deliverySystemID int;
	DECLARE @financialId int;
	DECLARE @financialMapId int;
	DECLARE @wholeFoodsLocale int;
	DECLARE @taxRomanceTraitID int;
	DECLARE @hiddenItemTraitID int;
	DECLARE @nationalHierarchyID int;	
	DECLARE @nationalClassCodeTraitID int;
	DECLARE @certificationAgencyHierarchyID int;
	DECLARE @notesTraitId int;
	DECLARE @createdDateTraitID int;
	DECLARE @modifiedDateTraitID int;
	DECLARE @modifiedUserTraitID int;
	DECLARE @cfTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'CF');
	DECLARE @ftcTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'FTC');
	DECLARE @hemTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'HEM');
	DECLARE @opcTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'OPC');
	DECLARE @nrTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NR');
	DECLARE @dwTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DW');
	DECLARE @dwuTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DWU');
	DECLARE @abvTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'ABV');
	DECLARE @mpnTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MPN');
	DECLARE @pftTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PFT');
	DECLARE @ploTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PLO');
	DECLARE @llpTraitID int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'LLP');

	-- Locale
	SET @wholeFoodsLocale = (SELECT l.localeID FROM Locale l WHERE l.localeName = 'Whole Foods');

	-- Traits
	SET @productDescriptionTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PRD');
	SET @posTraitID					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'POS');
	SET @packageUnitTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PKG');
	SET @foodStampEligibleTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'FSE');
	SET @departmentSaleTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DPT');
	SET @validationDateTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'VAL');
	SET @retailSizeID				= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'RSZ');
	SET @retailUomID				= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'RUM');
	SET @deliverySystemID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DS');
	SET @scaleTareID				= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'SCT');
	SET @taxRomanceTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'TRM');
	SET @hiddenItemTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'HID');
	SET @notesTraitId				= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NTS');
	SET @createdDateTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'INS');
	SET @modifiedDateTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MOD');
	SET @modifiedUserTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'USR');

	-- Hierarchy
	SET @brandHierarchyID				= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Brands')
	SET @merchandiseClassID				= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
	SET @taxClassID						= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax')
	SET @nationalHierarchyID			= (SELECT h.hierarchyID from Hierarchy h where h.hierarchyName = 'National')
	SET @certificationAgencyHierarchyID	= (SELECT h.hierarchyID from Hierarchy h where h.hierarchyName = 'Certification Agency Management')
	SET @nationalClassCodeTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NCC')

	-- ====================================================
	-- Setup CTEs for HierarchyClass-Item associations
	-- ====================================================
	DECLARE @selectItemsSql nvarchar(max) = '',
			@selectCountSql nvarchar(max) = '',
			@fromSql nvarchar(max) = '',
			@cteSql nvarchar(max) = '',
			@whereSql nvarchar(max) = '',
			@orderBySql nvarchar(max) = ''

	SET @cteSql = N'
	WITH Brand_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID
		FROM 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
		WHERE
			hc.hierarchyID = @brandHierarchyID
	),
	Tax_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyId, taxRomance)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hct.traitValue as taxRomance
		FROM 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
			JOIN HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and hct.traitID = @taxRomanceTraitID
		WHERE
			hc.hierarchyID = @taxClassID
	),
	Merch_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID)
	AS
	(
		SELECT ihc.itemID, mrc.hierarchyClassID, mrc.hierarchyClassName, mrc.hierarchyID
		FROM 
			ItemHierarchyClass			ihc
			JOIN HierarchyClass			mrc	on	ihc.hierarchyClassID = mrc.hierarchyClassID
		WHERE
			mrc.hierarchyID = @merchandiseClassID
	),
	National_CTE (itemID, hierarchyClassID, hierarchyClassName, hierarchyID, hierarchyLineage)
	AS
	(
		SELECT ihc.itemID, hc.hierarchyClassID, hc.hierarchyClassName, hc.hierarchyID, hc.hierarchyClassName + '': '' + hct.traitValue as hierarchyLineage
		FROM 
			ItemHierarchyClass	ihc
			JOIN HierarchyClass hc	on ihc.hierarchyClassID = hc.hierarchyClassID
			INNER JOIN HierarchyClassTrait hct on
					hc.hierarchyClassID = hct.hierarchyClassID
					and hct.traitID  = @nationalClassCodeTraitID
		WHERE
			hc.hierarchyID = @nationalHierarchyID
	)
	'
	
	-- ====================================================
	-- Set SELECT Items statement
	-- ====================================================
	SET @selectItemsSql = N'
	SELECT
		i.itemID					as ItemId,
		sc.scanCode					as ScanCode,
		brand.hierarchyClassName	as BrandName,
		brand.hierarchyClassID		as BrandHierarchyClassId,
		pd.traitValue				as ProductDescription,
		pos.traitValue				as PosDescription,
		pack.traitValue				as PackageUnit,
		fs.traitValue				as FoodStampEligible,
		tare.traitValue				as PosScaleTare,
		size.traitValue				as RetailSize,
		uom.traitValue				as RetailUom,
		ds.traitValue				as DeliverySystem,	
		merch.hierarchyClassName	as MerchandiseHierarchyName,
		merch.hierarchyClassID		as MerchandiseHierarchyClassId,
		tax.hierarchyClassName		as TaxHierarchyName,
		tax.hierarchyClassID		as TaxHierarchyClassId,
		vld.traitValue				as ValidatedDate,
		dept.traitValue				as DepartmentSale,
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
		modusr.traitValue			as LastModifiedUser'
		
	-- ====================================================
	-- Set SELECT COUNT statement
	-- ====================================================
	SET @selectCountSql = N' SELECT COUNT(*) '

	-- ====================================================
	-- Set FROM statement
	-- ====================================================
	SET @fromSql = N'
	FROM
	Item				i
	LEFT JOIN ItemType	tp				on	i.itemTypeID = tp.itemTypeID
	LEFT JOIN ScanCode	sc				on	i.itemID = sc.itemID
	LEFT JOIN ItemTrait	pd				on	i.itemID = pd.itemID
											and pd.traitID = @productDescriptionTraitID
											and pd.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	pos				on	i.itemID = pos.itemID
											and pos.traitID = @posTraitID
											and pos.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	pack			on	i.itemID = pack.itemID
											and pack.traitID = @packageUnitTraitID
											and pack.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	fs				on	i.itemID = fs.itemID
											and fs.traitID = @foodStampEligibleTraitID
											and fs.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	tare			on	i.itemID = tare.itemID
											and tare.traitID = @scaleTareID
											and tare.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	size			on	i.itemID = size.itemID
											and size.traitID = @retailSizeID
											and size.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	uom				on	i.itemID = uom.itemID
											and uom.traitID = @retailUomID
											and uom.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	ds				on	i.itemID = ds.itemID
											and ds.traitID = @deliverySystemID
											and ds.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	vld				on	i.itemID = vld.itemID
											and vld.traitID = @validationDateTraitID
											and vld.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait dept			on	i.itemID = dept.itemID
											and dept.traitID = @departmentSaleTraitID
											and dept.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait hid				on	i.itemID = hid.itemID
											and hid.traitID = @hiddenItemTraitID
											and hid.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	note			on	i.itemID = note.itemID
											and note.traitID = @notesTraitId
											and note.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	crdate			on	i.itemID = crdate.itemID
											and crdate.traitID = @createdDateTraitID
											and crdate.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	moddate			on	i.itemID = moddate.itemID
											and moddate.traitID = @modifiedDateTraitID
											and moddate.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait	modusr			on	i.itemID = modusr.itemID
											and modusr.traitID = @modifiedUserTraitID
											and modusr.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait dwu				on	i.itemID = dwu.itemID
											and dwu.traitID = @dwuTraitID
											and dwu.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait cf				on	i.itemID = cf.itemID
											and cf.traitID = @cfTraitID
											and cf.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait ftc				on	i.itemID = ftc.itemID
											and ftc.traitID = @ftcTraitID
											and ftc.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait hem				on	i.itemID = hem.itemID
											and hem.traitID = @hemTraitID
											and hem.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait opc				on	i.itemID = opc.itemID
											and opc.traitID = @opcTraitID
											and opc.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait nr				on	i.itemID = nr.itemID
											and nr.traitID = @nrTraitID
											and nr.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait dw				on	i.itemID = dw.itemID
											and dw.traitID = @dwTraitID
											and dw.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait abv				on	i.itemID = abv.itemID
											and abv.traitID = @abvTraitID
											and abv.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait mpn				on	i.itemID = mpn.itemID
											and mpn.traitID = @mpnTraitID
											and mpn.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait pft				on	i.itemID = pft.itemID
											and pft.traitID = @pftTraitID
											and pft.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait plo				on	i.itemID = plo.itemID
											and plo.traitID = @ploTraitID
											and plo.localeID = @wholeFoodsLocale
	LEFT JOIN ItemTrait llp				on	i.itemID = llp.itemID
											and llp.traitID = @llpTraitID
											and llp.localeID = @wholeFoodsLocale
	LEFT JOIN Brand_CTE	brand			on	i.itemID = brand.itemID
	LEFT JOIN Tax_CTE	tax				on	i.itemID = tax.itemID
	LEFT JOIN Merch_CTE	merch			on	i.itemID = merch.itemID
	LEFT JOIN National_CTE nat			on	i.itemID = nat.itemID		
	LEFT JOIN ItemSignAttribute isa		on	i.itemID = isa.ItemID'
		
	-- ====================================================
	-- Set WHERE statement
	-- ====================================================
	IF NOT ISNULL(@scanCode, '') = ''
	BEGIN
		IF @partialScanCodeSearch = 1
			SET @whereSql = 'sc.scanCode like ''%'' + @scanCode + ''%'' '
		ELSE
			SET @whereSql = 'sc.scanCode like @scanCode + ''%'' '
	END
	IF NOT ISNULL(@brandName, '') = ''
	BEGIN
		IF @partialBrandName = 1
			SET @whereSql = @whereSql + 'and brand.hierarchyClassName like ''%'' + @brandName + ''%''  '
		ELSE
			SET @whereSql = @whereSql + 'and @brandName = brand.hierarchyClassName '			
	END
	IF NOT ISNULL(@merchandiseHierarchy, '') = ''
		SET @whereSql = @whereSql + 'and merch.hierarchyClassName like ''%'' + @merchandiseHierarchy + ''%''  '
	IF NOT ISNULL(@taxRomanceName, '') = ''
		SET @whereSql = @whereSql + 'and tax.taxRomance like ''%'' + @taxRomanceName + ''%'' '
	IF NOT ISNULL(@productDescription, '') = ''
		SET @whereSql = @whereSql + 'and pd.traitValue like ''%'' + @productDescription + ''%'' '
	IF NOT ISNULL(@posDescription, '') = ''
		SET @whereSql = @whereSql + 'and pos.traitValue like ''%'' + @posDescription + ''%'' '
	IF NOT ISNULL(@retailSize, '') = ''
		SET @whereSql = @whereSql + 'and @retailSize = size.traitValue '
	IF NOT ISNULL(@retailUom, '') = ''
		SET @whereSql = @whereSql + 'and @retailUom = uom.traitValue '
	IF NOT ISNULL(@deliverySystem, '') = ''
		SET @whereSql = @whereSql + 'and @deliverySystem = ds.traitValue '
	IF NOT ISNULL(@posScaleTare, '') = ''
		SET @whereSql = @whereSql + 'and @posScaleTare = tare.traitValue '
	IF NOT ISNULL(@foodStampEligible, '') = ''
	BEGIN
		IF @foodStampEligible = 'yes'
			SET @whereSql = @whereSql + 'and fs.traitValue = ''1'' '
		ELSE 
			SET @whereSql = @whereSql + 'and fs.traitValue = ''0'' '
	END
	IF NOT ISNULL(@pkgUnit, '') = ''
		SET @whereSql = @whereSql + 'and @pkgUnit = pack.traitValue '
	IF NOT ISNULL(@departmentSale, '') = ''
	BEGIN
		IF @departmentSale = 'yes'
			SET @whereSql = @whereSql + 'and dept.traitID IS NOT NULL and dept.traitValue = ''1'' '
		ELSE 
			SET @whereSql = @whereSql + 'and dept.traitValue IS NULL '
	END
	IF NOT ISNULL(@itemStatus, '') = ''
	BEGIN
		IF @itemStatus = 'loaded'
			SET @whereSql = @whereSql + 'and vld.traitID IS NULL '
		ELSE IF @itemStatus = 'validated'
			SET @whereSql = @whereSql + 'and vld.traitID IS NOT NULL '
	END
	IF NOT ISNULL(@hiddenItemStatus, '') = ''
	BEGIN
		IF @hiddenItemStatus = 'hidden'
			SET @whereSql = @whereSql + 'and hid.traitID IS NOT NULL and hid.traitValue = ''1'' '
		ELSE IF @hiddenItemStatus = 'visible'
			SET @whereSql = @whereSql + 'and (hid.traitID IS NULL or hid.traitID IS NOT NULL and hid.traitValue = ''0'') '
	END
	IF NOT ISNULL(@nationalHierarchy, '') = ''
		SET @whereSql = @whereSql + 'and nat.hierarchyLineage like ''%'' + @nationalHierarchy + ''%'' '
	IF NOT ISNULL(@notes, '') = ''
		SET @whereSql = @whereSql + 'and note.traitValue like ''%'' + @notes + ''%'' '
	IF NOT ISNULL(@dwu, '') = ''
		SET @whereSql = @whereSql + 'and dwu.traitValue = @dwu '
	IF NOT ISNULL(@cf, '') = ''
		SET @whereSql = @whereSql + 'and cf.traitValue = @cf '
	IF NOT ISNULL(@ftc, '') = ''
		SET @whereSql = @whereSql + 'and ftc.traitValue = @ftc '
	IF NOT ISNULL(@hem, '') = ''
		SET @whereSql = @whereSql + 'and hem.traitValue = @hem '
	IF NOT ISNULL(@opc, '') = ''
		SET @whereSql = @whereSql + 'and opc.traitValue = @opc '
	IF NOT ISNULL(@nr, '') = ''
		SET @whereSql = @whereSql + 'and nr.traitValue = @nr '
	IF NOT ISNULL(@dw, '') = ''
		SET @whereSql = @whereSql + 'and dw.traitValue = @dw '
	IF NOT ISNULL(@abv, '') = ''
		SET @whereSql = @whereSql + 'and abv.traitValue = @abv '
	IF NOT ISNULL(@mpn, '') = ''
		SET @whereSql = @whereSql + 'and mpn.traitValue like ''%'' + @mpn + ''%'' '
	IF NOT ISNULL(@pft, '') = ''
		SET @whereSql = @whereSql + 'and pft.traitValue like ''%'' + @pft + ''%'' '
	IF NOT ISNULL(@plo, '') = ''
		SET @whereSql = @whereSql + 'and plo.traitValue = @plo '
	IF NOT ISNULL(@llp, '') = ''
		SET @whereSql = @whereSql + 'and llp.traitValue = @llp '
	IF NOT ISNULL(@createDate, '') = ''
		SET @whereSql = @whereSql + 'and crdate.traitValue like ''%'' + @createDate + ''%'' '
	IF NOT ISNULL(@modifiedDate, '') = ''
		SET @whereSql = @whereSql + 'and moddate.traitValue like ''%'' + @modifiedDate + ''%'' '
	IF NOT ISNULL(@modifiedUser, '') = ''
		SET @whereSql = @whereSql + 'and modusr.traitValue like ''%'' + @modifiedUser + ''%'' '
	IF NOT ISNULL(@animalWelfareRatingId, '') = ''
		SET @whereSql = @whereSql + 'and @animalWelfareRatingId = isa.AnimalWelfareRatingId '
	IF NOT ISNULL(@biodynamic, '') = ''
	BEGIN
		IF @biodynamic = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.Biodynamic '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.Biodynamic '
	END
	IF NOT ISNULL(@milkTypeId, '') = ''
		SET @whereSql = @whereSql + 'and @milkTypeId = isa.CheeseMilkTypeId '
	IF NOT ISNULL(@cheeseRaw, '') = ''
	BEGIN
		IF @cheeseRaw = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.CheeseRaw '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.CheeseRaw '
	END
	IF NOT ISNULL(@ecoScaleRatingId, '') = ''
		SET @whereSql = @whereSql + 'and @ecoScaleRatingId = isa.EcoScaleRatingId '
	IF NOT ISNULL(@glutenFreeAgency, '') = ''
		SET @whereSql = @whereSql + 'and @glutenFreeAgency = ISA.GlutenFreeAgencyName '
	IF NOT ISNULL(@kosherAgency, '') = ''
		SET @whereSql = @whereSql + 'and @kosherAgency = ISA.KosherAgencyName '
	IF NOT ISNULL(@nonGmoAgency, '') = ''
		SET @whereSql = @whereSql + 'and @nonGmoAgency = ISA.NonGmoAgencyName '
	IF NOT ISNULL(@organicAgency, '') = ''
		SET @whereSql = @whereSql + 'and @organicAgency = ISA.OrganicAgencyName '
	IF NOT ISNULL(@premiumBodyCare, '') = ''
	BEGIN
		IF @premiumBodyCare = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.PremiumBodyCare '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.PremiumBodyCare '
	END
	IF NOT ISNULL(@seafoodFreshOrFrozenId, '') = ''
		SET @whereSql = @whereSql + 'and @seafoodFreshOrFrozenId = isa.SeafoodFreshOrFrozenId '
	IF NOT ISNULL(@seafoodCatchTypeId, '') = ''
		SET @whereSql = @whereSql + 'and @seafoodCatchTypeId = isa.SeafoodCatchTypeId '
	IF NOT ISNULL(@veganAgency, '') = ''
		SET @whereSql = @whereSql + 'and @veganAgency = ISA.VeganAgencyName '
	IF NOT ISNULL(@vegetarian, '') = ''
	BEGIN
		IF @vegetarian = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.Vegetarian '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.Vegetarian '
	END
	IF NOT ISNULL(@wholeTrade, '') = ''
	BEGIN
		IF @wholeTrade = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.WholeTrade '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.WholeTrade '
	END
	IF NOT ISNULL(@isGrassFed, '') = ''
	BEGIN
		IF @isGrassFed = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.GrassFed '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.GrassFed '
	END
	IF NOT ISNULL(@isPastureRaised, '') = ''
	BEGIN
		IF @isPastureRaised = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.PastureRaised '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.PastureRaised '
	END
	IF NOT ISNULL(@isFreeRange, '') = ''
	BEGIN
		IF @isFreeRange = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.FreeRange '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.FreeRange '
	END
	IF NOT ISNULL(@isDryAged, '') = ''
	BEGIN
		IF @isDryAged = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.DryAged '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.DryAged '
	END
	IF NOT ISNULL(@isAirChilled, '') = ''
	BEGIN
		IF @isAirChilled = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.AirChilled '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.AirChilled '
	END
	IF NOT ISNULL(@isMadeInHouse, '') = ''
	BEGIN
		IF @isMadeInHouse = 'yes'
			SET @whereSql = @whereSql + 'and 1 = isa.MadeinHouse '
		ELSE
			SET @whereSql = @whereSql + 'and 0 = isa.MadeinHouse '
	END
	IF NOT ISNULL(@isMsc, '') = ''
	BEGIN
		IF @isMsc = 'yes'
			SET @whereSql = @whereSql + 'and isa.Msc = 1 '
		ELSE
			SET @whereSql = @whereSql + 'and isa.Msc = 0 '
	END
	IF NOT LEN(@whereSql) = 0 
	BEGIN
		IF @whereSql like 'and %'
			SET @whereSql = 'WHERE ' + SUBSTRING(@whereSql, 5, LEN(@whereSql))
		ELSE 
			SET @whereSql = 'WHERE ' + @whereSql
	END
		
	-- ====================================================
	-- Set ORDER BY statement
	-- ====================================================
	IF @sortColumn IS NOT NULL AND @sortOrder IS NOT NULL
	BEGIN
		SET @orderBySql = ' ORDER BY ' + @sortColumn + ' ' + @sortOrder
	END
	ELSE
	BEGIN
		SET @orderBySql = ' ORDER BY sc.scanCode'
	END
	IF @pageIndex IS NOT NULL AND @pageSize IS NOT NULL
	BEGIN 
		SET @orderBySql = @orderBySql + ' OFFSET @pageSize * (@pageIndex) ROWS FETCH NEXT @pageSize ROWS ONLY'
	END

	-- ====================================================
	-- Create complete SQL statement and parameter definitions
	-- ====================================================
	IF @getOnlyCount = 0
	BEGIN
		DECLARE @sql NVARCHAR(max) = CONCAT(@cteSql, @selectItemsSql, @fromSql, @whereSql, @orderBySql)
	END
	ELSE
	BEGIN
		SET @sql = CONCAT(@cteSql, @selectCountSql, @fromSql, @whereSql)		
	END

	DECLARE @paramDef nvarchar(max) = 
		N'@wholeFoodsLocale int,
		@productDescriptionTraitID int,
		@posTraitID int,
		@packageUnitTraitID int,
		@foodStampEligibleTraitID int,
		@departmentSaleTraitID int,
		@validationDateTraitID int,
		@retailSizeID int,
		@retailUomID int,
		@deliverySystemID int,
		@scaleTareID int,
		@taxRomanceTraitID int,
		@hiddenItemTraitID int,
		@notesTraitId int,
		@dwuTraitID int,
		@cfTraitID int,
		@ftcTraitID int,
		@hemTraitID int,
		@opcTraitID int,
		@nrTraitID int,
		@dwTraitID int,
		@abvTraitID int,
		@mpnTraitID int,
		@pftTraitID int,
		@ploTraitID int,
		@llpTraitID int,
		@createdDateTraitID int,
		@modifiedDateTraitID int,
		@modifiedUserTraitID int,
		@brandHierarchyID int,
		@merchandiseClassID int, 
		@taxClassID int,
		@nationalHierarchyID int,
		@certificationAgencyHierarchyID int,
		@nationalClassCodeTraitID int,
		@scanCode nvarchar(13),
		@merchandiseHierarchy varchar(255), 
		@taxRomanceName varchar(255), 
		@brandName varchar(255), 
		@productDescription varchar(255),
		@retailSize varchar(255),
		@retailUom varchar(255),
		@deliverySystem varchar(255),
		@posDescription varchar(255), 
		@pkgUnit varchar(255),
		@posScaleTare varchar(255),
		@foodStampEligible varchar(3),
		@departmentSale varchar(3),
		@itemStatus varchar(30),
		@partialScanCodeSearch bit = 0,
		@partialBrandName bit = 0,
		@hiddenItemStatus varchar(10),
		@nationalHierarchy varchar(255),
		@dwu varchar(255),
		@cf varchar(255),
		@ftc varchar(255),
		@hem varchar(255),
		@opc varchar(255),
		@nr varchar(255),
		@dw varchar(255),
		@abv varchar(255),
		@mpn varchar(255),
		@pft varchar(255),
		@plo varchar(255),
		@llp varchar(255),
		@animalWelfareRatingId int,
		@biodynamic varchar(3),
		@milkTypeId int,
		@cheeseRaw varchar(3),
		@ecoScaleRatingId int,
		@glutenFreeAgency varchar(255),
		@kosherAgency varchar(255),
		@isMsc varchar(3),
		@nonGmoAgency varchar(255),
		@organicAgency varchar(255),
		@premiumBodyCare varchar(3),
		@seafoodFreshOrFrozenId int,
		@seafoodCatchTypeId int,
		@veganAgency varchar(255),
		@vegetarian varchar(3),
		@wholeTrade varchar(3),
		@notes varchar(255),
		@isGrassFed varchar(3),
		@isPastureRaised varchar(3),
		@isFreeRange varchar(3),
		@isDryAged varchar(3),
		@isAirChilled varchar(3),
		@isMadeInHouse varchar(3),
		@createDate varchar(255),
		@modifiedDate varchar(255),
		@modifiedUser varchar(255),
		@pageSize int,
		@pageIndex int,
		@sortColumn nvarchar(255),
		@sortOrder nvarchar(4)'
		
		print @whereSql
	-- ====================================================
	-- Execute SQL for Items
	-- ====================================================
	EXECUTE sp_executesql 
		@sql, 
		@paramDef, 
		@wholeFoodsLocale, 
		@productDescriptionTraitID, 
		@posTraitID,
		@packageUnitTraitID, 
		@foodStampEligibleTraitID, 
		@departmentSaleTraitID, 
		@validationDateTraitID,
		@retailSizeID, 
		@retailUomID,
		@deliverySystemID, 
		@scaleTareID, 
		@taxRomanceTraitID, 
		@hiddenItemTraitID, 
		@notesTraitId,	
		@dwuTraitID,
		@cfTraitID,
		@ftcTraitID,
		@hemTraitID,
		@opcTraitID, 
		@nrTraitID,
		@dwTraitID,
		@abvTraitID,
		@mpnTraitID, 
		@pftTraitID,
		@ploTraitID,
		@llpTraitID,
		@createdDateTraitID,
		@modifiedDateTraitID, 
		@modifiedUserTraitID, 
		@brandHierarchyID, 
		@merchandiseClassID, 
		@taxClassID,
		@nationalHierarchyID, 
		@certificationAgencyHierarchyID, 
		@nationalClassCodeTraitID,
		@scanCode, 
		@merchandiseHierarchy, 
		@taxRomanceName, 
		@brandName, 
		@productDescription,
		@retailSize,
		@retailUom,
		@deliverySystem,
		@posDescription, 
		@pkgUnit,
		@posScaleTare,
		@foodStampEligible,
		@departmentSale,
		@itemStatus,
		@partialScanCodeSearch,
		@partialBrandName,
		@hiddenItemStatus,
		@nationalHierarchy,
		@dwu,
		@cf,
		@ftc,
		@hem,
		@opc,
		@nr,
		@dw,
		@abv,
		@mpn,
		@pft,
		@plo,
		@llp,
		@animalWelfareRatingId,
		@biodynamic,
		@milkTypeId,
		@cheeseRaw,
		@ecoScaleRatingId,
		@glutenFreeAgency,
		@kosherAgency,
		@isMsc,
		@nonGmoAgency,
		@organicAgency,
		@premiumBodyCare,
		@seafoodFreshOrFrozenId,
		@seafoodCatchTypeId,
		@veganAgency,
		@vegetarian,
		@wholeTrade,
		@notes,
		@isGrassFed,
		@isPastureRaised,
		@isFreeRange,
		@isDryAged,
		@isAirChilled,
		@isMadeInHouse,
		@createDate,
		@modifiedDate,
		@modifiedUser,
		@pageSize,
		@pageIndex,
		@sortColumn,
		@sortOrder;

END