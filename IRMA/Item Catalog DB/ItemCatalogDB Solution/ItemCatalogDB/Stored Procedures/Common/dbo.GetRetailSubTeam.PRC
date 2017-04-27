SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRetailSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRetailSubTeam]
GO

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
    ORDER BY 
		RTRIM(SubTeam_Name)
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


