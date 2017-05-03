IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllOnHandDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetAllOnHandDetail]
GO

/****** Object:  StoredProcedure [dbo].[GetAllOnHandDetail]    Script Date: 07/25/2012 14:57:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetAllOnHandDetail]  
    @Item_Key int
AS

/*
################################################################
Update History

TFS #		Date		Who			Comments
TFS 11692	2/2/10		Tom Lux		Changed checks against the ItemHistory.Quantity field in the SELECT and HAVING clauses
									from ">" to "<>" so that negative values are included.
n/a			10/01/2012	Damon F		Removed all references to ItemCaseHistory.

################################################################
*/

--DECLARE @Item_Key INT
--SELECT @Item_Key = 178491

SELECT 
            Store.Store_No,
            ItemHistory.Item_Key, 
            Package_Desc1 Pack,
			SUM(
				CASE WHEN ISNULL(ItemHistory.Quantity, 0) <> 0 
					THEN ItemHistory.Quantity / CASE WHEN Package_Desc1 <> 0 
													THEN Package_Desc1 
												ELSE 1 
												END
                ELSE ISNULL(ItemHistory.Weight, 0) / CASE WHEN Package_Desc1 * Package_Desc2 <> 0 
														THEN (Package_Desc1 * Package_Desc2) 
													ELSE 1 
													END 
				END * ItemAdjustment.Adjustment_Type
				) AS OnHand,
            dbo.fn_GetRetailUnitAbbreviation(@Item_Key) As PackUOM,
            dbo.fn_GetDistributionUnitAbbreviation(@Item_Key) As QuantityUOM,
            ISNULL(ItemHistory.SubTeam_No, Item.SubTeam_No) As SubTeam_No
    FROM OnHand (nolock)
    CROSS JOIN 
            Store
    INNER JOIN
        ItemHistory (nolock) 
        ON ItemHistory.Item_Key = OnHand.Item_Key AND ItemHistory.Store_No = OnHand.Store_No AND ItemHistory.SubTeam_No = OnHand.SubTeam_No
    INNER JOIN 
        ItemAdjustment (nolock)
        ON ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID
    INNER JOIN 
        Item (nolock)
        ON Item.Item_Key = OnHand.Item_Key
    INNER JOIN 
        ItemIdentifier (nolock)  
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1   
    WHERE OnHand.Store_No = Store.Store_No
            AND OnHand.SubTeam_No = ISNULL(ItemHistory.SubTeam_No, Item.SubTeam_No)
            AND OnHand.Item_Key = @Item_Key
        AND ItemHistory.DateStamp >= ISNULL(OnHand.LastReset, ItemHistory.DateStamp)
        AND Deleted_Item = 0
    GROUP BY Store.Store_No, ItemHistory.Item_Key,  Package_Desc1, ISNULL(ItemHistory.SubTeam_No, Item.SubTeam_No)
    HAVING SUM(
			CASE WHEN ISNULL(ItemHistory.Quantity, 0) <> 0 
				THEN ItemHistory.Quantity / CASE WHEN Package_Desc1 <> 0 
												THEN Package_Desc1 
											ELSE 1 
											END
            ELSE ISNULL(ItemHistory.Weight, 0) / CASE WHEN Package_Desc1 * Package_Desc2 <> 0 
													THEN (Package_Desc1 * Package_Desc2) 
												ELSE 1 
												END 
			END * ItemAdjustment.Adjustment_Type) <> 0
