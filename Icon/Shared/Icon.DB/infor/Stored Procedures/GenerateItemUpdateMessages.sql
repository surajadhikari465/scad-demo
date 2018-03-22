CREATE PROCEDURE [infor].[GenerateItemUpdateMessages] 
	@updatedItemIDs app.UpdatedItemIDsType readonly
AS

--************************************************************************
-- This will generate a Product Message for the ESB for each itemID
-- that was updated in the ItemImport.sql stored procedure.
--************************************************************************

BEGIN
	set nocount on;
	
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
		@ptaTraitId int,
		@ftcTraitId int,
		@fxtTraitId int,
		@mogTraitId int,
		@prbTraitId int,
		@rfaTraitId int,
        @rfdTraitId int,
        @sbfTraitId int,
        @wicTraitId int,
        @shelfLife int,
        @itgTraitId int

	declare @distinctProductMessageIDs table (MessageQueueId int, scancode varchar(13));

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
	SET @ptaTraitId					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PTA')
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

	insert into 
		app.MessageQueueProduct
		output inserted.MessageQueueId, inserted.ScanCode into @distinctProductMessageIDs
	select
		@productMessageTypeID				as MessageTypeID,
		case
			when brandhctesb.traitValue IS NULL then @stagedMessageStatusID
			when merchhctesb.traitValue IS NULL then @stagedMessageStatusID
			when finhctesb.traitValue IS NULL then @stagedMessageStatusID
			else @readyMessageStatusID
		end									as MessageStatusId,
		NULL								as MessageHistoryId,
		sysdatetime()						as InsertDate,
		ui.itemID							as ItemId,
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
		isnull(merchhctprh.traitValue, 0)	as ProhibitDiscount,	
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
		null								as InProcessBy,
		null								as ProcessedDate,
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
		pta.traitValue						AS PercentageTareWeight,
		CASE 
			WHEN LCL_TYPE.localeTypeCode = 'RG' THEN 
				CASE 
					WHEN LCL.localeName='Florida' THEN 'FL'
					WHEN LCL.localeName='Mid Atlantic' THEN 'MA'
					WHEN LCL.localeName='Mid West' THEN 'MW'
					WHEN LCL.localeName='North Atlantic' THEN 'NA'
					WHEN LCL.localeName='Northern California' THEN 'NC'
					WHEN LCL.localeName='North East' THEN 'NE'
					WHEN LCL.localeName='Pacific Northwest' THEN 'PN'
					WHEN LCL.localeName='Rocky Mountain' THEN 'RM'
					WHEN LCL.localeName='South' THEN 'SO'
					WHEN LCL.localeName='Southern Pacific' THEN 'SP'
					WHEN LCL.localeName='Southwest' THEN 'SW'
					WHEN LCL.localeName='United Kingdom' THEN 'UK'
					WHEN LCL.localeName like '365%' THEN 'TS'
				END
			WHEN PRNT_TYPE.localeTypeCode = 'RG' THEN 
				CASE 
					WHEN PRNT_LCL.localeName='Florida' THEN 'FL'
					WHEN PRNT_LCL.localeName='Mid Atlantic' THEN 'MA'
					WHEN PRNT_LCL.localeName='Mid West' THEN 'MW'
					WHEN PRNT_LCL.localeName='North Atlantic' THEN 'NA'
					WHEN PRNT_LCL.localeName='Northern California' THEN 'NC'
					WHEN PRNT_LCL.localeName='North East' THEN 'NE'
					WHEN PRNT_LCL.localeName='Pacific Northwest' THEN 'PN'
					WHEN PRNT_LCL.localeName='Rocky Mountain' THEN 'RM'
					WHEN PRNT_LCL.localeName='South' THEN 'SO'
					WHEN PRNT_LCL.localeName='Southern Pacific' THEN 'SP'
					WHEN PRNT_LCL.localeName='Southwest' THEN 'SW'
					WHEN PRNT_LCL.localeName='United Kingdom' THEN 'UK'
					WHEN PRNT_LCL.localeName like '365%' THEN 'TS'
				END
			WHEN GPRNT_TYPE.localeTypeCode = 'RG' THEN 
				CASE 
					WHEN GPRNT_LCL.localeName='Florida' THEN 'FL'
					WHEN GPRNT_LCL.localeName='Mid Atlantic' THEN 'MA'
					WHEN GPRNT_LCL.localeName='Mid West' THEN 'MW'
					WHEN GPRNT_LCL.localeName='North Atlantic' THEN 'NA'
					WHEN GPRNT_LCL.localeName='Northern California' THEN 'NC'
					WHEN GPRNT_LCL.localeName='North East' THEN 'NE'
					WHEN GPRNT_LCL.localeName='Pacific Northwest' THEN 'PN'
					WHEN GPRNT_LCL.localeName='Rocky Mountain' THEN 'RM'
					WHEN GPRNT_LCL.localeName='South' THEN 'SO'
					WHEN GPRNT_LCL.localeName='Southern Pacific' THEN 'SP'
					WHEN GPRNT_LCL.localeName='Southwest' THEN 'SW'
					WHEN GPRNT_LCL.localeName='United Kingdom' THEN 'UK'
					WHEN GPRNT_LCL.localeName like '365%' THEN 'TS'
				END
		END									AS RegionAbbrev,
		LCL_TERR.territoryCode 				AS GeographicalState,
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
			 ELSE 0 END					AS WicEligible
	from 
		@updatedItemIDs					ui
		JOIN Item						i			ON	ui.itemID					= i.itemID
		JOIN ItemType					it			ON	i.itemTypeID				= it.itemTypeID
		JOIN ScanCode					sc			ON	i.itemID					= sc.itemID
		JOIN ScanCodeType				sct			ON	sc.scanCodeTypeID			= sct.scanCodeTypeID
		JOIN dbo.Locale					LCL			ON  LCL.localeID			    = @localeID
		JOIN dbo.LocaleType				LCL_TYPE	ON  LCL_TYPE.localeTypeID		= LCL.localeTypeID
		LEFT JOIN dbo.LocaleAddress		LCL_ADDR	ON  LCL_ADDR.localeID			= LCL.localeID
		LEFT JOIN dbo.PhysicalAddress	LCL_PHYS	ON  LCL_PHYS.addressID			= LCL_ADDR.addressID
		LEFT JOIN dbo.Territory			LCL_TERR	ON  LCL_TERR.territoryID		= LCL_PHYS.territoryID
		LEFT JOIN dbo.Locale			PRNT_LCL	ON  PRNT_LCL.localeID			= LCL.parentLocaleID
		LEFT JOIN dbo.LocaleType		PRNT_TYPE	ON  PRNT_TYPE.localeTypeID		= PRNT_LCL.localeTypeID
		LEFT JOIN dbo.Locale			GPRNT_LCL	ON  GPRNT_LCL.localeID			= PRNT_LCL.parentLocaleID
		LEFT JOIN dbo.LocaleType		GPRNT_TYPE	ON  GPRNT_TYPE.localeTypeID		= GPRNT_LCL.localeTypeID
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
		LEFT JOIN HierarchyClassTrait	merchhctprh	ON	merchhc.hierarchyClassID	= merchhctprh.hierarchyClassID
														AND merchhctprh.traitID		= @prohibitDiscountTraitID
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
		LEFT JOIN ItemTrait				pta			ON pta.traitID					= @ptaTraitId AND pta.itemID = i.itemID AND pta.localeID = @localeID
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
	where
		it.itemTypeID <> @couponItemTypeId

	insert into app.MessageQueueNutrition select 
		 [MessageQueueId] AS [MessageQueueId]
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
		,NULL AS [HazardousMaterialTypeCode],
		sysdatetime() AS InsertDate
	from 
		@distinctProductMessageIDs dpm
		JOIN nutrition.ItemNutrition inn on dpm.scancode = inn.Plu	
END
