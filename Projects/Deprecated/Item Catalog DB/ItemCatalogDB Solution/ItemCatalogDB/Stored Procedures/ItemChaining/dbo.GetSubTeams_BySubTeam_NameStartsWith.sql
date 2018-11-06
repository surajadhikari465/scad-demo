if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeams_BySubTeam_NameStartsWith]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeams_BySubTeam_NameStartsWith]
GO

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
go