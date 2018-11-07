CREATE PROCEDURE dbo.CheckForDuplicateCustomers 
@Customer_ID int,
@CustomerName varchar(25) 
AS 

SELECT COUNT(*) AS CustomerCount 
FROM FSCustomer
WHERE CustomerName = @CustomerName AND Customer_Id <> @Customer_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCustomers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCustomers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCustomers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCustomers] TO [IRMAReportsRole]
    AS [dbo];

