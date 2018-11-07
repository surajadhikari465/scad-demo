CREATE PROCEDURE dbo.Teams_LoadTeams
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*
	FROM	Team
	Order By Team_Name


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_LoadTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_LoadTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_LoadTeams] TO [IRMAClientRole]
    AS [dbo];

