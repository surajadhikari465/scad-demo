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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreUserSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreUserSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreUserSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreUserSubTeam] TO [IRMAReportsRole]
    AS [dbo];

