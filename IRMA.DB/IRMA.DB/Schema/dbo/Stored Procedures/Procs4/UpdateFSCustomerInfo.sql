CREATE PROCEDURE dbo.UpdateFSCustomerInfo
@Customer_ID int, 
@Organization_ID int,
@CustomerName varchar(50), 
@Address_Line_1 varchar(50), 
@Address_Line_2 varchar(50), 
@City varchar(30), 
@State varchar(2), 
@Zip_Code varchar(10), 
@Phone varchar(20), 
@Phone_Ext varchar(5), 
@Fax varchar(20), 
@Customer_Code int, 
@Email_Address varchar(50),
@Birthday varchar (20),
@Comment varchar(255), 
@ActiveCustomer bit
AS

UPDATE FSCustomer
SET User_ID = NULL,
    Organization_ID = @Organization_ID,
    CustomerName = @CustomerName, 
    Address_Line_1 = @Address_Line_1, 
    Address_Line_2 = @Address_Line_2, 
    City = @City, 
    State = @State, 
    Zip_Code = @Zip_Code, 
    Phone = @Phone, 
    Phone_Ext = @Phone_Ext, 
    Fax = @Fax, 
    Customer_Code = @Customer_Code, 
    Email_Address = @Email_Address, 
    Birthday = @Birthday,
    Comment = @Comment, 
    ActiveCustomer = @ActiveCustomer 
WHERE Customer_ID = @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateFSCustomerInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateFSCustomerInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateFSCustomerInfo] TO [IRMAReportsRole]
    AS [dbo];

