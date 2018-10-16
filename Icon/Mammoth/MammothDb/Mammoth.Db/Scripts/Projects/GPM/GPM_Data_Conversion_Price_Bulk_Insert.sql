USE Mammoth
go

print 'Creating temp GPM Prices table...'
IF OBJECT_ID('tempdb..#gpmPrices') IS NOT NULL
	DROP TABLE #gpmPrices
CREATE TABLE #gpmPrices
(
	GUID UNIQUEIDENTIFIER,
	ITEM_ID INT,
	STORE_NUMBER INT,
	PRICE SMALLMONEY,
	MULTIPLE TINYINT,
	SELLING_UOM NVARCHAR(3),
	PRICE_TYPE_CODE NVARCHAR(3),
	PRICE_ATTRIBUTE_CODE NVARCHAR(10),
	TAG_EXPIRATION_DATE DATETIME2(0) NULL,
	START_DATE DATETIME2(0),
	END_DATE DATETIME2(0) NULL,
	CREATED_DATE DATETIME2(0),
	REGION_CODE NVARCHAR(2),
	SCAN_CODE NVARCHAR(13),
	REWARDS_PERCENT_OFF DECIMAL(5,2) NULL
)

print 'Inserting GPM prices into temp GPM Prices table...'
bulk insert #gpmPrices
from ''  --left blank intentionally: needs to be specified and updated when this script is used in an ECC
with 
(
	FIRSTROW = 2,
	FIELDTERMINATOR = '|',
	ROWTERMINATOR = '0x0a'
);

go

print 'Inserting temp GPM Prices into stage.GpmDataConversion table...'
INSERT INTO stage.GpmDataConversion
(
	Region,
	GpmID,
	ItemID,
	BusinessUnitID,
	StartDate,
	EndDate,
	Price,
	PriceType,
	PriceTypeAttribute,
	SellableUOM,
	CurrencyCode,
	Multiple,
	NewTagExpiration,
	PercentOff
)
SELECT 
	t.REGION_CODE,
	t.GUID,
	t.ITEM_ID,
	t.STORE_NUMBER,
	t.START_DATE,
	t.END_DATE,
	t.PRICE,
	t.PRICE_TYPE_CODE,
	t.PRICE_ATTRIBUTE_CODE,
	t.SELLING_UOM,
	'USD',
	t.MULTIPLE,
	t.TAG_EXPIRATION_DATE,
	t.REWARDS_PERCENT_OFF
FROM #gpmPrices t
go

print 'Executing stage.GpmInsertPriceDataConversion'
go
EXEC stage.GpmInsertPriceDataConversion

print 'Finished'