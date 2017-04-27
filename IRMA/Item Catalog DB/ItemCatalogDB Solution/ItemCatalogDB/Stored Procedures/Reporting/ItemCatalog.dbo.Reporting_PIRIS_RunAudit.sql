IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_PIRIS_RunAudit]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Reporting_PIRIS_RunAudit]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

----------------------------------------------
-- Returns differences between PIRIS and IRMA data
----------------------------------------------
CREATE PROCEDURE dbo.[Reporting_PIRIS_RunAudit] 
	(
	@Vendor_ID int,
	@Zone_ID int,
	@IsRegional bit,
	@Store_No_List varchar(8000),
	@Team_No int,
	@SubTeam_No int,
	@Category_ID int,
	@Brand_ID int,
	@Identifier varchar(14)
	)
AS 

BEGIN

	SET NOCOUNT ON

	DECLARE @CurrDate smalldatetime

	DECLARE @tblStoreList table(Store_No int)

	----------------------------------------------
	-- Get the stores to use in the report
	----------------------------------------------
	IF @IsRegional = 1
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK)
		END
	ELSE IF @Zone_ID IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK)
				WHERE Zone_ID = @Zone_ID
		END
	ELSE IF @Store_No_List IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Key_Value
				FROM fn_Parse_List(@Store_No_List, ',') FN
				WHERE EXISTS (SELECT * FROM Store (NOLOCK) WHERE Store_No = FN.Key_Value)
		END
	ELSE
		-- default to all stores
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK)
		END

	----------------------------------------------
	-- Verify stores were selected to use
	----------------------------------------------
	IF NOT EXISTS (SELECT * FROM @tblStoreList)
		BEGIN
			RAISERROR('No stores have been selected for the report!', 16, 1)
		END

	----------------------------------------------
	-- Get the current date
	----------------------------------------------
	SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar, GETDATE(), 101))

PRINT CONVERT(varchar, GetDate(), 120) + ' [BEFORE INSERT INTO @tblItemBarcodes (Item_Key, Barcode)...]'
	----------------------------------------------
	-- Create a temp table of relevant info for all items
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblItemBarcodes') IS NOT NULL
		DROP TABLE #tblItemBarcodes

	CREATE TABLE #tblItemBarcodes
		(
		Item_Key int NOT NULL,
		Barcode varchar(14) NOT NULL
		)

	CREATE NONCLUSTERED INDEX IX_ItemBarcodes_Barcode ON #tblItemBarcodes
		(Barcode)

	CREATE CLUSTERED INDEX CLIX_ItemBarcodes_Item_Key ON #tblItemBarcodes
		(Item_Key)

	INSERT INTO #tblItemBarcodes (Item_Key, Barcode)
		SELECT Item_Key, Identifier + ISNULL(CheckDigit, '') AS Item_Barcode
		FROM [ItemIdentifier] (NOLOCK)
		ORDER BY Item_Key
