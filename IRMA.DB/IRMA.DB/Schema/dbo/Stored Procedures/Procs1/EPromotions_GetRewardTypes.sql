/*
	Retreives records from RewardType.
*/
CREATE  PROCEDURE [dbo].[EPromotions_GetRewardTypes] 
AS

	SELECT [RewardType_ID], [Reward_Name]
	FROM [RewardType]
	ORDER BY Reward_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetRewardTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetRewardTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetRewardTypes] TO [IRMAReportsRole]
    AS [dbo];

