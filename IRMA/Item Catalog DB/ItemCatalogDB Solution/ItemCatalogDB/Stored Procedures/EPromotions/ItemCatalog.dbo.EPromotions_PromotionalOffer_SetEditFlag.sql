 
/****** Object:  StoredProcedure [dbo].[EPromotions_PromotionalOffer_SetEditFlag]    Script Date: 05/31/2006 16:02:48 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_PromotionalOffer_SetEditFlag]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_PromotionalOffer_SetEditFlag]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_PromotionalOffer_SetEditFlag]    Script Date: 05/31/2006 16:02:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
