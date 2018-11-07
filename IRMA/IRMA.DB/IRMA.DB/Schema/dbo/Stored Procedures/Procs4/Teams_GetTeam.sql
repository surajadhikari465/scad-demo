CREATE PROCEDURE dbo.Teams_GetTeam
	@Team_No int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*
	FROM	Team
	WHERE Team_No = @Team_No


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_GetTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_GetTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_GetTeam] TO [IRMAClientRole]
    AS [dbo];

