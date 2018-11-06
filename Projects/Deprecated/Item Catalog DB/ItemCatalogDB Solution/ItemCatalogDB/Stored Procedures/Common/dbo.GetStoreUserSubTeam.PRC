SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreUserSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreUserSubTeam]
GO


CREATE PROCEDURE dbo.GetStoreUserSubTeam 
	@Store_No int,
	@User_ID int
AS 

SELECT 
	  US.SubTeam_No 
	, SubTeam.SubTeam_Name 
	, SubTeam_Unrestricted = 
		CASE 
			WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
					OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
					OR (SubTeam.SubTeamType_ID = 4)	-- Expense
				 ) THEN 1 -- Unrestricted
			ELSE 0 -- Restricted to retail subteam
		END
FROM 
	UserStoreSubTeam US WITH (NOLOCK)
INNER JOIN SubTeam WITH (NOLOCK)
	ON US.SubTeam_No = SubTeam.SubTeam_No 
	AND (SubTeam.SubTeamType_ID <= 4)	-- Retail, Manufacturing, RetailManufacturing, Expense
	AND US.[User_ID] = @User_ID
	AND	US.Store_No = @Store_No
ORDER BY 
	SubTeam_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

