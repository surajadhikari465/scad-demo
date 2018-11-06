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
GRANT EXECUTE
    ON OBJECT::[dbo].[ManufacturingOrders] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ManufacturingOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ManufacturingOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ManufacturingOrders] TO [IRMAReportsRole]
    AS [dbo];

