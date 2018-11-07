CREATE PROCEDURE dbo.GetEXEDistSubTeams
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		  SubTeam_Name, SubTeam_No 
    FROM 
		SubTeam (NOLOCK)
	WHERE EXEDistributed = 1
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetEXEDistSubTeams] TO [IRMAClientRole]
    AS [dbo];

