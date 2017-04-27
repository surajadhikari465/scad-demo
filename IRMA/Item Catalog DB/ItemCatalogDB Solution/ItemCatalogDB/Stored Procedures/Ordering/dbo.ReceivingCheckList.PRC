SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReceivingCheckList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReceivingCheckList]
GO


CREATE PROCEDURE dbo.ReceivingCheckList
@OrderHeader_ID int, 
@Item_ID bit
AS 

BEGIN
    SET NOCOUNT ON
    
    DECLARE @Store_No int

    SELECT @Store_No = Store_No 
    FROM Vendor INNER JOIN OrderHeader ON (Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID) 
    WHERE OrderHeader_ID = @OrderHeader_ID

    SELECT OrderItem.OrderItem_ID, 
          (CASE WHEN ISNULL(ItemVendor.Item_ID,'') > '' THEN ItemVendor.Item_ID ELSE Identifier END) AS Identifier, 
          Item.Item_Description, 
          OrderItem.QuantityOrdered, 
          OrderItem.QuantityReceived, 
          OrderItem.QuantityDiscount, 
          OrderItem.DiscountType, 
          ItemUnit.Unit_Name,
          OrderItem.Package_Desc1,
          OrderItem.Package_Desc2,
          ISNULL(I.Unit_Name, 'Unit') AS Package_Unit,
          LineItemCost As Cost, 
          OrderItem.UnitExtCost as UnitCost 
    FROM  ItemVendor (NOLOCK) RIGHT JOIN (
             ItemUnit I (NOLOCK) RIGHT JOIN ( 
               ItemIdentifier (NOLOCK) INNER JOIN (
                 Item (NOLOCK) INNER JOIN (
                   ItemUnit (NOLOCK) INNER JOIN (
                     OrderHeader (NOLOCK) INNER JOIN OrderItem (NOLOCK) ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
                   ) ON (OrderItem.QuantityUnit = ItemUnit.Unit_ID)
                 ) ON (OrderItem.Item_Key = Item.Item_Key)
                ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
             ) ON (OrderItem.Package_Unit_ID = I.Unit_ID)
           ) ON (@Item_ID = 1 AND ItemVendor.Item_Key = OrderItem.Item_Key AND ItemVendor.Vendor_ID = OrderHeader.Vendor_ID)
    WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID 
    GROUP BY OrderItem.OrderItem_ID, ItemVendor.Item_ID, Identifier, Item_Description, OrderItem.QuantityOrdered, OrderItem.QuantityReceived, 
             OrderItem.QuantityDiscount, OrderItem.DiscountType, ItemUnit.Unit_Name, OrderItem.Package_Desc1, OrderItem.Package_Desc2, I.Unit_Name, 
             LineItemCost, OrderItem.UnitExtCost
    ORDER BY OrderItem.OrderItem_ID ASC
    
    SET NOCOUNT OFF

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



