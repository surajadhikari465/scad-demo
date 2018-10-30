DECLARE @RegPriceTypeID INT = (SELECT TOP 1 PriceChgTypeID FROM PriceChgType WHERE PriceChgTypeDesc = 'REG');
DECLARE @RegularPromoPricingMethodId INT = (SELECT TOP 1 PricingMethod_ID FROM PricingMethod WHERE PricingMethod_Name = 'Regular Promo')

IF OBJECT_ID('tempdb..#pricesUpdated') IS NOT NULL
	DROP TABLE #pricesUpdated
CREATE TABLE #pricesUpdated
(
	Item_Key INT NOT NULL,
	Store_No INT NOT NULL,
	PriceChgTypeID INT NOT NULL,
	Price smallmoney NOT NULL,
	Multiple tinyint NOT NULL,
	PricingMethod_ID INT NOT NULL
)

INSERT INTO #pricesUpdated
(
	Item_Key,
	Store_No,
	PriceChgTypeID,
	Price,
	Multiple,
	PricingMethod_ID
)
SELECT
	ii.Item_Key,
	s.Store_No,
	@RegPriceTypeID,
	c.PRICE,
	c.MULTIPLE,
	@RegularPromoPricingMethodId
FROM infor.GpmConversionTprCancellations c
JOIN dbo.ValidatedScanCode v on c.ITEM_ID = v.InforItemId
JOIN dbo.Store s on c.STORE_NUMBER = s.BusinessUnit_ID
JOIN dbo.ItemIdentifier ii on v.ScanCode = ii.Identifier
WHERE ii.Default_Identifier = 1
AND ii.Deleted_Identifier = 0
AND c.PRICE_TYPE_CODE = 'REG'

CREATE NONCLUSTERED INDEX IX_ItemKey_StoreNo on #pricesUpdated (Store_No ASC, Item_Key ASC);

UPDATE dbo.Price
SET
	PriceChgTypeId = u.PriceChgTypeID,
	Price = u.Price,
	POSPrice = u.Price,
	Multiple = u.Multiple,
	PricingMethod_ID = u.PricingMethod_ID
FROM dbo.Price p
INNER JOIN #pricesUpdated u on p.Item_Key = u.Item_Key
	AND p.Store_No = u.Store_No