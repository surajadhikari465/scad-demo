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
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ReturnStoreActiveFlag] TO [IRMAClientRole]
    AS [dbo];

