CREATE PROCEDURE dbo.GetVendStoreSubTeam 
	@Vendor_ID int
AS 
-- **************************************************************************
-- Procedure: GetVendStoreSubTeam()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from Global.vb to determine transfer to subteams
-- upon the Vendor ID provided.
--
-- Modification History:
-- Date			Init	Comment
-- 11/17/2009	BBB		update existing SP that will correctly identify Non-Regional
--						WFM Facilities and return all Retail/Manufacturing subteams
--						for these types of orders; reformatted for readability;
-- 04/06/2010	BBB		Removed lookup needed for GL Enhancements and leave values
--						set in initial query as is
-- **************************************************************************
	
--**************************************************************************
--Pre-existing logic to pull subteams based upon Vendor ID
--**************************************************************************
BEGIN
	SELECT 
		StoreSubTeam.SubTeam_No,
		SubTeam.SubTeam_Name, 
		SubTeam_Unrestricted = 
				CASE 
					WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
							OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
							OR (SubTeam.SubTeamType_ID = 4)	-- Expense
						 ) THEN 1 -- Unrestricted
					ELSE 0 -- Restricted to retail subteam
				END
		FROM 
			Vendor					WITH (NOLOCK)
			INNER JOIN Store		WITH (NOLOCK)	ON	Vendor.Store_No			= Store.Store_No 
													AND Vendor_ID				= @Vendor_ID
			INNER JOIN StoreSubTeam WITH (NOLOCK)	ON	StoreSubTeam.Store_No	= Store.Store_No
			INNER JOIN SubTeam		WITH (NOLOCK)	ON	SubTeam.SubTeam_No		= StoreSubTeam.SubTeam_No
			--	AND (SubTeam.SubTeamType_ID <= 4)	-- Retail, Manufacturing, RetailManufacturing, Expense
		ORDER BY 
			SubTeam_Name
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendStoreSubTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendStoreSubTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendStoreSubTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendStoreSubTeam] TO [IRMAReportsRole]
    AS [dbo];

