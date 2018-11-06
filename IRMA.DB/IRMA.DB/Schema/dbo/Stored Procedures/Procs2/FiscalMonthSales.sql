CREATE PROCEDURE dbo.FiscalMonthSales
@Team_No int,
@SubTeam_No int,
@Store_No int,
@Zone_Id int,
@Month int,
@Year int,
@SubtractNumber tinyint

AS
SELECT Date.Day_Of_Month, SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) AS TotalPrice
FROM Item (NOLOCK) INNER JOIN (Store (NOLOCK) INNER JOIN (Date (NOLOCK) INNER JOIN (
       Sales_SumByItem (NOLOCK) INNER JOIN StoreSubTeam (NOLOCK) ON (Sales_SumByItem.SubTeam_No = StoreSubTeam.SubTeam_No AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No)
       ) ON (Sales_SumByItem.Date_Key = Date.Date_Key)
     ) ON (Store.Store_No = Sales_SumByItem.Store_No) 
   ) ON (Item.Item_Key = Sales_SumByItem.Item_Key)
WHERE Period = @Month AND Year = @Year - @SubtractNumber AND
      ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No) = StoreSubTeam.SubTeam_No AND
      ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id AND
      Sales_Account IS NULL
GROUP BY Date.Day_Of_Month
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalMonthSales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalMonthSales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalMonthSales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FiscalMonthSales] TO [IRMAReportsRole]
    AS [dbo];

