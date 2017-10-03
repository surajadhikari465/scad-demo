﻿CREATE PROCEDURE [infor].[GenerateItemUpdateMessages] 
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
		@customerFriendlyDescriptionTraitID int,
		@nutritionRequired int

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
	SET @customerFriendlyDescriptionTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'CFD')
	SET @nutritionRequired          = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'NR')

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
		CASE WHEN ISNULL(cfd.traitValue,'') = '' THEN prd.traitValue	
		            ELSE cfd.traitValue END AS CustomerFriendlyDescription,
		nr.traitValue						AS NutritionRequired    
	from 
		@updatedItemIDs					ui
		JOIN Item						i			ON	ui.itemID					= i.itemID
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
		LEFT JOIN ItemTrait				cfd			ON	i.itemID					= cfd.itemID
														AND cfd.traitID				= @customerFriendlyDescriptionTraitID
														AND cfd.localeID			= @localeID
		LEFT JOIN ItemTrait				nr			ON	i.itemID					= nr.itemID
														AND nr.traitID				= @nutritionRequired
														AND nr.localeID				= @localeID												
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
