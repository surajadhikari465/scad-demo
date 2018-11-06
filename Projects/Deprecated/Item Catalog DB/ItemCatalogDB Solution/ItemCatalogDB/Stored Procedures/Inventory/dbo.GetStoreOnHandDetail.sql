IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStoreOnHandDetail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStoreOnHandDetail]
GO

/****** Object:  StoredProcedure [dbo].[GetStoreOnHandDetail]    Script Date: 07/25/2012 14:57:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetStoreOnHandDetail]  
    @Item_Key int,
	@Store_No int,
	@SubTeam_No int
AS 
/**********************************************************************
!!      Procedure:  GetStoreOnHandDetail
!!         Author:  n/a
!!  Creation Date:  n/a
!!
!!  Modification History
!!  ================================================================
!!  Date		Who			Description
!!  10/02/2012	D Floyd		Removed all references to ItemCaseHistory
!!
!!
**********************************************************************/
    SELECT ItemHistory.Item_Key, 
       Package_Desc1                             AS PackSize, 
       Sum(CASE 
             WHEN Isnull(ItemHistory.Quantity, 0) > 0 THEN 
             ItemHistory.Quantity / CASE 
                                      WHEN Package_Desc1 <> 0 THEN Package_Desc1 
                                      ELSE 1 
                                    END 
             ELSE Isnull(ItemHistory.Weight, 0) / CASE WHEN Package_Desc1 * Package_Desc2 <> 0 
                                                    THEN (Package_Desc1 * Package_Desc2 ) 
													ELSE 1 
                                                  END 
           END * ItemAdjustment.Adjustment_Type) AS OnHand 
FROM   OnHand (nolock) 
       INNER JOIN ItemHistory (nolock) 
               ON ItemHistory.Item_Key = OnHand.Item_Key 
                  AND ItemHistory.Store_No = OnHand.Store_No 
                  AND ItemHistory.SubTeam_No = OnHand.SubTeam_No 
       INNER JOIN ItemAdjustment (nolock) 
               ON ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID 
       INNER JOIN Item (nolock) 
               ON Item.Item_Key = OnHand.Item_Key 
       INNER JOIN ItemIdentifier (nolock) 
               ON ItemIdentifier.Item_Key = Item.Item_Key 
                  AND ItemIdentifier.Default_Identifier = 1 
WHERE  OnHand.Store_No = @Store_No 
       AND OnHand.SubTeam_No = @SubTeam_No 
       AND OnHand.Item_Key = @Item_Key 
       AND ItemHistory.DateStamp >= Isnull(OnHand.LastReset, ItemHistory.DateStamp) 
       AND Deleted_Item = 0 
GROUP  BY ItemHistory.Item_Key, Package_Desc1 
    -- Bug 13787: removed the having condition as it was limiting records to be returned when onhand was 0
	-- we still want to return a row back to let the user edit the quantity onhand from 0 to a value