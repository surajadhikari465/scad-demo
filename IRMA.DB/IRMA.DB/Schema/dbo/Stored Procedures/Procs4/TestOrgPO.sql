CREATE PROCEDURE dbo.TestOrgPO
@OrderHeader_ID int,
@Vendor_ID int
AS 

SELECT COUNT(*) 
FROM OrderHeader 
WHERE OrderHeader.Orderheader_Id =  @OrderHeader_ID
AND Vendor_ID = @Vendor_ID 
AND CloseDate IS NOT NULL
AND Return_Order = 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TestOrgPO] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TestOrgPO] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TestOrgPO] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TestOrgPO] TO [IRMAReportsRole]
    AS [dbo];

