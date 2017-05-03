CREATE PROCEDURE dbo.GetOrderHeaderLockStatus 
@OrderHeader_ID int 
AS 

SELECT User_ID 

FROM OrderHeader 

WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderLockStatus] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderLockStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderLockStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderHeaderLockStatus] TO [IRMAReportsRole]
    AS [dbo];

