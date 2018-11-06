/*
	Retreives records from PromotionalOffers which have the specified GroupID as a member.
*/
CREATE  PROCEDURE dbo.EPromotions_GetOffersByGroupID 
	@GroupID as integer
AS

	SELECT POM.[Offer_ID]
      ,[Description]
      ,[PricingMethod_ID]
      ,[StartDate]
      ,[EndDate]
      ,[RewardType]
      ,[RewardQuantity]
      ,[RewardAmount]
      ,[RewardGroupID]
      ,[createdate]
      ,[modifieddate]
      ,POM.[User_ID]
      ,[ReferenceCode]
      ,[TaxClass_ID]
      ,[SubTeam_No]
      ,[IsEdited]
	FROM [PromotionalOfferMembers] POM
	INNER JOIN [PromotionalOffer] PO
	ON POM.Offer_ID = PO.Offer_ID
	WHERE [Group_ID] = @GroupID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetOffersByGroupID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetOffersByGroupID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetOffersByGroupID] TO [IRMAReportsRole]
    AS [dbo];

