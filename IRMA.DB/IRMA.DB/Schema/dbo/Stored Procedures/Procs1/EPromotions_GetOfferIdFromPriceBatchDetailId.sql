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
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetOfferIdFromPriceBatchDetailId] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetOfferIdFromPriceBatchDetailId] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetOfferIdFromPriceBatchDetailId] TO [IRMAReportsRole]
    AS [dbo];

