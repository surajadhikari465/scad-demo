/****** Object:  Stored Procedure dbo.CharcuterieInventoryItems    Script Date: 3/9/2006 8:41:07 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CharcuterieInventoryItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CharcuterieInventoryItems]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.CharcuterieInventoryItems    Script Date: 3/9/2006 8:41:07 AM ******/

CREATE PROCEDURE dbo.CharcuterieInventoryItems 
@Store_No int

AS

DECLARE @StartDate smalldatetime,  @EndDate smalldatetime

SET @EndDate = GETDATE()
SET @StartDate = GETDATE() - 28



SELECT DISTINCT ItemIdentifier.Identifier, Item.Item_Description, Item.Package_Desc1, Item.Package_Desc2, ItemUnit.Unit_Name, 
ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, @Store_No, Item.SubTeam_No, GETDATE()), 0) AS 'AvgCost'
FROM Item
JOIN ItemIdentifier ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
JOIN Price ON Item.Item_key = Price.Item_Key AND Price.Store_No = @Store_No
JOIN Sales_SumByItem ON Price.Item_Key = Sales_SumByItem.Item_Key AND Price.Store_No = Sales_SumByItem.Store_No 
JOIN ItemUnit  ON ItemUnit.Unit_ID = Item.Package_Unit_ID
WHERE Sales_SumByItem.SubTeam_No = 5100 
AND Sales_SumByItem.Date_Key>= @StartDate And Sales_SumByItem.Date_Key<= @EndDate
ORDER BY Item.Item_Description




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
