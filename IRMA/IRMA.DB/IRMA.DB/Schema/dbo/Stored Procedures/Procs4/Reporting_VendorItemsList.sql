CREATE PROCEDURE [dbo].[Reporting_VendorItemsList] 
	@Vendor_ID int,
	@Zone_ID int,
	@IsRegional bit,
	@Store_No_List varchar(8000),
	@Team_No int,
	@SubTeam_No int,
	@Category_ID int,
	@Brand_ID int,
	@Identifier varchar(14)
AS 
--**************************************************************************
-- Procedure: Reporting_VendorItemsList
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with 
--                   StoreItemVendor.DiscontinueItem
--**************************************************************************
BEGIN

	SET NOCOUNT ON

	DECLARE @VendorName varchar(50),
			@CurrDate smalldatetime,
			@Identifier2 varchar(16)

	DECLARE @tblStoreList table(Store_No int)

	DECLARE @AvailStoreItemVendor table(
                                    Store_No int,
									Store_Name varchar(50), 
									Zone_Name varchar(100), 
									Item_Key int, 
									Vendor_ID int, 
									StoreItemVendorID int,
									NetCost smallmoney, 									
									PriceExcVAT smallmoney, 
									PriceIncVAT smallmoney, 
									PromotionStatus varchar(7),
                                    MultipleModifier int,
                                    NetCaseCost smallmoney
									)

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
				--RAISERROR('No items found matching the UPC ''%s''!', 16, 1, @Identifier)
				SELECT NULL
			END

	----------------------------------------------
	-- Verify the VendorID exists (if specified)
	----------------------------------------------
	IF NOT EXISTS (SELECT * FROM Vendor WHERE Vendor_ID = ISNULL(@Vendor_ID, Vendor_ID))
		BEGIN
			--RAISERROR('No Vendor found with Vendor_ID = %d!', 16, 1, @Vendor_ID)
				SELECT NULL
		END

	----------------------------------------------
	-- Get the stores to use in the report
	----------------------------------------------
	IF @IsRegional = 1
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store
		END
	ELSE IF @Zone_ID IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Store_No 
				FROM Store
				WHERE Zone_ID = @Zone_ID
		END
	ELSE IF @Store_No_List IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT Key_Value
				FROM fn_Parse_List(@Store_No_List, ',') FN
				WHERE EXISTS (SELECT * FROM Store WHERE Store_No = FN.Key_Value)
		END

	----------------------------------------------
	-- Verify stores were selected to use
	----------------------------------------------
	IF NOT EXISTS (SELECT * FROM @tblStoreList)
		BEGIN
			--RAISERROR('No stores have been selected for the report!', 16, 1)
				SELECT NULL
		END

	----------------------------------------------
	-- Get the current date
	----------------------------------------------
	SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar, GETDATE(), 101))

	----------------------------------------------
	-- Create a temp table based on fn_VendorCostItemsStores
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblVendorCostItemsStores') IS NOT NULL
		DROP TABLE #tblVendorCostItemsStores

	CREATE TABLE #tblVendorCostItemsStores
		(
		Item_Key int NOT NULL,
		Store_No int NOT NULL,
		Vendor_ID int NOT NULL,
		UnitCost smallmoney NULL,
		UnitFreight smallmoney NULL,
		Package_Desc1 decimal(9,4) NULL
		)

	CREATE CLUSTERED INDEX CLIX_VendorCostItemsStores_ItemKey_StoreNo_VendorID ON #tblVendorCostItemsStores
		(Item_Key, Store_No, Vendor_ID)

	----------------------------------------------
	-- Fill the temp table (based on fn_VendorCostItemsStores)
	----------------------------------------------
	INSERT INTO #tblVendorCostItemsStores (Item_Key, Store_No, Vendor_ID, UnitCost, UnitFreight, Package_Desc1) 
	SELECT MVCH.Item_Key, 
		MVCH.Store_No, 
		MVCH.Vendor_ID, 
		VCH.UnitCost,
		VCH.UnitFreight, 
		VCH.Package_Desc1
    FROM VendorCostHistory VCH (NOLOCK)
        INNER JOIN
           (SELECT Store_No, 
				Vendor_ID, 
				Item_Key, 
				ISNULL((SELECT TOP 1 VendorCostHistoryID
                       FROM VendorCostHistory (NOLOCK)
						INNER JOIN StoreItemVendor (NOLOCK)
                           ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
                       WHERE Promotional = 1
                           AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = ISNULL(@Vendor_ID, Vendor_ID)
                           AND ((@CurrDate >= StartDate) AND (@CurrDate <= EndDate))
                           AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
                       ORDER BY VendorCostHistoryID DESC),
                      (SELECT TOP 1 VendorCostHistoryID
                       FROM VendorCostHistory (NOLOCK)
						INNER JOIN StoreItemVendor (NOLOCK)
                           ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
                       WHERE Promotional = 0
                           AND Store_No = SIV.Store_No AND Item_Key = SIV.Item_Key AND Vendor_ID = ISNULL(@Vendor_ID, Vendor_ID)
                           AND ((@CurrDate >= StartDate) AND (@CurrDate <= EndDate))
                           AND @CurrDate < ISNULL(DeleteDate, DATEADD(day, 1, @CurrDate))
                       ORDER BY VendorCostHistoryID DESC)) As MaxVCHID
            FROM StoreItemVendor SIV (NOLOCK)
            WHERE SIV.Vendor_ID = ISNULL(@Vendor_ID, SIV.Vendor_ID)
				AND SIV.Store_No IN (SELECT Store_No FROM @tblStoreList)
            GROUP BY Store_No, Vendor_ID, Item_Key) MVCH ON MVCH.MaxVCHID = VCH.VendorCostHistoryID
	ORDER BY Item_Key, Store_No, Vendor_ID

	----------------------------------------------
	-- Create a temp table
	----------------------------------------------
	INSERT INTO @AvailStoreItemVendor (Store_no,Store_Name, Zone_Name, Item_Key, Vendor_ID, StoreItemVendorID, NetCost, PriceExcVAT, PriceIncVAT, PromotionStatus,MultipleModifier,NetCaseCost)
	SELECT 
        P.Store_No,
        S.Store_Name, 
		Z.Zone_Name, 
		SIV.Item_Key, 
		SIV.Vendor_ID,	
		SIV.StoreItemVendorID,	
		--NET COST = REG COST - NET DISCOUNT + Freight
		ISNULL(VC.UnitCost, 0) - ISNULL(dbo.fn_ItemNetDiscount(S.Store_No, SIV.Item_Key, SIV.Vendor_ID, ISNULL(VC.UnitCost, 0), @CurrDate),0) + ISNULL(VC.UnitFreight, 0) AS NetCost,
		CASE WHEN PCT.On_Sale = 1 
			THEN dbo.fn_Price(P.PriceChgTypeId, P.Multiple, P.Price, P.PricingMethod_ID, P.Sale_Multiple, P.Sale_Price) 
			ELSE P.Price
		END AS PriceExcVAT,
		CASE WHEN PCT.On_Sale = 1 
			THEN dbo.fn_Price(P.PriceChgTypeId, P.Multiple, P.POSPrice, P.PricingMethod_ID, P.Sale_Multiple, P.POSSale_Price) 
			ELSE P.POSPrice
		END AS PriceIncVAT,
		PCT.PriceChgTypeDesc AS PromotionStatus,
        P.Multiple as MultipleModifier,
        ISNULL(dbo.fn_GetCurrentNetCost(SIV.Item_key,s.Store_no),0) as NetCaseCost
	FROM Store S (NOLOCK)
		INNER JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Store_No = S.Store_No
		INNER JOIN Price P (NOLOCK) ON SIV.Store_No = P.Store_No AND SIV.Item_Key = P.Item_Key
		INNER JOIN Zone Z (NOLOCK) ON S.Zone_ID = Z.Zone_ID   
		INNER JOIN PriceChgType PCT (NOLOCK) ON PCT.PriceChgTypeID = P.PriceChgTypeID                    
		LEFT JOIN #tblVendorCostItemsStores VC
			ON VC.Vendor_ID = SIV.Vendor_ID AND VC.Store_No = SIV.Store_No AND VC.Item_Key = SIV.Item_Key
	WHERE SIV.Vendor_ID = ISNULL(@Vendor_ID, SIV.Vendor_ID)
		AND S.Store_No IN (SELECT Store_No FROM @tblStoreList)
		AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > @CurrDate)
		AND SIV.DiscontinueItem = 0
	ORDER BY SIV.Item_Key, S.Store_Name, Z.Zone_Name

	----------------------------------------------
	-- Drop the temp table
	----------------------------------------------
	IF OBJECT_ID('tempdb..#tblVendorCostItemsStores') IS NOT NULL
		DROP TABLE #tblVendorCostItemsStores

	----------------------------------------------
	-- Return the data
	----------------------------------------------
	SELECT DISTINCT 
		ST.SubTeam_Name,
		IC.Category_Name,
		II.Identifier,
		IV.Item_ID,
		ASIV.Vendor_ID,
		V.CompanyName AS Vendor_Name,
		IB.Brand_Name,
        ISNULL(dbo.fn_ItemOverride_ItemDescription(ASIV.Item_key,ASIV.Store_No),NULL) as Item_Description,
        ISNULL(dbo.fn_ItemOverride_Package_Desc1(ASIV.Item_key,ASIV.Store_No),NULL) as Package_Desc1,
        ISNULL(dbo.fn_ItemOverride_Package_Desc2(ASIV.Item_key,ASIV.Store_No),NULL) as Package_Desc2,
        ISNULL(dbo.fn_ItemOverride_Package_Unit_Description(ASIV.Item_key,ASIV.Store_No),NULL) as Package_Unit,
		I.TaxClassID AS VAT_Flag,
		ASIV.Store_Name,
		ASIV.NetCost AS Cost,
        ASIV.MultipleModifier as MultipleModifier,
        ASIV.NetCaseCost as NetCaseCost,
		ASIV.PriceIncVAT AS Price,
		(CASE WHEN ISNULL(ASIV.PriceExcVAT, 0) <> 0 
			THEN (ASIV.PriceExcVAT - (ASIV.NetCost / VCH.Package_Desc1))/ASIV.PriceExcVAT  --DIVIDE NET COST BY PACK DESC 1 TO GET ITEM COST
		END) AS Margin,
		ASIV.PromotionStatus,
		(CASE WHEN @IsRegional = 1 THEN ''
			ELSE (CASE WHEN @Store_No_List IS NULL THEN ASIV.Zone_Name ELSE '' END)
		END) AS Zone_Name
	FROM Item I (NOLOCK) 
		INNER JOIN @AvailStoreItemVendor ASIV ON I.Item_Key = ASIV.Item_Key
		INNER JOIN Vendor V (NOLOCK) ON V.Vendor_ID = ASIV.Vendor_ID
		INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = I.SubTeam_No
		INNER JOIN ItemIdentifier II (NOLOCK) ON II.Item_Key = I.Item_Key 
			AND II.Default_Identifier = (CASE WHEN @Identifier IS NULL THEN 1 ELSE Default_Identifier END)
			AND II.Deleted_Identifier = 0
		INNER JOIN ItemVendor IV (NOLOCK) ON IV.Item_Key = I.Item_Key
		INNER JOIN VendorCostHistory VCH (nolock)ON VCH.StoreItemVendorID = ASIV.StoreItemVendorID
		LEFT JOIN ItemUnit IU (NOLOCK) ON IU.Unit_ID = I.Package_Unit_ID
		LEFT JOIN ItemBrand IB (NOLOCK) ON I.Brand_ID = IB.Brand_ID
		LEFT JOIN ItemCategory IC (NOLOCK) ON I.Category_ID = IC.Category_ID
	WHERE I.Deleted_Item = 0 
		AND ST.Team_No = ISNULL(@Team_No, ST.Team_No) 
		AND ST.SubTeam_No = ISNULL(@SubTeam_No, ST.SubTeam_No) 
		AND ISNULL(I.Category_ID, 0) = ISNULL(@Category_ID, ISNULL(I.Category_ID, 0)) 
		AND II.Identifier LIKE ISNULL(@Identifier2, II.Identifier) 
		AND ISNULL(I.Brand_ID,0) = ISNULL(@Brand_ID,ISNULL(I.Brand_ID, 0)) 
		AND IV.Vendor_ID = ISNULL(@Vendor_ID, IV.Vendor_ID) 
		AND IV.DeleteDate IS NULL
	ORDER BY V.CompanyName, IC.Category_Name, Item_Description

	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_VendorItemsList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_VendorItemsList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_VendorItemsList] TO [IRMAReportsRole]
    AS [dbo];

