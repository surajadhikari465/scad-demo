SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreSubTeam]
GO


CREATE PROCEDURE dbo.GetStoreSubTeam 
	@Store_No int 
AS 
BEGIN
    SET NOCOUNT ON

    SELECT 
		  SubTeam_Name 
		, StoreSubTeam.SubTeam_No 
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
    FROM Store WITH (NOLOCK)
    INNER JOIN StoreSubTeam WITH (NOLOCK) 
		ON StoreSubTeam.Store_No = Store.Store_No 
		AND Store.Store_No = @Store_No
    INNER JOIN SubTeam WITH (NOLOCK) 
		ON SubTeam.SubTeam_No = StoreSubTeam.SubTeam_No
    ORDER BY 
		SubTeam_Name

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


