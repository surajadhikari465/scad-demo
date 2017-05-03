CREATE PROCEDURE dbo.InsertContact
@Vendor_ID int,
@Contact_Name varchar(50)
AS 

INSERT INTO Contact (Vendor_ID, Contact_Name)
VALUES (@Vendor_ID, @Contact_Name)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertContact] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertContact] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertContact] TO [IRMAReportsRole]
    AS [dbo];

