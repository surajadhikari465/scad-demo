CREATE PROCEDURE dbo.DailyTaxSales
    @Store_No int,
    @StartDate datetime,
    @EndDate datetime

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Store_No, Date_Key, 
        SUM(CASE WHEN Taxed = 0 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TaxExempt,
        SUM(CASE WHEN Tax_Table = 1 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TT1,
        SUM(CASE WHEN Tax_Table = 2 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TT2,
        SUM(CASE WHEN Tax_Table = 3 THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS TT3,
        SUM(CASE WHEN (Taxed = 0 or Tax_Table = 0) THEN Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount ELSE 0 END) AS NoTax 
    FROM Sales_Fact (NOLOCK) 
    INNER JOIN 
        Time 
        ON Sales_Fact.Time_Key = Time.Time_Key 
    WHERE Date_Key >= @StartDate AND Date_Key < @EndDate + 1 
    AND Store_No = @Store_No
    GROUP By Store_No, Date_Key
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTaxSales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTaxSales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTaxSales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTaxSales] TO [IRMAReportsRole]
    AS [dbo];

