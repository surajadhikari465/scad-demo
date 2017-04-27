  /****** Object:  StoredProcedure [dbo].[EPromotions_ReturnStoreActiveFlag]    Script Date: 05/31/2006 16:02:45 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[EPromotions_ReturnStoreActiveFlag]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[EPromotions_ReturnStoreActiveFlag]
GO
/****** Object:  StoredProcedure [dbo].[EPromotions_ReturnStoreActiveFlag]    Script Date: 05/31/2006 16:02:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  dbo.EPromotions_ReturnStoreActiveFlag
	@OfferID int,
	@StoreID int,
	@Active bit OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

	-- Returns value of active falg for given Store/Offer pair

	Select @Active = active
	FROM PromotionalOfferStore POS (nolock)
	WHERE Offer_ID = @OfferID
	AND Store_No = @StoreID


    SET NOCOUNT OFF

END


GO
   