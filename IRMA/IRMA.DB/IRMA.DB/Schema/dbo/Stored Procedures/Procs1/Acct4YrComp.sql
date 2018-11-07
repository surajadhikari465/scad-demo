CREATE PROCEDURE dbo.Acct4YrComp
@iLoop as tinyint,
@StartDate datetime,
@EndDate datetime
As

SELECT Sales_SumByItem.Store_No, 
    SubTeam.SubTeam_No, 
    SubTeam.SubTeam_Name, 
    SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount) As TotalPrice 
FROM Sales_SumByItem (nolock) 
    INNER JOIN 
        StoreSubTeam (nolock) 
        ON (StoreSubTeam.SubTeam_No = Sales_SumByItem.SubTeam_No 
            AND StoreSubTeam.Store_No = Sales_SumByItem.Store_No)	
    INNER JOIN 
        Item (nolock) 
        ON Item.Item_Key = Sales_SumByItem.Item_Key
    INNER JOIN 
        SubTeam (nolock) 
        ON Sales_SumByItem.SubTeam_No = SubTeam.SubTeam_No
WHERE Date_Key >= @StartDate - (364 * (@iLoop - 1)) AND  Date_Key <= @EndDate - (364 * (@iLoop - 1))
      AND Sales_Account IS NULL 
      AND SubTeam.SubTeam_no NOT IN (4000, 4010) AND SubTeam.SubTeam_no NOT LIKE '9%'
GROUP BY Sales_SumByItem.Store_No, SubTeam.SubTeam_No, SubTeam.SubTeam_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Acct4YrComp] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Acct4YrComp] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Acct4YrComp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Acct4YrComp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Acct4YrComp] TO [IRMAReportsRole]
    AS [dbo];

