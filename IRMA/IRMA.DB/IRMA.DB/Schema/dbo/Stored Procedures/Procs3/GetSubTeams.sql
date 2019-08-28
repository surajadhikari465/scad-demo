CREATE PROCEDURE dbo.GetSubTeams
AS 
BEGIN
    SET NOCOUNT ON
    
     SELECT 
		SubTeam_Name, 
		SubTeam_No, 
		SubTeam_Unrestricted = 	CASE 
									WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
											OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
											OR (SubTeam.SubTeamType_ID = 4)	-- Expense
										 ) THEN 1 -- Unrestricted
									ELSE 0 -- Restricted to retail subteam
								END,
		SubTeamType_ID,
		FixedSpoilage		=	ISNULL(FixedSpoilage, 0),
		Dept_No,
		IsDisabled,
		AlignedSubTeam
    FROM 
		SubTeam (NOLOCK)
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeams] TO [IRMAExcelRole]
    AS [dbo];

