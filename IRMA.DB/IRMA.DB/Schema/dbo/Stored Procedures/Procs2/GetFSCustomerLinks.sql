CREATE PROCEDURE dbo.GetFSCustomerLinks
@Customer_ID int 
AS 
SELECT Customer_ID AS Customer_ID FROM FSCustomer WHERE Customer_ID = @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerLinks] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerLinks] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerLinks] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFSCustomerLinks] TO [IRMAReportsRole]
    AS [dbo];

