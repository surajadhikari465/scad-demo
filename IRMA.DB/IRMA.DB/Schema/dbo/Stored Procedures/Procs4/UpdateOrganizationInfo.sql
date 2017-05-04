﻿CREATE PROCEDURE dbo.UpdateOrganizationInfo
@Organization_ID int, 
@OrganizationName varchar(50), 
@Address_Line_1 varchar(50), 
@Address_Line_2 varchar(50), 
@City varchar(30), 
@State varchar(2), 
@Zip_Code varchar(10), 
@Phone varchar(20), 
@Phone_Ext varchar(5), 
@Fax varchar(20), 
@Contact varchar(45), 
@Email_Address varchar(50),
@Comment varchar(255), 
@ActiveOrganization bit
AS

UPDATE FSOrganization
SET User_ID = NULL,
    OrganizationName = @OrganizationName, 
    Address_Line_1 = @Address_Line_1, 
    Address_Line_2 = @Address_Line_2, 
    City = @City, 
    State = @State, 
    Zip_Code = @Zip_Code, 
    Phone = @Phone, 
    Phone_Ext = @Phone_Ext, 
    Fax = @Fax, 
    Contact = @Contact, 
    Email_Address = @Email_Address, 
    Comment = @Comment, 
    ActiveOrganization = @ActiveOrganization 
WHERE Organization_ID = @Organization_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrganizationInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrganizationInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrganizationInfo] TO [IRMAReportsRole]
    AS [dbo];

