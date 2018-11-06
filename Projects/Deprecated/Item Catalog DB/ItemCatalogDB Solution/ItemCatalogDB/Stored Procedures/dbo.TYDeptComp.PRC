SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TYDeptComp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TYDeptComp]
GO


CREATE PROCEDURE dbo.TYDeptComp
@Month int,
@Year int,
@Team_No int,
@SubTeam_No int,
@Store_No int,
@Zone_ID int
AS

SELECT Date.Date_Key, SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) + SUM(Promotion_Amount)AS TotalPrice
FROM StoreSubTeam (NOLOCK) INNER JOIN (Date (NOLOCK) INNER JOIN (Sales_SumByItem (NOLOCK) INNER JOIN Item (NOLOCK) ON (Sales_SumByItem.Item_Key = Item.Item_Key)
	)ON (Sales_SumByItem.Date_Key = Date.Date_Key)
      )ON (Sales_SumByItem.SubTeam_No = StoreSubTeam.SubTeam_No AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No)
WHERE Period = @Month AND Year = @Year AND 
    ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No) = StoreSubTeam.SubTeam_No AND
    ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No --AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id 
     AND Sales_Account IS NULL   
GROUP BY Date.Date_Key

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


