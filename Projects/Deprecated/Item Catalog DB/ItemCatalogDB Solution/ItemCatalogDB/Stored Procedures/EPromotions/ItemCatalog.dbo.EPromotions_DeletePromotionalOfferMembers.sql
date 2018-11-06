SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.EPromotions_DeletePromotionalOfferMembers    Script Date: 6/6/2006 5:51:10 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EPromotions_DeletePromotionalOfferMembers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[EPromotions_DeletePromotionalOfferMembers]
GO





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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 