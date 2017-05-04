CREATE PROCEDURE dbo.GetOrderAllocateItemsQty
	@ItemKey int
AS 

	DECLARE @tblAllocItems TABLE (RowID int, Store_No int, SubTeam_No int, UserName varchar(35), Item_Key int, Identifier varchar(13), 
								  Item_Description varchar(60), Category_Name varchar(35), Pre_Order bit, PackSize decimal(18,4), 
								  BOH decimal(18,4), WOO decimal(18,4), SOO decimal(18,4), FIFODateTime datetime, Alloc decimal(18,4))

	INSERT INTO @tblAllocItems
		SELECT tmpOrdersAllocateItems.*, 
			   (SELECT ISNULL(SUM(QuantityAllocated), 0)
				FROM tmpOrdersAllocateOrderItems 
				WHERE Item_Key = @ItemKey AND Package_Desc1 = tmpOrdersAllocateItems.PackSize) As Alloc
		FROM 
			tmpOrdersAllocateItems WHERE Item_Key = @ItemKey AND (BOH <> 0 OR WOO <> 0 OR SOO <> 0)


	SELECT 
		tbl.PackSize, tbl.BOH, tbl.WOO, tbl.SOO, tbl.Alloc, (tbl.BOH + tbl.WOO - tbl.Alloc) AS EOH
	FROM 
		@tblAllocItems tbl
	ORDER BY 
		Identifier, FIFODateTime
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderAllocateItemsQty] TO [IRMAClientRole]
    AS [dbo];

