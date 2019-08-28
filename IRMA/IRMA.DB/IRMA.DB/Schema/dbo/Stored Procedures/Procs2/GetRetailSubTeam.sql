CREATE PROCEDURE dbo.GetRetailSubTeam 
AS 
BEGIN
    SET NOCOUNT ON
    
SELECT DISTINCT
		  RTRIM(SubTeam_Name) As SubTeam_Name 
		, StoreSubTeam.SubTeam_No
		, SubTeam_Unrestricted = 
		CASE 
			WHEN ((SubTeam.SubTeamType_ID = 3) 		-- RetailManufacturing
				 ) THEN 1 -- Unrestricted
			ELSE 0 -- Restricted to retail subteam
		END
		, SubTeam.IsDisabled
		, AlignedSubTeam
    FROM 
		Store WITH (NOLOCK)
    INNER JOIN 
        StoreSubTeam WITH (NOLOCK) ON
        StoreSubTeam.Store_No = Store.Store_No
    INNER JOIN
        SubTeam WITH (NOLOCK) ON
        SubTeam.SubTeam_No = StoreSubTeam.SubTeam_No
    WHERE 
		SubTeam.SubTeamType_ID = 1 
		OR SubTeam.SubTeamType_ID = 3
		OR SubTeam.SubTeamType_ID= 7
    ORDER BY  SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailSubTeam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRetailSubTeam] TO [IRMAReportsRole]
    AS [dbo];

