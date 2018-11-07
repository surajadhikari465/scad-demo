CREATE PROCEDURE dbo.GetDistributionHeader 
@OrderHeader_ID int 
AS 

SELECT Vendor.CompanyName,
       OrderHeader.Expected_Date,
       OrderHeader.OrderDate
FROM OrderHeader (NOLOCK) INNER JOIN Vendor (NOLOCK) ON (OrderHeader.Vendor_ID = Vendor.Vendor_ID)
WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionHeader] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistributionHeader] TO [IRMAReportsRole]
    AS [dbo];

