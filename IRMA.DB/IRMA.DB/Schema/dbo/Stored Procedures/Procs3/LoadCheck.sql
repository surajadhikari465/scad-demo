CREATE PROCEDURE dbo.LoadCheck
@Store_No int,
@StartDate datetime,
@EndDate datetime
AS

SELECT CASE WHEN NOT P.Transaction_No IS NULL THEN P.Transaction_No WHEN NOT B.Transaction_No IS NULL THEN B.Transaction_No ELSE S.Transaction_No END AS Transaction_No, 
        CASE WHEN NOT P.Register_No IS NULL THEN P.Register_No WHEN NOT B.Register_No IS NULL THEN B.Register_No ELSE S.Register_No END AS Register_No, 
        CASE WHEN NOT P.Time_Key IS NULL THEN P.Time_Key WHEN NOT B.Time_Key IS NULL THEN B.Time_Key ELSE S.Time_Key END AS Time_Key, 
        CASE WHEN NOT P.Store_No IS NULL THEN P.Store_No WHEN NOT B.Store_No IS NULL THEN B.Store_No ELSE S.Store_No END AS Store_No, 
        P.Takings AS Payment_Takings, B.NetSales AS Buggy_NetSales, B.Takings AS Buggy_Takings, S.NetSales AS Sales_NetSales  
 FROM 
     (SELECT Store_No, Transaction_No, Register_No, Time_Key, SUM(Payment_Amount) AS Takings 
      FROM Payment_Fact (NOLOCK) 
      WHERE  Store_no IN  (@Store_No) AND  Time_Key >= @StartDate AND Time_Key < @EndDate
      GROUP BY Store_No, Transaction_No, Register_No, Time_Key) P 
      FULL OUTER JOIN 
     (SELECT Store_No, Transaction_No, Register_No, Time_Key, SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) AS NetSales 
      FROM Sales_Fact (NOLOCK) 
      WHERE  Store_no IN  (@Store_No) AND  Time_Key >= @StartDate AND Time_Key < @EndDate 
      GROUP BY Store_No, Transaction_No, Register_No, Time_Key 
     ) S ON (P.Store_No = S.Store_No AND P.Transaction_No = S.Transaction_No AND P.Register_No = S.Register_No AND P.Time_Key = S.Time_Key) 
     FULL OUTER JOIN 
     (SELECT Store_No, Transaction_No, Register_No, Time_Key, SUM(Cash_Amount) + SUM(Credit_Amount) + SUM(Check_Amount) + SUM(Food_Stamp_Amount) + SUM(Vendor_Coupon_Amount) + SUM(Coupon_Amount)+ SUM(GC_In_Amount) + SUM(Employee_Dis_Amount) - SUM(Change_Amount) - SUM(Tax_Table1_Amount) - SUM(Tax_Table2_Amount) - SUM(Tax_Table3_Amount)  AS NetSales, 
             SUM(Cash_Amount) + SUM(Credit_Amount) + SUM(Check_Amount) + SUM(Food_Stamp_Amount) + SUM(Vendor_Coupon_Amount) + SUM(Coupon_Amount) + SUM(GC_In_Amount) - SUM(Change_Amount) AS Takings 
      FROM Buggy_Fact (NOLOCK) 
      WHERE  Store_no IN  (@Store_No) AND  Time_Key >= @StartDate AND Time_Key < @EndDate
      GROUP BY Store_No, Transaction_No, Register_No, Time_Key 
     ) B ON (P.Store_No = B.Store_No AND P.Transaction_No = B.Transaction_No AND P.Register_No = B.Register_No AND P.Time_Key = B.Time_Key) 
 WHERE (B.Time_Key IS NULL OR P.Time_Key IS NULL OR S.Time_Key IS NULL) OR (P.Takings <> B.Takings) OR (B.NetSales <> S.NetSales)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCheck] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCheck] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[LoadCheck] TO [IRMAReportsRole]
    AS [dbo];

