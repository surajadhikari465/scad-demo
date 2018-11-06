CREATE PROCEDURE  dbo.EPromotions_ShowLockedPromotions
@OfferId int
as
BEGIN
-- return a list of users that are currently editing a specific group.
	SELECT IsEdited, FullName  
	FROM PromotionalOffer po LEFT JOIN Users u on u.User_Id = po.IsEdited 
	WHERE Offer_Id = @OfferId AND u.FullName IS NOT NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ShowLockedPromotions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ShowLockedPromotions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ShowLockedPromotions] TO [IRMAReportsRole]
    AS [dbo];

