SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCycleCountItemsZeroCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCycleCountItemsZeroCount]
GO


CREATE PROCEDURE dbo.InsertCycleCountItemsZeroCount
	@CycleCountID int
	,@SubTeam_No int
	,@Store_No int
	,@InvLocID int = null
AS 

DECLARE @Error_No int
SELECT @Error_No = 0

BEGIN TRAN

	IF @InvLocID IS NOT NULL
	BEGIN
		-- Insert Items into the InventoryLocationItems table (if they are not already there).
		INSERT INTO
		 	InventoryLocationItems (InvLocID, Item_Key)
		SELECT
			@InvLocID, Item.Item_Key
		FROM
			OnHand 
		RIGHT JOIN 
			Item ON (OnHand.Item_Key = Item.Item_Key)
		WHERE
			ISNULL(OnHand.SubTeam_No, Item.SubTeam_No) = @SubTeam_No 
			AND ISNULL(OnHand.Store_No, @Store_No) = @Store_No
			AND ISNULL((Quantity + Weight), 0) <= 0
			AND Deleted_Item = 0 
			AND NOT (Item.Item_Key IN (SELECT Item_Key FROM InventoryLocationItems WHERE InvLocID = @InvLocID))
            AND EXISTS (SELECT * FROM StoreItemVendor SIV (nolock) WHERE SIV.Store_No = @Store_No AND SIV.Item_Key = Item.Item_Key AND DeleteDate IS NULL)

		SELECT @Error_No = @@ERROR
	END

	IF @Error_No = 0 
	BEGIN
		-- Insert Items into the CycleCountItems table (if they are not already there).
 		INSERT INTO
			 CycleCountItems (CycleCountID, Item_Key)
		SELECT 
			@CycleCountID, Item.Item_Key 
		FROM
			OnHand 
		RIGHT JOIN 
			Item ON OnHand.Item_Key = Item.Item_Key
		WHERE 
			ISNULL(OnHand.SubTeam_No, Item.SubTeam_No) = @SubTeam_No 
			AND ISNULL(OnHand.Store_No, @Store_No) = @Store_No
			AND ISNULL((Quantity + Weight), 0) <= 0
			AND Deleted_Item = 0 
			AND NOT (Item.Item_Key IN (SELECT Item_Key FROM CycleCountItems WHERE CycleCountID = @CycleCountID))
            AND EXISTS (SELECT * FROM StoreItemVendor SIV (nolock) WHERE SIV.Store_No = @Store_No AND SIV.Item_Key = Item.Item_Key AND DeleteDate IS NULL)

        SELECT @Error_No = @@ERROR
	END
	
	IF @Error_No = 0
		COMMIT TRAN
	ELSE
	BEGIN
		ROLLBACK TRAN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
		RAISERROR ('InsertCycleCountItemsCategory failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

