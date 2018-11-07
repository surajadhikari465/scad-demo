/*
	Retreives records from ItemGroup table.
	If @GroupID < 0, all are returned. Otherwise, the results are filtered
	by the value of OfferID.
*/

CREATE  PROCEDURE dbo.EPromotions_GetItemGroups 
	@GroupID as Integer
AS
	select ig.[Group_ID], [GroupName], [GroupLogic], [createdate], [modifieddate], [User_ID], 
	(select count(distinct Offer_Id) from PromotionalOfferMembers pom where pom.Group_Id= ig.Group_Id) as PromotionCount,
	(select count(distinct POM.Offer_Id) 
	from PromotionalOfferMembers pom 
	INNER Join PriceBatchDetail PBD
	ON POM.Offer_ID = PBD.Offer_ID
	where pom.Group_Id= ig.Group_Id) as ActivePromotionCount,
	(select count(distinct POM.Offer_Id) 
	from PromotionalOfferMembers pom 
	INNER Join PriceBatchDetail PBD
	ON POM.Offer_ID = PBD.Offer_ID
	INNER Join PriceBatchHeader PBH				-- this join filters out unbatched details
	ON PBH.PriceBatchHeaderID = PBD.PriceBatchHeaderID
	where pom.Group_Id= ig.Group_Id
	AND PBH.PriceBatchStatusID <> 6) as PendingPromotionCount 

	FROM [ItemGroup] ig
	WHERE (@GroupID < 0 OR ig.[Group_ID] = @GroupID)
	ORDER BY ig.[GroupName]
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetItemGroups] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetItemGroups] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetItemGroups] TO [IRMAReportsRole]
    AS [dbo];

