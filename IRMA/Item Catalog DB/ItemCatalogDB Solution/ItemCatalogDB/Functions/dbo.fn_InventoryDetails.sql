
/****** Object:  UserDefinedFunction [dbo].[fn_InventoryDetails]    Script Date: 10/04/2012 16:28:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_InventoryDetails]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_InventoryDetails]
GO


/****** Object:  UserDefinedFunction [dbo].[fn_InventoryDetails]    Script Date: 10/04/2012 16:28:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_InventoryDetails]
(
    @Store_No int,
    @SubTeam_No int,
    @Item_Key  int
)

RETURNS @Table TABLE 
(
	Item_Key int,
	SubTeam_No int,
	CasesOnHand decimal(38, 4),
    UnitsOnHand decimal(38, 4),
    Identifier varchar(16),
    Package_desc2 decimal(9,4)
)
AS

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
BEGIN 

INSERT @Table            
SELECT 
				ItemHistory.Item_Key, 
				ItemHistory.SubTeam_No, 
				SUM(
					CASE WHEN ISNULL(ItemHistory.Quantity, 0) > 0 
						THEN ItemHistory.Quantity / CASE WHEN Item.Package_Desc1 <> 0 THEN Item.Package_Desc1 ELSE 1 END
						ELSE ISNULL(ItemHistory.Weight, 0) / 
									CASE WHEN Item.Package_Desc1 * Package_Desc2 <> 0 
										THEN (Item.Package_Desc1 * Package_Desc2) ELSE 1 END 
					END * ItemAdjustment.Adjustment_Type) AS CasesOnHand,
				SUM(ISNULL(ItemHistory.Quantity, 0) 
							+ ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) AS UnitsOnHand,
                 ItemIdentifier.identifier,
                 VCH.Package_Desc1
               	FROM 
					ItemHistory (nolock) 
					INNER JOIN OnHand (nolock) ON OnHand.Item_Key = ItemHistory.Item_Key 
									AND OnHand.Store_No = @store_No
									AND OnHand.SubTeam_No = isnull(@SubTeam_No,ItemHistory.SubTeam_No)
					INNER JOIN	ItemAdjustment (nolock)
									ON (ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID)
					INNER JOIN Item (nolock)  ON (Item.Item_Key = ItemHistory.Item_Key)
					INNER JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key 
									AND ItemIdentifier.Default_Identifier = 1 AND (ItemIdentifier.identifiertype='S' or ItemIdentifier.identifiertype='s')
					INNER JOIN StoreItemVendor SIV (nolock) ON SIV.Store_No = @Store_No AND SIV.Item_Key = Item.Item_Key
					INNER JOIN VendorCostHistory VCH (nolock) ON VCH.VendorCostHistoryID = 
													(SELECT max(Max.VendorCostHistoryID)
													FROM VendorCostHistory Max
													WHERE Max.StoreItemVendorID = SIV.StoreItemVendorID)					
			   WHERE 
					ItemHistory.Store_No = isnull(@store_No,ItemHistory.Store_No)
					AND ItemHistory.SubTeam_No = isnull(@SubTeam_No,ItemHistory.SubTeam_No)
					AND DateStamp >= OnHand.LastReset
                    and Item.Item_key=ISNULL(@Item_Key,Item.Item_Key)
                    AND SIV.PrimaryVendor = 1
    
			   GROUP BY ItemHistory.Item_Key, ItemHistory.SubTeam_No, Item.Package_Desc1,VCH.Package_Desc1,itemIdentifier.identifier
			   HAVING SUM( ISNULL(ItemHistory.Quantity, 0) 
				+ ISNULL(ItemHistory.Weight, 0) * ItemAdjustment.Adjustment_Type) <> 0
	RETURN 
END
GO


