SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckCostChanges]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckCostChanges]
GO

CREATE PROCEDURE dbo.CheckCostChanges
    @OrderHeader_ID int,
    @RecordCount int = NULL OUTPUT		-- COUNT OF THE NUMBER OF RECORDS BEING RETURNED
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @Vendor_ID int, @Store_No int, @OrderDate datetime

    SELECT @Vendor_ID = OrderHeader.Vendor_ID, @Store_No = RecvVend.Store_No, @OrderDate = OrderDate
    FROM OrderHeader (nolock)
    INNER JOIN
        Vendor RecvVend (nolock)
        ON RecvVend.Vendor_ID = OrderHeader.ReceiveLocation_ID
    WHERE OrderHeader_ID = @OrderHeader_ID

    SELECT Store.Store_Name, 
           Item.Item_Description, 
           Identifier, 
           ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight, 0) As ExtCost, 
           OrderItem.UnitExtCost As NewExtCost,
           CASE WHEN ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight, 0) = 0 
                THEN 100.0 
                ELSE ABS((UnitExtCost - (ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight,0))) / (ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight,0))) * 100 END AS CostDiff
    FROM fn_VendorCostItems(@Vendor_ID, @Store_No, @OrderDate) VC RIGHT JOIN (
           Store (nolock) INNER JOIN (
                   OrderHeader (nolock) INNER JOIN (
                     ItemIdentifier (nolock) INNER JOIN (
                       OrderItem (nolock) INNER JOIN Item (nolock) ON (OrderItem.Item_Key = Item.Item_Key) AND OrderItem.DateReceived IS NOT NULL 
                     ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
                   ) ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
           ) ON (Store.Store_No = @Store_No)
         ) ON (VC.Item_Key = OrderItem.Item_Key)
    WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID AND 
          OrderHeader.Return_Order = 0 AND 
          ((UnitExtCost = 0) OR
                      (CASE WHEN ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight, 0) = 0 
                            THEN 100.0 
                            ELSE ABS((UnitExtCost - (ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight,0))) / (ISNULL(VC.UnitCost, 0) + ISNULL(VC.UnitFreight,0))) * 100 END > 50))
                            
	SELECT @RecordCount = @@ROWCOUNT                            

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


