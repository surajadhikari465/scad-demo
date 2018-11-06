/****** Object:  StoredProcedure [dbo].[EPromotions_PromotionExistence]    Script Date: 05/31/2006 16:02:33 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_PromotionExistence]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_PromotionExistence]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_PromotionExistence]    Script Date: 05/31/2006 16:02:33 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_PromotionExistence]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE  PROCEDURE dbo.EPromotions_PromotionExistence
	@ItemKey int,
	@StoreId int
AS 

BEGIN
    SET NOCOUNT ON


    DECLARE @error_no int
    SELECT @error_no = 0
    
    -- Return 1 if an EPromotion has been created for a particular Item and Store combo, otherwise return 0.
	select case when (select count(PriceBatchDetailId) from PriceBatchDetail where Offer_Id is not null and Item_Key = @ItemKey and Store_No = @StoreId) > 1 then 1 
	else 0 end as PromotionExistence

    SELECT @error_no = @@ERROR

    SET NOCOUNT OFF

    IF @error_no <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR (''EPromotions_PromotionExistence failed with @@ERROR: %d'', @Severity, 1, @error_no)
    END
END' 
END
GO

