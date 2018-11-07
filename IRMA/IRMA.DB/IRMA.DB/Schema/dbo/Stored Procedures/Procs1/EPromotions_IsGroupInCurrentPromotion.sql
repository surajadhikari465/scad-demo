 CREATE PROCEDURE dbo.EPromotions_IsGroupInCurrentPromotion
	@GroupId int,
	@OfferId int
AS
BEGIN

	SET NOCOUNT ON;
	
	select case when exists (
		select * from PromotionalOfferMembers 
		where Group_Id = @GroupId and Offer_Id= @OfferId
	) then 'True' else 'False' end as IsGroupInCurrentPromotion


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_IsGroupInCurrentPromotion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_IsGroupInCurrentPromotion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_IsGroupInCurrentPromotion] TO [IRMAReportsRole]
    AS [dbo];

