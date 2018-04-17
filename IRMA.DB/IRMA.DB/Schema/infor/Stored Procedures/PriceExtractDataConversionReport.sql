CREATE PROCEDURE [infor].[PriceExtractDataConversionReport]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	DECLARE @today DATETIME = CONVERT(DATE, getdate())
	--Current REGs
	SELECT 
		 vsc.inforItemId AS 'ITEM_ID'
		,s.BusinessUnit_ID AS 'STORE_NUMBER'
		,ii.Identifier AS 'IDENTIFIER'
		,s.Store_Name AS 'STORE'
		,p.POSPrice AS 'PRICE'
		,p.Multiple AS 'MULTIPLE'
		,ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) AS 'SELLING_UOM'
		,'REG' AS 'PRICE_TYPE_CODE'
		,'REG' AS 'PRICE_ATTRIBUTE_CODE'
		,NULL AS 'NEW_TAG_EXPIRATION_DATE'
		,@today AS 'START_DATE'
		,NULL AS 'END_DATE'
		,@today AS 'CREATED_DATE'
		,srm.Region_Code AS 'REGION_CODE'
		,c.CurrencyCode AS 'CURRENCY_CODE'
	FROM Price p
	JOIN PriceChgType pct ON p.PriceChgTypeId = pct.PriceChgTypeID
	JOIN Item i ON p.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON p.Item_Key = ii.Item_Key
	JOIN ItemUnit runit ON i.Retail_Unit_ID = runit.Unit_ID
	JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
	JOIN Store s ON p.Store_No = s.Store_No
	JOIN StoreItem si ON si.Store_No = s.Store_No
		AND si.Item_Key = i.Item_Key
	JOIN StoreJurisdiction sj ON s.StoreJurisdictionID = sj.StoreJurisdictionID
	JOIN Currency c ON sj.CurrencyID = c.CurrencyID
	JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = p.Item_Key
		AND iuo.Store_No = p.Store_No
	LEFT JOIN ItemUnit rounit ON iuo.Retail_Unit_ID = rounit.Unit_ID
	WHERE i.Deleted_Item = 0
		AND i.Remove_Item = 0
		AND ii.Deleted_Identifier = 0
		AND ii.Remove_Identifier = 0
		AND (
			s.WFM_Store = 1
			OR s.Mega_Store = 1
			)
		AND s.Internal = 1
		AND s.StoreJurisdictionID = 1
		AND si.Authorized = 1
		AND NOT (
			pct.PriceChgTypeDesc IN (
				'DIS'
				,'CLR'
				,'CMP'
				,'FRC'
				,'GSP'
				,'EDV'
				,'NEW'
				)
			AND @today BETWEEN p.Sale_Start_Date
				AND p.Sale_End_Date
			)
		AND (SUBSTRING(CAST(CAST(p.POSPrice as decimal(9,4)) as varchar), CHARINDEX('.',CAST(p.POSPrice AS VARCHAR),1) + 3, 2) > 0
		OR  ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) NOT IN ('LB', 'EA'))
	
	UNION
	
	--Current Long Running TPRs that are becoming REGs
	SELECT 
		vsc.inforItemId AS 'ITEM_ID'
		,s.BusinessUnit_ID AS 'STORE_NUMBER'
		,ii.Identifier AS 'IDENTIFIER'
		,s.Store_Name AS 'STORE'
		,p.POSSale_Price
		,p.Sale_Multiple AS 'MULTIPLE'
		,ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) AS 'SELLING_UOM'
		,'REG' AS 'PRICE_TYPE_CODE'
		,CASE 
			WHEN pct.PriceChgTypeDesc = 'NEW'
				THEN 'REG'
			WHEN pct.PriceChgTypeDesc = 'FRC'
				THEN 'CMP'
			ELSE pct.PriceChgTypeDesc
		 END AS 'PRICE_ATTRIBUTE_CODE'
		,CASE 
			WHEN pct.PriceChgTypeDesc = 'NEW'
				THEN DATEADD(s, -1, DATEADD(day, DATEDIFF(DAY, 0, p.Sale_End_Date), 1))
			ELSE NULL
		 END AS 'NEW_TAG_EXPIRATION_DATE'
		,@today AS 'START_DATE'
		,NULL AS 'END_DATE'
		,@today AS 'CREATED_DATE'
		,srm.Region_Code AS 'REGION_CODE'
		,c.CurrencyCode AS 'CURRENCY_CODE'
	FROM Price p
	JOIN Item i ON p.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON p.Item_Key = ii.Item_Key
	JOIN ItemUnit runit ON i.Retail_Unit_ID = runit.Unit_ID
	JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
	JOIN Store s ON p.Store_No = s.Store_No
	JOIN StoreItem si ON si.Store_No = s.Store_No
		AND si.Item_Key = i.Item_Key
	JOIN StoreJurisdiction sj ON s.StoreJurisdictionID = sj.StoreJurisdictionID
	JOIN Currency c ON sj.CurrencyID = c.CurrencyID
	JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
	JOIN PriceChgType pct ON pct.PriceChgTypeID = p.PriceChgTypeID
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = p.Item_Key
		AND iuo.Store_No = p.Store_No
	LEFT JOIN ItemUnit rounit ON iuo.Retail_Unit_ID = rounit.Unit_ID
	WHERE i.Deleted_Item = 0
		AND i.Remove_Item = 0
		AND ii.Deleted_Identifier = 0
		AND ii.Remove_Identifier = 0
		AND (
			s.WFM_Store = 1
			OR s.Mega_Store = 1
			)
		AND s.Internal = 1
		AND s.StoreJurisdictionID = 1
		AND si.Authorized = 1
		AND p.POSSale_Price IS NOT NULL
		AND @today BETWEEN p.Sale_Start_Date
			AND p.Sale_End_Date
		AND pct.PriceChgTypeDesc IN (
			'DIS'
			,'CLR'
			,'CMP'
			,'FRC'
			,'GSP'
			,'EDV'
			,'NEW'
			)
		AND (SUBSTRING(CAST(CAST(p.POSSale_Price as decimal(9,4)) as varchar), CHARINDEX('.',CAST(p.POSSale_Price AS VARCHAR),1) + 3, 2) > 0
		OR  ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) NOT IN ('LB', 'EA'))

	UNION
	
	--Current TPRs
	SELECT 
		vsc.inforItemId AS 'ITEM_ID'
		,s.BusinessUnit_ID AS 'STORE_NUMBER'
		,ii.Identifier AS 'IDENTIFIER'
		,s.Store_Name AS 'STORE'
		,p.POSSale_Price AS 'PRICE'
		,p.Sale_Multiple AS 'MULTIPLE'
		,ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) AS 'SELLING_UOM'
		,'TPR' AS 'PRICE_TYPE_CODE'
		,CASE 
			WHEN pct.PriceChgTypeDesc = 'SAL'
				THEN 'SSAL'
			WHEN pct.PriceChgTypeDesc = 'FRZ'
				THEN 'MSAL'
			ELSE pct.PriceChgTypeDesc
			END AS 'PRICE_ATTRIBUTE_CODE'
		,NULL AS 'NEW_TAG_EXPIRATION_DATE'
		,@today AS 'START_DATE'
		,DATEADD(s, -1, DATEADD(day, DATEDIFF(DAY, 0, p.Sale_End_Date), 1)) AS 'END_DATE'
		,@today AS 'CREATED_DATE'
		,srm.Region_Code AS 'REGION_CODE'
		,c.CurrencyCode AS 'CURRENCY_CODE'
	FROM Price p
	JOIN Item i ON p.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON p.Item_Key = ii.Item_Key
	JOIN ItemUnit runit ON i.Retail_Unit_ID = runit.Unit_ID
	JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
	JOIN Store s ON p.Store_No = s.Store_No
	JOIN StoreItem si ON si.Store_No = s.Store_No
		AND si.Item_Key = i.Item_Key
	JOIN StoreJurisdiction sj ON s.StoreJurisdictionID = sj.StoreJurisdictionID
	JOIN Currency c ON sj.CurrencyID = c.CurrencyID
	JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
	JOIN PriceChgType pct ON pct.PriceChgTypeID = p.PriceChgTypeID
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = p.Item_Key
		AND iuo.Store_No = p.Store_No
	LEFT JOIN ItemUnit rounit ON iuo.Retail_Unit_ID = rounit.Unit_ID
	WHERE i.Deleted_Item = 0
		AND i.Remove_Item = 0
		AND ii.Deleted_Identifier = 0
		AND ii.Remove_Identifier = 0
		AND (
			s.WFM_Store = 1
			OR s.Mega_Store = 1
			)
		AND s.Internal = 1
		AND s.StoreJurisdictionID = 1
		AND si.Authorized = 1
		AND p.POSSale_Price IS NOT NULL
		AND @today BETWEEN p.Sale_Start_Date
			AND p.Sale_End_Date
		AND pct.PriceChgTypeDesc NOT IN (
			'DIS'
			,'CLR'
			,'CMP'
			,'FRC'
			,'GSP'
			,'EDV'
			,'NEW'
			)
		AND (SUBSTRING(CAST(CAST(p.POSSale_Price as decimal(9,4)) as varchar), CHARINDEX('.',CAST(p.POSSale_Price AS VARCHAR),1) + 3, 2) > 0
		OR  ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) NOT IN ('LB', 'EA'))

	UNION
	
	--Future REGs (excluding Sale-Off)
	SELECT 
		vsc.inforItemId AS 'ITEM_ID'
		,s.BusinessUnit_ID AS 'STORE_NUMBER'
		,ii.Identifier AS 'IDENTIFIER'
		,s.Store_Name AS 'STORE'
		,pbd.POSPrice AS 'PRICE'
		,pbd.Multiple AS 'MULTIPLE'
		,ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) AS 'SELLING_UOM'
		,'REG' AS 'PRICE_TYPE_CODE'
		,'REG' AS 'PRICE_ATTRIBUTE_CODE'
		,NULL AS 'NEW_TAG_EXPIRATION_DATE'
		,pbd.StartDate AS 'START_DATE'
		,NULL AS 'END_DATE'
		,@today AS 'CREATED_DATE'
		,srm.Region_Code
		,c.CurrencyCode
	FROM infor.tmpGpmFuturePbd l
	JOIN Item i ON l.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON l.Item_Key = ii.Item_Key
	JOIN PriceBatchDetail pbd ON l.PriceBatchDetailId = pbd.PriceBatchDetailID
	JOIN PriceChgType pct ON pct.PriceChgTypeID = pbd.PriceChgTypeID
	JOIN Price p ON p.Item_Key = l.Item_Key AND p.Store_No = l.Store_No
	JOIN ItemUnit runit ON i.Retail_Unit_ID = runit.Unit_ID
	JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
	JOIN Store s ON pbd.Store_No = s.Store_No
	JOIN StoreJurisdiction sj ON s.StoreJurisdictionID = sj.StoreJurisdictionID
	JOIN Currency c ON sj.CurrencyID = c.CurrencyID
	JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = l.Item_Key
		AND iuo.Store_No = l.Store_No
	LEFT JOIN ItemUnit rounit WITH (NOLOCK) ON iuo.Retail_Unit_ID = rounit.Unit_ID
	WHERE pbd.POSPrice IS NOT NULL
		AND i.Deleted_Item = 0
		AND i.Remove_Item = 0
		AND ii.Deleted_Identifier = 0
		AND ii.Remove_Identifier = 0
		AND CAST(p.POSPrice AS DECIMAL(9, 2)) <> CAST(pbd.POSPrice AS DECIMAL(9, 2))
		AND pct.PriceChgTypeDesc NOT IN (
			'DIS'
			,'CLR'
			,'CMP'
			,'FRC'
			,'GSP'
			,'EDV'
			,'NEW'
			)
		AND (SUBSTRING(CAST(CAST(pbd.POSPrice as decimal(9,4)) as varchar), CHARINDEX('.',CAST(pbd.POSPrice AS VARCHAR),1) + 3, 2) > 0
		OR  ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) NOT IN ('LB', 'EA'))

	UNION
	
	--Future Long Running TPRs that are becoming REGs
	SELECT 
		vsc.inforItemId AS 'ITEM_ID'
		,s.BusinessUnit_ID AS 'STORE_NUMBER'
		,ii.Identifier AS 'IDENTIFIER'
		,s.Store_Name AS 'STORE'
		,pbd.POSSale_Price AS 'PRICE'
		,pbd.Sale_Multiple AS 'MULTIPLE'
		,ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) AS 'SELLING_UOM'
		,'REG' AS 'PRICE_TYPE_CODE'
		,CASE 
			WHEN pct.PriceChgTypeDesc = 'NEW'
				THEN 'REG'
			WHEN pct.PriceChgTypeDesc = 'FRC'
				THEN 'CMP'
			ELSE pct.PriceChgTypeDesc
		 END AS 'PRICE_ATTRIBUTE_CODE'
		,CASE 
			WHEN pct.PriceChgTypeDesc = 'NEW'
				THEN DATEADD(s, -1, DATEADD(day, DATEDIFF(DAY, 0, pbd.Sale_End_Date), 1))
			ELSE NULL
		 END AS 'NEW_TAG_EXPIRATION_DATE'
		,pbd.StartDate AS 'START_DATE'
		,NULL AS 'END_DATE'
		,@today AS 'CREATED_DATE'
		,srm.Region_Code
		,c.CurrencyCode
	FROM infor.tmpGpmFuturePbd l
	JOIN Item i ON l.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON l.Item_Key = ii.Item_Key
	JOIN PriceBatchDetail pbd ON l.PriceBatchDetailId = pbd.PriceBatchDetailID
	JOIN PriceChgType pct ON pct.PriceChgTypeID = pbd.PriceChgTypeID
	JOIN Price p ON p.Item_Key = l.Item_Key AND p.Store_No = l.Store_No
	JOIN ItemUnit runit ON i.Retail_Unit_ID = runit.Unit_ID
	JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
	JOIN Store s ON pbd.Store_No = s.Store_No
	JOIN StoreJurisdiction sj ON s.StoreJurisdictionID = sj.StoreJurisdictionID
	JOIN Currency c ON sj.CurrencyID = c.CurrencyID
	JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = l.Item_Key
		AND iuo.Store_No = l.Store_No
	LEFT JOIN ItemUnit rounit ON iuo.Retail_Unit_ID = rounit.Unit_ID
	WHERE pbd.POSSale_Price IS NOT NULL
		AND CAST(p.POSSale_Price AS DECIMAL(9, 2)) <> CAST(pbd.POSSale_Price AS DECIMAL(9, 2))
		AND pct.PriceChgTypeDesc IN (
			'DIS'
			,'CLR'
			,'CMP'
			,'FRC'
			,'GSP'
			,'EDV'
			,'NEW'
			)
		AND (SUBSTRING(CAST(CAST(pbd.POSSale_Price as decimal(9,4)) as varchar), CHARINDEX('.',CAST(pbd.POSSale_Price AS VARCHAR),1) + 3, 2) > 0
		OR  ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) NOT IN ('LB', 'EA'))

	UNION
	
	--Future TPRs (excluding Sale-Off)
	SELECT 
		vsc.inforItemId AS 'ITEM_ID'
		,s.BusinessUnit_ID AS 'STORE_NUMBER'
		,ii.Identifier AS 'IDENTIFIER'
		,s.Store_Name AS 'STORE'
		,pbd.POSSale_Price AS 'PRICE'
		,pbd.Sale_Multiple AS 'MULTIPLE'
		,ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) AS 'SELLING_UOM'
		,pct.PriceChgTypeDesc AS 'PRICE_TYPE_CODE'
		,CASE 
			WHEN pct.PriceChgTypeDesc = 'SAL'
				THEN 'SSAL'
			WHEN pct.PriceChgTypeDesc = 'FRZ'
				THEN 'MSAL'
			ELSE pct.PriceChgTypeDesc
		 END AS 'PRICE_ATTRIBUTE_CODE'
		,NULL AS 'NEW_TAG_EXPIRATION_DATE'
		,pbd.StartDate AS 'START_DATE'
		,DATEADD(s, -1, DATEADD(day, DATEDIFF(DAY, 0, pbd.Sale_End_Date), 1)) AS 'END_DATE'
		,@today AS 'CREATED_DATE'
		,srm.Region_Code
		,c.CurrencyCode
	FROM infor.tmpGpmFuturePbd l
	JOIN Item i ON l.Item_Key = i.Item_Key
	JOIN ItemIdentifier ii ON l.Item_Key = ii.Item_Key
	JOIN PriceBatchDetail pbd ON l.PriceBatchDetailId = pbd.PriceBatchDetailID
	JOIN PriceChgType pct ON pct.PriceChgTypeID = pbd.PriceChgTypeID
	JOIN ItemUnit runit ON i.Retail_Unit_ID = runit.Unit_ID
	JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
	JOIN Store s ON pbd.Store_No = s.Store_No
	JOIN StoreJurisdiction sj ON s.StoreJurisdictionID = sj.StoreJurisdictionID
	JOIN Currency c ON sj.CurrencyID = c.CurrencyID
	JOIN StoreRegionMapping srm ON s.Store_No = srm.Store_No
	LEFT JOIN ItemUomOverride iuo ON iuo.Item_Key = l.Item_Key
		AND iuo.Store_No = l.Store_No
	LEFT JOIN ItemUnit rounit ON iuo.Retail_Unit_ID = rounit.Unit_ID
	WHERE pbd.POSSale_Price IS NOT NULL
		AND pct.PriceChgTypeDesc NOT IN (
			'REG'
			,'DIS'
			,'CLR'
			,'CMP'
			,'FRC'
			,'GSP'
			,'EDV'
			,'NEW'
			)
		AND pbd.StartDate = (
			SELECT MIN(l2.StartDate)
			FROM infor.tmpGpmFuturePbd l2
			WHERE l2.PriceChgTypeDesc NOT IN (
					'REG'
					,'DIS'
					,'CLR'
					,'CMP'
					,'FRC'
					,'GSP'
					,'EDV'
					,'NEW'
					)
				AND l2.Item_Key = l.Item_Key
				AND l2.Store_No = l.Store_No
			)
		AND (SUBSTRING(CAST(CAST(pbd.POSSale_Price as decimal(9,4)) as varchar), CHARINDEX('.',CAST(pbd.POSSale_Price AS VARCHAR),1) + 3, 2) > 0
		OR  ISNULL(rounit.Unit_Abbreviation, runit.Unit_Abbreviation) NOT IN ('LB', 'EA'))

END
GO

GRANT EXECUTE on [infor].[PriceExtractDataConversionReport] to [MammothRole]
GO