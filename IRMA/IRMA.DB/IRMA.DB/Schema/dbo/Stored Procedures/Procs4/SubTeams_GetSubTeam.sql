CREATE PROCEDURE dbo.[SubTeams_GetSubTeam]
	@SubTeam_No int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	S.*
			, case
				when D.DistSubteam_No is not null then 1
				else 0
				end as Distribution
	FROM	
	SubTeam S 
	left outer join DistSubteam D on S.Subteam_No = D.DistSubteam_No
	WHERE S.SubTeam_No = @SubTeam_No
	


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetSubTeam] TO [IRMAClientRole]
    AS [dbo];

