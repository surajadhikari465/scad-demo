CREATE  PROCEDURE dbo.EPromotions_DeletePromotionalOfferMembers
	@OfferMemberID int	
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- delete Promotional Offer
    DELETE FROM PromotionalOfferMembers
    WHERE OfferMember_ID = @OfferMemberID

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
        RAISERROR ('EPromotions_DeletePromotionalOfferMembers failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeletePromotionalOfferMembers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeletePromotionalOfferMembers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_DeletePromotionalOfferMembers] TO [IRMAReportsRole]
    AS [dbo];

