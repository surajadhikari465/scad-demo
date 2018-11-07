/****** Object:  StoredProcedure [dbo].[Reporting_Movement_Promo]    Script Date: 09/11/2006 08:46:00 ******/
CREATE PROCEDURE dbo.[Reporting_Movement_Promo]
--    @StoreList varchar(8000),
--    @StoreListSeparator char(1),
    @TeamNo int,
    @StartDate smalldatetime,
    @EndDate smalldatetime--,
--    @Brand_ID int,
--    @FamilyCode varchar(13),
--    @VendorID int,
--    @CatID int
AS

BEGIN
    SET NOCOUNT ON

	----------------------------------------------
	-- Create a temp table of items on promo during specified date range (intersection > 0 days)
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemPromo') IS NOT NULL
		DROP TABLE #tblStoreItemPromo

	CREATE TABLE #tblStoreItemPromo
		(
		Item_Key int NOT NULL,
		Store_No int NOT NULL,
		Sale_Price smallmoney NULL,
		Sale_Start_Date smalldatetime NULL,		-- actual promo start date
		Sale_End_Date smalldatetime NULL,		-- actual promo end date
		Eff_Start_Date smalldatetime NULL,		-- get effective sale dates which are the intersection (i.e. overlap)
		Eff_End_Date smalldatetime NULL			-- of the actual promo dates and the specified parameter dates
		)

	CREATE NONCLUSTERED INDEX IX_StoreItemPromo_Store_No_ItemKey ON #tblStoreItemPromo
		(Store_No, Item_Key)

	CREATE CLUSTERED INDEX CLIX_StoreItemPromo_ItemKey_StoreNo ON #tblStoreItemPromo
		(Item_Key, Store_No)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemPromo (Item_Key, Store_No, Sale_Price, Sale_Start_Date, Sale_End_Date, Eff_Start_Date, Eff_End_Date)
	SELECT PH.Item_Key,
		PH.Store_No,
		PH.POSSale_Price,
		PH.Sale_Start_Date,
		PH.Sale_End_Date,
		CASE WHEN PH.Sale_Start_Date <= @StartDate THEN @StartDate ELSE PH.Sale_Start_Date END,
		CASE WHEN PH.Sale_End_Date >= @EndDate THEN @EndDate ELSE PH.Sale_End_Date END
	FROM PriceHistory PH (NOLOCK)
		INNER JOIN (SELECT Item_Key, Store_No, MAX(PriceHistoryID) AS MaxPriceHistoryID
					FROM PriceHistory (NOLOCK)
					WHERE Sale_Start_Date <= @EndDate
						AND Sale_End_Date >= @StartDate
					GROUP BY Item_Key, Store_No) MPH 
				ON MPH.Item_Key = PH.Item_Key AND MPH.Store_No = PH.Store_No AND MPH.MaxPriceHistoryID = PH.PriceHistoryID
	WHERE PH.Sale_Start_Date <= @EndDate
		AND PH.Sale_End_Date >= @StartDate
	ORDER BY PH.Item_Key, PH.Store_No

	--------------------------------
	-- return the results
	--------------------------------
--	IF OBJECT_ID('tempdb..#tblMovement') IS NOT NULL
--		DROP TABLE #tblMovement

	SELECT V.CompanyName AS [Primary Vendor],
		II.Identifier AS [Identifier],
		I.Item_Description As [Description],
		IB.Brand_Name As [Brand],
		P.Store_No,
		S.Store_Name,
		S.StoreAbbr,
		T.Team_Name,
		ST.SubTeam_Name,
		P.POSPrice AS [Reg_Price],
		SIP.Sale_Price,
		SIP.Sale_End_Date,
        ISNULL(dbo.fn_ItemSalesQty(II.Identifier, IU.Weight_Unit, SBI.Price_Level, SBI.Sales_Quantity, SBI.Return_Quantity, I.Package_Desc1, SBI.Weight), 0) AS [Quantity],
		ISNULL((SBI.Sales_Amount + SBI.Return_Amount + SBI.Markdown_Amount + SBI.Promotion_Amount), 0) AS [Sales]
