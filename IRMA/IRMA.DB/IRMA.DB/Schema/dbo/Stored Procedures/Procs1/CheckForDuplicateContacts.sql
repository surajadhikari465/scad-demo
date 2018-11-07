CREATE PROCEDURE dbo.CheckForDuplicateContacts
@Vendor_ID int,
@Contact_ID int,
@Contact_Name varchar(25) 
AS 

SELECT COUNT(*) AS ContactCount 
FROM Contact 
WHERE Contact_Name = @Contact_Name AND 
      Vendor_ID = @Vendor_ID AND 
      Contact_ID <> @Contact_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateContacts] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateContacts] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateContacts] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateContacts] TO [IRMAReportsRole]
    AS [dbo];

