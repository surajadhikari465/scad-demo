CREATE PROCEDURE dbo.DeleteContacts 
@Vendor_Id int 
AS 

DELETE 
FROM Contact 
WHERE Vendor_ID = @Vendor_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteContacts] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteContacts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteContacts] TO [IRMAReportsRole]
    AS [dbo];

