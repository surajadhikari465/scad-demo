CREATE PROCEDURE [dbo].[UpdateInventoryAdjustmentCode] 

	@strAbbreviation					char(2),
	@strAdjustmentDescription			varchar(255),
	@blnAllowsInventoryAdd				bit,
	@blnAllowsInventoryDelete			bit,
	@blnAllowsInventoryReset			bit,
	@intGLAccount						int,
	@blnActiveForWarehouse				bit,
	@blnActiveForStore					bit,
	@intAdjustment_Id					int,
	@intLastUpdateUser_Id				int,
	@dtmLastUpdateDateTime				datetime
AS

BEGIN

    SET NOCOUNT ON
	
	UPDATE [dbo].[InventoryAdjustmentCode] 
	SET 
		Abbreviation = @strAbbreviation,
		AdjustmentDescription = @strAdjustmentDescription,
		AllowsInventoryAdd = @blnAllowsInventoryAdd,
		AllowsInventoryDelete = @blnAllowsInventoryDelete,
		AllowsInventoryReset = @blnAllowsInventoryReset,
		GLAccount = @intGLAccount,
		ActiveForWarehouse = @blnActiveForWarehouse,
		ActiveForStore = @blnActiveForStore,
		Adjustment_Id = @intAdjustment_Id,
		LastUpdateUser_Id = @intLastUpdateUser_Id,
		LastUpdateDateTime = @dtmLastUpdateDateTime
	WHERE 
		Abbreviation = @strAbbreviation		
		
    SET NOCOUNT OFF	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInventoryAdjustmentCode] TO [IRMAClientRole]
    AS [dbo];

