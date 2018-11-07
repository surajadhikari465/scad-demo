CREATE PROCEDURE dbo.UpdateOrderHeaderDesc
@OrderHeader_ID int,
@OrderHeaderDesc varchar(4000) --Sekhara changed size from 255 to 4000 to fix the bug 5920.
AS 

UPDATE OrderHeader 
SET OrderHeaderDesc = @OrderHeaderDesc
WHERE OrderHeader_ID = @OrderHeader_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderDesc] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderDesc] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderHeaderDesc] TO [IRMAReportsRole]
    AS [dbo];

