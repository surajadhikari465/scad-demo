CREATE PROCEDURE [dbo].[Reporting_ItemList_UseLastCost]
    @Store_No int,
    @SubTeam_No int,
    @VendorName varchar(50),
    @Item_Description varchar(60),
    @Identifier varchar(13),
    @Item_ID varchar(20),
    @IncludeDiscontinuedItems bit,		--include only active items or all items?
    @WFM_Item bit,						--exclude items not sold at WFM stores?
    @Nat_Item bit = 0,					--grab national scale items only
    @Team_No int = NULL
AS
   -- **************************************************************************
   -- Procedure: Reporting_ItemList_UseLastCost()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from multiple RDL files and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 08/11/2009  BBB	Added calls to ItemOverride to both queries for jurisdiction
   -- 01/11/2013  TD    updated to use SIV.DiscontinueItem instead of item.discontinue_item
   -- 09/12/2013  MZ    TFS#13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
   -- **************************************************************************
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

    DECLARE @CurrDate smalldatetime,
		@VendorName2 varchar(52),		-- use local variables to allow for wildcard characters
		@ItemDescription2 varchar(62),
		@Identifier2 varchar(14)

	----------------------------------------------
	-- Get the current date
	----------------------------------------------
	SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar, GETDATE(), 101))

	----------------------------------------------
	-- Add wildcards for searching if none exist within these parameters
	----------------------------------------------
	IF CHARINDEX('%', @VendorName) = 0
		SELECT @VendorName2 = '%' + @VendorName + '%'
	ELSE
		SELECT @VendorName2 = @VendorName
	
	IF CHARINDEX('%', @Item_Description) = 0
		SELECT @ItemDescription2 = '%' + @Item_Description + '%'
	ELSE
		SELECT @ItemDescription2 = @Item_Description
	
	IF CHARINDEX('%', @Identifier) = 0
		SELECT @Identifier2 = @Identifier + '%'
	ELSE
		SELECT @Identifier2 = @Identifier
		
	----------------------------------------------
	-- Create the temp table
	----------------------------------------------
    create table #TempItemList (
        Store_No int,
        Item_Description varchar(60),
        Identifier varchar(13),
        Package_Desc1 decimal(9,4),
        Package_Desc2 decimal(9,4),
        Insert_Date datetime,
        Unit_Name varchar(25),
        SubTeam_Name varchar(100),
        AvgCost smallmoney,
        Multiple tinyint,
        Price smallmoney,
        Case_Price smallmoney
    )
    
    IF ISNULL(@Nat_Item, 0) = 0
    BEGIN
   	   ----------------------------------------------
    	-- If Nat_Item = 0; Insert data into the temp table
    	----------------------------------------------
        INSERT INTO #TempItemList
        SELECT DISTINCT Price.Store_No,
    		ISNULL(iov.Item_Description, Item.Item_Description) As Item_Description, 
    		ItemIdentifier.Identifier, 
			ISNULL(iov.Package_Desc1, Item.Package_Desc1)  As Package_Desc1, 
    		ISNULL(iov.Package_Desc2, Item.Package_Desc2)  As Package_Desc2, 
    		Item.Insert_Date,
    		ISNULL(iu2.Unit_Name, ItemUnit.Unit_Name)  As Unit_Name, 
		    SubTeam.SubTeam_Name, 
		    ISNULL(dbo.fn_GetCurrentNetCost(Item.Item_Key, Price.Store_No), 0) AS AvgCost, 
    		Price.Multiple, 
    		Price.POSPrice AS Price,
    		ROUND(dbo.fn_Price(Price.PriceChgTypeID, Price.Multiple, Price.POSPrice, Price.PricingMethod_ID, Price.Sale_Multiple, Price.POSSale_Price) * SST.CasePriceDiscount * Item.Package_Desc1, 2) AS Case_Price
        FROM Item (nolock)
    		LEFT JOIN ItemUnit (nolock)
    			ON ItemUnit.Unit_ID = Item.Package_Unit_ID
    		INNER JOIN SubTeam (nolock)
    			ON SubTeam.SubTeam_No = Item.SubTeam_No
    		INNER JOIN ItemIdentifier (nolock)
    			ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    		INNER JOIN Price (nolock) 
    			ON Item.Item_Key = Price.Item_Key 
    		INNER JOIN StoreSubTeam SST (nolock)
    			ON SST.Store_No = Price.Store_No AND SST.SubTeam_No = Item.SubTeam_No
			INNER JOIN Store s (nolock)
				ON SST.Store_No = s.Store_No
    		LEFT JOIN
    			(SELECT IV.Item_Key, IV.Vendor_ID, IV.Item_ID
    			 FROM ItemVendor IV (nolock)
    	   			INNER JOIN StoreItemVendor SIV (nolock)
    					ON SIV.Item_Key = IV.Item_Key AND SIV.Vendor_ID = IV.Vendor_ID
    			 WHERE SIV.Store_No = ISNULL(@Store_No, SIV.Store_No)
    				AND @CurrDate < ISNULL(IV.DeleteDate, DATEADD(day, 1, @CurrDate))
    				AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
                    AND SIV.DiscontinueItem <= ISNULL(@IncludeDiscontinuedItems, 0)
    	   		) As ItemVendor
    			ON Item.Item_Key = ItemVendor.Item_Key
    		LEFT JOIN Vendor (nolock) 
    			ON ItemVendor.Vendor_ID = Vendor.Vendor_ID
			LEFT JOIN dbo.ItemOverride iov (nolock)
							 on Item.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = s.StoreJurisdictionID
			LEFT JOIN dbo.ItemUnit iu2 ON iov.Package_Unit_ID = iu2.Unit_ID
        WHERE 
			Item.Deleted_Item = 0
       		AND Price.Store_No = ISNULL(@Store_No, Price.Store_No)
		    AND Vendor.CompanyName LIKE ISNULL(@VendorName2, Vendor.CompanyName)
    		AND Item.Item_Description LIKE ISNULL(@ItemDescription2, Item.Item_Description)
    		AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier2, ItemIdentifier.Identifier)
    		AND ISNULL(ItemVendor.Item_ID, '') = COALESCE(@Item_ID, ItemVendor.Item_ID, '')
    		AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
    		AND Item.WFM_Item >= ISNULL(@WFM_Item, 0)
    		--AND SIV.DiscontinueItem <= ISNULL(@IncludeDiscontinuedItems, 0)
    		AND SubTeam.Team_No = ISNULL(@Team_No, SubTeam.Team_No)
    		AND SST.Team_No = ISNULL(@Team_No, SST.Team_No)
    END
    ELSE
    BEGIN

    		
        INSERT INTO #TempItemList  -- second dataset for national_identifier=1
        SELECT DISTINCT Price.Store_No,
    		ISNULL(iov.Item_Description, Item.Item_Description) As Item_Description, 
    		ItemIdentifier.Identifier, 
			ISNULL(iov.Package_Desc1, Item.Package_Desc1)  As Package_Desc1, 
    		ISNULL(iov.Package_Desc2, Item.Package_Desc2)  As Package_Desc2, 
    		Item.Insert_Date,
    		ISNULL(iu2.Unit_Name, ItemUnit.Unit_Name)  As Unit_Name, 
		    SubTeam.SubTeam_Name, 
		    ISNULL(dbo.fn_GetCurrentCost(Item.Item_Key, Price.Store_No), 0) AS AvgCost, 
    		Price.Multiple, 
    		Price.POSPrice AS Price,
    		ROUND(dbo.fn_Price(Price.PriceChgTypeID, Price.Multiple, Price.POSPrice, Price.PricingMethod_ID, Price.Sale_Multiple, Price.POSSale_Price) * SST.CasePriceDiscount * Item.Package_Desc1, 2) AS Case_Price
        FROM Item (nolock)
            INNER JOIN ItemIdentifier (nolock)
    			ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.National_Identifier = 1            
    		LEFT OUTER JOIN Nat_Plu_Lookup (nolock)
    		    ON ItemIdentifier.Identifier = Nat_Plu_Lookup.UPCNo
            LEFT JOIN ItemUnit (nolock)
    			ON ItemUnit.Unit_ID = Item.Package_Unit_ID
    		INNER JOIN SubTeam (nolock)
    			ON SubTeam.SubTeam_No = Item.SubTeam_No
    		INNER JOIN Price (nolock) 
    			ON Item.Item_Key = Price.Item_Key 
    		INNER JOIN StoreSubTeam SST (nolock)
    			ON SST.Store_No = Price.Store_No AND SST.SubTeam_No = Item.SubTeam_No
			INNER JOIN Store s (nolock)
				ON SST.Store_No = s.Store_No
    		LEFT JOIN
    			(SELECT IV.Item_Key, IV.Vendor_ID, IV.Item_ID
    			 FROM ItemVendor IV (nolock)
    	   			INNER JOIN StoreItemVendor SIV (nolock)
    					ON SIV.Item_Key = IV.Item_Key AND SIV.Vendor_ID = IV.Vendor_ID
    			 WHERE SIV.Store_No = ISNULL(@Store_No, SIV.Store_No)
    				AND @CurrDate < ISNULL(IV.DeleteDate, DATEADD(day, 1, @CurrDate))
    				AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
                    AND SIV.DiscontinueItem <= ISNULL(@IncludeDiscontinuedItems, 0)
    	   		) As ItemVendor
    			ON Item.Item_Key = ItemVendor.Item_Key
    		LEFT JOIN Vendor (nolock) 
    			ON ItemVendor.Vendor_ID = Vendor.Vendor_ID
			LEFT JOIN dbo.ItemOverride iov (nolock)
							 on Item.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = s.StoreJurisdictionID
			LEFT JOIN dbo.ItemUnit iu2 ON iov.Package_Unit_ID = iu2.Unit_ID
        WHERE 
			Item.Deleted_Item = 0
       		AND Price.Store_No = ISNULL(@Store_No, Price.Store_No)
		    AND Vendor.CompanyName LIKE ISNULL(@VendorName2, Vendor.CompanyName)
    		AND Item.Item_Description LIKE ISNULL(@ItemDescription2, Item.Item_Description)
    		AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier2, ItemIdentifier.Identifier)
    		AND ISNULL(ItemVendor.Item_ID, '') = COALESCE(@Item_ID, ItemVendor.Item_ID, '')
    		AND Item.SubTeam_No = ISNULL(@SubTeam_No, Item.SubTeam_No)
    		AND Item.WFM_Item >= ISNULL(@WFM_Item, 0)
    		--AND SIV.DiscontinueItem <= ISNULL(@IncludeDiscontinuedItems, 0)
    		AND SubTeam.Team_No = ISNULL(@Team_No, SubTeam.Team_No)
    		AND SST.Team_No = ISNULL(@Team_No, SST.Team_No)
        ORDER BY ItemIdentifier.Identifier
    		
    END
    
    SELECT DISTINCT *
    FROM #TempItemList
    ORDER BY Item_Description
    
    
    DROP TABLE #TempItemList
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList_UseLastCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList_UseLastCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList_UseLastCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList_UseLastCost] TO [IRMAReportsRole]
    AS [dbo];

