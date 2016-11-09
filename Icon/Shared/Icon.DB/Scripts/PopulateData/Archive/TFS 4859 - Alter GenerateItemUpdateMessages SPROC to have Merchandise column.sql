ALTER PROCEDURE [app].[GenerateItemUpdateMessages] 
	@updatedItemIDs app.UpdatedItemIDsType READONLY
AS

--************************************************************************
-- This stored procedure is called by the ItemImport.sql stored procedure.
-- This will generate a Product Message for the ESB for each itemID
-- that was updated in the ItemImport.sql stored procedure.
--************************************************************************

DECLARE @distinctItemIDs app.UpdatedItemIDsType;
INSERT INTO @distinctItemIDs SELECT DISTINCT itemID FROM @updatedItemIDs

-- Set up local variables.
DECLARE
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
	@merchFinMappingTraitID int;

SET @localeID					= (SELECT l.localeID FROM Locale l WHERE l.localeName = 'Whole Foods')
SET @productDescriptionTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PRD')
SET @posTraitID					= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'POS')
SET @packageUnitTraitID			= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'PKG')
SET @foodStampEligibleTraitID	= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'FSE')
SET @departmentSaleTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'DPT')
SET @brandHierarchyID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Brands')
SET @browsingClassID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Browsing')
SET @merchandiseClassID		= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
SET @financialClassID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Financial')
SET @taxClassID					= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax')
SET @validationDateTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'VAL')
SET @sentToEsbTraitID			= (SELECT traitID FROM Trait WHERE traitCode = 'ESB')
SET @readyMessageStatusID		= (SELECT s.MessageStatusId FROM app.MessageStatus s WHERE s.MessageStatusName = 'Ready')
SET @stagedMessageStatusID		= (SELECT s.MessageStatusId FROM app.MessageStatus s WHERE s.MessageStatusName = 'Staged')
SET @productMessageTypeID		= (SELECT t.MessageTypeId FROM app.MessageType t WHERE t.MessageTypeName = 'Product')
SET @merchFinMappingTraitID		= (SELECT t.traitID FROM Trait t WHERE t.traitDesc = 'Merch Fin Mapping')

-- Get Browsing ItemHierarchyClass relationships for left join.
select
	ihc.itemID,
	hc.hierarchyClassID,
	hc.hierarchyClassName,
	hc.hierarchyLevel,
	hc.hierarchyID,
	hc.hierarchyParentClassID
into 
	#tempBrowsing
from
	ItemHierarchyClass		ihc
	JOIN HierarchyClass		hc on ihc.hierarchyClassID = hc.hierarchyClassID AND hc.hierarchyID = @browsingClassID
	JOIN @distinctItemIDs	i on ihc.itemID = i.itemID

print 'Inserting into app.MessageQueueProduct'
INSERT INTO app.MessageQueueProduct
SELECT
	@productMessageTypeID			as MessageTypeID,
	CASE
		WHEN brandhct.traitValue IS NULL THEN @stagedMessageStatusID
		WHEN merchhctesb.traitValue IS NULL THEN @stagedMessageStatusID
		WHEN finhct.traitValue IS NULL THEN @stagedMessageStatusID
		WHEN taxhct.traitValue IS NULL THEN @stagedMessageStatusID
		ELSE @readyMessageStatusID
	END								as MessageStatusID,
	NULL							as MessageHistoryID,
	SYSDATETIME()					as InsertDate,
	dii.itemID						as ItemID,
	@localeID						as LocaleID,
	it.itemTypeCode					as ItemTypeCode,
	it.itemTypeDesc					as ItemTypeDesc,
	sc.scanCodeID					as ScanCodeID,
	sc.scanCode						as ScanCode,
	sct.scanCodeTypeID				as ScanCodeTypeID,
	sct.scanCodeTypeDesc			as ScanCodeTypeDesc,
	pd.traitValue					as ProductDescription,
	pos.traitValue					as PosDescription,
	pu.traitValue					as PackageUnit,
	fse.traitValue					as FoodStamp,	
	CASE 
		WHEN ds.traitValue IS NULL THEN '0'
		ELSE ds.traitValue
	END								as DepartmentSale,
	brandhc.hierarchyClassID		as BrandID,
	brandhc.hierarchyClassName		as BrandName,
	brandhc.hierarchyLevel			as BrandLevel,
	brandhc.hierarchyParentClassID	as BrandParentId,
	browhc.hierarchyClassID			as BrowsingId,
	browhc.hierarchyClassName		as BrowswingName,
	browhc.hierarchyLevel			as BrowsingLevel,
	browhc.hierarchyParentClassID	as BrowsingParentId,
	merchhc.hierarchyClassID		as MerchandiseId,
	merchhc.hierarchyClassName		as MerchandiseName,
	merchhc.hierarchyLevel			as MerchandiseLevel,
	merchhc.hierarchyParentClassID	as MerchandiseParentId,
	taxhc.hierarchyClassID			as TaxID,
	taxhc.hierarchyClassName		as TaxName,
	taxhc.hierarchyLevel			as TaxLevel,
	taxhc.hierarchyParentClassID	as TaxParentId,
	finhc.hierarchyClassID			as FinancialId,
	finhc.hierarchyClassName		as FinancialName,
	finhc.hierarchyLevel			as FinancialLevel,
	finhc.hierarchyParentClassID	as FinancialParentId,
	null,
	null	
