CREATE PROCEDURE dbo.UpdateContactInfo
@Contact_ID int,
@Contact_Name varchar(50),
@Phone varchar(14),
@Phone_Ext varchar(5),
@Fax varchar(14),
@Email varchar(50)
AS 

UPDATE Contact 
SET Contact_Name = @Contact_Name,
    Phone = @Phone,
    Phone_Ext = @Phone_Ext,
    Fax = @Fax,
	Email = @Email
WHERE Contact_ID = @Contact_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateContactInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateContactInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateContactInfo] TO [IRMAReportsRole]
    AS [dbo];

