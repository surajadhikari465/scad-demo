CREATE PROCEDURE dbo.GetStoreFromOrder 
@OrderHeader_ID int 
AS 

SELECT Vendor.Store_No 
FROM Vendor INNER JOIN OrderHeader ON Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID 
WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreFromOrder] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreFromOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreFromOrder] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreFromOrder] TO [IRMAReportsRole]
    AS [dbo];

