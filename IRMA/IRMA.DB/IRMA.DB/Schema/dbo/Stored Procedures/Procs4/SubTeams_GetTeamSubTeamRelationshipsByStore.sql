create PROCEDURE [dbo].[SubTeams_GetTeamSubTeamRelationshipsByStore]
	-- Add the parameters for the stored procedure here
	@Store_no int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	ss.SubTeam_No, s.SubTeam_Name, s.SubTeam_Abbreviation, ss.Team_No, t.Team_Name, ss.PS_Team_No, ss.PS_SubTeam_No,
            ss.CostFactor, ss.ICVID, IsNull( ccv.ICVABBRV, '-NONE-' ) as ICVABBRV
	FROM StoreSubteam ss 
	INNER JOIN SubTeam s ON 
		ss.SubTeam_No = s.SubTeam_No
	INNER JOIN Team t ON 
		ss.Team_No =t.Team_no
	left outer join CycleCountVendor ccv
	    on  ccv.ICVID = ss.ICVID
	WHERE Store_No = @Store_no
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetTeamSubTeamRelationshipsByStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetTeamSubTeamRelationshipsByStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_GetTeamSubTeamRelationshipsByStore] TO [IRMAClientRole]
    AS [dbo];

