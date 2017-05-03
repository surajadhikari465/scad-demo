CREATE PROCEDURE dbo.InsertAccountingIn
@OrderHeader_ID  int,
@User_ID int
AS

UPDATE OrderHeader 
SET Accounting_In_DateStamp = GetDate(), Accounting_In_UserID = @User_ID 
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertAccountingIn] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertAccountingIn] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertAccountingIn] TO [IRMAReportsRole]
    AS [dbo];

