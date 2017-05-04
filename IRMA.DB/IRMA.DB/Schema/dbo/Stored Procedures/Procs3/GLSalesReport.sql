CREATE PROCEDURE dbo.GLSalesReport
@Begin_Date as smalldatetime, 
@End_Date as smalldatetime,
@Store_No int
AS

SELECT Store.BusinessUnit_ID AS Unit, 'ACTUALS' AS Ledger, '220000' AS Account, NULL AS Dept, NULL AS Prod,
       '' AS Aff, '' AS Proj, 'USD' AS Curr, 
       (SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount)) +
       (SELECT SUM(Tax_Table1_Amount) + SUM(Tax_Table2_Amount) + SUM(Tax_Table3_Amount) - SUM(Employee_Dis_Amount)
        FROM Buggy_SumByRegister (NOLOCK)
        WHERE Buggy_SumByRegister.Date_Key = Sales_SumByItem.Date_Key  AND
              Buggy_SumByRegister.Store_No = Store.Store_No) AS Amount,
       'Suspense Account' AS Descr, CONVERT(CHAR(10), Date_Key, 1) AS Trans_Date
FROM Store (NOLOCK) INNER JOIN (
     StoreSubTeam (NOLOCK) INNER JOIN (
       SubTeam (NOLOCK) INNER JOIN Sales_SumByItem (NOLOCK) ON (SubTeam.SubTeam_No = Sales_SumByItem.SubTeam_No)
           ) ON (StoreSubTeam.SubTeam_No = SubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_SumByItem.Store_No)     
         ) ON (Store.Store_No = Sales_SumByItem.Store_No)
WHERE Date_Key >= @Begin_Date AND Date_Key < DATEADD(DAY,1,@End_Date) AND 
      Sales_SumByItem.Store_No = ISNULL(@Store_No,Sales_SumByItem.Store_No) 
GROUP BY Store.BusinessUnit_ID, Sales_SumByItem.Date_Key, Store.Store_Name, Store.Store_No
UNION
SELECT Store.BusinessUnit_ID AS Unit, 'ACTUALS' AS Ledger, '400000' AS Account, StoreSubTeam.Team_No AS Dept, SubTeam.SubTeam_No AS Prod,
       '' AS Aff, '' AS Proj, 'USD' AS Curr, 
       0 - (SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount)) AS Amount,
       'Sales - Retail' AS Descr, CONVERT(CHAR(10), Date_Key, 1) AS Trans_Date
FROM Store (NOLOCK) INNER JOIN (
     StoreSubTeam (NOLOCK) INNER JOIN (
       Item (NOLOCK) INNER JOIN (
         SubTeam (NOLOCK) INNER JOIN Sales_SumByItem (NOLOCK) ON (SubTeam.SubTeam_No = Sales_SumByItem.SubTeam_No)
        ) ON (Item.Item_Key = Sales_SumByItem.Item_Key)
      ) ON (StoreSubTeam.SubTeam_No = SubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_SumByItem.Store_No)
    ) ON (Store.Store_No = Sales_SumByItem.Store_No)
WHERE Date_Key >= @Begin_Date AND Date_Key < DATEADD(DAY,1,@End_Date) AND
      Sales_SumByItem.Store_No = ISNULL(@Store_No,Sales_SumByItem.Store_No) AND Item.Sales_Account IS NULL
GROUP BY Store.BusinessUnit_ID, SubTeam.SubTeam_No, StoreSubTeam.Team_No, CONVERT(CHAR(10), Date_Key, 1), Store.Store_Name, Item.Sales_Account
UNION
SELECT Store.BusinessUnit_ID AS Unit, 'ACTUALS' AS Ledger, Item.Sales_Account AS Account, StoreSubTeam.Team_No AS Dept, SubTeam.SubTeam_No AS Prod,
       '' AS Aff, '' AS Proj, 'USD' AS Curr, 
       0 - SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) AS Amount,
       Item.Item_Description AS Descr, CONVERT(CHAR(10), Date_Key, 1) AS Trans_Date
