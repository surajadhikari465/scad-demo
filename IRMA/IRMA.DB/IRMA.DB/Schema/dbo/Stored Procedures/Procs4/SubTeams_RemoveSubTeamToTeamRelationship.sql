create PROCEDURE [dbo].[SubTeams_RemoveSubTeamToTeamRelationship]
	-- Add the parameters for the stored procedure here
	@Store_No int, 
	@SubTeam_No int, 
	@Team_No int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    delete  OrderItemQueue
    where   Store_No = @Store_No
    and     TransferToSubTeam_No = @SubTeam_No

    delete  ScanGunStoreSubTeam
    where   Store_No = @Store_No
    and     SubTeam_No = @SubTeam_No
    
	delete  StoreSubTeam
	where   Store_No = @Store_no
	and     Team_No = @team_No
	and     SubTeam_No = @SubTeam_No
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveSubTeamToTeamRelationship] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveSubTeamToTeamRelationship] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveSubTeamToTeamRelationship] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveSubTeamToTeamRelationship] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveSubTeamToTeamRelationship] TO [IRMAReportsRole]
    AS [dbo];

