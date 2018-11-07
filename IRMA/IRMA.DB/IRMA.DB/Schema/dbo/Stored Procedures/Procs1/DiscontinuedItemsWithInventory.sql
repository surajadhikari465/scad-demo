CREATE PROCEDURE [dbo].[DiscontinuedItemsWithInventory]
    @Store_No int,
    @SubTeam_No int
AS 

   -- Modification History:
   -- Date			Init		TFS		Comment
   -- 10/03/2012	AlexB				Removed all references to ItemCaseHistory
   -- 01/09/2012	BenS		8755	Updated Item.Discontinue_Item reference to account for
   --									schema change.  Using a table variable was much faster
   --									than using the scalar function

BEGIN
    SET NOCOUNT ON
 
	
	--**************************************************************************
	-- Populate internal variables
	--**************************************************************************
	DECLARE @DiscoTable TABLE (ItemKey int, DiscontinueItem int)
	INSERT INTO @DiscoTable (ItemKey, DiscontinueItem)
	SELECT
		siv.Item_Key,
		MIN(CAST(siv.DiscontinueItem As Int)) As DiscontinueItem
	FROM
		StoreItemVendor siv (nolock)
	WHERE
		siv.Store_No = @Store_No
	GROUP BY siv.Item_Key


	--**************************************************************************
	-- Main SQL
	--**************************************************************************
	SELECT Item.Item_Key, Item_Description, Identifier, 
			PackSize As Package_Desc1, Item.Package_Desc2, ItemUnit.Unit_Name,
			UnitsOnHand AS On_Hand
	FROM 
		Item (nolock)
	INNER JOIN
		ItemIdentifier (nolock)
		ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
	LEFT JOIN
		ItemUnit (nolock)
		ON ItemUnit.Unit_ID = Item.Package_Unit_ID
	INNER JOIN (
		SELECT ItemHistory.Item_Key, Package_Desc1 As PackSize, 
				SUM( ISNULL(ItemHistory.Quantity, 0)
				+ ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) AS UnitsOnHand
		FROM
				ItemAdjustment (nolock)
				INNER JOIN ItemHistory (nolock) ON ItemHistory.Store_No = @Store_No
													AND ItemHistory.SubTeam_No = @SubTeam_No
													AND ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID
				INNER JOIN Item (nolock) ON Item.Item_Key = ItemHistory.Item_Key
				INNER JOIN OnHand (nolock) ON	OnHand.Item_Key = ItemHistory.Item_Key
												AND OnHand.Store_No = ItemHistory.Store_No
												AND OnHand.SubTeam_No = ItemHistory.SubTeam_No
				INNER JOIN @DiscoTable dt ON Item.Item_Key = dt.ItemKey
		WHERE Deleted_Item = 0 AND dt.DiscontinueItem = 1 AND DateStamp >= OnHand.LastReset 
		GROUP BY ItemHistory.Item_Key,  Package_Desc1
		HAVING SUM(ISNULL(ItemHistory.Quantity, 0) + ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) > 0
		) T2 ON (Item.Item_Key = T2.Item_Key) 
	Order By Item_Key

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DiscontinuedItemsWithInventory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DiscontinuedItemsWithInventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DiscontinuedItemsWithInventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DiscontinuedItemsWithInventory] TO [IRMAReportsRole]
    AS [dbo];