PRINT CONVERT(varchar, GetDate(), 120) + ' [AFTER INSERT INTO @tblItemBarcodes...]'

	----------------------------------------------
	-- Create a temp table of UnitCost by StoreItemVendorID
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemVendorCost') IS NOT NULL
		DROP TABLE #tblStoreItemVendorCost

	CREATE TABLE #tblStoreItemVendorCost
		(
		StoreItemVendorID int NOT NULL,
		UnitCost smallmoney NULL,
		CaseSize decimal(9,4) NULL,
		CaseCost smallmoney NULL
		)

	ALTER TABLE #tblStoreItemVendorCost ADD CONSTRAINT
		PK_StoreItemVendorCost PRIMARY KEY CLUSTERED (StoreItemVendorID)

	INSERT INTO #tblStoreItemVendorCost
	SELECT MVCH.StoreItemVendorID,
		VCH.UnitCost,
		VCH.Package_Desc1 AS CaseSize,
		CONVERT(smallmoney, VCH.UnitCost * VCH.Package_Desc1) AS CaseCost 
	FROM VendorCostHistory VCH (NOLOCK)
		INNER JOIN 
			(SELECT SIV.StoreItemVendorID,
				ISNULL((SELECT TOP 1 VendorCostHistoryID
					   FROM VendorCostHistory (NOLOCK)
					   WHERE Promotional = 1
							AND StoreItemVendorID = SIV.StoreItemVendorID
							AND StartDate <= @CurrDate 
							AND EndDate >= @CurrDate
					   ORDER BY VendorCostHistoryID DESC),
					  (SELECT TOP 1 VendorCostHistoryID
					   FROM VendorCostHistory (NOLOCK)
					   WHERE Promotional = 0
							AND StoreItemVendorID = SIV.StoreItemVendorID
							AND StartDate <= @CurrDate 
							AND EndDate >= @CurrDate
					   ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
			FROM StoreItemVendor SIV (NOLOCK)
				INNER JOIN @tblStoreList S ON S.Store_No = SIV.Store_No
			WHERE @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
		) MVCH ON MVCH.MaxVCHID = VCH.VendorCostHistoryID
	ORDER BY StoreItemVendorID
PRINT CONVERT(varchar, GetDate(), 120) + ' [AFTER INSERT INTO @tblStoreItemVendorCost...]'
	----------------------------------------------
	-- Return the data
	----------------------------------------------
	SELECT SIV.Store_No,
		I.Item_Description,
		IB.Barcode, 
		(CASE
			WHEN (SIVC.UnitCost IS NULL AND RPA.Cost IS NULL) THEN 'Both NULL'
			WHEN (SIVC.UnitCost IS NULL) THEN 'IRMA IS NULL'
			WHEN (SIVC.CaseCost IS NULL) THEN 'IRMA CaseCost ERROR'
			WHEN (RPA.Cost IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(SIVC.CaseCost, 0) = ISNULL(RPA.Cost, 0)) THEN 'Y'		-- Piris cost in RPA table is case cost
			WHEN (ISNULL(RPA.CaseSize, 0) = 0) THEN 'PIRIS CaseSize ERROR'
			ELSE 'N; ' + CAST(SIVC.UnitCost AS varchar) + ' <> ' + CAST(RPA.Cost/RPA.CaseSize AS varchar)	-- display unit costs
		END) AS Same_Cost,
		(CASE 
			WHEN (P.POSPrice IS NULL AND RPA.UnitPrice1 IS NULL) THEN 'Both NULL'
			WHEN (P.POSPrice IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.UnitPrice1 IS NULL) THEN 'PIRIS IS NULL'
			WHEN (dbo.fn_OnSale(P.PriceChgTypeId) = 0 AND P.POSPrice = RPA.UnitPrice1) THEN 'Y'
			WHEN (dbo.fn_OnSale(P.PriceChgTypeId) = 1 AND P.POSSale_Price = RPA.UnitPrice1) THEN 'Y'
			WHEN (dbo.fn_OnSale(P.PriceChgTypeId) = 0 AND P.POSPrice <> RPA.UnitPrice1) THEN 'N; ' + CAST(P.POSPrice AS varchar) + ' <> ' + CAST(RPA.UnitPrice1 AS varchar)
			WHEN (dbo.fn_OnSale(P.PriceChgTypeId) = 1 AND P.POSSale_Price <> RPA.UnitPrice1) THEN 'N; ' + CAST(P.POSSale_Price AS varchar) + ' <> ' + CAST(RPA.UnitPrice1 AS varchar)
			ELSE 'N; ???????'
		END) AS Same_POSPrice,
		(CASE
			WHEN (I.TaxClassID IS NULL AND RPA.VATCode IS NULL) THEN 'Both NULL'
			WHEN (I.TaxClassID IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.VATCode IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(I.TaxClassID, 0) = ISNULL(RPA.VATCode, 0)) THEN 'Y'
			ELSE 'N; ' + CAST(I.TaxClassID AS varchar) + ' <> ' + CAST(RPA.VATCode AS varchar)
		END) AS Same_VATCode,
		(CASE
			WHEN (V.Vendor_Key IS NULL AND RPA.Supplier IS NULL) THEN 'Both NULL'
			WHEN (V.Vendor_Key IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.Supplier IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(V.Vendor_Key, '') = ISNULL(RPA.Supplier, '')) THEN 'Y'
			ELSE 'N; ' + V.Vendor_Key + ' <> ' + RTRIM(RPA.Supplier)
		END) AS Same_Vendor,
		(CASE
			WHEN (SIV.Item_Key IS NULL AND RPA.ProductCode IS NULL) THEN 'Both NULL'
			WHEN (SIV.Item_Key IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.ProductCode IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(SIV.Item_Key, 0) = ISNULL(RPA.ProductCode, 0)) THEN 'Y'
			ELSE 'N; ' + CAST(SIV.Item_Key AS varchar) + ' <> ' + CAST(RPA.ProductCode AS varchar)
		END) AS Same_ProductCode,
		(CASE
			WHEN (I.Category_ID IS NULL AND RPA.SubSection IS NULL) THEN 'Both NULL'
			WHEN (I.Category_ID IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.SubSection IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(I.Category_ID, 0) = ISNULL(RPA.SubSection, 0)) THEN 'Y'
			ELSE 'N; ' + CAST(I.Category_ID AS varchar) + ' <> ' + CAST(RPA.SubSection AS varchar)
		END) AS Same_Category,
		(CASE
			WHEN (SIVC.CaseSize IS NULL AND RPA.CaseSize IS NULL) THEN 'Both NULL'
			WHEN (SIVC.CaseSize IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.CaseSize IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(SIVC.CaseSize, 0) = ISNULL(RPA.CaseSize, 0)) THEN 'Y'
			ELSE 'N; ' + CAST(SIVC.CaseSize AS varchar) + ' <> ' + CAST(RPA.CaseSize AS varchar)
		END) AS Same_CaseSize,
		(CASE
			WHEN (IV.Item_ID IS NULL AND RPA.Item IS NULL) THEN 'Both NULL'
			WHEN (IV.Item_ID IS NULL) THEN 'IRMA IS NULL'
			WHEN (RPA.Item IS NULL) THEN 'PIRIS IS NULL'
			WHEN (ISNULL(IV.Item_ID, 0) = ISNULL(RPA.Item, 0)) THEN 'Y'
			ELSE 'N; ' + CAST(IV.Item_ID AS varchar) + ' <> ' + CAST(RPA.Item AS varchar)
		END) AS Same_ItemID
	FROM #tblItemBarcodes IB (NOLOCK)
		LEFT JOIN [Item] I (NOLOCK) ON I.Item_Key = IB.Item_Key
		LEFT JOIN [StoreItemVendor] SIV (NOLOCK) ON SIV.Item_Key = IB.Item_Key
		LEFT JOIN [ItemVendor] IV (NOLOCK) ON IV.Item_Key = IB.Item_Key AND IV.Vendor_ID = SIV.Vendor_ID
		LEFT JOIN [Price] P (NOLOCK) ON P.Item_Key = IB.Item_Key AND P.Store_No = SIV.Store_No
		LEFT JOIN [Vendor] V (NOLOCK) ON V.Vendor_ID = SIV.Vendor_ID
		LEFT JOIN #tblStoreItemVendorCost SIVC (NOLOCK) ON SIVC.StoreItemVendorID = SIV.StoreItemVendorID
		LEFT JOIN [Reporting_PIRIS_Audit] RPA (NOLOCK) ON RPA.Store = SIV.Store_No
			AND RPA.Barcode = IB.Barcode
	WHERE SIV.Store_No IN (SELECT Store_No FROM @tblStoreList)
		AND SIV.PrimaryVendor = 1
		AND (ISNULL(SIVC.CaseCost, 0) <> ISNULL(RPA.Cost, 0)					-- Piris cost in RPA table is case cost
			OR (dbo.fn_OnSale(P.PriceChgTypeId) = 0 AND P.POSPrice <> ISNULL(RPA.UnitPrice1, 0))
			OR (dbo.fn_OnSale(P.PriceChgTypeId) = 1 AND P.POSSale_Price <> ISNULL(RPA.UnitPrice1, 0))
			OR ISNULL(I.TaxClassID, 0) <> ISNULL(RPA.VATCode, 0)
			OR ISNULL(SIV.Item_Key, 0) <> ISNULL(RPA.ProductCode, 0)
			OR ISNULL(I.Category_ID, 0) <> ISNULL(RPA.SubSection, 0)
			OR ISNULL(SIVC.CaseSize, 0) <> ISNULL(RPA.CaseSize, 0)
			OR ISNULL(IV.Item_ID, '') <> ISNULL(RPA.Item, '')
			OR V.Vendor_Key <> ISNULL(RPA.Supplier, ''))
	ORDER BY CAST(IB.Barcode AS bigint), SIV.Store_No

PRINT CONVERT(varchar, GetDate(), 120) + ' [AFTER SELECT output...]'

	IF OBJECT_ID('tempdb..#tblItemBarcodes') IS NOT NULL
		DROP TABLE #tblItemBarcodes

	IF OBJECT_ID('tempdb..#tblStoreItemVendorCost') IS NOT NULL
		DROP TABLE #tblStoreItemVendorCost

	SET NOCOUNT OFF

END

GO