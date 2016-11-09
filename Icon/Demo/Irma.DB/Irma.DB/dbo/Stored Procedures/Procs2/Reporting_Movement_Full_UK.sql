CREATE PROCEDURE [dbo].[Reporting_Movement_Full_UK] 
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@Team_No int,
	@SubTeam_No int,
	@Category_ID int,
	@Identifier varchar(14),
	@Item_Description varchar(60),
	@Brand_ID_List varchar(max),
	@Vendor_ID_List varchar(max),

	@VendorItemID varchar(20)
AS 

-- **************************************************************************
-- Procedure: Reporting_Movement_Full_UK()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/11/2013  BAS	Update i.Discontinue_Item reference to
--					account for schema change.
-- **************************************************************************

BEGIN
----------------------------------------------
-- Use TRY...CATCH for error handling
----------------------------------------------
--===============================================================================================
BEGIN TRY
	SET NOCOUNT ON

	-- possible parameters in the future
	DECLARE @Zone_ID int,
			@IsRegional bit,
			@Store_No_List varchar(8000)

	-- local variables
	DECLARE @CurrDate smalldatetime,
			@Identifier2 varchar(16),
			@VendorItemID2 varchar(22),
			@Item_Description2 varchar(62)

	DECLARE @tblStoreList table(Store_No int PRIMARY KEY)

	----------------------------------------------
	-- Add wildcards for searching if none exist within these parameters
	----------------------------------------------
	IF CHARINDEX('%', @Identifier) = 0
		SELECT @Identifier2 = '%' + @Identifier + '%'
	ELSE
		SELECT @Identifier2 = @Identifier
		
	IF CHARINDEX('%', @Item_Description) = 0
		SELECT @Item_Description2 = '%' + @Item_Description + '%'
	ELSE
		SELECT @Item_Description2 = @Item_Description
		
	IF CHARINDEX('%', @VendorItemID) = 0
		SELECT @VendorItemID2 = '%' + @VendorItemID + '%'
	ELSE
		SELECT @VendorItemID2 = @VendorItemID
		
	----------------------------------------------
	-- Verify the UPC (if specified)
	----------------------------------------------
	IF @Identifier2 IS NOT NULL
		IF NOT EXISTS (SELECT * FROM ItemIdentifier (NOLOCK) WHERE Identifier LIKE @Identifier2)
			BEGIN
				RAISERROR('No items found matching the UPC ''%s''!', 1, 1, @Identifier)
			END

	----------------------------------------------
	-- Verify the Item_Description exists (if specified)
	----------------------------------------------
	IF @Item_Description2 IS NOT NULL
		IF NOT EXISTS (SELECT * FROM Item (NOLOCK) WHERE Item_Description LIKE @Item_Description2)
			BEGIN
				RAISERROR('No items found matching the description ''%s''!', 1, 1, @Item_Description2)
			END

	----------------------------------------------
	-- Get the stores to use in the report
	----------------------------------------------
	IF @IsRegional = 1
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK)
				WHERE (Mega_Store = 1 OR WFM_Store = 1)
					AND StoreAbbr <> 'NOT'
		END
	ELSE IF @Zone_ID IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK)
				WHERE Zone_ID = @Zone_ID
					AND (Mega_Store = 1 OR WFM_Store = 1)
					AND StoreAbbr <> 'NOT'
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
				WHERE (Mega_Store = 1 OR WFM_Store = 1)
					AND StoreAbbr <> 'NOT'
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

	----------------------------------------------
	-- Create a temp table of item cost by store and vendor
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemVendorCost') IS NOT NULL
		DROP TABLE #tblStoreItemVendorCost

	CREATE TABLE #tblStoreItemVendorCost
		(
		Item_Key int NOT NULL,
		Store_No int NOT NULL,
		Vendor_ID int NOT NULL,
		UnitCost smallmoney NULL,
		CaseCost smallmoney NULL,
		UnitFreight smallmoney NULL,
		CaseFreight smallmoney NULL,
		CaseSize int NOT NULL
		)

	CREATE CLUSTERED INDEX CLIX_StoreItemVendorCost_ItemKey_StoreNo_VendorID ON #tblStoreItemVendorCost
		(Item_Key, Store_No, Vendor_ID)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemVendorCost (Item_Key, Store_No, Vendor_ID, UnitCost, CaseCost, UnitFreight, CaseFreight, CaseSize) 
	SELECT MVCH.Item_Key, 
		MVCH.Store_No, 
		MVCH.Vendor_ID,
		dbo.fn_GetCurrentNetCost(MVCH.Item_Key, MVCH.Store_No)/ case when isnull(VCH.Package_Desc1,1) = 0 then 1 else VCH.Package_Desc1 end as UnitCost,
		dbo.fn_GetCurrentNetCost(MVCH.Item_Key, MVCH.Store_No)  AS CaseCost,
		VCH.UnitFreight,
		CONVERT(smallmoney, VCH.UnitFreight) AS CaseFreight,
		VCH.Package_Desc1
	FROM VendorCostHistory VCH (NOLOCK)
		INNER JOIN
			   (SELECT SIV.Vendor_ID, 
					SIV.Store_No, 
					SIV.Item_Key, 
					ISNULL((SELECT TOP 1 VendorCostHistoryID
						   FROM VendorCostHistory (NOLOCK)
						   INNER JOIN StoreItemVendor (NOLOCK)
							   ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						   WHERE Promotional = 1
							   AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = SIV.Vendor_ID
							   AND @CurrDate BETWEEN StartDate AND EndDate
							   AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
						   ORDER BY VendorCostHistoryID DESC),
						  (SELECT TOP 1 VendorCostHistoryID
						   FROM VendorCostHistory (NOLOCK)
						   INNER JOIN StoreItemVendor (NOLOCK)
							   ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						   WHERE Promotional = 0
							   AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = SIV.Vendor_ID
							   AND @CurrDate BETWEEN StartDate AND EndDate
							   AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
						   ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
				FROM StoreItemVendor SIV (NOLOCK)
					INNER JOIN @tblStoreList S ON S.Store_No = SIV.Store_No
					INNER JOIN Item I (NOLOCK) ON I.Item_Key = SIV.Item_Key
					INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
						AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE Default_Identifier END)
						AND II.Deleted_Identifier = 0
					INNER JOIN ItemVendor IV (NOLOCK) ON IV.Item_Key = SIV.Item_Key AND IV.Vendor_ID = SIV.Vendor_ID
					LEFT JOIN fn_Parse_List(@Brand_ID_List, ',') Brand ON Brand.Key_Value = I.Brand_ID
					LEFT JOIN fn_Parse_List(@Vendor_ID_List, ',') Vendor ON Vendor.Key_Value = IV.Vendor_ID
				WHERE SIV.PrimaryVendor = 1
					AND I.Deleted_Item = 0 
					AND SIV.DiscontinueItem = 0 
					AND I.Category_ID = ISNULL(@Category_ID, I.Category_ID)
					AND I.SubTeam_No IN (SELECT SubTeam_No 
										FROM StoreSubTeam (NOLOCK) 
										WHERE Store_No = SIV.Store_No 
											AND Team_No = ISNULL(@Team_No, Team_No)
											AND SubTeam_No = ISNULL(@SubTeam_No, SubTeam_No))
					AND II.Identifier LIKE ISNULL(@Identifier2, II.Identifier)
					AND I.Item_Description LIKE ISNULL(@Item_Description2, I.Item_Description)
					AND (@Brand_ID_List IS NULL 
						OR (@Brand_ID_List IS NOT NULL AND Brand.Key_Value IS NOT NULL))
					AND (@Vendor_ID_List IS NULL 
						OR (@Vendor_ID_List IS NOT NULL AND Vendor.Key_Value IS NOT NULL))
					AND IV.Item_ID LIKE ISNULL(@VendorItemID2, IV.Item_ID)
					AND IV.DeleteDate IS NULL
				GROUP BY SIV.Vendor_ID, SIV.Store_No, SIV.Item_Key) MVCH
			   ON MVCH.MaxVCHID = VCH.VendorCostHistoryID
		ORDER BY Item_Key, Store_No, Vendor_ID


	----------------------------------------------
	-- Create a temp table of item prices by store
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemPrice') IS NOT NULL
		DROP TABLE #tblStoreItemPrice

	CREATE TABLE #tblStoreItemPrice
		(
		Item_Key int NOT NULL,
		Store_No int NOT NULL,
		PriceIncVAT smallmoney NULL,
		PriceExcVAT smallmoney NULL,
		TaxRate decimal(9,4) NULL
		)

	CREATE CLUSTERED INDEX CLIX_StoreItemPrice_ItemKey_StoreNo ON #tblStoreItemPrice
		(Item_Key, Store_No)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemPrice (Item_Key, Store_No, PriceIncVAT, PriceExcVAT) 
	SELECT P.Item_Key,
		P.Store_No,
		(CASE PCT.On_Sale --**
			WHEN 1 THEN 
				(CASE P.PricingMethod_ID 
					WHEN 0 THEN (CASE WHEN P.Sale_Multiple > 0 THEN P.POSSale_Price / P.Sale_Multiple ELSE P.POSSale_Price END)
					WHEN 1 THEN (CASE WHEN P.Sale_Multiple > 0 THEN P.POSSale_Price / P.Sale_Multiple ELSE P.POSSale_Price END)
					WHEN 2 THEN (CASE WHEN P.Multiple > 0 THEN P.POSPrice / P.Multiple ELSE P.POSPrice END) + P.POSSale_Price
					WHEN 4 THEN (CASE WHEN P.Multiple > 0 THEN P.POSPrice / P.Multiple ELSE P.POSPrice END) 
				END)
			ELSE (CASE WHEN P.Multiple > 0 THEN P.POSPrice / P.Multiple ELSE P.POSPrice END)
		END) AS PriceIncVAT,
		(CASE PCT.On_Sale
			WHEN 1 THEN 
				(CASE P.PricingMethod_ID 
					WHEN 0 THEN (CASE WHEN P.Sale_Multiple > 0 THEN P.Sale_Price / P.Sale_Multiple ELSE P.Sale_Price END)
					WHEN 1 THEN (CASE WHEN P.Sale_Multiple > 0 THEN P.Sale_Price / P.Sale_Multiple ELSE P.Sale_Price END)
					WHEN 2 THEN (CASE WHEN P.Multiple > 0 THEN P.Price / P.Multiple ELSE P.Price END) + P.Sale_Price
					WHEN 4 THEN (CASE WHEN P.Multiple > 0 THEN P.Price / P.Multiple ELSE P.Price END) 
				END)
			ELSE (CASE WHEN P.Multiple > 0 THEN P.Price / P.Multiple ELSE P.Price END)
		END) AS PriceExcVAT
	FROM Price P (NOLOCK)
		INNER JOIN #tblStoreItemVendorCost SIVC ON SIVC.Store_No = P.Store_No AND SIVC.Item_Key = P.Item_Key
		INNER JOIN PriceChgType PCT ON P.PriceChgTypeID = PCT.PriceChgTypeID
	ORDER BY P.Item_Key, P.Store_No

	----------------------------------------------
	-- Update the temp table with tax rates for each item/store combination
	--	- logic taken from [GetRetailStoresPOSPriceTax]
	----------------------------------------------
	UPDATE #tblStoreItemPrice
	SET TaxRate = VAT.TaxRate
	FROM #tblStoreItemPrice SIP
		INNER JOIN	
			(SELECT TaxRates.Store_No,
				TaxRates.Item_Key,
				SUM(TaxRates.TaxRate) as TaxRate
			FROM (	
					--------------------------------------------
					--Select all stores and their tax rates 
					--------------------------------------------
					SELECT S.Store_No,
						I.Item_Key,
						CASE 
							WHEN TOV.TaxFlagValue IS NOT NULL THEN 
								(CASE 
									WHEN TOV.TaxFlagValue = 1 THEN TD.TaxPercent
									ELSE 0 
								END)
							ELSE 
								(CASE 
									WHEN TF.TaxFlagValue = 1 THEN TD.TaxPercent
									ELSE 0 
								END)
 						END AS TaxRate
					FROM Store S (NOLOCK)
						CROSS JOIN Item I (NOLOCK) 
						LEFT JOIN TaxFlag TF (NOLOCK) ON TF.TaxClassID = I.TaxClassID AND TF.TaxJurisdictionID = S.TaxJurisdictionID
						LEFT JOIN TaxDefinition TD (NOLOCK) ON TD.TaxJurisdictionID = TF.TaxJurisdictionID AND TD.TaxFlagKey = TF.TaxFlagKey
						LEFT JOIN TaxOverride TOV (NOLOCK) ON TOV.Item_Key = I.Item_Key AND TOV.Store_No = S.Store_No AND TOV.TaxFlagKey = TF.TaxFlagKey
					WHERE (S.Mega_Store = 1 OR S.WFM_Store = 1) 
						AND EXISTS (SELECT * FROM #tblStoreItemVendorCost SIVC WHERE SIVC.Store_No = S.Store_No AND SIVC.Item_Key = I.Item_Key)
				UNION 
					-------------------------------------------------------------------------------
					-- Select any overrides that are not associated to current taxflags.
					-- ie. overrides that are not overriding existing tax flags.
					-------------------------------------------------------------------------------
					SELECT S.Store_No,
						I.Item_Key,
						TD.TaxPercent
					FROM Store S (NOLOCK)
						CROSS JOIN Item I (NOLOCK) 
						INNER JOIN TaxOverride TOV (NOLOCK) ON (TOV.Item_Key = I.Item_Key AND TOV.Store_No = S.Store_No)
							AND TOV.TaxFlagKey NOT IN 
								(SELECT TaxFlagKey 
								 FROM TaxDefinition (NOLOCK)
								 WHERE TaxJurisdictionID = S.TaxJurisdictionID)
						INNER JOIN TaxDefinition TD (NOLOCK) ON TOV.TaxFlagKey = TD.TaxFlagKey AND
								TD.TaxJurisdictionID = S.TaxJurisdictionID
					WHERE (S.Mega_Store = 1 OR S.WFM_Store = 1) 
						AND EXISTS (SELECT * FROM #tblStoreItemVendorCost SIVC WHERE SIVC.Store_No = S.Store_No AND SIVC.Item_Key = I.Item_Key)
				) AS TaxRates
			GROUP BY TaxRates.Store_No, TaxRates.Item_Key
			) VAT ON VAT.Store_No = SIP.Store_No AND VAT.Item_Key = SIP.Item_Key

	----------------------------------------------
	-- Create a temp table of item sales by store
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemSales') IS NOT NULL
		DROP TABLE #tblStoreItemSales

	CREATE TABLE #tblStoreItemSales
		(
		Item_Key int NOT NULL,
		Store_No int NOT NULL,
		Quantity decimal(9,2) NULL,
		Amount decimal(9,2) NULL
		)

	CREATE CLUSTERED INDEX CLIX_StoreItemSales_ItemKey_StoreNo ON #tblStoreItemSales
		(Item_Key, Store_No)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemSales (Item_Key, Store_No, Quantity, Amount) 
	SELECT SBI.Item_Key, 
		SBI.Store_No, 
        ISNULL(SUM(dbo.fn_ItemSalesQty(II.Identifier, IU.Weight_Unit, SBI.Price_Level, 
							SBI.Sales_Quantity, SBI.Return_Quantity, I.Package_Desc1, SBI.Weight)), 0) AS Quantity,
		ISNULL(SUM(SBI.Sales_Amount - SBI.Return_Amount - SBI.Markdown_Amount - SBI.Promotion_Amount), 0) AS Amount
	FROM Sales_SumByItem SBI (NOLOCK)
		INNER JOIN Item I (NOLOCK) ON SBI.Item_Key = I.Item_Key
		INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
			AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE II.Default_Identifier END)
			AND II.Deleted_Identifier = 0
		LEFT JOIN ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Retail_Unit_ID
	WHERE SBI.Date_Key BETWEEN @StartDate AND @EndDate
		AND EXISTS (SELECT * FROM #tblStoreItemVendorCost SIVC WHERE SIVC.Store_No = SBI.Store_No AND SIVC.Item_Key = SBI.Item_Key)
		AND II.Identifier LIKE ISNULL(@Identifier2, II.Identifier)
	GROUP BY SBI.Item_Key, SBI.Store_No
	ORDER BY SBI.Item_Key, SBI.Store_No

	----------------------------------------------
	-- Create a temp table of movement data for output
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblMovement') IS NOT NULL
		DROP TABLE #tblMovement

	CREATE TABLE #tblMovement
		(
		[Rank] int identity(1,1),
		[Team] varchar(100), 
		[SubTeam] varchar(100), 
		[Category] varchar(35),
		[DefaultIdentifier] bigint,
		[Description] varchar(60),
		[CaseSize] decimal(9,4),
		[ItemSize] decimal(9,4),
		[ItemUOM] varchar(25),
		[PkgDesc] varchar(50),
		[Brand] varchar(25),
		[InsertDate] smalldatetime,
		----------------------------------
		[Price_ZoneID_1] smallmoney,
		[PrimaryVendor_ZoneID_1] varchar(50),
		[VendorItemNumber_ZoneID_1] varchar(20),
		[UnitCost_ZoneID_1] smallmoney,
		[UnitFreight_ZoneID_1] smallmoney,
		[Margin_ZoneID_1] decimal(6,4),
		----------------------------------
		[Price_ZoneID_2] smallmoney,
		[PrimaryVendor_ZoneID_2] varchar(50),
		[VendorItemNumber_ZoneID_2] varchar(20),
		[UnitCost_ZoneID_2] smallmoney,
		[UnitFreight_ZoneID_2] smallmoney,
		[Margin_ZoneID_2] decimal(6,4),
		----------------------------------
		[Price_ZoneID_3] smallmoney,
		[PrimaryVendor_ZoneID_3] varchar(50),
		[VendorItemNumber_ZoneID_3] varchar(20),
		[UnitCost_ZoneID_3] smallmoney,
		[UnitFreight_ZoneID_3] smallmoney,
		[Margin_ZoneID_3] decimal(6,4),
		----------------------------------
		[Units_StoreNo_2] decimal(9,2),
		[Units_StoreNo_5] decimal(9,2),
		[Units_StoreNo_6] decimal(9,2),
		[Units_StoreNo_7] decimal(9,2),
		[Units_StoreNo_9] decimal(9,2),
		----------------------------------
		[Sales_StoreNo_2] decimal(9,2),
		[Sales_StoreNo_5] decimal(9,2),
		[Sales_StoreNo_6] decimal(9,2),
		[Sales_StoreNo_7] decimal(9,2),
		[Sales_StoreNo_9] decimal(9,2),
		----------------------------------
		[VAT_StoreNo_2] decimal(9,4),
		[VAT_StoreNo_5] decimal(9,4),
		[VAT_StoreNo_6] decimal(9,4),
		[VAT_StoreNo_7] decimal(9,4),
		[VAT_StoreNo_9] decimal(9,4),
		----------------------------------
		[TotalUnits] smallmoney,
		[TotalSales] smallmoney
		)

	----------------------------------------------
	-- Return the data
	----------------------------------------------
	INSERT INTO #tblMovement 
		([Team], [SubTeam], [Category], [DefaultIdentifier], [Description], [CaseSize], [ItemSize], [ItemUOM], [PkgDesc], [Brand], [InsertDate], 
		[Price_ZoneID_1], [PrimaryVendor_ZoneID_1], [VendorItemNumber_ZoneID_1], [UnitCost_ZoneID_1], [UnitFreight_ZoneID_1], [Margin_ZoneID_1],
		[Price_ZoneID_2], [PrimaryVendor_ZoneID_2], [VendorItemNumber_ZoneID_2], [UnitCost_ZoneID_2], [UnitFreight_ZoneID_2], [Margin_ZoneID_2],
		[Price_ZoneID_3], [PrimaryVendor_ZoneID_3], [VendorItemNumber_ZoneID_3], [UnitCost_ZoneID_3], [UnitFreight_ZoneID_3], [Margin_ZoneID_3],
		[Units_StoreNo_2], [Units_StoreNo_5], [Units_StoreNo_6], [Units_StoreNo_7], [Units_StoreNo_9], 
		[Sales_StoreNo_2], [Sales_StoreNo_5], [Sales_StoreNo_6], [Sales_StoreNo_7], [Sales_StoreNo_9], 
		[VAT_StoreNo_2], [VAT_StoreNo_5], [VAT_StoreNo_6], [VAT_StoreNo_7], [VAT_StoreNo_9], 
		[TotalUnits], [TotalSales])
	SELECT [Team], 
		[SubTeam], 
		[Category],
		[DefaultIdentifier],
		[Description],
		[CaseSize],
		[ItemSize],
		[ItemUOM],
		[PkgDesc],
		[Brand],
		[InsertDate],
		MAX(CASE WHEN [Zone_ID] = 1 THEN [ZonePrice] END),			-- Bristol
		MAX(CASE WHEN [Zone_ID] = 1 THEN [PrimaryVendor] END),
		MAX(CASE WHEN [Zone_ID] = 1 THEN [VendorItemNumber] END),
		MAX(CASE WHEN [Zone_ID] = 1 THEN [UnitCost] END),
		MAX(CASE WHEN [Zone_ID] = 1 THEN [UnitFreight] END),
		MAX(CASE WHEN [Zone_ID] = 1 THEN [Margin] END),

		MAX(CASE WHEN [Zone_ID] = 2 THEN [ZonePrice] END),			-- London
		MAX(CASE WHEN [Zone_ID] = 2 THEN [PrimaryVendor] END),
		MAX(CASE WHEN [Zone_ID] = 2 THEN [VendorItemNumber] END),
		MAX(CASE WHEN [Zone_ID] = 2 THEN [UnitCost] END),
		MAX(CASE WHEN [Zone_ID] = 2 THEN [UnitFreight] END),
		MAX(CASE WHEN [Zone_ID] = 2 THEN [Margin] END),

		MAX(CASE WHEN [Zone_ID] = 3 THEN [ZonePrice] END),			-- Kensington
		MAX(CASE WHEN [Zone_ID] = 3 THEN [PrimaryVendor] END),
		MAX(CASE WHEN [Zone_ID] = 3 THEN [VendorItemNumber] END),
		MAX(CASE WHEN [Zone_ID] = 3 THEN [UnitCost] END),
		MAX(CASE WHEN [Zone_ID] = 3 THEN [UnitFreight] END),
		MAX(CASE WHEN [Zone_ID] = 3 THEN [Margin] END),

		SUM(CASE WHEN [Store_No] = 2 THEN [TotalStoreUnits] END),		-- CMD (Camden)
		SUM(CASE WHEN [Store_No] = 5 THEN [TotalStoreUnits] END),		-- CLJ (Clapham Junction)
		SUM(CASE WHEN [Store_No] = 6 THEN [TotalStoreUnits] END),		-- SOH (Soho)
		SUM(CASE WHEN [Store_No] = 7 THEN [TotalStoreUnits] END),		-- STK (Stoke)
		SUM(CASE WHEN [Store_No] = 9 THEN [TotalStoreUnits] END),		-- HSK (Kensington)

		SUM(CASE WHEN [Store_No] = 2 THEN [TotalStoreSales] END),		-- CMD (Camden)
		SUM(CASE WHEN [Store_No] = 5 THEN [TotalStoreSales] END),		-- CLJ (Clapham Junction)
		SUM(CASE WHEN [Store_No] = 6 THEN [TotalStoreSales] END),		-- SOH (Soho)
		SUM(CASE WHEN [Store_No] = 7 THEN [TotalStoreSales] END),		-- STK (Stoke)
		SUM(CASE WHEN [Store_No] = 9 THEN [TotalStoreSales] END),		-- HSK (Kensington)

		SUM(CASE WHEN [Store_No] = 2 THEN [VAT] END),		-- CMD (Camden)
		SUM(CASE WHEN [Store_No] = 5 THEN [VAT] END),		-- CLJ (Clapham Junction)
		SUM(CASE WHEN [Store_No] = 6 THEN [VAT] END),		-- SOH (Soho)
		SUM(CASE WHEN [Store_No] = 7 THEN [VAT] END),		-- STK (Stoke)
		SUM(CASE WHEN [Store_No] = 9 THEN [VAT] END),		-- HSK (Kensington)

		SUM([TotalStoreUnits]) AS [TotalUnits],
		SUM([TotalStoreSales]) AS [TotalSales]
	FROM 
		(SELECT 
			[Team] = RTRIM(T.Team_Name), 
			[SubTeam] = RTRIM(ST.SubTeam_Name), 
			[Category] = RTRIM(IC.Category_Name),
			[DefaultIdentifier] = CONVERT(bigint, II.Identifier),
			[Description] = RTRIM(I.Item_Description),
			[CaseSize] = SIVC.CaseSize, --I.Package_Desc1,
			[ItemSize] = I.Package_Desc2,
			[ItemUOM] = RTRIM(PU.Unit_Name),
			[PkgDesc] = CONVERT(varchar, CONVERT(real, I.Package_Desc1)) + ' / ' + CONVERT(varchar, CONVERT(real, I.Package_Desc2)) + ' ' + RTRIM(PU.Unit_Name),
			[Brand] = RTRIM(IB.Brand_Name),
			[PrimaryVendor] = RTRIM(V.CompanyName),
			[VendorItemNumber] = RTRIM(IV.Item_ID),
			S.Store_No AS [Store_No],
			S.Zone_ID AS [Zone_ID],
			SIP.PriceIncVAT AS [ZonePrice],
			SIVC.UnitCost AS [UnitCost],
			SIVC.CaseCost AS [CaseCost],
			SIVC.UnitFreight AS [UnitFreight],
			SIVC.CaseFreight AS [CaseFreight],
			SIP.TaxRate AS [VAT],
			(CASE WHEN ISNULL(SIP.PriceExcVAT, 0) <> 0 
				THEN (SIP.PriceExcVAT - SIVC.UnitCost)/SIP.PriceExcVAT 
			END) AS [Margin],
			I.Insert_Date AS [InsertDate],
			SIS.Amount AS [TotalStoreSales],
			SIS.Quantity AS [TotalStoreUnits]
		FROM Item I (NOLOCK) 
			INNER JOIN #tblStoreItemVendorCost SIVC ON I.Item_Key = SIVC.Item_Key
			INNER JOIN #tblStoreItemPrice SIP ON SIVC.Item_Key = SIP.Item_Key AND SIVC.Store_No = SIP.Store_No 
			INNER JOIN Store S (NOLOCK) ON SIVC.Store_No = S.Store_No
			INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = I.SubTeam_No
			INNER JOIN StoreSubTeam SST (NOLOCK) ON SST.Store_No = S.Store_No AND SST.SubTeam_No = ST.SubTeam_No
			INNER JOIN Team T (NOLOCK) ON T.Team_No = SST.Team_No
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
				AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE Default_Identifier END)
				AND II.Deleted_Identifier = 0
			INNER JOIN ItemVendor IV (NOLOCK) ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = SIVC.Vendor_ID
			INNER JOIN Vendor V (NOLOCK) ON IV.Vendor_ID = V.Vendor_ID
			LEFT JOIN ItemUnit PU (NOLOCK) ON PU.Unit_ID = I.Package_Unit_ID
			LEFT JOIN ItemCategory IC (NOLOCK) ON I.Category_ID = IC.Category_ID
			LEFT JOIN ItemBrand IB (NOLOCK) ON IB.Brand_ID = I.Brand_ID
			LEFT JOIN #tblStoreItemSales SIS ON SIVC.Item_Key = SIS.Item_Key AND SIVC.Store_No = SIS.Store_No
		WHERE II.Identifier LIKE ISNULL(@Identifier2, II.Identifier)
			AND SST.Team_No = ISNULL(@Team_No, SST.Team_No) 
			AND ST.SubTeam_No = ISNULL(@SubTeam_No, ST.SubTeam_No) 
			AND IV.DeleteDate IS NULL) TEMP
	GROUP BY [Team], [SubTeam], [Category], [DefaultIdentifier], [Description], 
		[CaseSize], [ItemSize], [ItemUOM], [PkgDesc], [Brand], [InsertDate]
	ORDER BY [TotalSales] DESC, [TotalUnits] DESC, [DefaultIdentifier] ASC

select * from #tblMovement

	----------------------------------------------
	-- Return the crosstab data
	----------------------------------------------
	SELECT [Rank], 
		[Team], 
		[SubTeam], 
		[Category], 
		[DefaultIdentifier], 
		[Description], 
		[CaseSize],
		[ItemSize],
		[ItemUOM],
		[PkgDesc], 
		[Brand],
		[InsertDate], 
		[Price_ZoneID_1], [PrimaryVendor_ZoneID_1], [VendorItemNumber_ZoneID_1], [UnitCost_ZoneID_1], [UnitFreight_ZoneID_1], [Margin_ZoneID_1],
		[Price_ZoneID_2], [PrimaryVendor_ZoneID_2], [VendorItemNumber_ZoneID_2], [UnitCost_ZoneID_2], [UnitFreight_ZoneID_2], [Margin_ZoneID_2],
		[Price_ZoneID_3], [PrimaryVendor_ZoneID_3], [VendorItemNumber_ZoneID_3], [UnitCost_ZoneID_3], [UnitFreight_ZoneID_3], [Margin_ZoneID_3],
		[Units_StoreNo_2], [Sales_StoreNo_2], [VAT_StoreNo_2], 
		[Units_StoreNo_5], [Sales_StoreNo_5], [VAT_StoreNo_5], 
		[Units_StoreNo_6], [Sales_StoreNo_6], [VAT_StoreNo_6], 
		[Units_StoreNo_7], [Sales_StoreNo_7], [VAT_StoreNo_7], 
		[Units_StoreNo_9], [Sales_StoreNo_9], [VAT_StoreNo_9], 
		[TotalUnits], 
		[TotalSales]
	FROM #tblMovement
	ORDER BY [Team], [SubTeam], [Category], [Description]
	
END TRY
--===============================================================================================
BEGIN CATCH
	-- log the error
	EXEC [TryCatch_GetErrorInfo]

END CATCH
--===============================================================================================
	----------------------------------------------
	-- Drop the temp tables
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemVendorCost') IS NOT NULL
		DROP TABLE #tblStoreItemVendorCost

	IF OBJECT_ID('tempdb..#tblStoreItemPrice') IS NOT NULL
		DROP TABLE #tblStoreItemPrice

	IF OBJECT_ID('tempdb..#tblStoreItemSales') IS NOT NULL
		DROP TABLE #tblStoreItemSales

	IF OBJECT_ID('tempdb..#tblMovement') IS NOT NULL
		DROP TABLE #tblMovement


	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Full_UK] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Full_UK] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Full_UK] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Full_UK] TO [IRMAReportsRole]
    AS [dbo];

