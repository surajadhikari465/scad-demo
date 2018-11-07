CREATE PROCEDURE dbo.GetFQSOrganizationInfoFirst
AS 
SELECT OrgNumber, DisabledFlag, Name, Address1, Address2, City, State, ZipCode, Phone, PhoneExtension, Fax, Email, Miscellaneous, Contact
FROM FQSOrganization
WHERE OrgNumber = (SELECT MIN(OrgNumber) FROM FQSOrganization)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

