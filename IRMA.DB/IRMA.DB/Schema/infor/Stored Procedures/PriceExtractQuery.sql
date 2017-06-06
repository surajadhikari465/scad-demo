CREATE PROCEDURE infor.PriceExtractQuery
AS
BEGIN

DECLARE @today DATETIME = CONVERT(DATE, getdate()) 

IF EXISTS (SELECT COUNT(1) FROM   [infor].[tmpGpmLatestPbd])
	TRUNCATE TABLE [infor].[tmpGpmLatestPbd]

INSERT INTO [infor].[tmpGpmLatestPbd]
(
	Item_Key,
	Store_No,
	BusinessUnit_ID,
	Region,
	PriceBatchDetailId
)
SELECT
	pbd.Item_Key,
	pbd.Store_No,
	s.BusinessUnit_ID,
	srm.Region_Code as Region,
	MAX(pbd.PriceBatchDetailId) as PriceBatchDetailID
FROM PriceBatchDetail	pbd
JOIN PriceBatchHeader	pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
JOIN Item				i	on i.Item_key = pbd.Item_key
JOIN ItemIdentifier		ii	on i.Item_Key = ii.Item_key 
JOIN Store				s	on s.Store_No = pbd.Store_No
JOIN StoreItem			si	on si.Store_No = s.Store_No 
							AND si.Item_Key = i.Item_Key
JOIN StoreRegionMapping srm on s.Store_No = srm.Store_No
WHERE i.Deleted_Item = 0 
	AND i.Remove_Item = 0
	AND ii.Deleted_Identifier = 0 and ii.Remove_Identifier = 0 
	AND ii.Default_Identifier = 1
	AND pbh.PriceBatchStatusID = 6
	AND pbd.Expired = 0 
	AND pbd.PriceChgTypeId IS NOT NULL
	AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	AND si.Authorized = 1
	AND pbd.StartDate <= @today
	AND (pbd.Sale_End_Date IS NULL OR pbd.Sale_End_Date >= @today)
GROUP BY pbd.Item_Key, pbd.Store_No, s.BusinessUnit_ID, srm.Region_Code

IF EXISTS (SELECT COUNT(1) FROM   [infor].[tmpgpmfuturepbd])
	TRUNCATE TABLE [infor].[tmpgpmfuturepbd]

INSERT INTO infor.tmpGpmFuturePbd
(
	Item_Key,
	Store_No,
	BusinessUnit_ID,
	Region,
	PriceBatchDetailID
)
SELECT
	pbd.Item_Key,
	pbd.Store_No,
	lpbd.BusinessUnit_ID,
	lpbd.Region,
	pbd.PriceBatchDetailID
FROM PriceBatchDetail pbd
JOIN PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
JOIN infor.tmpGpmLatestPbd lpbd on lpbd.Item_key = pbd.Item_key and lpbd.Store_No = pbd.Store_No
WHERE pbd.Expired = 0 
	AND pbh.PriceBatchStatusID = 6
	AND pbd.PriceChgTypeId IS NOT NULL
	AND pbd.PriceBatchDetailID > lpbd.PriceBatchDetailId
	AND pbd.StartDate > @today

INSERT INTO infor.tmpGpmLatestPbd
SELECT Item_Key, Store_No, BusinessUnit_ID, Region, PriceBatchDetailId
FROM infor.tmpGpmFuturePbd

SELECT 
	vsc.inforItemId as 'Item ID', 
	ii.Identifier as 'Scan Code', 
	pbd.Price as Price, 
	'REG' as 'Price Type', 
	CASE 
		WHEN pbd.Sale_End_Date is null 
			THEN pbd.StartDate 
		ELSE Convert(Date,pbd.Sale_End_Date + 1, 102) 
	END as 'Start Date', 
	NULL as 'End Date', 
	pbd.Insert_Date as 'Insert Date', 
	CASE 
		WHEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			THEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation)
		ELSE 'EA'
	END as 'Selling UOM', 
	pbd.Multiple, 
	l.BusinessUnit_ID as Location,
	l.Region,
	c.CurrencyCode
FROM infor.tmpGpmLatestPbd	l
JOIN Item					i	on l.Item_Key = i.Item_Key 
JOIN ItemIdentifier			ii	on l.Item_Key = ii.Item_Key
JOIN PriceBatchDetail		pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
JOIN PriceChgType			pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
JOIN ItemUnit				runit on i.Retail_Unit_ID = runit.Unit_ID
JOIN ValidatedScanCode		vsc on vsc.ScanCode = ii.Identifier
JOIN Store					s	on pbd.Store_No = s.Store_No
JOIN StoreJurisdiction		sj	on s.StoreJurisdictionID = sj.StoreJurisdictionID
JOIN Currency				c	on sj.CurrencyID = c.CurrencyID
LEFT JOIN ItemUomOverride	iuo on iuo.Item_Key = l.Item_Key 
								AND iuo.Store_No = l.Store_No
LEFT JOIN ItemUnit			rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
WHERE 
	pbd.Price IS NOT NULL
	AND ii.Default_Identifier = 1
UNION
SELECT 
	vsc.inforItemId as 'Item ID', 
	ii.Identifier as 'Scan Code', 
	pbd.Sale_Price as Price, 
	pct.PriceChgTypeDesc as 'Price Type', 
	pbd.StartDate as 'Start Date', 
	pbd.Sale_End_Date as 'End Date', 
	pbd.Insert_Date as 'Insert Date', 
	CASE 
		WHEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			THEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation)
		 ELSE 'EA' 
	END AS 'Selling UOM',
	pbd.Multiple, 
	l.BusinessUnit_ID as Location,
	l.Region,
	c.CurrencyCode
FROM infor.tmpGpmLatestPbd	l
JOIN Item					i	on l.Item_Key = i.Item_Key 
JOIN ItemIdentifier			ii	on l.Item_Key = ii.Item_Key 
JOIN PriceBatchDetail		pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
JOIN PriceChgType			pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
JOIN ItemUnit				runit on i.Retail_Unit_ID = runit.Unit_ID
JOIN ValidatedScanCode		vsc on vsc.ScanCode = ii.Identifier
JOIN Store					s	on pbd.Store_No = s.Store_No
JOIN StoreJurisdiction		sj	on s.StoreJurisdictionID = sj.StoreJurisdictionID
JOIN Currency				c	on sj.CurrencyID = c.CurrencyID
LEFT JOIN ItemUomOverride	iuo on iuo.Item_Key = l.Item_Key
							AND iuo.Store_No = l.Store_No
LEFT JOIN ItemUnit			rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
WHERE
	pbd.Sale_Price IS NOT NULL
	AND ii.Default_Identifier = 1
END