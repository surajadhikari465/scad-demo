SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.UpdatePriceBatchDetailCutHeader') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.UpdatePriceBatchDetailCutHeader
GO


-- This stored procedure is used to remove a single batch detail record from a batch that is in the
-- building or packaged state.  It leaves the item and price changes in place for assignment to another
-- batch.
-- If this was the last detail record assigned to the batch, then the batch header record is deleted.
CREATE PROCEDURE dbo.UpdatePriceBatchDetailCutHeader
    @PriceBatchDetailID int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

    DECLARE @PriceBatchHeaderID int, @Item_Key int, @Store_No int

    SELECT @PriceBatchHeaderID = PriceBatchHeaderID,
           @Item_Key = Item_Key,
           @Store_No = Store_No
    FROM PriceBatchDetail
    WHERE PriceBatchDetailID = @PriceBatchDetailID

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
		-- Unpackage the PBD record.  This will restore the data for the ITEM and PRICE change 
		-- PBD records as they were when the batch was in the Building state.  The PBD records are
		-- still assigned to the batch header after this step is completed.
        EXEC UpdatePriceBatchUnpackage @PriceBatchHeaderID, @PriceBatchDetailID
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- Remove the PBD record from the batch.  This makes the change availalbe for assignment to
		-- another batch.  This is done using the item-store data instead of the @PriceBatchDetailID
		-- because the unpackage step can result in separate item and price records if they were
		-- combined into one at the time of packaging.
        UPDATE PriceBatchDetail
        SET PriceBatchHeaderID = NULL
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
            AND Item_Key = @Item_Key AND Store_No = @Store_No
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        -- If the header is now empty, delete it
        DELETE PriceBatchHeader
        FROM PriceBatchHeader PBH
        WHERE PBH.PriceBatchHeaderID = @PriceBatchHeaderID
            AND NOT EXISTS (SELECT * FROM PriceBatchDetail D WHERE D.PriceBatchHeaderID = @PriceBatchHeaderID)
    
        SELECT @error_no = @@ERROR
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdatePriceBatchDetailCutHeader failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

