CREATE PROCEDURE dbo.RemoveItemOnHand
AS 

TRUNCATE TABLE ItemOnHand
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveItemOnHand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveItemOnHand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RemoveItemOnHand] TO [IRMAReportsRole]
    AS [dbo];

