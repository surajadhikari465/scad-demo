CREATE PROCEDURE dbo.LockFSOrganization
@Organization_ID int, 
@User_ID int  
AS 

UPDATE FSOrganization 
SET User_ID = @User_ID 
WHERE Organization_ID = @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockFSOrganization] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockFSOrganization] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LockFSOrganization] TO [IRMAReportsRole]
    AS [dbo];

