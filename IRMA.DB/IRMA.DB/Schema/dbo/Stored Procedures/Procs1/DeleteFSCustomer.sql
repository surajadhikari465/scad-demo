CREATE PROCEDURE dbo.DeleteFSCustomer 
@Customer_Id int 
AS 

DELETE 
FROM FSCustomer 
WHERE Customer_ID = @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSCustomer] TO [IRMAReportsRole]
    AS [dbo];

