CREATE PROCEDURE dbo.OutOfStockReport
@Store_No int,
@SubTeam_No int, 
@StartDate varchar(12), 
@EndDate varchar(12)
AS

BEGIN
    SET NOCOUNT ON

SELECT OrderItem.OrderHeader_ID AS PO_Number, ItemIdentifier.Identifier, OrderItem.Item_Key, Item_Description, OrderItem.QuantityOrdered, ItemUnit.Unit_Name, OrderHeader.OrderDate, OrderHeader.Expected_Date, Vendor.CompanyName, ReceiveLocation.CompanyName AS Receiving_Store
FROM OrderHeader INNER JOIN OrderItem ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
	          INNER JOIN Item ON Item.Item_Key = OrderItem.Item_Key
		   INNER JOIN ItemIdentifier ON ItemIdentifier.Item_Key = Item.Item_Key AND Default_Identifier = 1
                    INNER JOIN Vendor ReceiveLocation ON ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID 
                     INNER JOIN Vendor ON Vendor.Vendor_ID = OrderHeader.Vendor_id
		      INNER JOIN ItemUnit ON OrderItem.QuantityUnit = ItemUnit.Unit_ID
WHERE OrderHeader.OrderDate >= @StartDate AND OrderHeader.OrderDate <= @EndDate AND 
      OrderItem.QuantityReceived = 0 AND OrderHeader.Expected_Date IS NOT NULL AND
      ReceiveLocation.Vendor_ID = @Store_No AND 
      ISNULL(@SubTeam_No, Item.SubTeam_No) = Item.SubTeam_No
ORDER BY PO_Number
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfStockReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfStockReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfStockReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfStockReport] TO [IRMAReportsRole]
    AS [dbo];