FROM 
	@distinctItemIDs				dii
	JOIN Item						i			ON	dii.itemID					= i.itemID
	JOIN ItemType					it			ON	i.itemTypeID				= it.itemTypeID
	JOIN ScanCode					sc			ON	i.itemID					= sc.itemID
	JOIN ScanCodeType				sct			ON	sc.scanCodeTypeID			= sct.scanCodeTypeID
	JOIN ItemTrait					vdit		ON	i.itemID					= vdit.itemID
													AND vdit.traitID			= @validationDateTraitID
	JOIN ItemTrait					pd			ON	i.itemID					= pd.itemID
													AND pd.traitID				= @productDescriptionTraitID
	JOIN ItemTrait					pos			ON	i.itemID					= pos.itemID
													AND pos.traitID				= @posTraitID
	JOIN ItemTrait					pu			ON	i.itemID					= pu.itemID
													AND pu.traitID				= @packageUnitTraitID
	JOIN ItemTrait					fse			ON	i.itemID					= fse.itemID
													AND fse.traitID				= @foodStampEligibleTraitID
	JOIN ItemHierarchyClass			brandihc	ON	i.itemID					= brandihc.itemID
	JOIN HierarchyClass				brandhc		ON	brandihc.hierarchyClassID	= brandhc.hierarchyClassID
													AND brandhc.hierarchyID		= @brandHierarchyID
	LEFT JOIN HierarchyClassTrait	brandhct	ON	brandhc.hierarchyClassID	= brandhct.hierarchyClassID
													AND brandhct.traitID		= @sentToEsbTraitID
	JOIN ItemHierarchyClass			merchihc	ON	i.itemID					= merchihc.itemID
	JOIN HierarchyClass				merchhc		ON	merchihc.hierarchyClassID	= merchhc.hierarchyClassID
													AND merchhc.hierarchyID		= @merchandiseClassID
	JOIN HierarchyClassTrait		merchhctfin	ON	merchhc.hierarchyClassID	= merchhctfin.HierarchyClassID
													AND merchhctfin.traitID		= @merchFinMappingTraitID
	LEFT JOIN HierarchyClassTrait	merchhctesb	ON	merchhc.hierarchyClassID	= merchhctesb.hierarchyClassID
													AND merchhctesb.traitID		= @sentToEsbTraitID
	JOIN HierarchyClass				finhc		ON	merchhctfin.traitValue		= finhc.hierarchyClassName
													AND finhc.hierarchyID		= @financialClassID	
	LEFT JOIN HierarchyClassTrait	finhct		ON	finhc.hierarchyClassID		= finhct.hierarchyClassID
													AND finhct.traitID			= @sentToEsbTraitID
	JOIN ItemHierarchyClass			taxihc		ON	i.itemID					= taxihc.itemID
	JOIN HierarchyClass				taxhc		ON	taxihc.hierarchyClassID		= taxhc.hierarchyClassID
													AND taxhc.hierarchyID		= @taxClassID
	LEFT JOIN HierarchyClassTrait	taxhct		ON	taxhc.hierarchyClassID		= taxhct.hierarchyClassID
													AND taxhct.traitID			= @sentToEsbTraitID
	LEFT JOIN ItemTrait				ds			ON	i.itemID					= ds.itemID
													AND ds.traitID				= @departmentSaleTraitID
	LEFT JOIN #tempBrowsing			browhc		on	i.itemID					= browhc.itemID
