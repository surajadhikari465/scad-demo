CREATE PROCEDURE dbo.GetPOInfo
@OrderHeader_ID int
AS

SELECT OrderHeader_ID, OrderType_ID, Transfer_SubTeam, ISNULL(Store.Store_No,-1) AS Store_No, CloseDate, 
       ISNULL(Vendor.Vendor_Key, Vendor.CompanyName) AS CompanyName
FROM Vendor Store WITH (NOLOCK) INNER JOIN (
       Vendor WITH (NOLOCK) INNER JOIN OrderHeader ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
     ) ON (Store.Vendor_ID = OrderHeader.ReceiveLocation_ID)
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOInfo] TO [IRMAReportsRole]
    AS [dbo];

