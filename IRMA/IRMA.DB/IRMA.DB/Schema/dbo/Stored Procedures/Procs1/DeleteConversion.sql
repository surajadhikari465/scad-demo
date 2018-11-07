CREATE PROCEDURE dbo.DeleteConversion 
@FromUnit_ID int, 
@ToUnit_ID int 
AS 

DELETE 
FROM ItemConversion 
WHERE FromUnit_ID = @FromUnit_ID AND ToUnit_ID = @ToUnit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteConversion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteConversion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteConversion] TO [IRMAReportsRole]
    AS [dbo];

