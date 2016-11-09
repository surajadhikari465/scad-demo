CREATE PROCEDURE dbo.CheckForDuplicateOrganizations 
@Organization_ID int,
@OrganizationName varchar(25) 
AS 

SELECT COUNT(*) AS OrganizationCount 
FROM FSOrganization
WHERE OrganizationName = @OrganizationName AND Organization_Id <> @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrganizations] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrganizations] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrganizations] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateOrganizations] TO [IRMAReportsRole]
    AS [dbo];

