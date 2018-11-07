CREATE PROCEDURE dbo.GetFSOrganizationInfoFirst
AS 

SELECT Organization_ID, OrganizationName, Address_Line_1, Address_Line_2, City, State, Zip_Code, Phone, Phone_Ext, Fax, 
       Contact, Email_Address,Comment, ActiveOrganization, User_ID
FROM  FSOrganization
WHERE Organization_ID = (SELECT MIN(Organization_ID) FROM FSOrganization)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSOrganizationInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

