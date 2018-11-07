SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSupplierSubteamByVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSupplierSubteamByVendor]
GO

CREATE PROCEDURE dbo.GetSupplierSubteamByVendor
	@Vendor_ID int,
	@ProductType_ID tinyint 
AS
BEGIN
    SET NOCOUNT ON

	-- Convert Product Type to a SubTeamType
	DECLARE @SubTeamType_ID as tinyint
	IF 		@ProductType_ID = 1 SET @SubTeamType_ID = 3	-- (<=3) will get all Retail/Manufacturing
	ELSE IF @ProductType_ID = 2 SET @SubTeamType_ID = 5	-- Packaging
	ELSE IF @ProductType_ID = 3 SET @SubTeamType_ID = 6	-- Supplies
	ELSE	SET @SubTeamType_ID = NULL					-- Don't limit

	IF @ProductType_ID = 1	-- Product
		BEGIN
		    SELECT DISTINCT 
				  SubTeam.SubTeam_No
				, SubTeam_Name 
				, SubTeam_Unrestricted = 
					CASE 
						WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
								OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
								OR (SubTeam.SubTeamType_ID = 5)	-- Packaging
								OR (SubTeam.SubTeamType_ID = 6)	-- Supplies
							 ) THEN 1 -- Unrestricted
						ELSE 0 -- Restricted to retail subteam
					END
		    FROM SubTeam WITH (NOLOCK)
		    INNER JOIN Vendor WITH (NOLOCK)
			ON Vendor.Vendor_ID = @Vendor_ID
		    INNER JOIN ZoneSubTeam WITH (NOLOCK)
				ON ZoneSubTeam.SubTeam_No = SubTeam.SubTeam_No 
				AND ZoneSubTeam.Supplier_Store_No = Vendor.Store_No
				AND (SubTeam.SubTeamType_ID <= @SubTeamType_ID)	-- Retail, Manufacturing, RetailManufacturing
		END
	ELSE 
		BEGIN
		    SELECT DISTINCT 
				  SubTeam.SubTeam_No
				, SubTeam_Name 
				, SubTeam_Unrestricted = 0
		    FROM SubTeam WITH (NOLOCK)
		    INNER JOIN Vendor WITH (NOLOCK)
			ON Vendor.Vendor_ID = @Vendor_ID
		    INNER JOIN ZoneSubTeam WITH (NOLOCK)
				ON ZoneSubTeam.SubTeam_No = SubTeam.SubTeam_No 
				AND ZoneSubTeam.Supplier_Store_No = Vendor.Store_No
				AND ISNULL(@SubTeamType_ID, SubTeam.SubTeamType_ID) = SubTeam.SubTeamType_ID -- Packaging or Supplies or ALL
		END
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



