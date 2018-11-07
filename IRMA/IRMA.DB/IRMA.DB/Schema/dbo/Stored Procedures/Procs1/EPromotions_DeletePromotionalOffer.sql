CREATE  PROCEDURE dbo.EPromotions_DeletePromotionalOffer
	@OfferID int	
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- delete Promotional Offer members
    DELETE FROM PromotionalOfferMembers
    WHERE Offer_ID = @OfferID

	SELECT @error_no = @@ERROR

	-- delete Promotional Offer Store associations
    DELETE FROM PromotionalOfferStore
    WHERE Offer_ID = @OfferID

	SELECT @error_no = @@ERROR
	

	If @error_no = 0
	BEGIN
	    -- delete Promotional Offer
		DELETE FROM PromotionalOffer
		WHERE Offer_ID = @OfferID

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
        RAISERROR ('EPromotions_DeletePromotionalOffer failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeletePromotionalOffer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeletePromotionalOffer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeletePromotionalOffer] TO [IRMAReportsRole]
    AS [dbo];

