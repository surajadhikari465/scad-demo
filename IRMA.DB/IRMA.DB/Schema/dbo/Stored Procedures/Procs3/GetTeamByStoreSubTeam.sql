CREATE PROCEDURE dbo.GetTeamByStoreSubTeam
@Store_No int,
@SubTeam_No int,
@SubTeam_Name varchar(100) output,
@Team_No int output,
@Team_Name varchar(100) output
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @Team_No = StoreSubTeam.Team_No, @Team_Name = Team.Team_Name, @SubTeam_Name = SubTeam.SubTeam_Name
    FROM StoreSubTeam
        inner join
            team
            on StoreSubTeam.Team_No = Team.Team_No
        inner join
            Subteam
            on StoreSubteam.SubTeam_No = SubTeam.Subteam_No
    where StoreSubTeam.Store_No = @Store_No and StoreSubTeam.SubTeam_no = @SubTeam_No

    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamByStoreSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamByStoreSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamByStoreSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTeamByStoreSubTeam] TO [IRMAReportsRole]
    AS [dbo];

