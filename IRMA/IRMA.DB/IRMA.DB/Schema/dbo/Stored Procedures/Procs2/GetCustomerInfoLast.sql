CREATE PROCEDURE dbo.GetCustomerInfoLast
AS 
SELECT FSCustomer.Customer_ID, FSCustomer.Organization_ID,FSOrganization.OrganizationName, FSCustomer.Customer_Code, FSCustomer.CustomerName, FSCustomer.Address_Line_1, FSCustomer.Address_Line_2, FSCustomer.City,FSCustomer.State, FSCustomer.Zip_Code, FSCustomer.Phone, FSCustomer.Phone_Ext, FSCustomer.Fax, FSCustomer.Email_Address, FSCustomer.Birthday,FSCustomer.Comment, FSCustomer.ActiveCustomer, FSCustomer.User_ID     
FROM  FSCustomer  INNER JOIN FSOrganization ON(FSCustomer.Organization_ID = FSOrganization.Organization_ID) 
WHERE Customer_ID = (SELECT MAX(Customer_ID) FROM FSCustomer)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerInfoLast] TO [IRMAReportsRole]
    AS [dbo];

