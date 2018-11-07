/*
	Retreives records from  PromotionalOfferMembers.
	If @OfferId < 0, all are returned. Otherwise, the results are filtered
	by the value of OfferID.
*/
CREATE  PROCEDURE dbo.EPromotions_GetPromotionalOfferMembers 
	@OfferID as integer
AS

	/*
		Returns records from table PromotionalOfferMembers.
		If OfferID < 0, returns all records that match the other criteria
		If OfferID >= 0, the requested record(s) matching this criteria is returned

		The results are returned sorted by JoinLogic 
	*/

	SELECT [OfferMember_ID], [Offer_ID], POM.[Group_ID] as [GroupID], IG.[GroupName], [Quantity], [Purpose], 
	[JoinLogic],[modified], POM.[User_ID] 
	FROM [PromotionalOfferMembers] POM
	INNER JOIN [ItemGroup] IG
	ON POM.Group_ID = IG.Group_ID
	WHERE (@OfferId < 0 OR [Offer_ID] = @OfferID)
	ORDER BY [JoinLogic], [Offer_ID]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOfferMembers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOfferMembers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetPromotionalOfferMembers] TO [IRMAReportsRole]
    AS [dbo];

