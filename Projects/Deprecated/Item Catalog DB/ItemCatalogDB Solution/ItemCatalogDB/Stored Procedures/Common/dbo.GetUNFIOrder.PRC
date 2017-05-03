SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUNFIOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUNFIOrder]
GO


CREATE PROCEDURE dbo.GetUNFIOrder 
    @OrderHeader_ID int
AS 

SELECT Store.UNFI_Store,
       ISNULL(ItemVendor.Item_ID,'') AS Item_ID,
            -- If the Default Identifier is not a Scale UPC, use it
       CASE WHEN NOT ((LEN(Identifier) = 11 AND LEFT(Identifier, 1) = '2' AND RIGHT(Identifier, 5) = '00000')) 
            THEN Identifier
            -- Else get the first UPC that is not a Scale UPC
            ELSE (SELECT TOP 1 Identifier FROM ItemIdentifier II WHERE (II.Item_Key = OrderItem.Item_Key) AND (NOT ((LEN(Identifier) = 11 AND LEFT(Identifier, 1) = '2' AND RIGHT(Identifier, 5) = '00000'))) AND (LEN(Identifier) >= 10))
            END AS Identifier,
       OrderItem.QuantityOrdered,
       OrderItem.QuantityUnit
FROM ItemVendor RIGHT JOIN (
       ItemIdentifier INNER JOIN (
         Item INNER JOIN (
           OrderItem INNER JOIN (
             Store INNER JOIN (
               Vendor Inner Join OrderHeader ON (OrderHeader.ReceiveLocation_ID = Vendor.Vendor_ID)
             ) ON (Store.Store_No = Vendor.Store_No)
           ) ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
         ) ON (Item.Item_Key = OrderItem.Item_Key)
       ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND Default_Identifier = 1)
     ) ON (ItemVendor.Item_Key = OrderItem.Item_Key AND ItemVendor.Vendor_ID = OrderHeader.Vendor_ID)
WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID
ORDER BY OrderItem_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


