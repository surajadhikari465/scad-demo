CREATE PROCEDURE dbo.GetFSCustomerInformation
@Customer_ID int
AS 

SELECT FSOrganization.OrganizationName, FSCustomer.Customer_ID, FSCustomer.Organization_ID, FSCustomer.Customer_Code, 
       FSCustomer.CustomerName, FSCustomer.Address_Line_1, FSCustomer.Address_Line_2, FSCustomer.City, FSCustomer.State, 
       FSCustomer.Zip_Code, FSCustomer.Phone, FSCustomer.Phone_Ext, FSCustomer.Fax, FSCustomer.Email_Address, 
       FSCustomer.Birthday, FSCustomer.Comment, FSCustomer.ActiveCustomer, FSCustomer.User_ID
FROM  FSCustomer INNER JOIN FSOrganization ON(FSCustomer.Organization_ID = FSOrganization.Organization_ID) 
WHERE FSCustomer.Customer_ID = @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerInformation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerInformation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerInformation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerInformation] TO [IRMAReportsRole]
    AS [dbo];

