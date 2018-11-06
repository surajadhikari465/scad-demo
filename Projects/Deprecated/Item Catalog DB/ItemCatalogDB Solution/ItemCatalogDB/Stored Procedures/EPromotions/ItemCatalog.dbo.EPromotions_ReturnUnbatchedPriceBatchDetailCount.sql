  /****** Object:  StoredProcedure [dbo].[EPromotions_ReturnUnbatchedPriceBatchDetailCount]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ReturnUnbatchedPriceBatchDetailCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_ReturnUnbatchedPriceBatchDetailCount]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_ReturnUnbatchedPriceBatchDetailCount]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_ReturnUnbatchedPriceBatchDetailCount
	@OfferID int,
	@StoreID int,
	@count int OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

	-- Returns count of Unbatched Price Batch Detail records, with "unbatched" being defined as having a NULL PriceBatchHeaderID

	Select @count = Count(PriceBatchDetailID)
	FROM PriceBatchDetail PBD (nolock)
	WHERE PriceBatchHeaderID Is Null
	AND (@OfferID = 0 OR PBD.Offer_ID = @OfferID)
	AND (@StoreID = 0 OR PBD.Store_No = @StoreID)


    SET NOCOUNT OFF

END


GO
  