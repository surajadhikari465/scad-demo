CREATE FUNCTION [dbo].[ExtractPsSubTeamNumberFromSubTeamName]
(
	@SubTeamName nvarchar(255)
)
RETURNS nvarchar(255)
AS
BEGIN
	RETURN SUBSTRING(@SubTeamName, LEN(@SubTeamName)-4,4)
END

GO