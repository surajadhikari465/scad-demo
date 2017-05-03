CREATE PROCEDURE  dbo.EPromotions_GetPriceBatchDetailsByOfferItem
	@OfferID int,
	@ItemID int,
	@Unbatched bit		-- boolean that tells whether to return only unbatched details or all details
AS 

BEGIN
    SET NOCOUNT ON

	Select *
	FROM PriceBatchDetail (nolock)
	Where  Offer_Id = @OfferID
	AND Item_Key = @ItemID
	AND (@Unbatched= 0 OR (NOT(PriceBatchHeaderID Is NULL)))


    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPriceBatchDetailsByOfferItem] TO [IRMAClientRole]
    AS [dbo];

