CREATE PROCEDURE  dbo.EPromotions_ShowLockedGroups
@OfferId int
as

BEGIN
-- Return a list of users that are editing any group in a specific promotion.

SELECT IsEdited = ISNULL(IsEdited,0), FullName  
FROM promotionaloffermembers  pom INNER JOIN itemgroup ig ON ig.group_id = pom.group_id  LEFT JOIN users u ON  u.user_id = ig.isedited 
WHERE offer_id = 47 AND u.fullname IS NOT NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ShowLockedGroups] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ShowLockedGroups] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ShowLockedGroups] TO [IRMAReportsRole]
    AS [dbo];

