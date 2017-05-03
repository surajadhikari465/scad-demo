CREATE PROCEDURE dbo.GetFSOrganizationLinks
@Organization_ID int 
AS 

SELECT Organization_ID AS Organization_ID 
FROM FSCustomer 
WHERE Organization_ID = @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationLinks] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationLinks] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationLinks] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationLinks] TO [IRMAReportsRole]
    AS [dbo];

