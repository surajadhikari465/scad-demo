SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendZones]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendZones]
GO

CREATE PROCEDURE dbo.GetVendZones
    @Store_No int,
	@Vendor_ID int
AS

SELECT DISTINCT Zone.Zone_ID, Zone_Name
FROM Zone
INNER JOIN 
    ZoneSubTeam
    ON ZoneSubTeam.Zone_ID = Zone.Zone_ID
    AND ZoneSubTeam.Supplier_Store_No = ISNULL(@Store_No, (SELECT Store_No FROM Vendor WHERE Vendor_ID = @Vendor_ID))
ORDER BY Zone_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

