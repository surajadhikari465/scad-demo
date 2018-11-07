CREATE PROCEDURE dbo.UnlockFSCustomer 
@Customer_ID int 
AS 

UPDATE FSCustomer SET 
User_ID = NULL 
WHERE Customer_ID = @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockFSCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockFSCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockFSCustomer] TO [IRMAReportsRole]
    AS [dbo];

