SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF (SELECT COUNT(1) FROM infor.GpmConversionTprCancellations) > 0
BEGIN
	PRINT 'Truncating conversion gpm price staging table...';
	TRUNCATE TABLE infor.GpmConversionTprCancellations
END

PRINT 'Inserting GPM prices into [dbo].[GpmConversionTprCancellations] table in IRMA...'
BULK INSERT infor.GpmConversionTprCancellations
FROM ''
WITH
(
	FIRSTROW = 2,
	FIELDTERMINATOR = '|',
	ROWTERMINATOR = '0x0a'
);
GO

DECLARE @now DATETIME = GETDATE();

DECLARE @UseRegionalScaleFile BIT = (
		SELECT FlagValue
		FROM dbo.InstanceDataFlags
		WHERE FlagKey = 'UseRegionalScaleFile'
		)
DECLARE @UseStoreJurisdictions INT = (
		SELECT FlagValue
		FROM dbo.InstanceDataFlags
		WHERE FlagKey = 'UseStoreJurisdictions'
		)

INSERT INTO IConPOSPushPublish (
	PriceBatchHeaderID
	,RegionCode
	,Store_No
	,Item_Key
	,Identifier
	,ChangeType
	,InsertDate
	,BusinessUnit_ID
	,RetailSize
	,RetailPackageUOM
	,TMDiscountEligible
	,Case_Discount
	,AgeCode
	,Recall_Flag
	,Restricted_Hours
	,Sold_By_Weight
	,ScaleForcedTare
	,Quantity_Required
	,Price_Required
	,QtyProhibit
	,VisualVerify
	,RestrictSale
	,Price
	,RetailUom
	,Multiple
	,SaleMultiple
	,Sale_Price
	,Sale_Start_Date
	,Sale_End_Date
	,LinkCode_ItemIdentifier
	,POSTare
	)
SELECT 0 AS PriceBatchHeaderID
	,cl.REGION_CODE AS RegionCode
	,s.Store_No AS Store_No
	,ii.Item_Key AS Item_Key
	,v.ScanCode AS Identifier
	,'CancelAllSales' AS ChangeType
	,@now AS InsertDate
	,cl.STORE_NUMBER AS BusinessUnit_ID
	,ISNULL(IO.Package_Desc2, I.Package_Desc2) AS RetailSize
	,CASE 
		WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1
			THEN 'LB'
		ELSE 'EA'
		END AS RetailPackageUOM
	,p.Discountable AS TMDiscountEligible
	,p.IBM_Discount AS Case_Discount
	,CASE 
		WHEN P.AgeCode = 0
			THEN NULL
		ELSE P.AgeCode
		END AS AgeCode
	,ISNULL(ISNULL([IO].Recall_Flag, I.Recall_Flag), 0) AS Recall_Flag
	,P.Restricted_Hours AS Restricted_Hours
	,CASE 
		WHEN COALESCE(RU_UOM_Override.Weight_Unit, RU_Override.Weight_Unit, RU.Weight_Unit, 0) = 1
			THEN 1
		ELSE 0
		END AS Sold_By_Weight
	,COALESCE(ISO.ForceTare, ItemScale.ForceTare, 0) AS ScaleForcedTare
	,ISNULL([IO].Quantity_Required, I.Quantity_Required) AS Quantity_Required
	,ISNULL(IO.Price_Required, I.Price_Required) AS Price_Required
	,ISNULL(ISNULL([IO].QtyProhibit, I.QtyProhibit), 0) AS QtyProhibit
	,CASE 
		WHEN ISNULL(P.VisualVerify, 0) = 1
			THEN 1
		ELSE 0
		END AS VisualVerify
	,CASE 
		WHEN P.NotAuthorizedForSale = 1
			THEN 1
		ELSE 0
		END AS RestrictSale
	,p.Price AS Price
	,ISNULL(ISNULL(PU_Override.Unit_Abbreviation, PU.Unit_Abbreviation), '') AS RetailUom
	,NULL AS Sale_Price
	,p.Multiple AS Multiple
	,NULL AS SaleMultiple
	,NULL AS Sale_Start_Date
	,NULL AS Sale_End_Date
	,LII.Identifier AS LinkCode_ItemIdentifier
	,P.PosTare AS POSTare
FROM infor.GpmConversionTprCancellations cl
INNER JOIN Store s ON cl.STORE_NUMBER = s.BusinessUnit_ID
INNER JOIN ValidatedScanCode v ON cl.ITEM_ID = v.InforItemId
INNER JOIN dbo.ItemIdentifier ii ON v.ScanCode = ii.Identifier
INNER JOIN dbo.Price p ON s.Store_No = p.Store_No
	AND ii.Item_Key = p.Item_Key
INNER JOIN dbo.Item I ON I.Item_Key = P.Item_Key
LEFT JOIN dbo.ItemOverride [IO] ON [IO].Item_Key = P.Item_Key
	AND [IO].StoreJurisdictionID = s.StoreJurisdictionID
	AND @UseRegionalScaleFile = 0
	AND @UseStoreJurisdictions = 1
LEFT JOIN dbo.ItemScaleOverride ISO ON ISO.Item_Key = P.Item_Key
	AND ISO.StoreJurisdictionID = s.StoreJurisdictionID
	AND @UseRegionalScaleFile = 0
	AND @UseStoreJurisdictions = 1
LEFT JOIN dbo.ItemUomOverride IUO ON IUO.Item_Key = P.Item_Key
	AND IUO.Store_No = s.Store_No
LEFT JOIN ItemIdentifier LII ON P.LinkedItem = LII.Item_Key
	AND LII.Default_Identifier = 1
	AND LII.Deleted_Identifier = 0
LEFT JOIN dbo.ItemScale ON ItemScale.Item_Key = I.Item_Key
LEFT JOIN dbo.ItemUnit PU ON PU.Unit_ID = I.Package_Unit_ID
LEFT JOIN dbo.ItemUnit PU_Override ON PU_Override.Unit_ID = [IO].Package_Unit_ID
LEFT JOIN dbo.ItemUnit RU ON RU.Unit_ID = I.Retail_Unit_ID
LEFT JOIN dbo.ItemUnit RU_Override ON RU_Override.Unit_ID = [IO].Retail_Unit_ID
LEFT JOIN dbo.ItemUnit RU_UOM_Override ON RU_UOM_Override.Unit_ID = IUO.Retail_Unit_ID
WHERE ii.Deleted_Identifier = 0
AND ii.Remove_Identifier = 0
AND i.Deleted_Item = 0
AND i.Remove_Item = 0