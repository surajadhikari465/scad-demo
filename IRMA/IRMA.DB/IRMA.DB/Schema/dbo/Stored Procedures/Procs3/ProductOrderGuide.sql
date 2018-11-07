CREATE PROCEDURE dbo.ProductOrderGuide
    @Store_No int,
    @Warehouse_ID int,
    @SubTeam_No int,
    @WFM_Item tinyint,
    @Pre_Order tinyint 
AS

-- **************************************************************************
-- Procedure: ProductOrderGuide()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ItemCatalogLib project within IRMA Client solution
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 01/14/2013  BAS		8755	Update i.Discontinue_Item to SIV.DiscontinueItem
--								to account for schema change
-- 09/16/2013  MZ       13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- **************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

    DECLARE @Vendor_ID int
	SET @Vendor_ID = (SELECT Vendor_ID FROM Vendor WHERE Store_No = @Warehouse_ID)


	SELECT 
		Item.Item_Key,
		Item_Description,
		Identifier,
		RetailUnit.Unit_Name, 
        CAST(Retail.Price AS DECIMAL(9,2)) AS RetailPrice,
		CAST(VendorCost.Package_Desc1 AS DECIMAL(9,2)) AS VendorCasePack,
		CAST(VendorCost.UnitCost AS DECIMAL(9,2)) AS VendorCost, 
        Category_Name,
		ItemOrigin.Origin_Name,
		Pre_Order

    FROM 
		Item
		INNER JOIN StoreItemVendor SIV	(NOLOCK) ON (Item.Item_Key				= SIV.Item_Key 
													AND SIV.Store_No			= @Store_No
													AND SIV.Vendor_ID			= @Vendor_ID)
		INNER JOIN dbo.fn_VendorCostItems(@Vendor_ID, @Store_No, GETDATE()) VendorCost ON (VendorCost.Item_Key = Item.Item_Key)
		INNER JOIN Price Retail			(NOLOCK) ON (Item.Item_Key				= Retail.Item_Key
													AND Retail.Store_No			= @Store_No)
		INNER JOIN ItemIdentifier		(NOLOCK) ON (ItemIdentifier.Item_Key	= Item.Item_Key 
													AND ItemIdentifier.Default_Identifier = 1)
		INNER JOIN StoreSubTeam SST		(NOLOCK) ON (SST.Store_No				= Retail.Store_No 
													AND SST.SubTeam_No			= Item.SubTeam_No)
		LEFT JOIN ItemOrigin			(NOLOCK) ON (ItemOrigin.Origin_ID		= Item.Origin_ID)
		LEFT JOIN ItemCategory			(NOLOCK) ON (ItemCategory.Category_ID	= Item.Category_ID)
		LEFT JOIN ItemUnit RetailUnit	(NOLOCK) ON (RetailUnit.Unit_ID			= Item.Retail_Unit_ID)

    WHERE
		(Item.SubTeam_No			= @SubTeam_No)
		AND (Item.WFM_Item			>= @WFM_Item)
		AND (SIV.DiscontinueItem	= 0)
		AND (Deleted_Item			= 0)
		AND (Pre_Order				= @Pre_Order)
		AND (Not_Available			= 0)
		  
	ORDER BY
		Category_Name,
		Item_Description 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductOrderGuide] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductOrderGuide] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductOrderGuide] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductOrderGuide] TO [IRMAReportsRole]
    AS [dbo];

