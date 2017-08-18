CREATE PROCEDURE infor.PriceExtractQuery
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @today DATETIME = CONVERT(DATE, getdate()) 

IF EXISTS (SELECT COUNT(1) FROM   [infor].[tmpGpmLatestPbd])
	TRUNCATE TABLE [infor].[tmpGpmLatestPbd]

INSERT INTO [infor].[tmpGpmLatestPbd]
(
	Item_Key,
	Store_No,
	PriceBatchDetailId
)
SELECT
	pbd.Item_Key,
	pbd.Store_No,
	MAX(pbd.PriceBatchDetailId) as PriceBatchDetailID
FROM PriceBatchDetail	pbd 
JOIN PriceBatchHeader	pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
JOIN Item				i	on i.Item_key = pbd.Item_key
JOIN Store				s	on s.Store_No = pbd.Store_No
JOIN StoreItem			si	on si.Store_No = s.Store_No 
							AND si.Item_Key = i.Item_Key
WHERE i.Deleted_Item = 0 
	AND i.Remove_Item = 0
	AND pbh.PriceBatchStatusID = 6
	AND pbd.Expired = 0 
	AND pbd.PriceChgTypeId IS NOT NULL
	AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	AND si.Authorized = 1
	AND pbd.StartDate <= @today
	AND (pbd.Sale_End_Date IS NULL OR pbd.Sale_End_Date >= @today)
GROUP BY pbd.Item_Key, pbd.Store_No, s.BusinessUnit_ID

IF EXISTS (SELECT COUNT(1) FROM   [infor].[tmpgpmfuturepbd])
	TRUNCATE TABLE [infor].[tmpgpmfuturepbd]

INSERT INTO infor.tmpGpmFuturePbd
(
	Item_Key,
	Store_No,
	PriceBatchDetailID
)
SELECT
	pbd.Item_Key,
	pbd.Store_No,
	pbd.PriceBatchDetailID
FROM PriceBatchDetail pbd 
JOIN PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
JOIN infor.tmpGpmLatestPbd lpbd on lpbd.Item_key = pbd.Item_key and lpbd.Store_No = pbd.Store_No
JOIN Price p on p.Store_No = lpbd.Store_No and p.Item_Key = lpbd.Item_Key
WHERE pbd.Expired = 0 
	AND pbd.PriceChgTypeId IS NOT NULL
	AND pbd.PriceBatchDetailID > lpbd.PriceBatchDetailId
	AND ((pbd.InsertApplication = 'Sale Off' AND ISNULL(p.Price, -1) <> ISNULL(pbd.Price, -1) AND pbd.PriceChgTypeID = 1)
	 OR (pbd.InsertApplication = 'Sale Off' AND pbd.PriceChgTypeID <> 1)
	 OR pbd.InsertApplication <> 'Sale Off')
	AND pbd.StartDate > @today
	AND pbd.StartDate < DATEADD(day,15,@today)

--Current REGs
SELECT 
	vsc.inforItemId as 'ITEM_ID', 
	ii.Identifier as 'SCAN_CODE',
	s.BusinessUnit_ID as 'STORE_NUMBER', 
	'REG' as 'PRICE_TYPE', 
	p.Multiple as MULTIPLE,
	p.Price as PRICE, 
	CASE 
		WHEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			THEN 'LB'
		 ELSE 'EA' 
	END AS 'SELLING_UOM',
	@today as 'START_DATE', 
	NULL as 'END_DATE', 
	NULL as 'INSERT_DATE',
	srm.Region_Code,
	c.CurrencyCode
FROM Price                  p
JOIN Item					i	on p.Item_Key = i.Item_Key 
JOIN ItemIdentifier			ii	on p.Item_Key = ii.Item_Key
JOIN ItemUnit				runit on i.Retail_Unit_ID = runit.Unit_ID
JOIN ValidatedScanCode		vsc on vsc.ScanCode = ii.Identifier
JOIN Store					s	on p.Store_No = s.Store_No
JOIN StoreItem			    si	on si.Store_No = s.Store_No 
							    AND si.Item_Key = i.Item_Key
JOIN StoreJurisdiction		sj	on s.StoreJurisdictionID = sj.StoreJurisdictionID
JOIN Currency				c	on sj.CurrencyID = c.CurrencyID
JOIN StoreRegionMapping     srm on s.Store_No = srm.Store_No
LEFT JOIN ItemUomOverride	iuo on iuo.Item_Key = p.Item_Key 
								AND iuo.Store_No = p.Store_No
LEFT JOIN ItemUnit			rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
WHERE 
	i.Deleted_Item = 0 
	AND i.Remove_Item = 0
	AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	AND si.Authorized = 1
	AND ii.Default_Identifier = 1
UNION
--Current TPRs
SELECT 
    vsc.inforItemId as 'ITEM_ID', 
	ii.Identifier as 'SCAN_CODE',
	s.BusinessUnit_ID as 'STORE_NUMBER', 
	pct.PriceChgTypeDesc as 'PRICE_TYPE', 
	p.Multiple as MULTIPLE,
	p.Sale_Price as PRICE, 
	CASE 
		WHEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			THEN 'LB'
		 ELSE 'EA' 
	END AS 'SELLING_UOM',
	p.Sale_Start_Date as 'START_DATE', 
	p.Sale_End_Date  as 'END_DATE', 
	NULL as 'INSERT_DATE',
	srm.Region_Code,
	c.CurrencyCode
