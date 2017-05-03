CREATE PROCEDURE dbo.YearlyBuggyCount2
@Store_No int,
@StartDate datetime
AS

SELECT Week +((Period-1)*4) AS Week, 
       Year,
       COUNT(*) AS CustomerCount,
       SUM(Sales) AS TotalSales
FROM (SELECT CONVERT(VARCHAR(10), Sales_Fact.Time_Key, 120) AS Date_Key, 
             Sales_Fact.Store_No, 
             Transaction_No,
             Register_No,
             SUM (Sales_Amount
    	          + Return_Amount 
    	          + Markdown_Amount 
    	          + Promotion_Amount 
    	          + Store_Coupon_Amount) AS Sales
      FROM Sales_Fact (NOLOCK) INNER JOIN Time (NOLOCK) ON Sales_Fact.Time_Key = Time.Time_Key
           INNER JOIN Item (NOLOCK) ON Sales_Fact.Item_Key = Item.Item_Key
           INNER JOIN StoreSubteam (NOLOCK) ON (Sales_Fact.SubTeam_No = StoreSubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_Fact.Store_No) 
      WHERE Sales_Fact.Store_No = @Store_No AND 
           Time.Date_Key >= DATEADD(d, -721, @StartDate) AND 
           Time.Date_Key <= DATEADD(d, -357, @StartDate) AND
           Item.Sales_Account IS NULL
      GROUP BY CONVERT(VARCHAR(10), Sales_Fact.Time_Key, 120), Sales_Fact.Store_No, Transaction_No, Register_No) AS Sales_Tran
INNER JOIN Date (NOLOCK) ON (Sales_Tran.Date_Key = Date.Date_Key)
GROUP BY Week, Period, Year
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[YearlyBuggyCount2] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[YearlyBuggyCount2] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[YearlyBuggyCount2] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[YearlyBuggyCount2] TO [IRMAReportsRole]
    AS [dbo];

