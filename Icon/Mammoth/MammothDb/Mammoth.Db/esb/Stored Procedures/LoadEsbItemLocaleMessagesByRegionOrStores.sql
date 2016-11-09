CREATE PROCEDURE [esb].[LoadEsbItemLocaleMessagesByRegionOrStores]
	@Region nvarchar(2),
	@BusinessUnit int = NULL,
	@BusinessUnitList nvarchar(500) = NULL
AS

-- Date
DECLARE @date datetime2(7);
SET @date = SYSDATETIME();

-- Message Queue IDs
DECLARE @MessageTypeId int;
DECLARE @MessageStatusId int;
DECLARE @MessageActionId int;

SET @MessageTypeId		= (SELECT MessageTypeId FROM esb.MessageType WHERE MessageTypeName = 'Item Locale');
SET @MessageStatusId	= (SELECT MessageStatusId FROM esb.MessageStatus WHERE MessageStatusName = 'Ready');
SET @MessageActionId	= (SELECT MessageActionId FROM esb.MessageAction WHERE MessageActionName = 'AddOrUpdate');

PRINT '
--======================================--
MessageTypeId: ' + CONVERT(nvarchar, @MessageTypeId) + '
MessageStatusId: ' + CONVERT(nvarchar, @MessageStatusId) + '
MessageActionId: ' + CONVERT(nvarchar, @MessageActionId) + '
--======================================--';

-- Attributes
DECLARE @chicagoBabyId int;
DECLARE @colorAddedId int;
DECLARE @countryOfProcessingId int;
DECLARE @estId int;
DECLARE @exclusiveId int;
DECLARE @linkedScanCodeId int;
DECLARE @numDigitsToScaleId int;
DECLARE @originId int;
DECLARE @scaleExtraTextId int;
DECLARE @tagUomId int;

SET @chicagoBabyId			= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Chicago Baby');
SET @colorAddedId			= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Color Added');
SET @countryOfProcessingId	= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Country of Processing');
SET @estId					= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Electronic Shelf Tag');
SET @exclusiveId			= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Exclusive');
SET @linkedScanCodeId		= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Linked Scan Code');
SET @numDigitsToScaleId		= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Number of Digits Sent To Scale');
SET @originId				= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Origin');
SET @scaleExtraTextId		= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Scale Extra Text');
SET @tagUomId				= (SELECT AttributeID FROM Attributes WHERE AttributeDesc = 'Tag UOM');

PRINT '
--======================================--
@chicagoBabyId: ' + CONVERT(nvarchar, @chicagoBabyId) + '
@colorAddedId: ' + CONVERT(nvarchar, @colorAddedId) + '
@countryOfProcessingId: ' + CONVERT(nvarchar, @countryOfProcessingId) + '
@estId: ' + CONVERT(nvarchar, @estId) + '
@exclusiveId: ' + CONVERT(nvarchar, @exclusiveId) + '
@linkedScanCodeId: ' + CONVERT(nvarchar, @linkedScanCodeId) + '
@@numDigitsToScaleId: ' + CONVERT(nvarchar, @numDigitsToScaleId) + '
@originId: ' + CONVERT(nvarchar, @originId) + '
@scaleExtraTextId: ' + CONVERT(nvarchar, @scaleExtraTextId) + '
@tagUomId: ' + CONVERT(nvarchar, @tagUomId) + '
--======================================--';

DECLARE @sql nvarchar(max);
SET @sql = N'
INSERT INTO esb.MessageQueueItemLocale
SELECT
	@MessageTypeId				as [MessageTypeId],
    @MessageStatusId			as [MessageStatusId],
    NULL						as [MessageHistoryId],
    @MessageActionId			as [MessageActionId],
    @date						as [InsertDate],
	@Region						as [RegionCode],
    l.BusinessUnitID			as [BusinessUnitId],
    ia.ItemID					as [ItemId],
    it.ItemTypeCode				as [ItemTypeCode],
    it.ItemTypeDesc				as [ItemTypeDesc],
    l.StoreName					as [LocaleName],
    i.ScanCode					as [ScanCode],
    ia.Discount_Case			as [CaseDiscount],
    ia.Discount_TM				as [TmDiscount],
    ia.Restriction_Age			as [AgeRestriction],
    ia.Restriction_Hours		as [RestrictedHours],
    ia.Authorized				as [Authorized],
    ia.Discontinued				as [Discontinued],
    ia.LabelTypeDesc			as [LabelTypeDescription],
    ia.LocalItem				as [LocalItem],
    ia.Product_Code				as [ProductCode],
    ia.RetailUnit				as [RetailUnit],
    ia.Sign_Desc				as [SignDescription],
    ia.Locality					as [Locality],
    ia.Sign_RomanceText_Long	as [SignRomanceLong],
    ia.Sign_RomanceText_Short	as [SignRomanceShort],
    ca.AttributeValue			as [ColorAdded],
    cop.AttributeValue			as [CountryOfProcessing],
    o.AttributeValue			as [Origin],
    est.AttributeValue			as [ElectronicShelfTag],
    exc.AttributeValue			as [Exclusive],
    num.AttributeValue			as [NumberOfDigitsSentToScale],
    cb.AttributeValue			as [ChicagoBaby],
    tag.AttributeValue			as [TagUom],
    lnk.AttributeValue			as [LinkedItem],
    sce.AttributeValue			as [ScaleExtraText],
	ia.MSRP						as [Msrp],
    NULL						as [InProcessBy],
    NULL						as [ProcessedDate]
