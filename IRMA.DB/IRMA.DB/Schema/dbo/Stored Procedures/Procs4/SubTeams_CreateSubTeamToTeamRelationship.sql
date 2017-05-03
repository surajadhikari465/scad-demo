create PROCEDURE [dbo].[SubTeams_CreateSubTeamToTeamRelationship]
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
	Insert Into StoreSubTeam
	(
		Store_no, team_No, SubTeam_No, CasePriceDiscount, CostFactor, ICVID, PS_SubTeam_No, PS_Team_No
	)
	VALUES
	(
		@Store_no, @team_No, @SubTeam_No, 1, @CostFactor, @ICVID, @PS_SubTeam_No, @PS_Team_No
	)
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_CreateSubTeamToTeamRelationship] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_CreateSubTeamToTeamRelationship] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_CreateSubTeamToTeamRelationship] TO [IRMAClientRole]
    AS [dbo];

