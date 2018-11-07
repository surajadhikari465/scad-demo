CREATE PROCEDURE dbo.DailyTax
    @Store_No int,
    @StartDate datetime,
    @EndDate datetime
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Date_Key,
           Store_No, 
           SUM(Tax_Table1_Amount) AS Tax1Collected, 
           SUM(Tax_Table2_Amount) AS Tax2Collected, 
           SUM(Tax_Table3_Amount) AS Tax3Collected,
           SUM(Food_Stamp_Amount) AS FS, 
           SUM(GC_Sales_Amount) AS GC
    FROM Buggy_SumByRegister (NOLOCK)
    WHERE Date_Key >= @StartDate 
    AND Date_Key < @EndDate +1 
    AND Store_No = @Store_No  
    GROUP BY Store_No, Date_Key
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTax] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTax] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTax] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DailyTax] TO [IRMAReportsRole]
    AS [dbo];

