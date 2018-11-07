CREATE PROCEDURE dbo.CheckForDuplicateCardNumbers
@Begin_Number int,
@End_Number int 
AS 

SELECT COUNT(*) AS CustomerCount
FROM FSCustomer
WHERE Customer_Code >= @Begin_Number AND Customer_Code <= @End_Number
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCardNumbers] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCardNumbers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCardNumbers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateCardNumbers] TO [IRMAReportsRole]
    AS [dbo];

