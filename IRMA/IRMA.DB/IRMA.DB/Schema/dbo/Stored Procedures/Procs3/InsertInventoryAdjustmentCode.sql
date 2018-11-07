CREATE PROCEDURE [dbo].[InsertInventoryAdjustmentCode] 

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
	
	INSERT INTO [dbo].[InventoryAdjustmentCode] 
		(
		Abbreviation,
		AdjustmentDescription,
		AllowsInventoryAdd,
		AllowsInventoryDelete,
		AllowsInventoryReset,
		GLAccount,
		ActiveForWarehouse,
		ActiveForStore,
		Adjustment_Id,
		LastUpdateUser_Id,
		LastUpdateDateTime
		)
	VALUES
		(
		@strAbbreviation,
		@strAdjustmentDescription,
		@blnAllowsInventoryAdd,
		@blnAllowsInventoryDelete,
		@blnAllowsInventoryReset,
		@intGLAccount,
		@blnActiveForWarehouse,
		@blnActiveForStore,
		@intAdjustment_Id,
		@intLastUpdateUser_Id,
		@dtmLastUpdateDateTime
		)
		
    SET NOCOUNT OFF	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInventoryAdjustmentCode] TO [IRMAClientRole]
    AS [dbo];

