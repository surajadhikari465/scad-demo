-- This stored procedure is used to delete a PBD record.  If this is a price change, the pending change is 
-- completely deleted.  If this is an item change, the pending change is removed from any batches it is 
-- assigned to, but it is not deleted completely because the item updates have already been made in IRMA so
-- they must be communicated to the external systems.
CREATE PROCEDURE dbo.DeletePriceBatchDetail
    @PriceBatchDetailID int
AS

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

    DECLARE @PriceBatchHeaderID int, @Item_Key int, @Store_No int, @StartDate smalldatetime, @EndDate smalldatetime,
            @PriceChgTypeID tinyint

    SELECT @PriceBatchHeaderID = PriceBatchHeaderID,
           @PriceChgTypeID = PriceChgTypeID
    FROM PriceBatchDetail
    WHERE PriceBatchDetailID = @PriceBatchDetailID

    SELECT @error_no = @@ERROR

    IF (@error_no = 0) AND (@PriceBatchHeaderID IS NOT NULL)
    BEGIN
 		-- Removes the change from the batch, and deletes the batch header record if there are no longer any
		-- changes assigned to it.
       EXEC UpdatePriceBatchDetailCutHeader @PriceBatchDetailID
    
        SELECT @error_no = @@ERROR
    END

    IF (@error_no = 0) AND (@PriceChgTypeID IS NOT NULL)
    BEGIN
		-- Deletes the pending change completely if this is a price change.
        DELETE PriceBatchDetail WHERE PriceBatchDetailID = @PriceBatchDetailID
            
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
        RAISERROR ('DeletePriceBatchDetail failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePriceBatchDetail] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePriceBatchDetail] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePriceBatchDetail] TO [IRMAReportsRole]
    AS [dbo];

