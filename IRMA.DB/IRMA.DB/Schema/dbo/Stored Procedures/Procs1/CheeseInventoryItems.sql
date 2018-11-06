/****** Object:  Stored Procedure dbo.CheeseInventoryItems    Script Date: 3/9/2006 7:29:37 AM ******/

CREATE PROCEDURE dbo.CheeseInventoryItems 
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
WHERE Sales_SumByItem.SubTeam_No = 2300 
AND Sales_SumByItem.Date_Key>= @StartDate And Sales_SumByItem.Date_Key<= @EndDate
ORDER BY Item.Item_Description
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheeseInventoryItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheeseInventoryItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheeseInventoryItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheeseInventoryItems] TO [IRMAReportsRole]
    AS [dbo];

