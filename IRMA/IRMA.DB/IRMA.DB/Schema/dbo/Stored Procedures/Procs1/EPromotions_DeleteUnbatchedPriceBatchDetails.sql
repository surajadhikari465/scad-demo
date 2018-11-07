CREATE  PROCEDURE dbo.EPromotions_DeleteUnbatchedPriceBatchDetails
	@OfferID int,
	@StoreID int,
	@ItemID int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- delete Unbatched Price Batch details By Offer and Item
    DELETE FROM PriceBatchDetail
    WHERE Offer_ID = @OfferID
    AND (@ItemID = 0 OR Item_Key = @ItemID)
    AND (@StoreID = 0 OR Store_No = @StoreID)
    AND PriceBatchHeaderId Is Null

    SELECT @error_no = @@ERROR

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_DeleteUnbatchedPriceBatchDetails failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeleteUnbatchedPriceBatchDetails] TO [IRMAClientRole]
    AS [dbo];

