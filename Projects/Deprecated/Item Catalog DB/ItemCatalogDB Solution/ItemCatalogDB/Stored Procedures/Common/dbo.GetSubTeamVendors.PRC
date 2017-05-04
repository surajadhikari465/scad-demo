SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeamVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeamVendors]
GO


CREATE PROCEDURE dbo.GetSubTeamVendors
	@SubTeam_No int
AS 

SET NOCOUNT ON

SELECT DISTINCT 
	Vendor.CompanyName
	,Vendor.Vendor_ID
FROM 
	Vendor (NOLOCK)
INNER JOIN  
	(ItemVendor INNER JOIN Item (NOLOCK) ON (ItemVendor.Item_Key = Item.Item_Key)) ON (Vendor.Vendor_ID = ItemVendor.Vendor_ID)
WHERE
	Item.SubTeam_No = @SubTeam_No

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

