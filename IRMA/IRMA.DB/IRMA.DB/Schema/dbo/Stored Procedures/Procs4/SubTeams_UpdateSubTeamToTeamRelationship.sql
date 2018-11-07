create PROCEDURE [dbo].[SubTeams_UpdateSubTeamToTeamRelationship]
	-- Add the parameters for the stored procedure here
	@Store_No int, 
	@SubTeam_No int, 
	@Team_No int,
	@PS_SubTeam_No int, 
	@PS_Team_No int,
    @CostFactor decimal(9,4),
    @ICVID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update StoreSubTeam
	Set Team_No = @Team_No, PS_SubTeam_No = @PS_SubTeam_No, PS_Team_No = @PS_Team_No,
            CostFactor = @CostFactor, ICVID = @ICVID
	Where Store_No = @Store_no and SubTeam_No = @SubTeam_No
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_UpdateSubTeamToTeamRelationship] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_UpdateSubTeamToTeamRelationship] TO [IRMAClientRole]
    AS [dbo];

