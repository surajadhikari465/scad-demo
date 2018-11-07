CREATE PROCEDURE dbo.UnlockFSOrganization 
@Organization_ID int 
AS 

UPDATE FSOrganization SET 
User_ID = NULL 
WHERE Organization_ID = @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockFSOrganization] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockFSOrganization] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnlockFSOrganization] TO [IRMAReportsRole]
    AS [dbo];

