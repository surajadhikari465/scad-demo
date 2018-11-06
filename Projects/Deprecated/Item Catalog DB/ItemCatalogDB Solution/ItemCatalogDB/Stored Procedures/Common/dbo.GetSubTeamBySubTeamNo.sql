IF OBJECT_ID('GetSubTeamBySubTeamNo', 'P') IS NOT NULL
	DROP PROCEDURE GetSubTeamBySubTeamNo
GO

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