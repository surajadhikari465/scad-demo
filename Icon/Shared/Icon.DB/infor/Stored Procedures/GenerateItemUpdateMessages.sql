﻿CREATE PROCEDURE [infor].[GenerateItemUpdateMessages] 
	@updatedItemIDs app.UpdatedItemIDsType readonly,
  @isDeleteNutrition bit = 0
AS

--************************************************************************
-- This will generate a Product Message for the ESB for each itemID
-- that was updated in the ItemImport.sql stored procedure.
--************************************************************************

BEGIN
	set nocount on;
	
  IF(object_id('tempdb..#itemIDs') is not null) DROP TABLE #itemIDs;

  SELECT DISTINCT itemID INTO #itemIDs
  FROM @updatedItemIDs;

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
		@hidTraitId int,
		@linTraitId int,
		@skuTraitId int,
		@plTraitId int,
		@vsTraitId int,
		@esnTraitId int,
		@pneTraitId int,
		@eseTraitId int,
		@tseTraitId int,
		@wfeTraitId int,
		@oteTraitId int,
		@datTraitId int,
        @ngtTraitId int,
        @idpTraitId int,
        @ihtTraitId int,
		@iwdTraitId int,
		@cubTraitId int,
		@iwtTraitId int,
		@tdpTraitId int,
		@thtTraitId int,
		@twdTraitId int,
		@lblTraitId int,
		@cooTraitId int,
		@pgTraitId int,
		@pgtTraitId int,
		@prlTraitId int,
		@aplTraitId int,
		@ftTraitId int,
		@gfcTraitId int,
		@ngcTraitId int,
		@ocTraitId int,
		@varTraitId int,
		@besTraitId int,
		@lexTraitId int, 
		@kiTraitId int, 
		@hiTraitId int, 
		@kdTraitId int, 
		@urlTraitId int,
		@llpTraitId int,
		@opcTraitId int,
		@ploTraitId int,
		@pftTraitId int,
		@cfTraitId int,
		@dsTraitId int,
		@hemTraitId int,
		@insTraitId int,
		@modTraitId int

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
	SET @linTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'LIN')
	SET @skuTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'SKU')
	SET @plTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PL')
	SET @vsTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'VS')
	SET @esnTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'ESN')
	SET @pneTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PNE')
	SET @eseTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'ESE')
	SET @tseTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'TSE')
	SET @wfeTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'WFE')
	SET @oteTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'OTE')
	SET @datTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'DAT')
    SET @ngtTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'NGT')
    SET @idpTraitId					= (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'IDP')
    SET @ihtTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'IHT')
	SET	@iwdTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'IWD')
	SET	@cubTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'CUB')
	SET	@iwtTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'IWT')
	SET	@tdpTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'TDP')
	SET	@thtTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'THT')
	SET	@twdTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'TWD')
	SET	@lblTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'LBL')
	SET	@cooTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'COO')
	SET	@pgTraitId                  = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PG')
	SET	@pgtTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PGT')
	SET	@prlTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PRL')
	SET	@aplTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'APL')
	SET	@ftTraitId                  = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'FT')
	SET	@gfcTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'GFC')
	SET	@ngcTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'NGC')
	SET	@ocTraitId                  = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'OC')
	SET	@varTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'VAR')
	SET	@besTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'BES')
	SET	@lexTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'LEX')
	SET	@kiTraitId                  = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'KI')
	SET	@hiTraitId                  = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'HI')
	SET	@kdTraitId                  = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'KD')
	SET	@urlTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'URL')
	SET	@llpTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'LLP')
	SET	@opcTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'OPC')
	SET	@ploTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PLO')
	SET	@pftTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'PFT')
	SET	@cfTraitId                 =  (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'CF')
	SET	@hemTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'HEM')
	SET	@dsTraitId                 =  (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'DS')
	SET	@insTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'INS')
	SET	@modTraitId                 = (SELECT t.TraitID FROM Trait t WHERE t.traitCode = 'MOD')

	insert into 
		app.MessageQueueProduct
		output inserted.MessageQueueId, inserted.ScanCode into @distinctProductMessageIDs
	select
		@productMessageTypeID															AS MessageTypeID,
		case
			when brandhctesb.traitValue IS NULL then @stagedMessageStatusID
			when merchhctesb.traitValue IS NULL then @stagedMessageStatusID
			when finhctesb.traitValue IS NULL then @stagedMessageStatusID
			else @readyMessageStatusID
		end																				AS MessageStatusId,
		NULL																			AS MessageHistoryId,
		sysdatetime()																	AS InsertDate,
		ui.itemID																		AS ItemId,
		@localeID																		AS LocaleId,
		it.itemTypeCode																	AS ItemTypeCode,
		it.itemTypeDesc																	AS ItemTypeDesc,
		sc.scanCodeID																	AS ScanCodeId,
		sc.scanCode																		AS ScanCode,
		sct.scanCodeTypeID																AS ScanCodeTypeId,
		sct.scanCodeTypeDesc															AS ScanCodeTypeDesc,
		prd.traitValue																	AS ProductDescription,
		pos.traitValue																	AS PosDescription,
		pkg.traitValue																	AS PackageUnit,
		rsz.traitValue																	AS RetailSize,
		rum.traitValue																	AS RetailUom,
		fse.traitValue																	AS FoodStampEligible,
		isnull(prh.traitValue, 0)														AS ProhibitDiscount,	
		isnull(ds.traitValue, '0')														AS DepartmentSale,
		brandhc.hierarchyClassID														AS BrandId,
		brandhc.hierarchyClassName														AS BrandName,
		brandhc.hierarchyLevel															AS BrandLevel,
		brandhc.hierarchyParentClassID													AS BrandParentId,
		null																			AS BrowsingClassId,
		null																			AS BrowsingClassName,
		null																			AS BrowsingLevel,
		null																			AS BrowsingParentId,
		merchhc.hierarchyClassID														AS MerchandiseClassId,
		merchhc.hierarchyClassName														AS MerchandiseClassName,
		merchhc.hierarchyLevel															AS MerchandiseLevel,
		merchhc.hierarchyParentClassID													AS MerchandiseParentId,
		taxhc.hierarchyClassID															AS TaxClassId,
		taxhc.hierarchyClassName														AS TaxClassName,
		taxhc.hierarchyLevel															AS TaxLevel,
		taxhc.hierarchyParentClassID													AS TaxParentId,
		substring(finhc.hierarchyClassName, 
			charindex('(', finhc.hierarchyClassName) + 1, 4)							AS FinancialClassId,
		case
			when substring(finhc.hierarchyClassName, 
					charindex('(', finhc.hierarchyClassName) + 1, 4) = '0000' then 'na'
			else finhc.hierarchyClassName
		end																				AS FinancialClassName,
		finhc.hierarchyLevel															AS FinancialLevel,
		finhc.hierarchyParentClassID													AS FinancialParentId,
		null																			AS InProcessBy,
		null																			AS ProcessedDate,
		ia.AnimalWelfareRating															AS [AnimalWelfareRating],
		ia.[Biodynamic]																	AS [Biodynamic],
		ia.MilkType																	    AS [MilkType],
		ia.[CheeseRaw]																	AS [CheeseRaw],
		ia.EcoScaleRating																AS [EcoScaleRating],
		ia.GlutenFreeAgencyName															AS [GlutenFreeAgency],
		he.Description																	AS [HealthyEatingRating],
		ia.KosherAgencyName																AS [KosherAgency], 
		ia.[Msc]																		AS [Msc],
		ia.NonGmoAgencyName																AS [NonGmoAgency],
		ia.OrganicAgencyName															AS [OrganicAgency],
		ia.[PremiumBodyCare]															AS [PremiumBodyCare],
		ia.FreshOrFrozen																AS [SeafoodFreshOrFrozen],
		ia.SeafoodCatchType																AS [SeafoodCatchType],
		ia.VeganAgencyName																AS [VeganAgency],
		ia.[Vegetarian]																	AS [Vegetarian],
		ia.[WholeTrade]																	AS [WholeTrade],
		ia.[GrassFed]																	AS [GrassFed],
		ia.[PastureRaised]																AS [PastureRaised],
		ia.[FreeRange]																	AS [FreeRange],
		ia.[DryAged]																	AS [DryAged],
		ia.[AirChilled]																	AS [AirChilled],
		ia.[MadeInHouse]																AS [MadeInHouse],
		CASE WHEN ISNULL(ia.CustomerFriendlyDescription,'') = '' THEN prd.traitValue	
			 ELSE ia.CustomerFriendlyDescription END									AS CustomerFriendlyDescription,
		nr.traitValue																	AS NutritionRequired,
		gpp.traitValue																	AS GlobalPricingProgram,
		itg.traitValue																	AS SelfCheckoutItemTareGroup,
		fxt.traitValue																	AS FlexibleText,
		slf.traitValue																	AS ShelfLife,
		ftc.traitValue																	AS FairTradeCertified,		
		mog.traitValue																	AS MadeWithOrganicGrapes,
		CASE WHEN prb.traitValue = '1'    THEN 1  
			 WHEN prb.traitValue = 'True' THEN 1  
			 WHEN prb.traitValue = 'Yes'  THEN 1 
			 ELSE 0 END																	AS PrimeBeef,
		CASE WHEN rfa.traitValue = '1'    THEN 1  
			 WHEN rfa.traitValue = 'True' THEN 1  
			 WHEN rfa.traitValue = 'Yes'  THEN 1 
			 ELSE 0 END																	AS RainforestAlliance,
		rfd.traitValue																	AS Refigerated,
		CASE WHEN smf.traitValue = '1'    THEN 1  
			 WHEN smf.traitValue = 'True' THEN 1  
			 WHEN smf.traitValue = 'Yes'  THEN 1 
			 ELSE 0 END																	AS SmithsonianBirdFriendly,
		CASE WHEN wic.traitValue = '1'    THEN 1  
			 WHEN wic.traitValue = 'True' THEN 1  
			 WHEN wic.traitValue = 'Yes'  THEN 1 
			 ELSE 0 END																	AS WicEligible,
		nathc.hierarchyClassID															AS NationalClassId,
		nathc.hierarchyClassName														AS NationalClassName,
		nathc.hierarchyLevel															AS NationalLevel,
		nathc.hierarchyParentClassID													AS NationalParentId,
		CAST(ISNULL(hid.traitValue, '0') AS BIT)										AS Hidden,
		lin.traitValue																	AS Line,
		sku.traitValue																	AS SKU,
		pl.traitValue																	AS PriceLine,
		vs.traitValue																	AS VariantSize,
		CASE 
			WHEN esn.traitValue = 'Y' OR esn.traitValue = 'Yes'
			 OR esn.traitValue = '1' OR esn.traitValue = 'True'  
			THEN 1 ELSE 0 END															AS EStoreNutritionRequired,
		CASE 
			WHEN pne.traitValue is null THEN NULL 
			WHEN pne.traitValue = 'Y' OR pne.traitValue = 'Yes'
			 OR pne.traitValue = '1' OR pne.traitValue = 'True'  
			THEN 1 ELSE 0 END															AS PrimeNowEligible,
		CASE 
			WHEN ese.traitValue = 'Y' OR ese.traitValue = 'Yes'
			 OR ese.traitValue = '1' OR ese.traitValue = 'True'  
			THEN 1 ELSE 0 END															AS EstoreEligible,
		CASE 
			WHEN tse.traitValue is null THEN NULL 
			WHEN tse.traitValue = 'Y' OR tse.traitValue = 'Yes'
			 OR tse.traitValue = '1' OR tse.traitValue = 'True'  
			THEN 1 ELSE 0 END															AS TSFEligible,
		CASE 
			WHEN wfe.traitValue is null THEN NULL 
			WHEN wfe.traitValue = 'Y' OR wfe.traitValue = 'Yes'
			 OR wfe.traitValue = '1' OR wfe.traitValue = 'True'  
			THEN 1 ELSE 0 END															AS WFMEligilble,
		CASE 
			WHEN ote.traitValue is null THEN NULL 
			WHEN ote.traitValue = 'Y' OR ote.traitValue = 'Yes'
			 OR ote.traitValue = '1' OR ote.traitValue = 'True'  
			THEN 1 ELSE 0 END															AS Other3PEligible,
		hi.traitValue																	AS HospitalityItem,
		ki.traitvalue																	AS KitchenItem,
		kd.traitValue       															AS KitchenDescription,
		url.traitValue																	AS ImageURL,
		dat.traitValue                                                                  AS DataSource,		
		ngt.traitValue  																AS NonGMOTransparency,
		idp.traitValue  																AS ItemDepth,
		iht.traitValue  																AS ItemHeight,
		iwd.traitValue  																AS ItemWidth,
		cub.traitValue  																AS [Cube],
		iwt.traitValue  																AS [ItemWeight],
		tdp.traitValue  																AS TrayDepth,
		tht.traitValue  																AS TrayHeight,
		twd.traitValue  																AS TrayWidth,
		lbl.traitValue  																AS Labeling,
		coo.traitValue  																AS CountryOfOrigin,
		pg.traitValue  																	AS PackageGroup,
		pgt.traitValue  																AS PackageGroupType,
		prl.traitValue  																AS PrivateLabel,
		apl.traitValue  																AS Appellation,
		CASE 
					WHEN ft.traitValue is null THEN NULL 
					WHEN ft.traitValue = 'Y' OR ft.traitValue = 'Yes'
					 OR ft.traitValue = '1' OR ft.traitValue = 'True'  
					THEN 1 ELSE 0 END													AS FairTradeClaim,
		CASE 
					WHEN gfc.traitValue is null THEN NULL 
					WHEN gfc.traitValue = 'Y' OR gfc.traitValue = 'Yes'
					 OR gfc.traitValue = '1' OR gfc.traitValue = 'True'  
					THEN 1 ELSE 0 END													AS GlutenFreeClaim,
		CASE 
					WHEN ngc.traitValue is null THEN NULL 
					WHEN ngc.traitValue = 'Y' OR ngc.traitValue = 'Yes'
					 OR ngc.traitValue = '1' OR ngc.traitValue = 'True'  
					THEN 1 ELSE 0 END													AS NonGMOClaim,
		CASE 
					WHEN oc.traitValue is null THEN NULL 
					WHEN oc.traitValue = 'Y' OR oc.traitValue = 'Yes'
					 OR oc.traitValue = '1' OR oc.traitValue = 'True'  
					THEN 1 ELSE 0 END													AS OrganicClaim,
		va.traitValue                                                                   AS Varietal,
		bes.traitValue                                                                  AS BeerStyle,
		lex.traitValue                                                                  AS LineExtension,
		llp.traitValue                                                                  AS LocalLoanProducer,
		opc.traitValue                                                                  AS OrganicPersonalCare,
		plo.traitValue                                                                  AS Paleo,
		pft.traitValue                                                                  AS ProductFlavorType,
		cf.traitValue                                                                   AS CaseinFree,
		hem.traitValue                                                                  AS Hemp,
		dst.traitValue                                                                  AS DeliverySystem,
		ins.traitValue                                                                  AS InsertDate,
		modt.traitValue                                                                 AS ModifiedDate

	from 
		#itemIDs					ui
		JOIN Item						i			ON	ui.itemID					= i.itemID
		JOIN ItemType					it			ON	i.itemTypeID				= it.itemTypeID
		JOIN ScanCode					sc			ON	i.itemID					= sc.itemID
		JOIN ScanCodeType				sct			ON	sc.scanCodeTypeID			= sct.scanCodeTypeID
		JOIN ItemTrait					val			ON	i.itemID					= val.itemID
															AND val.traitID			= @validationDateTraitID
															AND val.localeID		= @localeID
		JOIN ItemTrait					prd			ON	i.itemID					= prd.itemID
															AND prd.traitID			= @productDescriptionTraitID
															AND prd.localeID		= @localeID
		JOIN ItemTrait					pos			ON	i.itemID					= pos.itemID
															AND pos.traitID			= @posTraitID
															AND pos.localeID		= @localeID
		JOIN ItemTrait					pkg			ON	i.itemID					= pkg.itemID
															AND pkg.traitID			= @packageUnitTraitID
															AND pkg.localeID		= @localeID
		JOIN ItemTrait					rsz			ON	i.itemID					= rsz.itemID
															AND rsz.traitID			= @retailSizeID
															AND rsz.localeID		= @localeID
		JOIN ItemTrait					rum			ON  i.itemID					= rum.itemID
															AND rum.traitID			= @retailUomID
															AND rum.localeID		= @localeID
		JOIN ItemTrait					fse			ON	i.itemID					= fse.itemID
															AND fse.traitID			= @foodStampEligibleTraitID
															AND fse.localeID		= @localeID
		LEFT JOIN ItemTrait				prh			ON	i.itemID					= prh.itemID
															AND prh.traitID			= @prohibitDiscountTraitID
															AND prh.localeID		= @localeID
		JOIN ItemHierarchyClass			brandihc	ON	i.itemID					= brandihc.itemID
		JOIN HierarchyClass				brandhc		ON	brandihc.hierarchyClassID	= brandhc.hierarchyClassID
															AND brandhc.hierarchyID	= @brandHierarchyID
		LEFT JOIN HierarchyClassTrait	brandhctesb	ON	brandhc.hierarchyClassID	= brandhctesb.hierarchyClassID
															AND brandhctesb.traitID	= @sentToEsbTraitID
		JOIN ItemHierarchyClass			merchihc	ON	i.itemID					= merchihc.itemID
		JOIN HierarchyClass				merchhc		ON	merchihc.hierarchyClassID	= merchhc.hierarchyClassID
															AND merchhc.hierarchyID	= @merchandiseClassID
		LEFT JOIN HierarchyClassTrait	merchhctesb	ON	merchhc.hierarchyClassID	= merchhctesb.hierarchyClassID
															AND merchhctesb.traitID	= @sentToEsbTraitID
		JOIN ItemHierarchyClass			finihc		ON	i.itemID					= finihc.itemID
		JOIN HierarchyClass				finhc		ON	finihc.hierarchyClassID		= finhc.hierarchyClassID
															AND finhc.hierarchyID	= @financialClassID	
		LEFT JOIN HierarchyClassTrait	finhctesb	ON	finhc.hierarchyClassID		= finhctesb.hierarchyClassID
															AND finhctesb.traitID	= @sentToEsbTraitID
		JOIN ItemHierarchyClass			taxihc		ON	i.itemID					= taxihc.itemID
		JOIN HierarchyClass				taxhc		ON	taxihc.hierarchyClassID		= taxhc.hierarchyClassID
														AND taxhc.hierarchyID		= @taxClassID
		LEFT JOIN HierarchyClassTrait	taxhctesb	ON	taxhc.hierarchyClassID		= taxhctesb.hierarchyClassID
															AND taxhctesb.traitID	= @sentToEsbTraitID
		JOIN ItemHierarchyClass			natihc		ON	i.itemID					= natihc.itemID
		JOIN HierarchyClass				nathc		ON	natihc.hierarchyClassID		= nathc.hierarchyClassID
															AND nathc.hierarchyID	= @nationalClassID
		LEFT JOIN ItemTrait				ds			ON	i.itemID					= ds.itemID
															AND ds.traitID			= @departmentSaleTraitID
															AND ds.localeID			= @localeID
		LEFT JOIN ItemSignAttribute		ia			ON	i.itemID					= ia.itemID
		LEFT JOIN HealthyEatingRating	he			ON	ia.HealthyEatingRatingID	= he.HealthyEatingRatingID
		LEFT JOIN ItemTrait				nr			ON	nr.traitID					= @nrTraitId  AND nr.itemID = i.itemID  AND nr.localeID = @localeID
		LEFT JOIN ItemTrait				gpp			ON	gpp.traitID					= @gppTraitId AND gpp.itemID = i.itemID AND gpp.localeID = @localeID
		LEFT JOIN ItemTrait				ftc			ON	ftc.traitID					= @ftcTraitId AND ftc.itemID = i.itemID AND ftc.localeID = @localeID
		LEFT JOIN ItemTrait				fxt			ON	fxt.traitID					= @fxtTraitId AND fxt.itemID = i.itemID AND fxt.localeID = @localeID
		LEFT JOIN ItemTrait				mog			ON	mog.traitID					= @mogTraitId AND mog.itemID = i.itemID AND mog.localeID = @localeID
		LEFT JOIN ItemTrait				prb			ON	prb.traitID					= @prbTraitId AND prb.itemID = i.itemID AND prb.localeID = @localeID							
		LEFT JOIN ItemTrait				rfa			ON	rfa.traitID					= @rfaTraitId AND rfa.itemID = i.itemID AND rfa.localeID = @localeID
		LEFT JOIN ItemTrait				rfd			ON	rfd.traitID					= @rfdTraitId AND rfd.itemID = i.itemID AND rfd.localeID = @localeID
		LEFT JOIN ItemTrait				smf			ON	smf.traitID					= @sbfTraitId AND smf.itemID = i.itemID AND smf.localeID = @localeID
		LEFT JOIN ItemTrait				wic			ON	wic.traitID					= @wicTraitId AND wic.itemID = i.itemID AND wic.localeID = @localeID
		LEFT JOIN ItemTrait				slf			ON	slf.traitID					= @shelfLife  AND slf.itemID = i.itemID AND slf.localeID = @localeID
		LEFT JOIN ItemTrait				itg			ON	itg.traitID					= @itgTraitId AND itg.itemID = i.itemID AND itg.localeID = @localeID	
		LEFT JOIN ItemTrait				hid			ON	hid.traitID					= @hidTraitId AND hid.itemID = i.itemID AND hid.localeID = @localeID			
		LEFT JOIN ItemTrait				lin			ON	lin.traitID					= @linTraitId AND lin.itemID = i.itemID AND lin.localeID = @localeID	
		LEFT JOIN ItemTrait				sku			ON	sku.traitID					= @skuTraitId AND sku.itemID = i.itemID AND sku.localeID = @localeID	
		LEFT JOIN ItemTrait				pl			ON	pl.traitID					= @plTraitId  AND  pl.itemID = i.itemID AND  pl.localeID = @localeID	
		LEFT JOIN ItemTrait				vs			ON	vs.traitID					= @vsTraitId  AND  vs.itemID = i.itemID AND  vs.localeID = @localeID	
		LEFT JOIN ItemTrait				esn			ON	esn.traitID					= @esnTraitId AND esn.itemID = i.itemID AND esn.localeID = @localeID	
		LEFT JOIN ItemTrait				pne			ON	pne.traitID					= @pneTraitId AND pne.itemID = i.itemID AND pne.localeID = @localeID	
		LEFT JOIN ItemTrait				ese			ON	ese.traitID					= @eseTraitId AND ese.itemID = i.itemID AND ese.localeID = @localeID	
		LEFT JOIN ItemTrait				tse			ON	tse.traitID					= @tseTraitId AND tse.itemID = i.itemID AND tse.localeID = @localeID	
		LEFT JOIN ItemTrait				wfe			ON	wfe.traitID					= @wfeTraitId AND wfe.itemID = i.itemID AND wfe.localeID = @localeID	
		LEFT JOIN ItemTrait				ote			ON	ote.traitID					= @oteTraitId AND ote.itemID = i.itemID AND ote.localeID = @localeID
        LEFT JOIN ItemTrait				dat			ON	dat.traitID					= @datTraitId AND dat.itemID = i.itemID AND dat.localeID = @localeID
		LEFT JOIN ItemTrait				ngt			ON	ngt.traitID					= @ngtTraitId AND ngt.itemID = i.itemID AND ngt.localeID = @localeID
		LEFT JOIN ItemTrait				idp			ON	idp.traitID					= @idpTraitId AND idp.itemID = i.itemID AND idp.localeID = @localeID
		LEFT JOIN ItemTrait				iht			ON	iht.traitID					= @ihtTraitId AND iht.itemID = i.itemID AND iht.localeID = @localeID
		LEFT JOIN ItemTrait				iwd			ON	iwd.traitID					= @iwdTraitId AND iwd.itemID = i.itemID AND iwd.localeID = @localeID
		LEFT JOIN ItemTrait				cub			ON	cub.traitID					= @cubTraitId AND cub.itemID = i.itemID AND cub.localeID = @localeID
		LEFT JOIN ItemTrait				iwt			ON	iwt.traitID					= @iwtTraitId AND iwt.itemID = i.itemID AND iwt.localeID = @localeID
		LEFT JOIN ItemTrait				tdp			ON	tdp.traitID					= @tdpTraitId AND tdp.itemID = i.itemID AND tdp.localeID = @localeID
		LEFT JOIN ItemTrait				tht			ON	tht.traitID					= @thtTraitId AND tht.itemID = i.itemID AND tht.localeID = @localeID
		LEFT JOIN ItemTrait				twd			ON	twd.traitID					= @twdTraitId AND twd.itemID = i.itemID AND twd.localeID = @localeID
		LEFT JOIN ItemTrait				lbl			ON	lbl.traitID					= @lblTraitId AND lbl.itemID = i.itemID AND lbl.localeID = @localeID
		LEFT JOIN ItemTrait				coo			ON	coo.traitID					= @cooTraitId AND coo.itemID = i.itemID AND coo.localeID = @localeID
		LEFT JOIN ItemTrait				pg			ON	pg.traitID					= @pgTraitId  AND pg.itemID = i.itemID  AND pg.localeID = @localeID
		LEFT JOIN ItemTrait				pgt			ON	pgt.traitID					= @pgtTraitId AND pgt.itemID = i.itemID AND pgt.localeID = @localeID
		LEFT JOIN ItemTrait				prl			ON	prl.traitID					= @prlTraitId AND prl.itemID = i.itemID AND prl.localeID = @localeID
		LEFT JOIN ItemTrait				apl			ON	apl.traitID					= @aplTraitId AND apl.itemID = i.itemID AND apl.localeID = @localeID
		LEFT JOIN ItemTrait				ft			ON	ft.traitID					= @ftTraitId  AND ft.itemID = i.itemID  AND ft.localeID = @localeID
		LEFT JOIN ItemTrait				gfc			ON	gfc.traitID					= @gfcTraitId AND gfc.itemID = i.itemID AND gfc.localeID = @localeID
		LEFT JOIN ItemTrait				ngc			ON	ngc.traitID					= @ngcTraitId AND ngc.itemID = i.itemID AND ngc.localeID = @localeID
		LEFT JOIN ItemTrait				oc			ON	oc.traitID					= @ocTraitId  AND oc.itemID = i.itemID  AND oc.localeID = @localeID
		LEFT JOIN ItemTrait				va			ON	va.traitID					= @varTraitId AND va.itemID = i.itemID  AND va.localeID = @localeID
		LEFT JOIN ItemTrait				bes			ON	bes.traitID					= @besTraitId AND bes.itemID = i.itemID AND bes.localeID = @localeID
		LEFT JOIN ItemTrait				lex			ON	lex.traitID					= @lexTraitId AND lex.itemID = i.itemID AND lex.localeID = @localeID
		LEFT JOIN ItemTrait				ki			ON	ki.traitID					= @kiTraitId AND ki.itemID = i.itemID AND ki.localeID = @localeID
		LEFT JOIN ItemTrait				hi			ON	hi.traitID					= @hiTraitId AND hi.itemID = i.itemID AND hi.localeID = @localeID
		LEFT JOIN ItemTrait				kd			ON	kd.traitID					= @kdTraitId AND kd.itemID = i.itemID AND kd.localeID = @localeID
		LEFT JOIN ItemTrait				url			ON	url.traitID					= @urlTraitId AND url.itemID = i.itemID AND url.localeID = @localeID
		LEFT JOIN ItemTrait				llp			ON	llp.traitID					= @llpTraitId AND llp.itemID = i.itemID AND llp.localeID = @localeID
		LEFT JOIN ItemTrait				opc			ON	opc.traitID					= @opcTraitId AND opc.itemID = i.itemID AND opc.localeID = @localeID
		LEFT JOIN ItemTrait				plo			ON	plo.traitID					= @ploTraitId AND plo.itemID = i.itemID AND plo.localeID = @localeID
		LEFT JOIN ItemTrait				pft			ON	pft.traitID					= @pftTraitId AND pft.itemID = i.itemID AND pft.localeID = @localeID
		LEFT JOIN ItemTrait				cf			ON	cf.traitID					= @cfTraitId AND cf.itemID = i.itemID AND cf.localeID = @localeID
		LEFT JOIN ItemTrait				hem			ON	hem.traitID					= @hemTraitId AND hem.itemID = i.itemID AND hem.localeID = @localeID
		LEFT JOIN ItemTrait				dst			ON	dst.traitID					= @dsTraitId AND dst.itemID = i.itemID AND dst.localeID = @localeID
		LEFT JOIN ItemTrait				ins			ON	ins.traitID					= @insTraitId AND ins.itemID = i.itemID AND ins.localeID = @localeID
		LEFT JOIN ItemTrait				modt	    ON	modt.traitID				= @modTraitId AND modt.itemID = i.itemID AND modt.localeID = @localeID
	
	where
		it.itemTypeID <> @couponItemTypeId
 
  IF(@isDeleteNutrition = 1)
  BEGIN
    INSERT INTO app.MessageQueueNutrition(
        MessageQueueId
       ,RecipeName
       ,InsertDate)
	  SELECT MessageQueueId
          ,'DELETED'
		     ,SysDateTime()
	  FROM @distinctProductMessageIDs
  END
  ELSE
  BEGIN
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
		,NULL AS [HazardousMaterialTypeCode]
		,sysdatetime() AS InsertDate
		,[AddedSugarsWeight]
		,[AddedSugarsPercent]
		,[CalciumWeight]
		,[IronWeight]
		,[VitaminDWeight]
	  from 
	  	@distinctProductMessageIDs dpm
	  	JOIN nutrition.ItemNutrition inn on dpm.scancode = inn.Plu
  END
END
