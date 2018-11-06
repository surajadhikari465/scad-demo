SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSupplierRetailSubteam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSupplierRetailSubteam]
GO

CREATE PROCEDURE dbo.GetSupplierRetailSubteam
	@Store_No int,
	@OrderHeader_SubTeamNo int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT DISTINCT 
		  SubTeam.SubTeam_No
		, SubTeam_Name 
		, SubTeam_Unrestricted = 
			CASE 
				WHEN (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
				THEN 1 -- Unrestricted
				ELSE 0 -- Restricted to retail subteam
			END
	FROM 
		ItemVendor WITH (NOLOCK) 
	INNER JOIN Item WITH (NOLOCK) 
		ON ItemVendor.Item_Key = Item.Item_Key
		AND Vendor_id = (SELECT Vendor_id FROM Vendor WHERE Store_No = @Store_No)
	INNER JOIN SubTeam WITH (NOLOCK) 
		ON Item.SubTeam_No = SubTeam.SubTeam_No
		AND (SubTeam.SubTeamType_ID = 1 OR SubTeam.SubTeamType_ID = 3) -- Retail or RetailManufacturing
        AND ISNULL(Item.DistSubTeam_No, Item.SubTeam_No) = @OrderHeader_SubTeamNo 

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



