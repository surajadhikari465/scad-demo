CREATE PROCEDURE dbo.GetInventoryAdjustmentAllows
	(@InventoryAdjustmentCode_ID int)
AS 

SELECT [AllowsInventoryAdd]
      ,[AllowsInventoryDelete]
      ,[AllowsInventoryReset]
  FROM [dbo].[InventoryAdjustmentCode]
WHERE InventoryAdjustmentCode_ID = @InventoryAdjustmentCode_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentAllows] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentAllows] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentAllows] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentAllows] TO [IRMAReportsRole]
    AS [dbo];