FROM
	JOIN ItemAttributes_Locale_' + @Region + ' ia
	Items									i	on	ia.ItemID = i.ItemID
	JOIN ItemTypes							it	on	i.ItemTypeID		= it.ItemTypeID
	JOIN Locales_' + @Region + '							l	on	ia.BusinessUnitID	= l.BusinessUnitID

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	cb	on	i.ItemID			= cb.ItemID
												AND l.LocaleID			= cb.LocaleID
												AND cb.AttributeID		= @chicagoBabyId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	ca	on	i.ItemID			= ca.ItemID
												AND l.LocaleID			= ca.LocaleID
												AND ca.AttributeID		= @colorAddedId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	cop	on	i.ItemID			= cop.ItemID
												AND l.LocaleID			= cop.LocaleID
												AND cop.AttributeID		= @countryOfProcessingId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	est	on	i.ItemID			= est.ItemID
												AND l.LocaleID			= est.LocaleID

												AND est.AttributeID		= @estId
	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	exc	on	i.ItemID			= exc.ItemID
												AND l.LocaleID			= exc.LocaleID
												AND est.AttributeID		= @exclusiveId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	lnk	on	i.ItemID			= lnk.ItemID
												AND l.LocaleID			= lnk.LocaleID
												AND est.AttributeID		= @linkedScanCodeId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	num	on	i.ItemID			= num.ItemID
												AND l.LocaleID			= num.LocaleID
												AND est.AttributeID		= @numDigitsToScaleId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	o	on	i.ItemID			= o.ItemID
												AND l.LocaleID			= o.LocaleID
												AND est.AttributeID		= @originId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	sce	on	i.ItemID			= sce.ItemID
												AND l.LocaleID			= sce.LocaleID
												AND est.AttributeID		= @scaleExtraTextId

	LEFT JOIN ItemAttributes_Locale_' + @Region + '_Ext	tag	on	i.ItemID			= tag.ItemID
												AND l.LocaleID			= tag.LocaleID
												AND est.AttributeID		= @tagUomId
WHERE
	(l.BusinessUnitID = @BusinessUnit 
		OR @BusinessUnit IS NULL)
	AND (l.BusinessUnitID IN (SELECT Key_Value FROM dbo.fn_Parse_List(@BusinessUnitList, ''|''))
		OR @BusinessUnitList IS NULL);';

PRINT @sql;

DECLARE @paramDefinitions nvarchar(max) =
	N'@Region nvarchar(2),
	@BusinessUnit int,
	@BusinessUnitList nvarchar(500),
	@date datetime2(7),
	@MessageTypeId int,
	@MessageStatusId int,
	@MessageActionId int,
	@chicagoBabyId int,
	@colorAddedId int,
	@countryOfProcessingId int,
	@estId int,
	@exclusiveId int,
	@linkedScanCodeId int,
	@numDigitsToScaleId int,
	@originId int,
	@scaleExtraTextId int,
	@tagUomId int';

EXECUTE sp_executesql 
	@sql,
	@paramDefinitions,
	@Region,
	@BusinessUnit,
	@BusinessUnitList,
	@date,
	@MessageTypeId,
	@MessageStatusId,
	@MessageActionId,
	@chicagoBabyId,
	@colorAddedId,
	@countryOfProcessingId,
	@estId,
	@exclusiveId,
	@linkedScanCodeId,
	@numDigitsToScaleId,
	@originId,
	@scaleExtraTextId,
	@tagUomId;

-- Return rows that were just inserted
SELECT * FROM esb.MessageQueueItemLocale WHERE InsertDate = @date;