CREATE PROCEDURE dbo.GetSubTeamByProductType
	@ProductType_ID as int 
AS 
BEGIN
    SET NOCOUNT ON
	DECLARE @SubTeamType_ID as tinyint
	IF 		@ProductType_ID = 1 SET @SubTeamType_ID = 4	-- (<=4) will get all Retail/Manufacturing/Expense
	ELSE IF @ProductType_ID = 2 SET @SubTeamType_ID = 5	-- Packaging
	ELSE IF @ProductType_ID = 3 SET @SubTeamType_ID = 6	-- Supplies

	IF @ProductType_ID = 1	-- Product
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
			WHERE 
				SubTeam.SubTeamType_ID <= @SubTeamType_ID	-- Retail, Manufacturing, RetailManufacturing, Expense
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
			WHERE 
				SubTeam.SubTeamType_ID = @SubTeamType_ID	-- Packaging or Supplies
		    ORDER BY 
				SubTeam_Name
		END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamByProductType] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamByProductType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamByProductType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamByProductType] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamByProductType] TO [IRMAExcelRole]
    AS [dbo];

