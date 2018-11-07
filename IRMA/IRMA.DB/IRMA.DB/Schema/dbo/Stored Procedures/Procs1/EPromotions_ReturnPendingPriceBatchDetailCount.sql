CREATE PROCEDURE  dbo.EPromotions_ReturnPendingPriceBatchDetailCount
	@OfferID int,
	@count int OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

	-- Returns count of Pending Price Batch Detail records, with "Pending" being defined as having a 
	-- non-NULL PriceBatchHeader (indicating that it has been batched) and the assciated PriceBatchHeader has 
	-- a PricebatchStatus of 6 (Processed)


	Select @count = Count(PriceBatchDetailID)
	FROM PriceBatchDetail PBD
	INNER JOIN PriceBatchHeader PBH
	ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	Where  Offer_Id = @OfferID
	AND PBH.PriceBatchStatusId <> 6 --Processed


    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ReturnPendingPriceBatchDetailCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ReturnPendingPriceBatchDetailCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ReturnPendingPriceBatchDetailCount] TO [IRMAReportsRole]
    AS [dbo];

