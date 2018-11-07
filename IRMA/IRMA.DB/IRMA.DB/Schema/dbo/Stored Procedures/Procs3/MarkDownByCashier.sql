CREATE PROCEDURE dbo.MarkDownByCashier
@StartDate Datetime,
@EndDate Datetime
AS

SELECT Cashier_ID, Store.Store_Name, SubTeam.SubTeam_Name, SUM(Markdown_Amount)
FROM SubTeam INNER JOIN (
       Store INNER JOIN (
         Sales_Fact INNER JOIN Time ON (Sales_Fact.Time_Key = Time.Time_Key)
       ) ON (Store.Store_No = Sales_Fact.Store_No)
     ) ON (SubTeam.SubTeam_No = Sales_Fact.SubTeam_No)
WHERE Date_Key >= @StartDate AND Date_Key <= @EndDate
GROUP BY Cashier_ID, Store.Store_Name, SubTeam.SubTeam_Name
HAVING SUM(MarkDown_Amount) <> 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarkDownByCashier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarkDownByCashier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarkDownByCashier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[MarkDownByCashier] TO [IRMAReportsRole]
    AS [dbo];

