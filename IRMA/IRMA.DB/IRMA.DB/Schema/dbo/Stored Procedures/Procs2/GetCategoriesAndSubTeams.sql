/****** Object:  StoredProcedure [dbo].[GetCategoriesAndSubTeams]    Script Date: 10/13/2006 14:39:00 ******/
CREATE PROCEDURE dbo.[GetCategoriesAndSubTeams]
  @bRetail as bit = null,
  @subteamId int = null
AS

BEGIN
    SET NOCOUNT ON;
	----------------------------------------------
	-- return all categories by subteam, sorted by subteam then category
	----------------------------------------------
    SELECT ST.SubTeam_No, 
           ST.SubTeam_Name, 
           IC.Category_ID, 
           IC.Category_Name
	  FROM ItemCategory IC (NOLOCK) 
	  INNER JOIN SubTeam ST (NOLOCK) ON ST.SubTeam_No = IC.SubTeam_No
    WHERE ST.SubTeam_No = IsNull(@subteamId, ST.SubTeam_No) AND (IsNull(@bRetail, 0) = 0 OR ST.SubTeamType_ID IN(1, 3, 7))
	  ORDER BY ST.SubTeam_Name, IC.Category_Name;

    SET NOCOUNT OFF;
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[GetCategoriesAndSubTeams] TO [IRMAClientRole] AS [dbo];
GO
GRANT EXECUTE ON OBJECT::[dbo].[GetCategoriesAndSubTeams] TO [IRMAExcelRole] AS [dbo];

