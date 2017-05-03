if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateOrderItemAdjustedCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateOrderItemAdjustedCost]
GO

CREATE PROCEDURE dbo.UpdateOrderItemAdjustedCost
    @OrderItem_ID	int,
    @AdjustedCost	money
AS 
-- **************************************************************************
-- Procedure: UpdateOrderItemAdjustedCost
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/22	KM		3744	Added update history template; coding standards;
-- **************************************************************************
BEGIN

	DECLARE 
		@DefaultCostAdjustmentReason_ID int

	SELECT TOP 1
		@DefaultCostAdjustmentReason_ID = CostAdjustmentReason_ID
	FROM
		CostAdjustmentReason
	WHERE
		IsDefault = 1

    UPDATE 
		OrderItem
    SET
		AdjustedCost			= @AdjustedCost,
		CostAdjustmentReason_ID = @DefaultCostAdjustmentReason_ID
    WHERE 
		OrderItem_ID			= @OrderItem_ID
END
GO