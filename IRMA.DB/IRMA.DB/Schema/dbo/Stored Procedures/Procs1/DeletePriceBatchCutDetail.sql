-- This stored procedure is used to remove all of the batch detail records from a batch that is in the
-- building or packaged state.  It then deletes the batch header record, leaving all of the item and 
-- price changes that were assigned to the batch in place.
CREATE PROCEDURE dbo.DeletePriceBatchCutDetail
    @PriceBatchHeaderID int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

 	-- Move the batch from the Packaged to the Building state.  This will restore the data for the 
	-- ITEM and PRICE change PBD records as they were when the batch was in the Building state.  
	-- The PBD records are still assigned to the batch header after this step is completed.
   EXEC UpdatePriceBatchUnpackage @PriceBatchHeaderID, NULL

    SELECT @error_no = @@ERROR

    IF @error_no = 0
    BEGIN
		-- Unassign all of the PBD records from the batch header.
        UPDATE PriceBatchDetail
        SET PriceBatchHeaderID = NULL
        WHERE PriceBatchHeaderID = @PriceBatchHeaderID
    
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
		-- Deletes the batch header record.
        DELETE FROM PriceBatchHeader WHERE PriceBatchHeaderID = @PriceBatchHeaderID

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
        RAISERROR ('DeletePriceBatchCutDetail failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePriceBatchCutDetail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePriceBatchCutDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePriceBatchCutDetail] TO [IRMAReportsRole]
    AS [dbo];

