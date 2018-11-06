 /****** Object:  StoredProcedure [dbo].[EPromotions_ReturnPendingPriceBatchDetailCount]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ReturnPendingPriceBatchDetailCount]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_ReturnPendingPriceBatchDetailCount]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_ReturnPendingPriceBatchDetailCount]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_ReturnPendingPriceBatchDetailCount
	@OfferID int,
	@count int OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

	-- Returns count of Pending Price Batch Detail records, with "Pending" being defined as having a 
	-- non-NULL PriceBatchHeader (indicating that it has been batched) and the assciated PriceBatchHeader has 
	-- a PricebatchStatus of 6 (Processed)


	Select @count = Count(PriceBatchDetailID)
	FROM PriceBatchDetail PBD
	INNER JOIN PriceBatchHeader PBH
	ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	Where  Offer_Id = @OfferID
	AND PBH.PriceBatchStatusId <> 6 --Processed


    SET NOCOUNT OFF

END


GO
  