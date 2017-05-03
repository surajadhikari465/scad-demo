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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPricingMethodInfo] TO [IRMAReportsRole]
    AS [dbo];

