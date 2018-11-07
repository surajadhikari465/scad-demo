CREATE PROCEDURE [dbo].[GetRetailSubteamsByTeamNo] 

	@Team_No int
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		SubTeam_No, SubTeam_Name 
    FROM 
		SubTeam
    WHERE 
		Team_No = @Team_No
		AND SubTeamType_ID IN(1,3)
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailSubteamsByTeamNo] TO [IRMAAdminRole]
    AS [dbo];

