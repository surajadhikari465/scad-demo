/****** Object:  StoredProcedure [dbo].[Reporting_Movement_Full]    Script Date: 09/11/2006 08:46:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_Movement_Full]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Reporting_Movement_Full]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[Reporting_Movement_Full]    Script Date: 09/11/2006 08:46:00 ******/
CREATE PROCEDURE dbo.[Reporting_Movement_Full] 
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@Vendor_ID int,
	@Category_ID int,
	@Team_No int,
	@SubTeam_No int,
	@Identifier varchar(14)
AS 
--**************************************************************************
-- Procedure: Reporting_Movement_Full
--
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with 
--                   StoreItemVendor.DiscontinueItem
--**************************************************************************
BEGIN

	SET NOCOUNT ON

	-- possible parameters in the future
	DECLARE @Zone_ID int,
			@IsRegional bit,
			@Store_No_List varchar(8000)

	-- local variables
	DECLARE @VendorName varchar(50),
			@CurrDate smalldatetime,
			@Identifier2 varchar(16)

	DECLARE @tblStoreList table(Store_No int)

	----------------------------------------------
	-- Add wildcards for searching if none exist within these parameters
	----------------------------------------------
	IF CHARINDEX('%', @Identifier) = 0
		SELECT @Identifier2 = '%' + @Identifier + '%'
	ELSE
		SELECT @Identifier2 = @Identifier
		
	----------------------------------------------
	-- Verify the UPC (if specified)
	----------------------------------------------
	IF @Identifier2 IS NOT NULL
		IF NOT EXISTS (SELECT * FROM ItemIdentifier WHERE Identifier LIKE @Identifier2)
			BEGIN
				RAISERROR('No items found matching the UPC ''%s''!', 16, 1, @Identifier)
			END

	----------------------------------------------
	-- Verify the VendorID exists (if specified)
	----------------------------------------------
	IF @Vendor_ID IS NOT NULL
		IF NOT EXISTS (SELECT * FROM Vendor WHERE Vendor_ID = @Vendor_ID)
			BEGIN
				RAISERROR('No Vendor found with Vendor_ID = %d!', 16, 1, @Vendor_ID)
			END

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
		CaseCost smallmoney NULL
		)

	CREATE NONCLUSTERED INDEX IX_StoreItemVendorCost_Store_No_ItemKey_VendorID ON #tblStoreItemVendorCost
		(Store_No, Item_Key, Vendor_ID)

	CREATE CLUSTERED INDEX CLIX_StoreItemVendorCost_ItemKey_StoreNo_VendorID ON #tblStoreItemVendorCost
		(Item_Key, Store_No, Vendor_ID)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemVendorCost (Item_Key, Store_No, Vendor_ID, UnitCost, CaseCost) 
	SELECT MVCH.Item_Key, 
		MVCH.Store_No, 
		MVCH.Vendor_ID, 
		VCH.UnitCost,
		CONVERT(smallmoney, VCH.UnitCost * VCH.Package_Desc1) AS CaseCost
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
							   AND ((@CurrDate >= StartDate) AND (@CurrDate <= EndDate))
							   AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
						   ORDER BY VendorCostHistoryID DESC),
						  (SELECT TOP 1 VendorCostHistoryID
						   FROM VendorCostHistory (NOLOCK)
						   INNER JOIN StoreItemVendor (NOLOCK)
							   ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						   WHERE Promotional = 0
							   AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = SIV.Vendor_ID
							   AND ((@CurrDate >= StartDate) AND (@CurrDate <= EndDate))
							   AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
						   ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
				FROM StoreItemVendor SIV (NOLOCK)
				WHERE SIV.Vendor_ID = ISNULL(@Vendor_ID, SIV.Vendor_ID)
					AND SIV.Store_No IN (SELECT Store_No FROM @tblStoreList)
					AND SIV.PrimaryVendor = 1
					AND SIV.DiscontinueItem = 0
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
		PriceExcVAT smallmoney NULL
		)

	CREATE NONCLUSTERED INDEX IX_StoreItemPrice_Store_No_ItemKey ON #tblStoreItemPrice
		(Store_No, Item_Key)

	CREATE CLUSTERED INDEX CLIX_StoreItemPrice_ItemKey_StoreNo ON #tblStoreItemPrice
		(Item_Key, Store_No)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemPrice (Item_Key, Store_No, PriceIncVAT, PriceExcVAT) 
	SELECT P.Item_Key,
		P.Store_No,
		(CASE dbo.fn_OnSale(P.PriceChgTypeId)
			WHEN 1 THEN 
				(CASE P.PricingMethod_ID 
					WHEN 0 THEN (CASE WHEN P.Sale_Multiple > 0 THEN P.POSSale_Price / P.Sale_Multiple ELSE P.POSSale_Price END)
					WHEN 1 THEN (CASE WHEN P.Sale_Multiple > 0 THEN P.POSSale_Price / P.Sale_Multiple ELSE P.POSSale_Price END)
					WHEN 2 THEN (CASE WHEN P.Multiple > 0 THEN P.POSPrice / P.Multiple ELSE P.POSPrice END) + P.POSSale_Price
					WHEN 4 THEN (CASE WHEN P.Multiple > 0 THEN P.POSPrice / P.Multiple ELSE P.POSPrice END) 
				END)
			ELSE (CASE WHEN P.Multiple > 0 THEN P.POSPrice / P.Multiple ELSE P.POSPrice END)
		END) AS PriceIncVAT,
		(CASE dbo.fn_OnSale(P.PriceChgTypeId)
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
	ORDER BY P.Item_Key, P.Store_No

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

	CREATE NONCLUSTERED INDEX IX_StoreItemSales_Store_No_ItemKey ON #tblStoreItemSales
		(Store_No, Item_Key)

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
		ISNULL(SUM(SBI.Sales_Amount + SBI.Return_Amount + SBI.Markdown_Amount + SBI.Promotion_Amount), 0) AS Amount
	FROM Sales_SumByItem SBI (NOLOCK)
		INNER JOIN Item I (NOLOCK) ON SBI.Item_Key = I.Item_Key
		INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
			AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE Default_Identifier END)
			AND II.Deleted_Identifier = 0
		LEFT JOIN ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Retail_Unit_ID
	WHERE SBI.Date_Key BETWEEN @StartDate AND @EndDate
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
		[PkgDesc] varchar(50),
		[PrimaryVendor] varchar(50),
		[VendorItemNumber] varchar(20),
		[UnitCost] smallmoney,
		[Margin] decimal(6,4),
		[InsertDate] smalldatetime,
		[Price_ZoneID_1] smallmoney,
		[Price_ZoneID_2] smallmoney,
		[Units_StoreNo_2] decimal(9,2),
		[Units_StoreNo_4] decimal(9,2),
		[Units_StoreNo_5] decimal(9,2),
		[Units_StoreNo_6] decimal(9,2),
		[Units_StoreNo_7] decimal(9,2),
		[Units_StoreNo_8] decimal(9,2),
		[TotalUnits] smallmoney,
		[TotalSales] smallmoney
		)

