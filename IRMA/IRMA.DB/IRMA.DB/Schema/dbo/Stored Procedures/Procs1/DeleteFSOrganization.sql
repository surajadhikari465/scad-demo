CREATE PROCEDURE dbo.DeleteFSOrganization 
@Organization_Id int 
AS 

BEGIN

  DELETE 
  FROM FSCustomer 
  WHERE Organization_ID = @Organization_ID 

  DELETE 
  FROM FSOrganization 
  WHERE Organization_ID = @Organization_ID 

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSOrganization] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSOrganization] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteFSOrganization] TO [IRMAReportsRole]
    AS [dbo];

