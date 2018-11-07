/****** Object:  StoredProcedure [dbo].[GetCategoriesAndSubTeams]    Script Date: 10/13/2006 14:39:00 ******/
CREATE PROCEDURE dbo.[GetCategoriesAndSubTeams]

AS

BEGIN
    SET NOCOUNT ON

	----------------------------------------------
	-- return all categories by subteam, sorted by subteam then category
	----------------------------------------------
	SELECT ST.SubTeam_No, 
		ST.SubTeam_Name, 
		IC.Category_ID, 
		IC.Category_Name
	FROM ItemCategory IC (NOLOCK) 
		INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = IC.SubTeam_No
	ORDER BY ST.SubTeam_Name, IC.Category_Name

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesAndSubTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCategoriesAndSubTeams] TO [IRMAExcelRole]
    AS [dbo];

