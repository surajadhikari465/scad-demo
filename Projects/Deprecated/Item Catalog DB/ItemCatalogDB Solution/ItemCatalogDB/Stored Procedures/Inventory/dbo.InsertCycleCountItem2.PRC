SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCycleCountItem2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCycleCountItem2]
GO


CREATE PROCEDURE dbo.InsertCycleCountItem2
	@CycleCountID int
	,@Item_Key int
	,@InvLocID int
	,@ScanDateTime datetime
	,@Count decimal(18,4)
	,@Weight decimal(18,4)
	,@PackSize decimal(18,4)
    ,@IsCaseCnt bit
AS 
BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SET @Error_No = 0

    DECLARE @CycleCountItemID int

    BEGIN TRAN

	-- If an InvLoc_ID was passed in, see if the item exists in the InventoryLocationItems  file.
	IF  @InvLocID IS NOT NULL
	BEGIN
		IF NOT EXISTS (SELECT * FROM InventoryLocationItems (nolock) WHERE InvLocID = @InvLocID AND Item_Key = @Item_Key)
		BEGIN
			EXEC InsertInventoryLocationItem @InvLocID, @Item_Key
			SELECT @Error_No = @@Error
		END	
	END

	IF @Error_No = 0
	BEGIN
        SELECT @CycleCountItemID = CycleCountItemID FROM CycleCountItems WHERE CycleCountID = @CycleCountID AND Item_Key = @Item_Key

        IF @CycleCountItemID IS NULL
        BEGIN
    		INSERT INTO CycleCountItems (CycleCountID, Item_Key)
            VALUES (@CycleCountID, @Item_Key)

		    SELECT @Error_No = @@Error, @CycleCountItemID = SCOPE_IDENTITY()
        END
	END

    IF @Error_No = 0
    BEGIN
        INSERT INTO CycleCountHistory (CycleCountItemID, ScanDateTime, [Count], Weight, PackSize, IsCaseCnt)
		VALUES (@CycleCountItemID, CONVERT(varchar, ISNULL(@ScanDateTime, GETDATE()), 120),@Count,@Weight,@PackSize,@IsCaseCnt)
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
		RAISERROR ('InsertCycleCountItem2 failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

