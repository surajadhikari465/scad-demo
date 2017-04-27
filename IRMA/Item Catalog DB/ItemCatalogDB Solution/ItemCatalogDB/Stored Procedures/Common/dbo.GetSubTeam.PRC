SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeam]
GO


CREATE PROCEDURE dbo.GetSubTeam
    @SubTeam_No int
AS 
BEGIN
    SET NOCOUNT ON
    
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
		, IsExpense = CASE WHEN SubTeamType_ID = 4 THEN 1 ELSE 0 END
        , SubTeam_Abbreviation
    FROM 
		SubTeam (NOLOCK)
    WHERE 
        SubTeam_No = @SubTeam_No
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

