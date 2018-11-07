CREATE PROCEDURE dbo.LockFSCustomer
@Customer_ID int, 
@User_ID int  
AS 

UPDATE FSCustomer 
SET User_ID = @User_ID 
WHERE Customer_ID = @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockFSCustomer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockFSCustomer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockFSCustomer] TO [IRMAReportsRole]
    AS [dbo];

