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
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPriceBatchDetailCountByOfferID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPriceBatchDetailCountByOfferID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPriceBatchDetailCountByOfferID] TO [IRMAReportsRole]
    AS [dbo];

