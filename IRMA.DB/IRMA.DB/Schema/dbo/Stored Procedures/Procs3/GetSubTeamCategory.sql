CREATE PROCEDURE dbo.GetSubTeamCategory 
	@SubTeam_No int
AS 

SET NOCOUNT ON

SELECT DISTINCT 
	ItemCategory.Category_ID
	,ItemCategory.Category_Name 
FROM 
	ItemCategory (NOLOCK)
INNER JOIN 
	Item (NOLOCK) ON (Item.Category_ID = ItemCategory.Category_ID)
WHERE
	Item.SubTeam_No = @SubTeam_No

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamCategory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamCategory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamCategory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamCategory] TO [IRMAReportsRole]
    AS [dbo];

