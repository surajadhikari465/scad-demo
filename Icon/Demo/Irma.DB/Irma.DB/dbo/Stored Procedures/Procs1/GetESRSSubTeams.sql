CREATE PROCEDURE dbo.GetESRSSubTeams
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		  SubTeam_Name 
		, SubTeam_No 
		, Team_No
		, Dept_No
		, SubDept_No
    FROM 
		SubTeam (NOLOCK)
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetESRSSubTeams] TO [IRMAClientRole]
    AS [dbo];

