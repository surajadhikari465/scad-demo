/****** Object:  StoredProcedure [dbo].[Reporting_RGIS_Extract]    Script Date: 08/28/2006 15:42:10 ******/
CREATE PROCEDURE dbo.[Reporting_RGIS_Extract] 
	@Store_No int,
	@Team_No int,
	@SubTeam_No int
AS 
--**************************************************************************
-- Procedure: Reporting_RGIS_Extract
--
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with 
--                   StoreItemVendor.DiscontinueItem
--**************************************************************************
BEGIN

	SET NOCOUNT ON

	DECLARE @CurrDate smalldatetime,
			@Vendor_ID int,				-- keep for future use
			@Identifier varchar(14)		-- keep for future use

	DECLARE @tblStoreList table(Store_No int)

	----------------------------------------------
	-- Get the stores to use in the report
	----------------------------------------------
/*	
	----------------------------------------------
	-- Leave the following code in case it's needed in the future...
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
	ELSE IF @Store_No IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK) 
				WHERE Store_No = @Store_No
		END
	ELSE
		-- default to all stores
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store (NOLOCK)
		END

*/

	INSERT INTO @tblStoreList
		SELECT Store_No 
		FROM Store (NOLOCK) 
		WHERE Store_No = @Store_No

	----------------------------------------------
	-- Verify stores were selected to use
	----------------------------------------------
	IF NOT EXISTS (SELECT * FROM @tblStoreList)
		BEGIN
			RAISERROR('No valid stores have been selected for the report!', 16, 1)
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
		ItemCost smallmoney NULL
		)

	CREATE NONCLUSTERED INDEX IX_StoreItemVendorCost_Store_No_ItemKey_VendorID ON #tblStoreItemVendorCost
		(Store_No, Item_Key, Vendor_ID)

	CREATE CLUSTERED INDEX CLIX_StoreItemVendorCost_ItemKey_StoreNo_VendorID ON #tblStoreItemVendorCost
		(Item_Key, Store_No, Vendor_ID)

	----------------------------------------------
	-- Fill the temp table
	----------------------------------------------
	INSERT INTO #tblStoreItemVendorCost (Item_Key, Store_No, Vendor_ID, UnitCost, ItemCost) 
	SELECT MVCH.Item_Key, 
		MVCH.Store_No, 
		MVCH.Vendor_ID, 
		VCH.UnitCost,
		(CASE WHEN ISNULL(VCH.Package_Desc1, 0) <> 0 
			THEN VCH.UnitCost/VCH.Package_Desc1 
		END) AS ItemCost
	FROM VendorCostHistory VCH (nolock)
		INNER JOIN
			   (SELECT SIV.Vendor_ID, 
					SIV.Store_No, 
					SIV.Item_Key, 
					ISNULL((SELECT TOP 1 VendorCostHistoryID
						   FROM VendorCostHistory (nolock)
						   INNER JOIN StoreItemVendor (nolock)
							   ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						   WHERE Promotional = 1
							   AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = SIV.Vendor_ID
							   AND ((@CurrDate >= StartDate) AND (@CurrDate <= EndDate))
							   AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
						   ORDER BY VendorCostHistoryID DESC),
						  (SELECT TOP 1 VendorCostHistoryID
						   FROM VendorCostHistory (nolock)
						   INNER JOIN StoreItemVendor (nolock)
							   ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						   WHERE Promotional = 0
							   AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = SIV.Vendor_ID
							   AND ((@CurrDate >= StartDate) AND (@CurrDate <= EndDate))
							   AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
						   ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
				FROM StoreItemVendor SIV (nolock)
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
	-- Return the data
	----------------------------------------------
    SELECT DISTINCT 
		SIVC.Store_No AS [Store],
		T.Team_No AS [Team Code],
		T.Team_Name AS [Team], 
		ST.SubTeam_Name AS [Sub-Team], 
		CASE 
			WHEN II.CheckDigit IS NULL THEN RIGHT('0000000000000' + II.Identifier, 13)
			ELSE RIGHT('0000000000000' + II.Identifier + II.CheckDigit, 13)
		END AS [Default Identifier], 
		I.Item_Description AS [Description],
		CONVERT(varchar, CONVERT(real, I.Package_Desc2)) + ' ' + IU.Unit_Name AS [Pkg Desc],
		SIVC.ItemCost AS [Cost],
		SIP.PriceIncVAT AS [Inc VAT Price],
		I.TaxClassID AS [VAT Code],
		T.Team_No AS [RGIS Code]
	FROM Item I (NOLOCK) 
		INNER JOIN #tblStoreItemVendorCost SIVC ON I.Item_Key = SIVC.Item_Key
		INNER JOIN #tblStoreItemPrice SIP ON SIVC.Store_No = SIP.Store_No AND SIVC.Item_Key = SIP.Item_Key 
		INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = I.SubTeam_No
		INNER JOIN Team T (NOLOCK) ON T.Team_No = ST.Team_No
		INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
			AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE Default_Identifier END)
			AND II.Deleted_Identifier = 0
		INNER JOIN ItemVendor IV (NOLOCK) ON IV.Item_Key = I.Item_Key AND IV.Vendor_ID = SIVC.Vendor_ID
		LEFT JOIN ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Package_Unit_ID
	WHERE I.Deleted_Item = 0 
		AND ST.Team_No = ISNULL(@Team_No, ST.Team_No) 
		AND ST.SubTeam_No = ISNULL(@SubTeam_No, ST.SubTeam_No) 
		AND IV.DeleteDate IS NULL
	ORDER BY T.Team_Name, [Default Identifier]

	----------------------------------------------
	-- Drop the temp tables
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblStoreItemVendorCost') IS NOT NULL
		DROP TABLE #tblStoreItemVendorCost

	IF OBJECT_ID('tempdb..#tblStoreItemPrice') IS NOT NULL
		DROP TABLE #tblStoreItemPrice

	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_RGIS_Extract] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_RGIS_Extract] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_RGIS_Extract] TO [IRMAReportsRole]
    AS [dbo];

