

CREATE PROCEDURE dbo.EPromotions_UpdatePromotionalOffer
	@OfferID int,
	@Description char(100),
	@PricingMethodID tinyint,
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@RewardType int,
	@RewardQuantity decimal,
	@RewardAmount money,
	@RewardGroupID int,
	@CreateDate smalldatetime,
	@ModifiedDate smalldatetime,
	@UserID int,
	@ReferenceCode varchar(20),
	@TaxClass_ID int,
	@SubTeam_No int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

    -- Determine if there is a concurrency issue before performing update by comparing the incoming ModifiedDate parameter
    -- with the existing ModifiedDate value in the record
	DECLARE @CurrentModified datetime    
	SELECT @CurrentModified = Modifieddate
	FROM [PromotionalOffer]
    WHERE [Offer_ID] = @OfferID
    
	If @CurrentModified <> @ModifiedDate
		Select @error_no = 1

    IF @error_no = 0
    BEGIN
		-- update PromotionOffer data
		UPDATE [PromotionalOffer]
		SET [Description]=@Description,
		[PricingMethod_ID]=@PricingMethodID,
		[StartDate]= @StartDate, 
		[EndDate]=@EndDate, 
		[RewardType]=@RewardType, 
		[RewardQuantity]=@RewardQuantity, 
		[RewardAmount]=@RewardAmount, 
		[RewardGroupID]=@RewardGroupID, 
		[createdate]=GETDATE(), 
		[modifieddate]=GETDATE(), 
		[User_ID]=@UserID,
		[ReferenceCode]=@ReferenceCode,
		[TaxClass_ID]= @TaxClass_ID,
		[SubTeam_No]= @SubTeam_No
		
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

        If @error_no = 1
			RAISERROR ('Concurrency Error: Promotional Offer Record %d has been updated by another user. Cannot perform update.', 16, 1, @OfferID)
		ELSE
		BEGIN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('UpdateTaxFlag failed with @@ERROR: %d', @Severity, 1, @error_no)
		END  -- ELSE BEGIN
    END
END





GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdatePromotionalOffer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdatePromotionalOffer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdatePromotionalOffer] TO [IRMAReportsRole]
    AS [dbo];

