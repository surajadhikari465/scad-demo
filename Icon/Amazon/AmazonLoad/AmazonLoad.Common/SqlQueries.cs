namespace AmazonLoad.Common
{
    public static class SqlQueries
    {
        public static string HierarchySql
        {
            get
            {
                return @"
                    select
		                h.hierarchyID as HierarchyId,
		                h.hierarchyName as HierarchyName,
		                hp.hierarchyLevelName as HierarchyLevelName,
		                hp.itemsAttached as ItemsAttached,
		                case
			                when @HierarchyName = 'Financial' then substring(hc.hierarchyClassName, charindex('(', hc.hierarchyClassName) + 1, 4)
			                else cast(hc.hierarchyClassID as nvarchar(32)) 
		                end as HierarchyClassId,
		                hc.hierarchyClassName as HierarchyClassName,		
		                hc.hierarchyLevel as HierarchyLevel,
		                hc.hierarchyParentClassID as HierarchyParentClassId
	                from
		                Hierarchy h
		                join HierarchyClass hc on h.hierarchyID = hc.hierarchyID
		                join HierarchyPrototype hp on hc.hierarchyID = hp.hierarchyID and hc.hierarchyLevel = hp.hierarchyLevel
                    where
		                h.hierarchyName = @HierarchyName
                    order by hc.hierarchyLevel";
            }
        }

        public static string ItemSql
        {
            get
            {
                return @"
                    declare
		                    @localeID int,
		                    @productDescriptionTraitID int,
		                    @posTraitID int,
		                    @packageUnitTraitID int,
		                    @foodStampEligibleTraitID int,
		                    @departmentSaleTraitID int,
		                    @brandHierarchyID int,
		                    @browsingClassID int,
		                    @merchandiseClassID int,
		                    @financialClassID int,
		                    @taxClassID int,
		                    @nationalClassID int,
		                    @validationDateTraitID int,
		                    @sentToEsbTraitID int,
		                    @readyMessageStatusID int,
		                    @stagedMessageStatusID int,
		                    @productMessageTypeID int,
		                    @merchFinMappingTraitID int,
		                    @prohibitDiscountTraitID int,
		                    @retailSizeID int,
		                    @retailUomID int,
		                    @couponItemTypeId int,
		                    @nonRetailItemTypeId int,
		                    @cfdTraitId int,
		                    @nrTraitId int,
		                    @gppTraitId int,
		                    @ftcTraitId int,
		                    @fxtTraitId int,
		                    @mogTraitId int,
		                    @prbTraitId int,
		                    @rfaTraitId int,
                            @rfdTraitId int,
                            @sbfTraitId int,
                            @wicTraitId int,
                            @shelfLife int,
                            @itgTraitId int,
		                    @hidTraitId int

	                    SET @localeID					= (SELECT l.localeID FROM Locale l WHERE l.localeName = 'Whole Foods')
	                    SET @productDescriptionTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PRD')
	                    SET @posTraitID					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'POS')
	                    SET @packageUnitTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PKG')
	                    SET @foodStampEligibleTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'FSE')
	                    SET @departmentSaleTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DPT')
	                    SET @brandHierarchyID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Brands')
	                    SET @browsingClassID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Browsing')
	                    SET @merchandiseClassID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
	                    SET @financialClassID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Financial')
	                    SET @taxClassID					= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax')
                        SET @nationalClassID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'National')
	                    SET @validationDateTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'VAL')
	                    SET @sentToEsbTraitID			= (SELECT traitID FROM Trait WHERE traitCode = 'ESB')
	                    SET @readyMessageStatusID		= (SELECT s.MessageStatusId FROM app.MessageStatus s WHERE s.MessageStatusName = 'Ready')
	                    SET @stagedMessageStatusID		= (SELECT s.MessageStatusId FROM app.MessageStatus s WHERE s.MessageStatusName = 'Staged')
	                    SET @productMessageTypeID		= (SELECT t.MessageTypeId FROM app.MessageType t WHERE t.MessageTypeName = 'Product')
	                    SET @merchFinMappingTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MFM')
	                    SET @prohibitDiscountTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PRH')
	                    SET @retailSizeID				= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'RSZ')
	                    SET @retailUomID				= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'RUM')
	                    SET @couponItemTypeId			= (SELECT tp.itemTypeID FROM ItemType tp WHERE tp.itemTypeCode = 'CPN')
	                    SET @nonRetailItemTypeId		= (SELECT tp.itemTypeID FROM ItemType tp WHERE tp.itemTypeCode = 'NRT')
	                    SET @cfdTraitId					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'CFD')
	                    SET @nrTraitId					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NR')
	                    SET @gppTraitId					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'GPP')
	                    SET	@ftcTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'FTC')
                        SET @fxtTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'FXT')
                        SET @mogTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'MOG')
                        SET @prbTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PRB')
                        SET @rfaTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'RFA')
                        SET @rfdTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'RFD')
                        SET @sbfTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'SMF')
                        SET @wicTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'WIC')
                        SET @shelfLife				    = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'SLF')
                        SET @itgTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'ITG')
	                    SET @hidTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'HID')

	                    select top 1
		                    i.itemID							as ItemId,
		                    @localeID							as LocaleId,
		                    it.itemTypeCode						as ItemTypeCode,
		                    it.itemTypeDesc						as ItemTypeDesc,
		                    sc.scanCodeID						as ScanCodeId,
		                    sc.scanCode							as ScanCode,
		                    sct.scanCodeTypeID					as ScanCodeTypeId,
		                    sct.scanCodeTypeDesc				as ScanCodeTypeDesc,
		                    prd.traitValue						as ProductDescription,
		                    pos.traitValue						as PosDescription,
		                    pkg.traitValue						as PackageUnit,
		                    rsz.traitValue						as RetailSize,
		                    rum.traitValue						as RetailUom,
		                    fse.traitValue						as FoodStampEligible,
		                    CAST(isnull(prh.traitValue, 0) AS BIT)			as ProhibitDiscount,	
		                    isnull(ds.traitValue, '0')			as DepartmentSale,
		                    brandhc.hierarchyClassID			as BrandId,
		                    brandhc.hierarchyClassName			as BrandName,
		                    brandhc.hierarchyLevel				as BrandLevel,
		                    brandhc.hierarchyParentClassID		as BrandParentId,
		                    null								as BrowsingClassId,
		                    null								as BrowsingClassName,
		                    null								as BrowsingLevel,
		                    null								as BrowsingParentId,
		                    merchhc.hierarchyClassID			as MerchandiseClassId,
		                    merchhc.hierarchyClassName			as MerchandiseClassName,
		                    merchhc.hierarchyLevel				as MerchandiseLevel,
		                    merchhc.hierarchyParentClassID		as MerchandiseParentId,
		                    taxhc.hierarchyClassID				as TaxClassId,
		                    taxhc.hierarchyClassName			as TaxClassName,
		                    taxhc.hierarchyLevel				as TaxLevel,
		                    taxhc.hierarchyParentClassID		as TaxParentId,
		                    substring(finhc.hierarchyClassName, charindex('(', finhc.hierarchyClassName) + 1, 4)
											                    as FinancialClassId,
		                    case
			                    when substring(finhc.hierarchyClassName, charindex('(', finhc.hierarchyClassName) + 1, 4) = '0000' then 'na'
			                    else finhc.hierarchyClassName
		                    end									as FinancialClassName,
		                    finhc.hierarchyLevel				as FinancialLevel,
		                    finhc.hierarchyParentClassID		as FinancialParentId,
		                    aw.Description						AS [AnimalWelfareRating],
		                    ia.[Biodynamic]						AS [Biodynamic],
		                    cm.Description						AS [CheeseMilkType],
		                    ia.[CheeseRaw]						AS [CheeseRaw],
		                    es.Description						AS [EcoScaleRating],
		                    ia.GlutenFreeAgencyName				AS [GlutenFreeAgency],
		                    he.Description						AS [HealthyEatingRating],
		                    ia.KosherAgencyName					AS [KosherAgency], 
		                    ia.[Msc]							As [Msc],
		                    ia.NonGmoAgencyName					AS [NonGmoAgency],
		                    ia.OrganicAgencyName				AS [OrganicAgency],
		                    ia.[PremiumBodyCare]				AS [PremiumBodyCare],
		                    sff.Description						AS [SeafoodFreshOrFrozen],
		                    sfc.Description						AS [SeafoodCatchType],
		                    ia.VeganAgencyName					AS [VeganAgency],
		                    ia.[Vegetarian]						AS [Vegetarian],
		                    ia.[WholeTrade]						AS [WholeTrade],
		                    ia.[GrassFed]						AS [GrassFed],
		                    ia.[PastureRaised]					AS [PastureRaised],
		                    ia.[FreeRange]						AS [FreeRange],
		                    ia.[DryAged]						AS [DryAged],
		                    ia.[AirChilled]						AS [AirChilled],
		                    ia.[MadeInHouse]					AS [MadeInHouse],
		                    CASE WHEN ISNULL(ia.CustomerFriendlyDescription,'') = '' THEN prd.traitValue	
			                     ELSE ia.CustomerFriendlyDescription END AS CustomerFriendlyDescription,
		                    nr.traitValue						AS NutritionRequired,
		                    gpp.traitValue						AS GlobalPricingProgram,
		                    itg.traitValue						AS SelfCheckoutItemTareGroup,
		                    fxt.traitValue						AS FlexibleText,
		                    slf.traitValue						AS ShelfLife,
		                    ftc.traitValue						AS FairTradeCertified,		
		                    mog.traitValue						AS MadeWithOrganicGrapes,
		                    CASE WHEN prb.traitValue = '1'    THEN 1  
			                     WHEN prb.traitValue = 'True' THEN 1  
			                     WHEN prb.traitValue = 'Yes'  THEN 1 
			                     ELSE 0 END						AS PrimeBeef,
		                    CASE WHEN rfa.traitValue = '1'    THEN 1  
			                     WHEN rfa.traitValue = 'True' THEN 1  
			                     WHEN rfa.traitValue = 'Yes'  THEN 1 
			                     ELSE 0 END						AS RainforestAlliance,
		                    rfd.traitValue						AS Refigerated,
		                    CASE WHEN smf.traitValue = '1'    THEN 1  
			                     WHEN smf.traitValue = 'True' THEN 1  
			                     WHEN smf.traitValue = 'Yes'  THEN 1 
			                     ELSE 0 END						AS SmithsonianBirdFriendly,
		                    CASE WHEN wic.traitValue = '1'    THEN 1  
			                     WHEN wic.traitValue = 'True' THEN 1  
			                     WHEN wic.traitValue = 'Yes'  THEN 1 
			                     ELSE 0 END					AS WicEligible,
		                    nathc.hierarchyClassID			AS NationalClassId,
		                    nathc.hierarchyClassName		AS NationalClassName,
		                    nathc.hierarchyLevel			AS NationalLevel,
		                    nathc.hierarchyParentClassID	AS NationalParentId,
		                    CAST(ISNULL(hid.traitValue, '0') AS BIT) AS Hidden
                            ,[Plu] AS [Plu]
		                    ,[RecipeName] AS [RecipeName]
		                    ,[Allergens] AS [Allergens]
		                    ,[Ingredients] AS [Ingredients]
		                    ,[ServingsPerPortion] AS [ServingsPerPortion]
		                    ,[ServingSizeDesc] AS [ServingSizeDesc]
		                    ,[ServingPerContainer] AS [ServingPerContainer]
		                    ,[HshRating] AS [HshRating]
		                    ,[ServingUnits] AS [ServingUnits]
		                    ,[SizeWeight] AS [SizeWeight]
		                    ,[Calories] AS [Calories]
		                    ,[CaloriesFat] AS [CaloriesFat]
		                    ,[CaloriesSaturatedFat] AS [CaloriesSaturatedFat]
		                    ,[TotalFatWeight] AS [TotalFatWeight]
		                    ,[TotalFatPercentage] AS [TotalFatPercentage]
		                    ,[SaturatedFatWeight] AS [SaturatedFatWeight]
		                    ,[SaturatedFatPercent] AS [SaturatedFatPercent]
		                    ,[PolyunsaturatedFat] AS [PolyunsaturatedFat]
		                    ,[MonounsaturatedFat] AS [MonounsaturatedFat]
		                    ,[CholesterolWeight] AS [CholesterolWeight]
		                    ,[CholesterolPercent] AS [CholesterolPercent]
		                    ,[SodiumWeight] AS [SodiumWeight]
		                    ,[SodiumPercent] AS [SodiumPercent]
		                    ,[PotassiumWeight] AS [PotassiumWeight]
		                    ,[PotassiumPercent] AS [PotassiumPercent]
		                    ,[TotalCarbohydrateWeight] AS [TotalCarbohydrateWeight]
		                    ,[TotalCarbohydratePercent] AS [TotalCarbohydratePercent]
		                    ,[DietaryFiberWeight] AS [DietaryFiberWeight]
		                    ,[DietaryFiberPercent] AS [DietaryFiberPercent]
		                    ,[SolubleFiber] AS [SolubleFiber]
		                    ,[InsolubleFiber] AS [InsolubleFiber]
		                    ,[Sugar] AS [Sugar]
		                    ,[SugarAlcohol] AS [SugarAlcohol]
		                    ,[OtherCarbohydrates] AS [OtherCarbohydrates]
		                    ,[ProteinWeight] AS [ProteinWeight]
		                    ,[ProteinPercent] AS [ProteinPercent]
		                    ,[VitaminA] AS [VitaminA]
		                    ,[Betacarotene] AS [Betacarotene]
		                    ,[VitaminC] AS [VitaminC]
		                    ,[Calcium] AS [Calcium]
		                    ,[Iron] AS [Iron]
		                    ,[VitaminD] AS [VitaminD]
		                    ,[VitaminE] AS [VitaminE]
		                    ,[Thiamin] AS [Thiamin]
		                    ,[Riboflavin] AS [Riboflavin]
		                    ,[Niacin] AS [Niacin]
		                    ,[VitaminB6] AS [VitaminB6]
		                    ,[Folate] AS [Folate]
		                    ,[VitaminB12] AS [VitaminB12]
		                    ,[Biotin] AS [Biotin]
		                    ,[PantothenicAcid] AS [PantothenicAcid]
		                    ,[Phosphorous] AS [Phosphorous]
		                    ,[Iodine] AS [Iodine]
		                    ,[Magnesium] AS [Magnesium]
		                    ,[Zinc] AS [Zinc]
		                    ,[Copper] AS [Copper]
		                    ,[Transfat] AS [Transfat]
		                    ,[CaloriesFromTransfat] AS [CaloriesFromTransfat]
		                    ,[Om6Fatty] AS [Om6Fatty]
		                    ,[Om3Fatty] AS [Om3Fatty]
		                    ,[Starch] AS [Starch]
		                    ,[Chloride] AS [Chloride]
		                    ,[Chromium] AS [Chromium]
		                    ,[VitaminK] AS [VitaminK]
		                    ,[Manganese] AS [Manganese]
		                    ,[Molybdenum] AS [Molybdenum]
		                    ,[Selenium] AS [Selenium]
		                    ,[TransfatWeight] AS [TransfatWeight]
		                    ,0 AS [HazardousMaterialFlag]
		                    ,NULL AS [HazardousMaterialTypeCode]
	                    from 
		                    Item    						i
		                    JOIN ItemType					it			ON	i.itemTypeID				= it.itemTypeID
		                    JOIN ScanCode					sc			ON	i.itemID					= sc.itemID
		                    JOIN ScanCodeType				sct			ON	sc.scanCodeTypeID			= sct.scanCodeTypeID
		                    JOIN ItemTrait					val			ON	i.itemID					= val.itemID
														                    AND val.traitID				= @validationDateTraitID
														                    AND val.localeID			= @localeID
		                    JOIN ItemTrait					prd			ON	i.itemID					= prd.itemID
														                    AND prd.traitID				= @productDescriptionTraitID
														                    AND prd.localeID			= @localeID
		                    JOIN ItemTrait					pos			ON	i.itemID					= pos.itemID
														                    AND pos.traitID				= @posTraitID
														                    AND pos.localeID			= @localeID
		                    JOIN ItemTrait					pkg			ON	i.itemID					= pkg.itemID
														                    AND pkg.traitID				= @packageUnitTraitID
														                    AND pkg.localeID			= @localeID
		                    JOIN ItemTrait					rsz			ON	i.itemID					= rsz.itemID
														                    AND rsz.traitID				= @retailSizeID
														                    AND rsz.localeID			= @localeID
		                    JOIN ItemTrait					rum			ON  i.itemID					= rum.itemID
														                    AND rum.traitID				= @retailUomID
														                    AND rum.localeID			= @localeID
		                    JOIN ItemTrait					fse			ON	i.itemID					= fse.itemID
														                    AND fse.traitID				= @foodStampEligibleTraitID
														                    AND fse.localeID			= @localeID
		                    LEFT JOIN ItemTrait				prh			ON	i.itemID					= prh.itemID
														                    AND prh.traitID				= @prohibitDiscountTraitID
														                    AND prh.localeID			= @localeID
		                    JOIN ItemHierarchyClass			brandihc	ON	i.itemID					= brandihc.itemID
		                    JOIN HierarchyClass				brandhc		ON	brandihc.hierarchyClassID	= brandhc.hierarchyClassID
														                    AND brandhc.hierarchyID		= @brandHierarchyID
		                    LEFT JOIN HierarchyClassTrait	brandhctesb	ON	brandhc.hierarchyClassID	= brandhctesb.hierarchyClassID
														                    AND brandhctesb.traitID		= @sentToEsbTraitID
		                    JOIN ItemHierarchyClass			merchihc	ON	i.itemID					= merchihc.itemID
		                    JOIN HierarchyClass				merchhc		ON	merchihc.hierarchyClassID	= merchhc.hierarchyClassID
														                    AND merchhc.hierarchyID		= @merchandiseClassID
		                    LEFT JOIN HierarchyClassTrait	merchhctesb	ON	merchhc.hierarchyClassID	= merchhctesb.hierarchyClassID
														                    AND merchhctesb.traitID		= @sentToEsbTraitID
		
		                    JOIN ItemHierarchyClass			finihc		ON	i.itemID					= finihc.itemID
		                    JOIN HierarchyClass				finhc		ON	finihc.hierarchyClassID		= finhc.hierarchyClassID
														                    AND finhc.hierarchyID		= @financialClassID	
		                    LEFT JOIN HierarchyClassTrait	finhctesb	ON	finhc.hierarchyClassID		= finhctesb.hierarchyClassID
														                    AND finhctesb.traitID		= @sentToEsbTraitID
		                    JOIN ItemHierarchyClass			taxihc		ON	i.itemID					= taxihc.itemID
		                    JOIN HierarchyClass				taxhc		ON	taxihc.hierarchyClassID		= taxhc.hierarchyClassID
														                    AND taxhc.hierarchyID		= @taxClassID
		                    LEFT JOIN HierarchyClassTrait	taxhctesb	ON	taxhc.hierarchyClassID		= taxhctesb.hierarchyClassID
														                    AND taxhctesb.traitID		= @sentToEsbTraitID
		                    JOIN ItemHierarchyClass			natihc		ON	i.itemID					= natihc.itemID
		                    JOIN HierarchyClass				nathc		ON	natihc.hierarchyClassID		= nathc.hierarchyClassID
															                    AND nathc.hierarchyID	= @nationalClassID
		                    LEFT JOIN ItemTrait				ds			ON	i.itemID					= ds.itemID
														                    AND ds.traitID				= @departmentSaleTraitID
														                    AND ds.localeID				= @localeID
		                    LEFT JOIN ItemSignAttribute		ia			ON	i.itemID					= ia.itemID
		                    LEFT JOIN AnimalWelfareRating	aw			ON ia.AnimalWelfareRatingId		= aw.AnimalWelfareRatingId
		                    LEFT JOIN MilkType				cm			ON ia.CheeseMilkTypeId			= cm.MilkTypeId
		                    LEFT JOIN EcoScaleRating		es			ON ia.EcoScaleRatingID			= es.EcoScaleRatingID
		                    LEFT JOIN HealthyEatingRating	he			ON ia.HealthyEatingRatingID		= he.HealthyEatingRatingID
		                    LEFT JOIN SeafoodCatchType		sfc			ON ia.SeafoodCatchTypeID		= sfc.SeafoodCatchTypeID
		                    LEFT JOIN SeafoodFreshOrFrozen	sff			ON ia.SeafoodFreshOrFrozenID	= sff.SeafoodFreshOrFrozenID
		                    LEFT JOIN ItemTrait				nr			ON nr.traitID					= @nrTraitId  AND nr.itemID = i.itemID  AND nr.localeID = @localeID
		                    LEFT JOIN ItemTrait				gpp			ON gpp.traitID					= @gppTraitId AND gpp.itemID = i.itemID AND gpp.localeID = @localeID
		                    LEFT JOIN ItemTrait				ftc			ON ftc.traitID					= @ftcTraitId AND ftc.itemID = i.itemID AND ftc.localeID = @localeID
		                    LEFT JOIN ItemTrait				fxt			ON fxt.traitID					= @fxtTraitId AND fxt.itemID = i.itemID AND fxt.localeID = @localeID
		                    LEFT JOIN ItemTrait				mog			ON mog.traitID					= @mogTraitId AND mog.itemID = i.itemID AND mog.localeID = @localeID
		                    LEFT JOIN ItemTrait				prb			ON prb.traitID					= @prbTraitId AND prb.itemID = i.itemID AND prb.localeID = @localeID							
		                    LEFT JOIN ItemTrait				rfa			ON rfa.traitID					= @rfaTraitId AND rfa.itemID = i.itemID AND rfa.localeID = @localeID
		                    LEFT JOIN ItemTrait				rfd			ON rfd.traitID					= @rfdTraitId AND rfd.itemID = i.itemID AND rfd.localeID = @localeID
		                    LEFT JOIN ItemTrait				smf			ON smf.traitID					= @sbfTraitId AND smf.itemID = i.itemID AND smf.localeID = @localeID
		                    LEFT JOIN ItemTrait				wic			ON wic.traitID					= @wicTraitId AND wic.itemID = i.itemID AND wic.localeID = @localeID
		                    LEFT JOIN ItemTrait				slf			ON slf.traitID					= @shelfLife  AND slf.itemID = i.itemID AND slf.localeID = @localeID
		                    LEFT JOIN ItemTrait				itg			ON itg.traitID					= @itgTraitId AND itg.itemID = i.itemID AND itg.localeID = @localeID	
		                    LEFT JOIN ItemTrait				hid			ON	hid.traitID					= @hidTraitId AND hid.itemID = i.itemID AND hid.localeID = @localeID	
                            LEFT JOIN nutrition.ItemNutrition inn       on sc.scancode = inn.Plu";
            }
        }

        public static string LocaleSql
        {
            get
            {
                return @"
DECLARE @RegStoreNumTraitId int;
DECLARE @regionAbbreviationTraitId int;
DECLARE @psBusinessUnitTraitId int;
DECLARE @storeAbbreviationTraitId int;
DECLARE @posTypeTraitId int;
DECLARE @PhoneTraitId int;
DECLARE @FaxTraitId int;
DECLARE @LastUserTraitId int;
DECLARE @TimeStampTraitId int;

select @RegStoreNumTraitId = TraitId from Trait where traitDesc = 'IRMA Store ID'
Select @regionAbbreviationTraitId = TraitId from Trait where traitDesc = 'Region Abbreviation'
select @psBusinessUnitTraitId = TraitId from Trait where traitDesc = 'PS Business Unit ID'
select @storeAbbreviationTraitId = TraitId from Trait where traitDesc = 'Store Abbreviation'
select @posTypeTraitId = TraitId from Trait where traitDesc = 'Store POS Type'
select @PhoneTraitId = TraitId from Trait where traitDesc = 'Phone Number'
select @FaxTraitId = TraitId from Trait where traitDesc = 'Fax'
select @LastUserTraitId = TraitId from Trait where traitDesc = 'Modified User'
select @TimeStampTraitId = TraitId from Trait where traitDesc = 'Modified Date'

select 
	chain.localeID 'ChainId', 
	chain.localeName 'ChainName',
	region.localeID 'RegionId', 
	region.localeName 'RegionName',
	metro.localeID 'MetroId', 
	metro.localeName 'MetroName',
	store.localeID 'StoreId',
	store.localeName 'StoreName',
	bult.traitValue 'BusinessUnit',
	abrlt.traitValue 'StoreAbbreviation',
	phlt.traitValue 'PhoneNumber',
    addr.AddressID 'AddressId',
	addr.addressLine1 'AddressLine1',
	addr.addressLine2 'AddressLine2',
	addr.addressLine3 'AddressLine3',
	city.cityName 'City',
	post.postalCode 'PostalCode',
	state.territoryName 'Territory',
	state.territoryCode 'TerritoryAbbrev',
	country.countryName 'Country',
	country.countryCode 'CountryAbbrev',
	zone.timezoneName 'Timezone'
from 
	dbo.Locale chain 
left join dbo.Locale region on region.parentLocaleID = chain.localeID
	and region.localeTypeID = 2
left join dbo.Locale metro on metro.parentLocaleID = region.localeID 
	and metro.localeTypeID = 3
left join dbo.Locale store on store.parentLocaleID = metro.localeID
	and store.localeTypeID = 4
left join LocaleTrait stlt  on stlt.localeID = store.localeID and stlt.traitID = @RegStoreNumTraitId
left join LocaleTrait bult  on bult.localeID = store.localeID and bult.traitID = @psBusinessUnitTraitId
left join LocaleTrait abrlt on abrlt.localeID = store.localeID and abrlt.traitID = @storeAbbreviationTraitId
left join LocaleTrait ptlt  on ptlt.localeID = store.localeID and ptlt.traitID = @posTypeTraitId
left join LocaleTrait phlt  on phlt.LocaleID = store.localeID and phlt.traitID = @PhoneTraitId
left join LocaleTrait faxlt on faxlt.LocaleID = store.localeID and faxlt.traitID = @FaxTraitId
left join LocaleTrait uslt  on uslt.LocaleID = store.localeID and uslt.traitID = @LastUserTraitId
left join LocaleTrait dtlt  on dtlt.LocaleID = store.localeID and dtlt.traitID = @TimeStampTraitId
left join LocaleAddress laddr on laddr.localeID = store.localeID 
left join PhysicalAddress addr on addr.addressID = laddr.addressID
left join City city on city.cityID = addr.cityID
left join Territory state on state.territoryID = addr.territoryID
left join PostalCode post on post.postalCodeID = addr.postalCodeID
left join Country country on country.countryID = post.countryID
left join Timezone zone on zone.timezoneID = addr.timezoneID
where
	chain.localeTypeID = 1
order by [ChainId], [RegionId], [MetroId], [StoreId]
";
            }
        }

        public static string ItemLocaleSql
        {
            get
            {
                return @"declare @ColorAddedId int = (select AttributeID from Attributes where AttributeDesc like '%Color%Add%')
                        declare @CountryOfProcessingId int = (select AttributeID from Attributes where AttributeDesc like 'Country of Processing')
                        declare @OriginId int = (select AttributeID from Attributes where AttributeDesc like 'Origin')
                        declare @EstId int = (select AttributeID from Attributes where AttributeDesc like 'Electronic Shelf Tag')
                        declare @ExclusiveId int = (select AttributeID from Attributes where AttributeDesc like 'Exclusive')
                        declare @NumDigitsToScaleId int = (select AttributeID from Attributes where AttributeDesc like 'Number of Digits Sent To Scale')
                        declare @ChicagoBabyId int = (select AttributeID from Attributes where AttributeDesc like 'Chicago Baby')
                        declare @TagUomId int = (select AttributeID from Attributes where AttributeDesc like 'Tag UOM')
                        declare @LinkedScanCodeId int = (select AttributeID from Attributes where AttributeDesc like 'Linked Scan Code')
                        declare @ScaleExtraTextId int = (select AttributeID from Attributes where AttributeDesc like 'Scale Extra Text')

                        select
	                        s.Region,
	                        s.BusinessUnitId,
	                        i.ItemID as ItemId,
                            it.ItemTypeCode,
                            it.ItemTypeDesc,
                            l.StoreName as LocaleName,
                            i.ScanCode,
                            s.Discount_Case as [CaseDiscount],
                            s.Discount_TM [TmDiscount],
                            s.Restriction_Age [AgeRestriction],
	                        s.Restriction_Hours    [RestrictedHours],
                            s.Authorized,
                            s.Discontinued,
                            s.LabelTypeDesc [LabelTypeDescription],
                            s.LocalItem,
                            s.Product_Code [ProductCode],
                            s.RetailUnit,
                            s.Sign_Desc [SignDescription],
                            s.Locality,
                            s.Sign_RomanceText_Long [SignRomanceLong],
                            s.Sign_RomanceText_Short [SignRomanceShort],
                            ca.AttributeValue [ColorAdded],
                            cop.AttributeValue [CountryOfProcessing],
                            o.AttributeValue [Origin],
                            est.AttributeValue [ElectronicShelfTag],
                            exc.AttributeValue [Exclusive],
                            num.AttributeValue [NumberOfDigitsSentToScale],
                            cb.AttributeValue [ChicagoBaby],
                            tag.AttributeValue [TagUom],
                            lnk.AttributeValue [LinkedItem],
                            sce.AttributeValue [ScaleExtraText],
                            s.Msrp [Msrp],
                            ils.SupplierName [SupplierName],
                            ils.IrmaVendorKey [IrmaVendorKey],
                            ils.SupplierItemID [SupplierItemID],
                            ils.SupplierCaseSize [SupplierCaseSize], 
                            s.OrderedByInfor [OrderedByInfor],
                            s.AltRetailSize [AltRetailSize],
                            s.AltRetailUOM [AltRetailUOM]
                        from
	                        dbo.ItemAttributes_Locale_{region} s
	                        join dbo.Items i						ON s.ItemID = i.ItemID
	                        join dbo.ItemTypes it					ON i.ItemTypeID = it.ItemTypeID
	                        join dbo.Locales_{region} l				ON s.BusinessUnitID = l.BusinessUnitID
	                        join dbo.ItemLocale_Supplier_{region} ils	ON ils.Region = l.Region
												                        AND ils.BusinessUnitID = l.BusinessUnitID
												                        AND ils.ItemID = i.ItemID
	                        left join dbo.ItemLocaleExtended ca		ON  i.ScanCode = ca.ScanCode
												                        AND l.BusinessUnitID = ca.BusinessUnitId
												                        AND ca.AttributeID = @ColorAddedId
	                        left join dbo.ItemLocaleExtended cop	ON  i.ScanCode = cop.ScanCode
												                        AND l.BusinessUnitID = cop.BusinessUnitId
												                        AND cop.AttributeID = @CountryOfProcessingId
	                        left join dbo.ItemLocaleExtended o		ON  i.ScanCode = o.ScanCode
												                        AND l.BusinessUnitID = o.BusinessUnitId
												                        AND o.AttributeID = @OriginId
	                        left join dbo.ItemLocaleExtended est	ON  i.ScanCode = est.ScanCode
												                        AND l.BusinessUnitID = est.BusinessUnitId
												                        AND est.AttributeID = @EstId
	                        left join dbo.ItemLocaleExtended exc	ON  i.ScanCode = exc.ScanCode
												                        AND l.BusinessUnitID = exc.BusinessUnitId
												                        AND exc.AttributeID = @ExclusiveId
	                        left join dbo.ItemLocaleExtended num	ON  i.ScanCode = num.ScanCode
												                        AND l.BusinessUnitID = num.BusinessUnitId
												                        AND num.AttributeID = @NumDigitsToScaleId
	                        left join dbo.ItemLocaleExtended cb		ON  i.ScanCode = cb.ScanCode
												                        AND l.BusinessUnitID = cb.BusinessUnitId
												                        AND cb.AttributeID = @ChicagoBabyId
	                        left join dbo.ItemLocaleExtended tag	ON  i.ScanCode = tag.ScanCode
												                        AND l.BusinessUnitID = tag.BusinessUnitId
												                        AND tag.AttributeID = @TagUomId
	                        left join dbo.ItemLocaleExtended lnk	ON  i.ScanCode = lnk.ScanCode
												                        AND l.BusinessUnitID = lnk.BusinessUnitId
												                        AND lnk.AttributeID = @LinkedScanCodeId
	                        left join dbo.ItemLocaleExtended sce	ON  i.ScanCode = sce.ScanCode
												                        AND l.BusinessUnitID = sce.BusinessUnitId
												                        AND sce.AttributeID = @ScaleExtraTextId
                        order by s.BusinessUnitID";
            }
        }

        public static string PriceSql
        {
            get
            {
                return @"SELECT top 2
	                            i.ItemID			as ItemId,
	                            t.ItemTypeCode		as ItemTypeCode,
	                            t.ItemTypeDesc		as ItemTypeDesc,
	                            l.BusinessUnitID	as BusinessUnitId,
	                            l.StoreName			as LocaleName,
	                            i.ScanCode			as ScanCode,
	                            p.PriceUom			as UomCode,
	                            c.CurrencyCode		as CurrencyCode,
                                CASE WHEN p.PriceType <> 'REG' THEN 'TPR' ELSE 'REG' END as PriceTypeCode,
	                            CASE WHEN p.PriceType <> 'REG' THEN p.PriceType END as SubPriceTypeCode,
	                            p.Price				as Price,
	                            p.Multiple			as Multiple,
	                            p.StartDate			as StartDate,
	                            p.EndDate			as EndDate
                            FROM
	                            dbo.Price_{region}          p
	                            JOIN dbo.Locales_{region}   l	on	p.BusinessUnitId	= l.BusinessUnitID
	                            JOIN dbo.Items		        i	on	p.ItemID			= i.ItemID
	                            JOIN dbo.ItemTypes	        t	on	i.ItemTypeID		= t.ItemTypeID
                                JOIN dbo.Currency           c   on  p.CurrencyID        = c.CurrencyID
                            ORDER BY p.BusinessUnitId";
            }
        }

        public static string ProductSelectionGroupSql
        {
            get
            {
                return @"SELECT 
                    psg.ProductSelectionGroupId,
                    psg.ProductSelectionGroupName,
                    psg.ProductSelectionGroupTypeId,
                    psg.TraitId,
                    psg.TraitValue,
                    psg.MerchandiseHierarchyClassId,
                    psgt.ProductSelectionGroupTypeName
                    FROM app.ProductSelectionGroup psg
                    JOIN app.ProductSelectionGroupType psgt ON psg.ProductSelectionGroupTypeId = psgt.ProductSelectionGroupTypeId";
            }
        }
    }
}
