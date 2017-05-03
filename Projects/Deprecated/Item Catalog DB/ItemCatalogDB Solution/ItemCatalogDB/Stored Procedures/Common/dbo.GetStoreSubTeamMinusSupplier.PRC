SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreSubTeamMinusSupplier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreSubTeamMinusSupplier]
GO


CREATE PROCEDURE dbo.GetStoreSubTeamMinusSupplier 
	@Store_No int
AS 

SELECT 
		  StoreSubTeam.SubTeam_No 
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
	Vendor
INNER JOIN
    Store ON
    Vendor.Store_No = Store.Store_No
INNER JOIN 
    StoreSubTeam ON
    Store.Store_No = StoreSubTeam.Store_No 
INNER JOIN
    SubTeam ON
    StoreSubTeam.SubTeam_No = SubTeam.SubTeam_No 
WHERE 
	Vendor.Store_No = @Store_No
	AND StoreSubTeam.SubTeam_No NOT IN (SELECT ZoneSubTeam.SubTeam_No 
										FROM ZoneSubTeam 
										INNER JOIN SubTeam 
											ON ZoneSubTeam.SubTeam_No = SubTeam.SubTeam_No
											-- Exclude any subteams that are not Manufacturing OR RetailManufacturing
											-- and which are found in zonesubteam.
											AND ((SubTeam.SubTeamType_ID <> 2) AND (SubTeam.SubTeamType_ID <> 3))
										WHERE ZoneSubTeam.Supplier_Store_No = @Store_No)
ORDER BY 
	SubTeam_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

