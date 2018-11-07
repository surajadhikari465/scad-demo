CREATE PROCEDURE dbo.SubTeams_ValidateSubTeamToTeamRelationships 
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
	SELECT 	* 
	FROM StoreSubTeam 
	WHERE Store_No = @Store_No  and SubTeam_No = @SubTeam_No
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_ValidateSubTeamToTeamRelationships] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_ValidateSubTeamToTeamRelationships] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_ValidateSubTeamToTeamRelationships] TO [IRMAClientRole]
    AS [dbo];

