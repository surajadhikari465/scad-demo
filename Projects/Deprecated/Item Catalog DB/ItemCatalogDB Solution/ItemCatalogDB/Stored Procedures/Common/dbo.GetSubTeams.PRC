SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeams]
GO


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
		Dept_No
    FROM 
		SubTeam (NOLOCK)
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


