



CREATE  PROCEDURE dbo.EPromotions_GetAvailableItemGroups
	@OfferID int	
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- Get ItemGroups that are not already assigned to the Promotion.
	Select *  FROM ItemGroup
	  WHERE Group_Id Not IN (Select Group_Id from PromotionalOfferMemebres where OfferId = @OfferId)

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
        RAISERROR ('EPromotions_GetAvailableItemGroupsr failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetAvailableItemGroups] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetAvailableItemGroups] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetAvailableItemGroups] TO [IRMAReportsRole]
    AS [dbo];

