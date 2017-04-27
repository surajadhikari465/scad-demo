/****** Object:  StoredProcedure [dbo].[Reporting_GetSubTeams]    Script Date: 08/22/2007 14:03:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetSubTeams]
GO
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