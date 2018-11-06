SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCycleCountItemsVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCycleCountItemsVendor]
GO


CREATE PROCEDURE dbo.InsertCycleCountItemsVendor
	@CycleCountID int
	,@SubTeam_No int
	,@Vendor_ID int
	,@InvLocID int = null
AS 

--**************************************************************************
-- Procedure: InsertCycleCountItemsVendor
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
SET NOCOUNT ON

DECLARE @Error_No int

SELECT @Error_No = 0

BEGIN TRAN

    DECLARE @Store_No int

    SELECT @Store_No = Store_No
    FROM CycleCountMaster M (nolock)
    INNER JOIN CycleCountHeader H (nolock) ON H.MasterCountID = M.MasterCountID
    WHERE H.CycleCountID = @CycleCountID

    SELECT @Error_No = @@ERROR

	IF (@InvLocID IS NOT NULL) AND (@Error_No = 0)
	BEGIN
		-- Insert Items into the InventoryLocationItems table (if they are not already there).
		INSERT INTO
			 InventoryLocationItems (InvLocID, Item_Key)
		SELECT
			@InvLocID, Item.Item_Key
		FROM	
			ItemVendor
		INNER JOIN
		  	Item ON ItemVendor.Item_Key = Item.Item_Key
		WHERE
			Item.SubTeam_No = @SubTeam_No 
			AND ItemVendor.Vendor_ID = @Vendor_ID 
			AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, ItemVendor.Vendor_ID) = 0 
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
			ItemVendor 
		INNER JOIN
		  	Item ON ItemVendor.Item_Key = Item.Item_Key
		WHERE 
			Item.SubTeam_No = @SubTeam_No 
			AND ItemVendor.Vendor_ID = @Vendor_ID 
			AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, ItemVendor.Vendor_ID) = 0 
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
		RAISERROR ('InsertCycleCountItemsVendor failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

