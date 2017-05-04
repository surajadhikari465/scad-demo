 
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetPricingMethodInfo]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetPricingMethodInfo]
GO 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.GetPricingMethodInfo
AS
BEGIN
	SET NOCOUNT ON;
	-- return the properties of the PricingMethods that are available to the Promo Screen
	SELECT 
			[PricingMethod_ID]
		  ,[PricingMethod_Name]
		  ,[EnableOfferEditor]
		  ,[EnablePromoScreen]
		  ,[EnableSaleMultiple]
		  ,[EnableEarnedRegMultiple]
		  ,[EarnedRegMultipleDefault]
		  ,[EnableEarnedSaleMultiple]
		  ,[EarnedSaleMultipleDefault]
		  ,[EnableEarnedLimit]
		  ,[EarnedLimitDefault]
	FROM [PricingMethod]
	WHERE ([EnablePromoScreen] = 1)
	ORDER BY [PricingMethod_ID]

END
GO