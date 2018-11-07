/*
	Retreives records from  PromotionOffers by PricingMethod
*/

CREATE  PROCEDURE dbo.EPromotions_GetPromotionalOffersByPricingMethod
	@PricingMethodId  as Integer
AS
	BEGIN
		SELECT [Offer_ID], [Description], [PricingMethod_ID], [StartDate], [EndDate], [RewardType], [RewardQuantity], [RewardAmount], [RewardGroupID], [createdate], [modifieddate], [User_ID],  [ReferenceCode], [TaxClass_ID], [SubTeam_No], [IsEdited]
		FROM [PromotionalOffer]
		WHERE (PricingMethod_Id = @PricingMethodId ) 
		ORDER BY [Description] 
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOffersByPricingMethod] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOffersByPricingMethod] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOffersByPricingMethod] TO [IRMAReportsRole]
    AS [dbo];

