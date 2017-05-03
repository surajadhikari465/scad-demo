CREATE PROCEDURE dbo.TaxReport1
    @Store_No int,
    @StartDate datetime,
    @EndDate datetime
AS

    SELECT Store_No, 
           SUM(Tax_Table1_Amount) AS Tax1Collected, 
           SUM(Tax_Table2_Amount) AS Tax2Collected, 
           SUM(Tax_Table3_Amount) AS Tax3Collected,
           SUM(Food_Stamp_Amount) AS FS, 
           SUM(GC_Sales_Amount) AS GC, 
           SUM(Employee_Dis_Amount) AS ED 
    FROM Buggy_SumByRegister (NOLOCK) 
    WHERE Date_Key >= @StartDate AND Date_Key < @EndDate + 1 AND Store_No = @Store_No 
    GROUP BY Store_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport1] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport1] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport1] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxReport1] TO [IRMAReportsRole]
    AS [dbo];

