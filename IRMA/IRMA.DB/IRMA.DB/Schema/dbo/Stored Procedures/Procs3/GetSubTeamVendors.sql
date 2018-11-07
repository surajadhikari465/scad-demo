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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamVendors] TO [IRMAReportsRole]
    AS [dbo];

