

/*
	Retreives records from  PricingMethods.
	If @PricingMethodID < 0, all are returned. Otherwise, the results are filtered
	by the value of PricingMethodID.
*/
CREATE  PROCEDURE dbo.EPromotions_GetPricingMethods 
	@PricingMethodID as Integer,
	@OfferEditorOnly as Bit
AS

	SELECT [PricingMethod_ID], [PricingMethod_Name], [EnableOfferEditor]
	FROM [PricingMethod]
	WHERE (@PricingMethodID < 0 OR [PricingMethod_ID] = @PricingMethodID)
	AND (@OfferEditorOnly = 0 or [EnableOfferEditor] = 1)
	ORDER BY PricingMethod_Name


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPricingMethods] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPricingMethods] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPricingMethods] TO [IRMAReportsRole]
    AS [dbo];

