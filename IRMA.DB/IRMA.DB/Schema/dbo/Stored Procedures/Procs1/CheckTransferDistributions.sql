CREATE PROCEDURE dbo.CheckTransferDistributions
@OrderHeader_ID int
AS 
SELECT Manifest_ID, Customer_ID
FROM Manifest
WHERE Customer_ID = (SELECT ReceiveLocation_ID FROM OrderHeader Where OrderHeader_ID = @OrderHeader_ID) AND
      DistLoc_ID = (SELECT Vendor_ID FROM OrderHeader Where OrderHeader_ID = @OrderHeader_ID) AND
      ShipDate IS NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckTransferDistributions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckTransferDistributions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckTransferDistributions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckTransferDistributions] TO [IRMAReportsRole]
    AS [dbo];

