CREATE PROCEDURE dbo.GetContactInfoLast
@Vendor_ID int
AS 

SELECT Contact_ID, Contact_Name, Phone, Phone_Ext, Fax , Email
FROM Contact 
WHERE Contact_ID = (SELECT MAX(Contact_ID) FROM Contact WHERE Vendor_ID = @Vendor_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetContactInfoLast] TO [IRMAReportsRole]
    AS [dbo];

