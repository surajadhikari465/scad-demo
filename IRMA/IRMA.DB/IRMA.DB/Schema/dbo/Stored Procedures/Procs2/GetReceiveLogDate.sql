CREATE PROCEDURE dbo.GetReceiveLogDate 
@Store_No int,
@Date datetime
AS 

SELECT RecvLogDate
FROM OrderHeader
INNER JOIN Vendor ON Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID
WHERE Vendor.Store_No = @Store_No
AND (RecvLogDate >= @Date AND RecvLogDate < DATEADD(d, 1, @Date))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceiveLogDate] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceiveLogDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceiveLogDate] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceiveLogDate] TO [IRMAReportsRole]
    AS [dbo];

