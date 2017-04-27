SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SubTeamSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SubTeamSales]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

