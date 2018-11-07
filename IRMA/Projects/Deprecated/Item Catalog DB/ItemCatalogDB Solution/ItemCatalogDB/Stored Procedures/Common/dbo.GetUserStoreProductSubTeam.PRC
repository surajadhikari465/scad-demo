SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserStoreProductSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserStoreProductSubTeam]
GO


CREATE PROCEDURE dbo.GetUserStoreProductSubTeam 
	@Store_No int,
	@User_ID int
AS 

SELECT 
	Users.UserName,Users.telxon_store_limit,Store.Store_No,
	SubTeam.SubTeam_No 
	,SubTeam.SubTeam_Name 
	,SubTeam_Unrestricted = 
		CASE 
			WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
					OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
					OR (SubTeam.SubTeamType_ID = 4)	-- Expense
				 ) THEN 1 -- Unrestricted
			ELSE 0 -- Restricted to retail subteam
		END
FROM 
	Store
INNER JOIN 
	StoreSubTeam (NOLOCK) ON Store.Store_No = StoreSubTeam.Store_No
INNER JOIN 
	SubTeam (NOLOCK) ON StoreSubTeam.SubTeam_No = SubTeam.SubTeam_No
INNER JOIN 
	Users (NOLOCK) ON ISNULL(Users.Telxon_Store_Limit, Store.Store_No) = Store.Store_No
WHERE 	
	Store.Store_No = @Store_No
	AND Users.[User_ID] = @User_ID
	AND (SubTeam.SubTeamType_ID <= 4) -- Retail, Manufacturing, RetailManufacturing, Expense

ORDER BY 
	SubTeam_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


