CREATE PROCEDURE dbo.EPromotions_UpdatePromotionalOfferMember
	@OfferMemberID int,
	@OfferID int,
	@GroupID int,
	@Quantity decimal,
	@Purpose bit,
	@JoinLogic bit,
	@Modified datetime,
	@UserID int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- Determine if there is a concurrency issue before performing update by comparing the incoming ModifiedDate parameter
    -- with the existing ModifiedDate value in the record
	DECLARE @CurrentModified datetime    
	SELECT @CurrentModified = Modified
	FROM [PromotionalOfferMembers]
    WHERE [OfferMember_ID] = @OfferMemberID
    
	If @CurrentModified <> @Modified
		Select @error_no = 1

    IF @error_no = 0
    BEGIN
		-- Update PromotionOfferMember data
		UPDATE [PromotionalOfferMembers]
		SET [Offer_ID]=@OfferID,
		[Group_ID]=@GroupID,
		[Quantity]= @Quantity, 
		[Purpose]=@Purpose, 
		[JoinLogic]=@JoinLogic, 
		[Modified]=GETDATE(), 
		[User_ID]=@UserID
		WHERE [OfferMember_ID] = @OfferMemberID
		SELECT @error_no = @@ERROR

	END 

	SET NOCOUNT OFF
		
    IF @error_no = 0
	COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
            
        If @error_no = 1
			RAISERROR ('Concurrency Error: Promotional Offer Member Record %d has been updated by another user. Cannot perform update.', 16, 1, @OfferMemberID) 
		ELSE
		BEGIN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('EPromotions_UpdatePromotionalOfferMember failed with @@ERROR: %d', @Severity, 1, @error_no)
		END -- ELSE BEGIN
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdatePromotionalOfferMember] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdatePromotionalOfferMember] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdatePromotionalOfferMember] TO [IRMAReportsRole]
    AS [dbo];

