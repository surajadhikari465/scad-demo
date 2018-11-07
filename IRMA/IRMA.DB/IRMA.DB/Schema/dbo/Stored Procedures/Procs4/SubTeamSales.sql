CREATE PROCEDURE dbo.SubTeamSales
@Store_No int,
@StartDate varchar(20),
@EndDate varchar(20)
AS

SELECT SubTeam.SubTeam_Name, 
       SubTeam.SubTeam_No, 
       SUM(Sales_Amount) As Gross,
       SUM(Return_Quantity) As NoOfReturns, 
       SUM(Return_Amount) As Returns, 
       SUM(MarkDown_Amount) + SUM(Promotion_Amount) As Discounts 
FROM Item (nolock) 
        INNER JOIN 
            Sales_SumByItem (nolock) 
            ON Item.Item_Key = Sales_SumByItem.Item_Key
        INNER JOIN
            Subteam (nolock) 
            ON Sales_SumByItem.Subteam_No = Subteam.Subteam_No
WHERE Date_Key >= @StartDate AND Date_Key <= @EndDate 
      AND @Store_No = Sales_SumByItem.Store_No 
      AND Sales_Account IS NULL  
GROUP BY SubTeam.SubTeam_Name, SubTeam.SubTeam_No
ORDER BY SubTeam.SubTeam_Name ASC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeamSales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeamSales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeamSales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeamSales] TO [IRMAReportsRole]
    AS [dbo];

