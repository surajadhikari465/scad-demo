SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCycleCountItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCycleCountItem]
GO


CREATE PROCEDURE dbo.InsertCycleCountItem
	@CycleCountID int
	,@Item_Key int
	,@InvLocID int = null
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SET @Error_No = 0

    BEGIN TRAN

	-- If an InvLoc_ID was passed in, see if the item exists in the InventoryLocationItems  file.
	IF  (@InvLocID IS NOT NULL)
	BEGIN
		IF NOT EXISTS (SELECT * FROM InventoryLocationItems (nolock) WHERE InvLocID = @InvLocID AND Item_Key = @Item_Key)
		BEGIN
			-- Item does not exist in the InventoryLocationItem file, so add it.
			EXEC InsertInventoryLocationItem @InvLocID, @Item_Key
			SELECT @Error_No = @@Error
		END	
	END

	IF @Error_No = 0
	BEGIN
		-- Now insert the item into the CycleCountItems.
		INSERT INTO
			 CycleCountItems (CycleCountID, Item_Key)
		SELECT
			 @CycleCountID, Item_Key 
		FROM 
			Item 
		WHERE
			Item.Item_Key = @Item_Key
			AND Deleted_Item = 0 
			AND NOT (Item_Key IN (SELECT Item_Key FROM CycleCountItems WHERE CycleCountID = @CycleCountID))
		SELECT @Error_No = @@Error
	END

    SET NOCOUNT OFF
	
	IF @Error_No = 0
        COMMIT TRAN
	ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('InsertCycleCountItem failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

