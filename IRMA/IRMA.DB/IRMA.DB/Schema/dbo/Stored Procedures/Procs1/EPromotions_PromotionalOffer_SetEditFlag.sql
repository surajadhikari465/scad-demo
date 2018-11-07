CREATE PROCEDURE  dbo.EPromotions_PromotionalOffer_SetEditFlag
	@OfferID int,
	@Value int
AS 

BEGIN
    SET NOCOUNT ON


    DECLARE @error_no int
    SELECT @error_no = 0

	UPDATE PromotionalOffer SET IsEdited = @Value
	WHERE Offer_ID = @OfferID

	SELECT @error_no = @@ERROR


    SET NOCOUNT OFF

    IF @error_no > 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_PromotionalOffer_SetEditFlag failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_PromotionalOffer_SetEditFlag] TO [IRMAClientRole]
    AS [dbo];

