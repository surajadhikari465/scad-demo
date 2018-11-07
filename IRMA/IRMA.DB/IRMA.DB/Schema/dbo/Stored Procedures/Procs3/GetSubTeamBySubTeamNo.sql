
CREATE PROCEDURE [dbo].[GetSubTeamBySubTeamNo]
	@SubTeam_No int
AS
	SELECT 
		st.SubTeam_No,
		st.SubTeam_Name,
		st.AlignedSubTeam
FROM SubTeam st (NOLOCK)
WHERE st.SubTeam_No = @SubTeam_No	

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBySubTeamNo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamBySubTeamNo] TO [IRSUser]
    AS [dbo];

