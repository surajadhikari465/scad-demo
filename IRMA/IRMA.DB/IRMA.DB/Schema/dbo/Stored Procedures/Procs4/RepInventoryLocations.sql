CREATE PROCEDURE dbo.RepInventoryLocations

	@Store_No int = null

AS

SET NOCOUNT ON

SELECT
	Store.Store_Name
	,InventoryLocation.InvLoc_Name
	,SubTeam.SubTeam_Name
	,InventoryLocation.InvLoc_Desc
FROM
	Store (nolock)
INNER JOIN 
	InventoryLocation (nolock) ON Store.Store_No = InventoryLocation.Store_No
INNER JOIN
	SubTeam (nolock) ON InventoryLocation.SubTeam_No = SubTeam.SubTeam_No 
WHERE
	InventoryLocation.Store_No = isnull(@Store_No, InventoryLocation.Store_No)
ORDER BY 
	Store.Store_Name, SubTeam.SubTeam_Name, InventoryLocation.InvLoc_Name

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocations] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocations] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocations] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RepInventoryLocations] TO [IRMAReportsRole]
    AS [dbo];

