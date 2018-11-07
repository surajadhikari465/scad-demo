CREATE PROCEDURE dbo.GetFQSOrganizationInfo
@OrgNumber int
AS 
SELECT OrgNumber, DisabledFlag, Name, Address1, Address2, City, State, ZipCode, Phone, PhoneExtension, Fax, Email, Miscellaneous, Contact
FROM FQSOrganization
WHERE OrgNumber = @OrgNumber
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFQSOrganizationInfo] TO [IRMAReportsRole]
    AS [dbo];

