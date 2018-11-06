CREATE PROCEDURE dbo.GetContactInfoFirst
@Vendor_ID int
AS 

SELECT Contact_ID, Contact_Name, Phone, Phone_Ext, Fax , Email
FROM Contact 
WHERE Contact_ID = (SELECT MIN(Contact_ID) FROM Contact WHERE Vendor_ID = @Vendor_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