--	CREATE CLUSTERED INDEX CLIX_Movement_Team_SubTeam_Category_Description ON #tblMovement
--		([Team], [SubTeam], [Category], [Description])

	----------------------------------------------
	-- Return the data
	----------------------------------------------
	INSERT INTO #tblMovement 
		([Team], [SubTeam], [Category], [DefaultIdentifier], [Description], [PkgDesc], [PrimaryVendor], [VendorItemNumber], [UnitCost], [Margin], [InsertDate], 
		[Price_ZoneID_1], [Price_ZoneID_2], [Units_StoreNo_2], [Units_StoreNo_4], [Units_StoreNo_5], [Units_StoreNo_6], [Units_StoreNo_7], [Units_StoreNo_8], [TotalUnits], [TotalSales])
	SELECT [Team], 
		[SubTeam], 
		[Category],
		[DefaultIdentifier],
		[Description],
		[PkgDesc],
		[PrimaryVendor],
		[VendorItemNumber],
		[UnitCost],
		[Margin],
		[InsertDate],
		MAX(CASE WHEN [Zone_ID] = 1 THEN [ZonePrice] END),			-- Bristol
		MAX(CASE WHEN [Zone_ID] = 2 THEN [ZonePrice] END),			-- London
		SUM(CASE WHEN [Store_No] = 2 THEN [TotalStoreUnits] END),		-- CMD (Camden)
		SUM(CASE WHEN [Store_No] = 4 THEN [TotalStoreUnits] END),		-- NOT (Notting Hill)
		SUM(CASE WHEN [Store_No] = 5 THEN [TotalStoreUnits] END),		-- CLJ (Clapham Junction)
		SUM(CASE WHEN [Store_No] = 6 THEN [TotalStoreUnits] END),		-- SOH (Soho)
		SUM(CASE WHEN [Store_No] = 7 THEN [TotalStoreUnits] END),		-- STK (Stoke)
		SUM(CASE WHEN [Store_No] = 8 THEN [TotalStoreUnits] END),		-- BRL (Bristol)
		SUM([TotalStoreUnits]) AS [TotalUnits],
		SUM([TotalStoreSales]) AS [TotalSales]
	FROM 
		(SELECT T.Team_Name AS [Team], 
			ST.SubTeam_Name AS [SubTeam], 
			IC.Category_Name AS [Category],
			CAST(II.Identifier AS bigint) AS [DefaultIdentifier],
			I.Item_Description AS [Description],
			CONVERT(varchar, CONVERT(real, I.Package_Desc1)) + ' / ' + CONVERT(varchar, CONVERT(real, I.Package_Desc2)) + ' ' + IU.Unit_Name AS [PkgDesc],
			V.CompanyName AS [PrimaryVendor],
			IV.Item_ID AS [VendorItemNumber],
			S.Store_No AS [Store_No],
			S.Zone_ID AS [Zone_ID],
			SIP.PriceIncVAT AS [ZonePrice],
			SIVC.CaseCost AS [UnitCost],
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
			INNER JOIN Team T (NOLOCK) ON T.Team_No = ST.Team_No
			INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
				AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE Default_Identifier END)
				AND II.Deleted_Identifier = 0
			INNER JOIN ItemVendor IV (NOLOCK) ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = SIVC.Vendor_ID
			INNER JOIN Vendor V (NOLOCK) ON IV.Vendor_ID = V.Vendor_ID
			LEFT JOIN ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Retail_Unit_ID
			LEFT JOIN ItemCategory IC (NOLOCK) ON I.Category_ID = IC.Category_ID
			LEFT JOIN #tblStoreItemSales SIS ON SIVC.Item_Key = SIS.Item_Key AND SIVC.Store_No = SIS.Store_No
		WHERE I.Deleted_Item = 0 
			AND I.Category_ID = ISNULL(@Category_ID, I.Category_ID)
			AND II.Identifier LIKE ISNULL(@Identifier2, II.Identifier)
			AND ST.Team_No = ISNULL(@Team_No, ST.Team_No) 
			AND ST.SubTeam_No = ISNULL(@SubTeam_No, ST.SubTeam_No) 
			AND IV.DeleteDate IS NULL) TEMP
	GROUP BY [Team], [SubTeam], [Category], [DefaultIdentifier], [Description], [PkgDesc], [PrimaryVendor],
		[VendorItemNumber], [UnitCost], [Margin], [InsertDate]
	ORDER BY [TotalSales] DESC, [TotalUnits] DESC, [DefaultIdentifier] ASC

	----------------------------------------------
	-- Drop the temp tables
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemVendorCost') IS NOT NULL
		DROP TABLE #tblStoreItemVendorCost

	IF OBJECT_ID('tempdb..#tblStoreItemPrice') IS NOT NULL
		DROP TABLE #tblStoreItemPrice

	IF OBJECT_ID('tempdb..#tblStoreItemSales') IS NOT NULL
		DROP TABLE #tblStoreItemSales

	----------------------------------------------
	-- Return the crosstab data
	----------------------------------------------
	SELECT [Rank], 
		[Team], 
		[SubTeam], 
		[Category], 
		[DefaultIdentifier], 
		[Description], 
		[PkgDesc], 
		[PrimaryVendor], 
		[VendorItemNumber], 
		[UnitCost], 
		[Margin], 
		[InsertDate], 
		[Price_ZoneID_1], 
		[Price_ZoneID_2], 
		[Units_StoreNo_2], 
		[Units_StoreNo_4], 
		[Units_StoreNo_5], 
		[Units_StoreNo_6], 
		[Units_StoreNo_7], 
		[Units_StoreNo_8], 
		[TotalUnits], 
		[TotalSales]
	FROM #tblMovement
	ORDER BY [Team], [SubTeam], [Category], [Description]
	
	----------------------------------------------
	-- Drop the temp tables
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblMovement') IS NOT NULL
		DROP TABLE #tblMovement


	SET NOCOUNT OFF

END

GO