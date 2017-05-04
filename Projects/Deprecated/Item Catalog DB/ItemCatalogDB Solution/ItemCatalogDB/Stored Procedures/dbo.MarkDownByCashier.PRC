SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[MarkDownByCashier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[MarkDownByCashier]
GO


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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


