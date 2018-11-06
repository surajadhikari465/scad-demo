CREATE PROCEDURE dbo.TaxReport2
@Store_No int,
@StartDate datetime,
@EndDate datetime
AS

SELECT Store_No, SUM(Payment_Amount) AS SalesReceipts 
FROM Payment_SumByRegister (NOLOCK) 
WHERE Date_Key >= @StartDate AND Date_Key < @EndDate + 1 AND Store_No = @Store_No 
GROUP BY Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport2] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport2] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport2] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport2] TO [IRMAReportsRole]
    AS [dbo];

