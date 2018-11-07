 
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_GetOfferIdFromPriceBatchDetailId]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_GetOfferIdFromPriceBatchDetailId]
GO 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.EPromotions_GetOfferIdFromPriceBatchDetailId
	@DetailId as int
AS
BEGIN

	SET NOCOUNT ON;

	-- return a list of PricingMethods that are excluded from the Epromotions screens.
	-- This is to be used on the Promotion Change Screen.
	SELECT Offer_id
	FROM PriceBatchDetail
	WHERE PriceBatchDetailId = @DetailId
	

END
GO