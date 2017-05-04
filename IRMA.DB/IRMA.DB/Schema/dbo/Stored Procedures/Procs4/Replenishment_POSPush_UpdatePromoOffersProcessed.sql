CREATE PROCEDURE dbo.Replenishment_POSPush_UpdatePromoOffersProcessed
    @PriceBatchHeaderID int,
    @POSBatchID int    
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN
    
    UPDATE PriceBatchHeader 
    SET PriceBatchStatusID = 6,
        ProcessedDate = GETDATE(),
        POSBatchID = @POSBatchID
    WHERE PriceBatchHeaderID = @PriceBatchHeaderID

    SELECT @error_no = @@ERROR

	-- UPDATE PromotionalOfferStore table to reflect that Offer/Store combo is now ACTIVE on the POS system
	IF @error_no = 0
	BEGIN
		UPDATE PromotionalOfferStore
		SET Active = 1
		FROM PromotionalOfferStore POS
		INNER JOIN
			PriceBatchDetail PBD
			ON PBD.Offer_ID = POS.Offer_ID
				AND PBD.Store_No = POS.Store_No
		INNER JOIN
			PriceBatchHeader PBH
			ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
		WHERE PBH.PriceBatchHeaderID = @PriceBatchHeaderID
			
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
        RAISERROR ('UpdatePromoOffersProcessed failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePromoOffersProcessed] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePromoOffersProcessed] TO [IRMAClientRole]
    AS [dbo];

