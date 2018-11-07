CREATE PROCEDURE dbo.ProductInventoryGuide
    @Store_No int,
    @Warehouse_ID int,
    @SubTeam_No int,
    @WFM_Item tinyint,
    @Include_Discontinued tinyint
AS
--**************************************************************************
-- Procedure: ProductInventoryGuide
--
-- Revision:
-- 01/11/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
-- 09/18/2013  MZ    TFS 13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--**************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

    DECLARE @MarkupPercent decimal(9,4), @Vendor_ID int, @CurrDate datetime

    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    SELECT @Vendor_ID = Vendor_ID FROM Vendor WHERE Store_No = @Warehouse_ID
 
    SELECT @MarkupPercent = Distribution_Markup 
    FROM ZoneSupply (NOLOCK) 
    WHERE SubTeam_No = @SubTeam_No AND
          FromZone_ID = (SELECT Zone_ID FROM Store WHERE Store_No = @Warehouse_ID) AND
          ToZone_ID = (SELECT Zone_ID FROM Store WHERE Store_No = @Store_No)

    SELECT @MarkupPercent = ISNULL(@MarkupPercent,0)

    SELECT Item.Item_Key, Item_Description, 
           Identifier, 
           dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key,@Warehouse_ID) AS Package_Desc1,  --Changed to Fix 6575.(Changed the code from Item.Package_Desc1)
           RetailUnit.Unit_Name, 
           Retail.Multiple AS RetailMultiple, Retail.Price AS RetailPrice, 
           (ISNULL(VC.UnitCost + VC.UnitFreight, 0) * (100 + CASE WHEN NoDistMarkup = 0 THEN @MarkupPercent ELSE 0 END)) / 100 AS Store_Cost,
           ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @Warehouse_ID, @SubTeam_No, @CurrDate), 0) AS Warehouse_Cost,
           Category_Name, ItemOrigin.Origin_Name
    FROM fn_VendorCostItems(@Vendor_ID, @Store_No, @CurrDate) VC RIGHT JOIN (
           ItemUnit RetailUnit (NOLOCK) RIGHT JOIN (
             ItemCategory (NOLOCK) RIGHT JOIN (
               ItemOrigin (NOLOCK) RIGHT JOIN (
                 Price StoreCost (NOLOCK) RIGHT JOIN (
                     ItemIdentifier (NOLOCK) INNER JOIN (
                       Price Retail (NOLOCK) INNER JOIN Item (NOLOCK) ON (Item.Item_Key = Retail.Item_Key AND Retail.Store_No = @Store_No)
                     ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
                 ) ON (Item.Item_Key = StoreCost.Item_Key AND StoreCost.Store_No = @Store_No)
               ) ON (ItemOrigin.Origin_ID = Item.Origin_ID)
             ) ON (ItemCategory.Category_ID = Item.Category_ID)	
           ) ON (RetailUnit.Unit_ID = Item.Retail_Unit_ID)
         ) ON (VC.Item_Key = Item.Item_Key)
    WHERE Item.SubTeam_No = @SubTeam_No AND 
          Item.WFM_Item >= @WFM_Item AND 
          dbo.fn_GetDiscontinueStatus(Item.Item_Key, @Store_No, @Vendor_ID) <= @Include_Discontinued AND
          Deleted_Item = 0
    ORDER BY Category_Name, Item_Description

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductInventoryGuide] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductInventoryGuide] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductInventoryGuide] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ProductInventoryGuide] TO [IRMAReportsRole]
    AS [dbo];

