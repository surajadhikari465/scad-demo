

/*
	Retreives records from  PricingMethods.
	If @OfferID < 0, all are returned. Otherwise, the results are filtered
	by the value of OfferID.
*/

CREATE  PROCEDURE dbo.EPromotions_GetPromotionalOffers 
	@OfferID as Integer
AS
	SELECT [Offer_ID], [Description], [PricingMethod_ID], [StartDate], [EndDate], [RewardType], [RewardQuantity], [RewardAmount], [RewardGroupID], [createdate], [modifieddate], [User_ID], [ReferenceCode], [TaxClass_ID], [SubTeam_No], [isEdited]
	FROM [PromotionalOffer]
	WHERE (@OfferID < 0 OR [Offer_ID] = @OfferID)
	ORDER BY [Description]


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOffers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOffers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOffers] TO [IRMAReportsRole]
    AS [dbo];

