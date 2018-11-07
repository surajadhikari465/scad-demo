CREATE PROCEDURE dbo.Teams_Validate_TeamAbbr
	@Team_No int, 
	@Team_Abbr varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	*
	FROM	Team
	WHERE rtrim(ltrim(Team_Abbreviation)) = @Team_Abbr
	AND Team_No not in  (@Team_No)


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_Validate_TeamAbbr] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_Validate_TeamAbbr] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_Validate_TeamAbbr] TO [IRMAClientRole]
    AS [dbo];

