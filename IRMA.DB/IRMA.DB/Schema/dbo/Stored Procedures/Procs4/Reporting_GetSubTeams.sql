-- =============================================
-- Author:		Hussain Hashim
-- Create date: 8/22/2007
-- Description:	SP used for parameter list for reports
-- =============================================
CREATE PROCEDURE [dbo].[Reporting_GetSubTeams]
	@blnAll	AS BIT=NULL
AS
BEGIN


    
IF @blnAll = 1
BEGIN

    SELECT 
		  ' All' AS SubTeam_Name, 
		  ' All' AS SubTeam_No, 
		  0 AS SubTeam_Unrestricted
	UNION
    SELECT 
		  SubTeam_Name, 
		  CONVERT(VARCHAR, SubTeam_No), 
		  SubTeam_Unrestricted = 
			CASE 
				WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
						OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
						OR (SubTeam.SubTeamType_ID = 4)	-- Expense
					 ) THEN 1 -- Unrestricted
				ELSE 0 -- Restricted to retail subteam
			END

    FROM 
		SubTeam (NOLOCK)
    ORDER BY 
		SubTeam_Name
END
ELSE
BEGIN
    SELECT 
		  SubTeam_Name 
		, SubTeam_No 
		, SubTeam_Unrestricted = 
			CASE 
				WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
						OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
						OR (SubTeam.SubTeamType_ID = 4)	-- Expense
					 ) THEN 1 -- Unrestricted
				ELSE 0 -- Restricted to retail subteam
			END

    FROM 
		SubTeam (NOLOCK)
    ORDER BY 
		SubTeam_Name
END


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetSubTeams] TO [IRMAReportsRole]
    AS [dbo];

