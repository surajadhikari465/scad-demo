 /****** Object:  StoredProcedure [dbo].[EPromotions_GetPriceBatchDetailCountByOfferID]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_GetPriceBatchDetailCountByOfferID]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_GetPriceBatchDetailCountByOfferID]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_GetPriceBatchDetailCountByOfferID]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_GetPriceBatchDetailCountByOfferID
	@OfferID int,
	@FilterNullHeaderIDs bit,
	@count int OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

	Select @count = Count(PriceBatchDetailID)
	FROM PriceBatchDetail
	Where  Offer_Id = @OfferID
	AND (@FilterNullHeaderIDs = 0 OR (NOT(PriceBatchHeaderID Is NULL)))


    SET NOCOUNT OFF

END


GO
 