CREATE PROCEDURE  dbo.EPromotions_InsertPromotionalOffer
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
	@SubTeam_No int,
	@OfferID int OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

	-- Make sure no existing offer has the same description - return error if it does
	Select @error_no = COUNT(Offer_ID)* -1
	FROM PromotionalOffer
	WHERE [Description] = @Description

    -- insert new Promotional Offer
    IF (@error_no = 0)
    BEGIN
	INSERT INTO PromotionalOffer
		([Description],
		PricingMethod_ID ,
		StartDate ,
		EndDate ,
		RewardType ,
		RewardQuantity ,
		RewardAmount ,
		RewardGroupID ,
		CreateDate ,
		ModifiedDate ,
		User_ID,
		ReferenceCode,
		TaxClass_ID,
		SubTeam_No )
	VALUES (@Description ,
		@PricingMethodID ,
		@StartDate ,
		@EndDate ,
		@RewardType ,
		@RewardQuantity ,
		@RewardAmount ,
		@RewardGroupID ,
		GETDATE() ,
		GETDATE() ,
		@UserID,
		@ReferenceCode,
		@TaxClass_ID,
		@SubTeam_No )
		
	SELECT @error_no = @@ERROR, @OfferID = SCOPE_IDENTITY()
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
	BEGIN
		DECLARE @Severity smallint
		If @error_no < 0 
			-- User defined error
			RAISERROR ('EPromotions_InsertPromotionalOffer failed: ''Description'' must be unique. ', 15, 1, @error_no)
		Else
		BEGIN
			IF @@TRANCOUNT <> 0
				ROLLBACK TRAN
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('EPromotions_InsertPromotionalOffer failed with @@ERROR: %d', @Severity, 1, @error_no)
		END
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertPromotionalOffer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertPromotionalOffer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertPromotionalOffer] TO [IRMAReportsRole]
    AS [dbo];

