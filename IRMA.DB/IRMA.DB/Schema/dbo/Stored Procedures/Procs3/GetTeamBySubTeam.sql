CREATE PROCEDURE dbo.GetTeamBySubTeam
@SubTeam_No int,
@SubTeam_Name varchar(100) output,
@Team_No int output,
@Team_Name varchar(100) output
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @Team_No = SubTeam.Team_No, @Team_Name = Team.Team_Name, @SubTeam_Name = SubTeam.SubTeam_Name
    FROM Subteam
        inner join
            team
            on SubTeam.Team_No = Team.Team_No
    where SubTeam.SubTeam_no = @SubTeam_No

    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamBySubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamBySubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamBySubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamBySubTeam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamBySubTeam] TO [IRMAAVCIRole]
    AS [dbo];

