CREATE PROCEDURE dbo.DeleteFSCustomers
@Organization_Id int 
AS 
DELETE 
FROM FSCustomer 
WHERE Organization_ID = @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSCustomers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSCustomers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSCustomers] TO [IRMAReportsRole]
    AS [dbo];

