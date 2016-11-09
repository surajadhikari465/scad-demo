CREATE PROCEDURE dbo.DeleteContact 
@Contact_ID int 
AS 

DELETE 
FROM Contact 
WHERE Contact_ID = @Contact_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteContact] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteContact] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteContact] TO [IRMAReportsRole]
    AS [dbo];

