CREATE PROCEDURE [dbo].[GetInventoryAdjustmentCode]

	@intInventoryCodeID AS int 
	
AS
BEGIN

	SET NOCOUNT ON
	
	SELECT [InventoryAdjustmentCode_ID]
		  ,[Abbreviation]
		  ,[AdjustmentDescription]
		  ,[AllowsInventoryAdd]
		  ,[AllowsInventoryDelete]
		  ,[GLAccount]
		  ,[Adjustment_Id]
		  ,[LastUpdateUser_Id]
		  ,[LastUpdateDateTime]
		  ,[ActiveForWarehouse]
          ,[ActiveForStore]
	  FROM [dbo].[InventoryAdjustmentCode]
	WHERE [InventoryAdjustmentCode_ID] = @intInventoryCodeID


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryAdjustmentCode] TO [IRMAClientRole]
    AS [dbo];