--	INTO #tblMovement
	FROM Item I (NOLOCK)
		INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = I.SubTeam_No
		INNER JOIN Team T (NOLOCK) ON T.Team_No = ST.Team_No
		INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key AND II.Default_Identifier = 1
		INNER JOIN ItemVendor IV (NOLOCK) ON IV.Item_key = I.Item_Key
		INNER JOIN Price P (NOLOCK) ON P.Item_Key = I.Item_Key
		INNER JOIN Store S (NOLOCK) ON S.Store_No = P.Store_No
		INNER JOIN #tblStoreItemPromo SIP ON SIP.Item_Key = I.Item_Key AND SIP.Store_No = P.Store_No
		INNER JOIN Sales_SumByItem SBI (NOLOCK) ON SBI.Item_Key = I.Item_Key AND SBI.Store_No = P.Store_No
			AND (SBI.Date_Key BETWEEN SIP.Eff_Start_Date AND SIP.Eff_End_Date)
		INNER JOIN ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Retail_Unit_ID
		LEFT JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Item_Key = I.Item_Key AND SIV.Store_No = P.Store_No AND SIV.PrimaryVendor = 1
		LEFT JOIN Vendor V (NOLOCK) ON SIV.Vendor_ID = V.Vendor_ID
		LEFT JOIN ItemBrand IB (NOLOCK) ON IB.Brand_ID = I.Brand_ID
	WHERE T.Team_No = ISNULL(@TeamNo, T.Team_No)
	ORDER BY V.CompanyName, II.Identifier, P.Store_No

--	----------------------------------------------
--	-- Drop the temp tables
--	----------------------------------------------
--	IF OBJECT_ID('tempdb..#tblStoreItemPromo') IS NOT NULL
--		DROP TABLE #tblStoreItemPromo
--
--	----------------------------------------------
--	-- Return the crosstab data
--	----------------------------------------------
--	SELECT [Primary Vendor], 
--		[Identifier],
--		[Description],
--		[Brand],
--		[Reg_Price],
--		[Sale_Price],
--		[Sale_End_Date],
--		SUM(CASE WHEN [Store_No] = 2 THEN [Quantity] END) AS [Qty CMD],
--		SUM(CASE WHEN [Store_No] = 2 THEN [Sales] END) AS [Sales CMD],
--		SUM(CASE WHEN [Store_No] = 4 THEN [Quantity] END) AS [Qty NOT],
--		SUM(CASE WHEN [Store_No] = 4 THEN [Sales] END) AS [Sales NOT],
--		SUM(CASE WHEN [Store_No] = 5 THEN [Quantity] END) AS [Qty CLJ],
--		SUM(CASE WHEN [Store_No] = 5 THEN [Sales] END) AS [Sales CLJ],
--		SUM(CASE WHEN [Store_No] = 6 THEN [Quantity] END) AS [Qty SOH],
--		SUM(CASE WHEN [Store_No] = 6 THEN [Sales] END) AS [Sales SOH],
--		SUM(CASE WHEN [Store_No] = 7 THEN [Quantity] END) AS [Qty STK],
--		SUM(CASE WHEN [Store_No] = 7 THEN [Sales] END) AS [Sales STK],
--		SUM(CASE WHEN [Store_No] = 8 THEN [Quantity] END) AS [Qty BRL],
--		SUM(CASE WHEN [Store_No] = 8 THEN [Sales] END) AS [Sales BRL]
--	FROM #tblMovement
--	GROUP BY [Primary Vendor], [Identifier], [Description], [Brand], [Reg_Price], [Sale_Price], [Sale_End_Date]
--	ORDER BY [Primary Vendor], [Identifier]
	

--	SELECT SBI.Store_No,
--		ISNULL(M.Promo_Sales, 0) AS [Promo Sales],
--		ISNULL(SBI.Total_Sales, 0) AS [Total Sales]
--	FROM (SELECT Store_No, SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) AS [Total_Sales]
--			FROM Sales_SumByItem
--			WHERE Date_Key BETWEEN @StartDate AND @EndDate
--			GROUP BY Store_No) SBI
--		INNER JOIN (SELECT [Store_No], SUM([Sales]) AS [Promo_Sales]
--					FROM #tblMovement
--					GROUP BY [Store_No]) M ON M.Store_No = SBI.Store_No
--	ORDER BY SBI.Store_No


--	----------------------------------------------
--	-- Drop the temp tables
--	----------------------------------------------
--	IF OBJECT_ID('tempdb..#tblMovement') IS NOT NULL
--		DROP TABLE #tblMovement


    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Promo] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Promo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_Promo] TO [IRMAReportsRole]
    AS [dbo];

