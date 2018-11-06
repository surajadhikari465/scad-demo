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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendZones] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendZones] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendZones] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendZones] TO [IRMAReportsRole]
    AS [dbo];

