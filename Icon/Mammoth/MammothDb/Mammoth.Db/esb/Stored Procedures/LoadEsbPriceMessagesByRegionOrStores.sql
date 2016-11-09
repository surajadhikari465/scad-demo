CREATE PROCEDURE [esb].[LoadEsbPriceMessagesByRegionOrStores]
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

DECLARE @sql nvarchar(max);
SET @sql = N'
INSERT INTO esb.MessageQueuePrice
SELECT
	@MessageTypeId		as MessageTypeId,
	@MessageStatusId	as MessageStatusId,
	NULL				as MessageHistoryId,
	@MessageActionId	as MessageActionId,
	@date				as InsertDate,
	i.ItemID			as ItemId,
	it.ItemTypeCode		as ItemTypeCode,
	it.ItemTypeDesc		as ItemTypeDesc,
	l.BusinessUnitID	as BusinessUnitId,
	l.StoreName			as LocaleName,
	i.ScanCode			as ScanCode,
	p.PriceUom			as UomCode,
	c.CurrencyCode		as CurrencyCode,
    CASE WHEN p.PriceType <> ''REG'' THEN ''TPR'' ELSE p.PriceType END as PriceTypeCode,
	CASE WHEN p.PriceType <> ''REG'' THEN p.PriceType END as SubPriceTypeCode,
	p.Price				as Price,
	p.Multiple			as Multiple,
	p.StartDate			as StartDate,
	p.EndDate			as EndDate,
	NULL				as InProcessBy,
    NULL				as ProcessedDate
FROM
	Items			i
	JOIN Price_' + @Region + '	p	on	i.ItemID			= p.ItemID
	JOIN Locales_' + @Region + '	l	on	p.BusinessUnitID	= l.BusinessUnitID
	JOIN ItemTypes	it	on	i.ItemTypeID		= it.ItemTypeID
	JOIN Currency	c	on	p.CurrencyID		= c.CurrencyID
WHERE
	(@BusinessUnit = l.BusinessUnitID
		OR @BusinessUnit IS NULL)
	AND (l.BusinessUnitID IN (SELECT Key_Value FROM dbo.fn_Parse_List(@BusinessUnitList, ''|''))
		OR @BusinessUnitList IS NULL);';

PRINT @sql;

DECLARE @paramDefinitions nvarchar(max) =
	N'@BusinessUnit int,
	@BusinessUnitList nvarchar(500),
	@date datetime2(7),
	@MessageTypeId int,
	@MessageStatusId int,
	@MessageActionId int';

EXECUTE sp_executesql 
	@sql,
	@paramDefinitions,
	@BusinessUnit,
	@BusinessUnitList,
	@date,
	@MessageTypeId,
	@MessageStatusId,
	@MessageActionId;

-- Return rows that were just inserted
SELECT * FROM esb.MessageQueuePrice WHERE InsertDate = @date;
