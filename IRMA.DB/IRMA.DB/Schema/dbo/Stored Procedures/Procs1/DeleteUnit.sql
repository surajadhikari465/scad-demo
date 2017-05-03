CREATE PROCEDURE dbo.DeleteUnit 
@Unit_ID int 
AS 

DELETE 
FROM ItemUnit 
WHERE Unit_ID = @Unit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteUnit] TO [IRMAReportsRole]
    AS [dbo];

