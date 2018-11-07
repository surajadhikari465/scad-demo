CREATE PROCEDURE [dbo].[Reporting_ItemList]
    @Store_No int,
    @Team_No int,
    @SubTeam_No int,
    @VendorName varchar(50),
    @Item_Description varchar(60),
    @Identifier varchar(13),
    @Item_ID varchar(20),
    @IncludeDiscontinuedItems bit,		--include only active items or all items?
    @WFM_Item bit						--exclude items not sold at WFM stores?
AS
--**************************************************************************
-- Procedure: Reporting_ItemList
--
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
BEGIN
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
	-- Return the data
	----------------------------------------------
    SELECT DISTINCT p.Store_No,
		i.Item_Description, 
		ii.Identifier, 
		i.Package_Desc1, 
		i.Package_Desc2, 
		iu.Unit_Name, 
		st.SubTeam_Name, 
		ISNULL(dbo.fn_AvgCostHistory(i.Item_Key, p.Store_No, i.SubTeam_No, GETDATE()), 0) AS AvgCost, 
		p.Multiple, 
		p.POSPrice AS Price,
		ROUND(dbo.fn_Price(p.PriceChgTypeID, p.Multiple, p.POSPrice, p.PricingMethod_ID, p.Sale_Multiple, p.POSSale_Price) * SST.CasePriceDiscount * i.Package_Desc1, 2) AS Case_Price
    FROM Item i (nolock)
		LEFT JOIN dbo.ItemUnit iu (nolock) ON iu.Unit_ID = i.Package_Unit_ID
		INNER JOIN dbo.SubTeam st (nolock) ON st.SubTeam_No = i.SubTeam_No
		INNER JOIN dbo.ItemIdentifier ii (nolock) ON ii.Item_Key = i.Item_Key
		INNER JOIN dbo.Price p (nolock) ON i.Item_Key = p.Item_Key 
		INNER JOIN dbo.StoreSubTeam SST (nolock) ON SST.Store_No = p.Store_No AND SST.SubTeam_No = i.SubTeam_No
		LEFT JOIN
			(SELECT IV.Item_Key, IV.Vendor_ID, IV.Item_ID
			 FROM dbo.ItemVendor IV (nolock)
				INNER JOIN dbo.StoreItemVendor SIV (nolock)
					ON SIV.Item_Key = IV.Item_Key AND SIV.Vendor_ID = IV.Vendor_ID
			 WHERE SIV.Store_No = ISNULL(@Store_No, SIV.Store_No)
				AND @CurrDate < ISNULL(IV.DeleteDate, DATEADD(day, 1, @CurrDate))
				AND @CurrDate < ISNULL(SIV.DeleteDate, DATEADD(day, 1, @CurrDate))
			) As ItV --ItemVendor 
			ON i.Item_Key = ItV.Item_Key
		LEFT JOIN dbo.Vendor v (nolock) ON ItV.Vendor_ID = v.Vendor_ID
    WHERE i.Deleted_Item = 0 AND ii.Default_Identifier = 1
		AND p.Store_No = ISNULL(@Store_No, p.Store_No)
		AND v.CompanyName LIKE ISNULL(@VendorName2, v.CompanyName)
		AND i.Item_Description LIKE ISNULL(@ItemDescription2, i.Item_Description)
		AND ii.Identifier LIKE ISNULL(@Identifier2, ii.Identifier)
		AND ISNULL(ItV.Item_ID, '') = COALESCE(@Item_ID, ItV.Item_ID, '')
		AND i.SubTeam_No = ISNULL(@SubTeam_No, i.SubTeam_No)
		AND st.Team_No = ISNULL(@Team_No, st.Team_No)
		AND i.WFM_Item >= ISNULL(@WFM_Item, 0)
		AND dbo.fn_GetDiscontinueStatus(i.Item_Key, SST.Store_No, NULL) <= ISNULL(@IncludeDiscontinuedItems, 0)
    ORDER BY i.Item_Description

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_ItemList] TO [IRMAReportsRole]
    AS [dbo];

