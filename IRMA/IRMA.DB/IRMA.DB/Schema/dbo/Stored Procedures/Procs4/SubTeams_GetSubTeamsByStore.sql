CREATE PROCEDURE dbo.SubTeams_GetSubTeamsByStore
	-- Add the parameters for the stored procedure here
	@Store_No int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	ss.SubTeam_No, s.SubTeam_Name, s.SubTeam_Abbreviation
	FROM StoreSubteam ss inner join SubTeam s on ss.SubTeam_No = s.SubTeam_No
	WHERE Store_No = @Store_No	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetSubTeamsByStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetSubTeamsByStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetSubTeamsByStore] TO [IRMAClientRole]
    AS [dbo];

