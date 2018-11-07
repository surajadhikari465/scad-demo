CREATE PROCEDURE dbo.GetUsersLastOrderHeaderID
@CreatedBy int
AS 

SELECT MAX(OrderHeader_ID) AS OrderHeader_ID
FROM OrderHeader 
WHERE CreatedBy = @CreatedBy
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersLastOrderHeaderID] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersLastOrderHeaderID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersLastOrderHeaderID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUsersLastOrderHeaderID] TO [IRMAReportsRole]
    AS [dbo];

