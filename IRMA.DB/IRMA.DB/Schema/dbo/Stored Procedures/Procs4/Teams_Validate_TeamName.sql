CREATE PROCEDURE dbo.Teams_Validate_TeamName
	@Team_No int,
	@Team_Name varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*
	FROM	Team
	WHERE rtrim(ltrim(Team_Name)) = @Team_Name
	AND Team_No not in (@Team_No)


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_Validate_TeamName] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_Validate_TeamName] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_Validate_TeamName] TO [IRMAClientRole]
    AS [dbo];

