 
  /****** Object:  StoredProcedure [dbo].[EPromotions_GetPriceBatchDetailsByOfferItem]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_GetPriceBatchDetailsByOfferItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_GetPriceBatchDetailsByOfferItem]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_GetPriceBatchDetailsByOfferItem]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_GetPriceBatchDetailsByOfferItem
	@OfferID int,
	@ItemID int,
	@Unbatched bit		-- boolean that tells whether to return only unbatched details or all details
AS 

BEGIN
    SET NOCOUNT ON

	Select *
	FROM PriceBatchDetail (nolock)
	Where  Offer_Id = @OfferID
	AND Item_Key = @ItemID
	AND (@Unbatched= 0 OR (NOT(PriceBatchHeaderID Is NULL)))


    SET NOCOUNT OFF

END


GO
 