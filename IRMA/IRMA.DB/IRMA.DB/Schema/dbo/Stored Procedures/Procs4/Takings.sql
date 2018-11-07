CREATE PROCEDURE dbo.Takings
@Store_No int,
@StartDate datetime
AS 

SELECT Payment_Fact.Cashier_ID, Payment.Description, SUM(Payment_Amount) AS PaymentAmount, COUNT(*) AS PaymentCount 
FROM Time (NOLOCK) INNER JOIN (Payment (NOLOCK) INNER JOIN Payment_Fact (NOLOCK) ON (Payment.Payment_Type = Payment_Fact.Payment_Type) 
     ) ON (Time.Time_Key = Payment_Fact.Time_Key) 
WHERE Store_No = @Store_No AND Date_Key = @StartDate
GROUP BY Payment_Fact.Cashier_ID, Payment.Description
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Takings] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Takings] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Takings] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Takings] TO [IRMAReportsRole]
    AS [dbo];

