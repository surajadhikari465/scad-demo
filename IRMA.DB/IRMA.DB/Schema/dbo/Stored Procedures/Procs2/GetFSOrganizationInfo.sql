CREATE PROCEDURE dbo.GetFSOrganizationInfo
@Organization_ID int
AS 
SELECT Organization_ID, OrganizationName, Address_Line_1, Address_Line_2, City, State, Zip_Code, Phone, Phone_Ext, Fax, Contact, Email_Address,Comment, ActiveOrganization, User_ID
FROM  FSOrganization
WHERE Organization_ID = @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfo] TO [IRMAReportsRole]
    AS [dbo];

