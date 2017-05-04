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
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ReturnUnbatchedPriceBatchDetailCount] TO [IRMAClientRole]
    AS [dbo];

