SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Acct4YrComp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Acct4YrComp]
GO

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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