FROM Store (NOLOCK) INNER JOIN (
       StoreSubTeam (NOLOCK) INNER JOIN (
        Item (NOLOCK) INNER JOIN (
         SubTeam (NOLOCK) INNER JOIN Sales_SumByItem (NOLOCK) ON (SubTeam.SubTeam_No = Sales_SumByItem.SubTeam_No)
       ) ON (Item.Item_Key = Sales_SumByItem.Item_Key)
      ) ON (StoreSubTeam.SubTeam_No = SubTeam.SubTeam_No AND StoreSubTeam.Store_No = Sales_SumByItem.Store_No)     
     ) ON (Store.Store_No = Sales_SumByItem.Store_No)
WHERE Date_Key >= @Begin_Date AND Date_Key < DATEADD(DAY,1,@End_Date) AND
      Sales_SumByItem.Store_No = ISNULL(@Store_No,Sales_SumByItem.Store_No) AND Item.Sales_Account IS NOT NULL
GROUP BY Store.BusinessUnit_ID, SubTeam.SubTeam_No, StoreSubTeam.Team_No, CONVERT(CHAR(10), Date_Key, 1), Store.Store_Name, Item.Sales_Account,
         Item.Item_Description
UNION 
SELECT Store.BusinessUnit_ID AS Unit, 'ACTUALS' AS Ledger, '234000' AS Account, NULL AS Dept, NULL AS Prod,
       '' AS Aff, '' AS Proj, 'USD' AS Curr, 
       0 - (SUM(Tax_Table1_Amount)) AS Amount,
       'State sales tax - food' AS Descr, CONVERT(CHAR(10), Date_Key, 1) AS Trans_Date
FROM Store (NOLOCK) INNER JOIN Buggy_SumByRegister (NOLOCK) ON (Buggy_SumByRegister.Store_No = Store.Store_No)
WHERE Date_Key >= @Begin_Date AND Date_Key < DATEADD(DAY,1,@End_Date) AND
      Buggy_SumByRegister.Store_No = ISNULL(@Store_No,Buggy_SumByRegister.Store_No)
GROUP BY Store.BusinessUnit_ID, Buggy_SumByRegister.Date_Key, Store.Store_Name, Store.Store_No
UNION
SELECT Store.BusinessUnit_ID AS Unit, 'ACTUALS' AS Ledger, '236000' AS Account, NULL AS Dept, NULL AS Prod,
       '' AS Aff, '' AS Proj, 'USD' AS Curr, 
       0 - (SUM(Tax_Table2_Amount)) AS Amount,
       'State sales tax - nonfood' AS Descr, CONVERT(CHAR(10), Date_Key, 1) AS Trans_Date
FROM Store (NOLOCK) INNER JOIN Buggy_SumByRegister (NOLOCK) ON (Buggy_SumByRegister.Store_No = Store.Store_No)
WHERE Date_Key >= @Begin_Date AND Date_Key < DATEADD(DAY,1,@End_Date) AND
      Buggy_SumByRegister.Store_No = ISNULL(@Store_No,Buggy_SumByRegister.Store_No)
GROUP BY Store.BusinessUnit_ID, Buggy_SumByRegister.Date_Key, Store.Store_Name, Store.Store_No
UNION
SELECT Store.BusinessUnit_ID AS Unit, 'ACTUALS' AS Ledger, '780000' AS Account, NULL AS Dept, NULL AS Prod,
       '' AS Aff, '' AS Proj, 'USD' AS Curr, 
       SUM(Employee_Dis_Amount) AS Amount,
       'Discount - Team member' AS Descr, CONVERT(CHAR(10), Date_Key, 1) AS Trans_Date
FROM Store (NOLOCK) INNER JOIN Buggy_SumByRegister (NOLOCK) ON (Buggy_SumByRegister.Store_No = Store.Store_No)
WHERE Date_Key >= @Begin_Date AND Date_Key < DATEADD(DAY,1,@End_Date) AND
      Buggy_SumByRegister.Store_No = ISNULL(@Store_No,Buggy_SumByRegister.Store_No)
GROUP BY Store.BusinessUnit_ID, CONVERT(CHAR(10), Date_Key, 1), Store.Store_Name
ORDER BY Trans_Date, Store.BusinessUnit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLSalesReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLSalesReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLSalesReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GLSalesReport] TO [IRMAReportsRole]
    AS [dbo];

