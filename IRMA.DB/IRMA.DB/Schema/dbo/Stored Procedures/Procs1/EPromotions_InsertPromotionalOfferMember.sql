CREATE PROCEDURE  dbo.EPromotions_InsertPromotionalOfferMember
	@OfferID int,
	@GroupID int,
	@Quantity decimal,
	@Purpose bit,
	@JoinLogic bit,
	@Modified datetime,
	@UserID int,
	@OfferMemberID int OUTPUT
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

    -- insert new Promotional Offer
    IF (@error_no = 0)
    BEGIN
	INSERT INTO PromotionalOfferMembers 
		(Offer_ID ,
		Group_ID ,
		Quantity ,
		Purpose ,
		JoinLogic ,
		Modified ,
		User_ID)
	VALUES (@OfferID ,
		@GroupID ,
		@Quantity ,
		@Purpose ,
		@JoinLogic ,
		GETDATE() ,
		@UserID )

	SELECT @error_no = @@ERROR
    END

    SET NOCOUNT OFF

    IF @error_no = 0
    BEGIN
		COMMIT TRAN
		SELECT @OfferMemberID =SCOPE_IDENTITY()
	END
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_InsertPromotionalOfferMember failed with @@ERROR: %d', @Severity, 1, @error_no)
        SELECT @OfferMemberID = 0
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertPromotionalOfferMember] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertPromotionalOfferMember] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertPromotionalOfferMember] TO [IRMAReportsRole]
    AS [dbo];

