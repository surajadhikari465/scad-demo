CREATE PROCEDURE dbo.InsertOrganization
@OrganizationName varchar(50)
AS 

INSERT INTO FSOrganization (OrganizationName)
VALUES (@OrganizationName)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrganization] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrganization] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrganization] TO [IRMAReportsRole]
    AS [dbo];

