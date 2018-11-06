CREATE PROCEDURE [dbo].[GetSubTeams_BySubTeam_NameStartsWith] 
@Start varchar(52)

AS

BEGIN
	SELECT @Start = @Start + '%'

	select 
		rtrim(subteam_name) [Value],
		subteam_no [ID] 
	from SubTeam 
	where 
		subteam_name like @Start
	order by 
		subteam_name

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams_BySubTeam_NameStartsWith] TO [IRMAClientRole]
    AS [dbo];

