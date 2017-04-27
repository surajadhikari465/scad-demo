SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ManufacturingOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ManufacturingOrders]
GO


CREATE PROCEDURE dbo.ManufacturingOrders
@SubTeam_No int,
@FromStore int,
@Begin datetime,
@End datetime
AS

SELECT Identifier, Item.Item_Description, SUM(QuantityOrdered) AS Quantity, ItemUnit.Unit_Name, Item.Package_Desc1, Item.Package_Desc2, PKU.Unit_Name AS Package_Unit
FROM ItemUnit PKU RIGHT JOIN (
       ItemUnit INNER JOIN (
         ItemIdentifier INNER JOIN (
           Item INNER JOIN (
            OrderHeader INNER JOIN OrderItem ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
           ) ON (Item.Item_Key = OrderItem.Item_Key)
         ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
       ) ON (OrderItem.QuantityUnit = ItemUnit.Unit_ID)
     ) ON (PKU.Unit_ID = Item.Package_Unit_ID) 
WHERE Vendor_ID IN (SELECT Vendor_ID FROM Vendor WHERE Store_No = @FromStore) AND
      Transfer_SubTeam = @SubTeam_No AND
      OrderDate >= @Begin AND OrderDate <= @End
GROUP BY Identifier, Item.Item_Description, ItemUnit.Unit_Name, Item.Package_Desc1, Item.Package_Desc2, PKU.Unit_Name
ORDER BY Identifier, Item.Item_Description, ItemUnit.Unit_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