FROM Price                  p
JOIN Item					i	on p.Item_Key = i.Item_Key 
JOIN ItemIdentifier			ii	on p.Item_Key = ii.Item_Key
JOIN ItemUnit				runit on i.Retail_Unit_ID = runit.Unit_ID
JOIN ValidatedScanCode		vsc on vsc.ScanCode = ii.Identifier
JOIN Store					s	on p.Store_No = s.Store_No
JOIN StoreItem			    si	on si.Store_No = s.Store_No 
							    AND si.Item_Key = i.Item_Key
JOIN StoreJurisdiction		sj	on s.StoreJurisdictionID = sj.StoreJurisdictionID
JOIN Currency				c	on sj.CurrencyID = c.CurrencyID
JOIN StoreRegionMapping     srm on s.Store_No = srm.Store_No
JOIN PriceChgType			pct on pct.PriceChgTypeID = p.PriceChgTypeID
LEFT JOIN ItemUomOverride	iuo on iuo.Item_Key = p.Item_Key 
								AND iuo.Store_No = p.Store_No
LEFT JOIN ItemUnit			rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
WHERE 
	i.Deleted_Item = 0 
	AND i.Remove_Item = 0
	AND ii.Deleted_Identifier = 0
	AND ii.Remove_Identifier = 0
	AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	AND si.Authorized = 1
	AND ii.Default_Identifier = 1
	AND p.Sale_Price IS NOT NULL
	AND p.Sale_Start_Date <= @today
	AND p.Sale_End_Date >=@today
UNION
--Future REGs (excluding Sale-Off)
SELECT 
	vsc.inforItemId as 'ITEM_ID', 
	ii.Identifier as 'SCAN_CODE',
	s.BusinessUnit_ID as 'STORE_NUMBER', 
	'REG' as 'PRICE_TYPE', 
	pbd.Multiple as MULTIPLE,
	pbd.Price as PRICE, 
	CASE 
		WHEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			THEN 'LB'
		 ELSE 'EA' 
	END AS 'SELLING_UOM',
	pbd.StartDate as 'START_DATE', 
	pbd.Sale_End_Date as 'END_DATE', 
	pbd.Insert_Date as 'INSERT_DATE',
	srm.Region_Code,
	c.CurrencyCode
FROM infor.tmpGpmFuturePbd	l
JOIN Item					i	on l.Item_Key = i.Item_Key 
JOIN ItemIdentifier			ii	on l.Item_Key = ii.Item_Key
JOIN PriceBatchDetail		pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
JOIN PriceChgType			pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
JOIN ItemUnit				runit on i.Retail_Unit_ID = runit.Unit_ID
JOIN ValidatedScanCode		vsc on vsc.ScanCode = ii.Identifier
JOIN Store					s	on pbd.Store_No = s.Store_No
JOIN StoreJurisdiction		sj	on s.StoreJurisdictionID = sj.StoreJurisdictionID
JOIN Currency				c	on sj.CurrencyID = c.CurrencyID
JOIN StoreRegionMapping     srm on s.Store_No = srm.Store_No
LEFT JOIN ItemUomOverride	iuo on iuo.Item_Key = l.Item_Key 
								AND iuo.Store_No = l.Store_No
LEFT JOIN ItemUnit			rounit WITH (NOLOCK) on iuo.Retail_Unit_ID = rounit.Unit_ID
WHERE 
	pbd.Price IS NOT NULL
	AND pbd.Sale_End_Date is null
	AND ii.Default_Identifier = 1
UNION
--Future TPRs (excluding Sale-Off)
SELECT 
	vsc.inforItemId as 'ITEM_ID', 
	ii.Identifier as 'SCAN_CODE',
	s.BusinessUnit_ID as 'STORE_NUMBER', 
	pct.PriceChgTypeDesc as 'PRICE_TYPE', 
	pbd.Multiple as MULTIPLE,
	pbd.Sale_Price as PRICE, 
	CASE 
		WHEN ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) in ('LB', 'KG')
			THEN 'LB'
		 ELSE 'EA' 
	END AS 'SELLING_UOM',
	pbd.StartDate as 'START_DATE', 
	pbd.Sale_End_Date as 'END_DATE', 
	pbd.Insert_Date as 'INSERT_DATE',
	srm.Region_Code,
	c.CurrencyCode
FROM infor.tmpGpmFuturePbd	l
JOIN Item					i	on l.Item_Key = i.Item_Key 
JOIN ItemIdentifier			ii	on l.Item_Key = ii.Item_Key 
JOIN PriceBatchDetail		pbd on l.PriceBatchDetailId = pbd.PriceBatchDetailID
JOIN PriceChgType			pct on pct.PriceChgTypeID = pbd.PriceChgTypeID
JOIN ItemUnit				runit on i.Retail_Unit_ID = runit.Unit_ID
JOIN ValidatedScanCode		vsc on vsc.ScanCode = ii.Identifier
JOIN Store					s	on pbd.Store_No = s.Store_No
JOIN StoreJurisdiction		sj	on s.StoreJurisdictionID = sj.StoreJurisdictionID
JOIN Currency				c	on sj.CurrencyID = c.CurrencyID
JOIN StoreRegionMapping     srm on s.Store_No = srm.Store_No
LEFT JOIN ItemUomOverride	iuo on iuo.Item_Key = l.Item_Key
							AND iuo.Store_No = l.Store_No
LEFT JOIN ItemUnit			rounit on iuo.Retail_Unit_ID = rounit.Unit_ID
WHERE
	pbd.Sale_Price IS NOT NULL
	AND ii.Default_Identifier = 1
END