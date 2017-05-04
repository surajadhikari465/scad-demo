CREATE PROCEDURE dbo.GetInventoryLocationsByStore
	@StoreID int
	,@SubTeamID int = null

AS

SET NOCOUNT ON

SELECT
	Loc.InvLoc_ID
	,Loc.InvLoc_Name
	,Loc.InvLoc_Desc
FROM
	InventoryLocation Loc (nolock)
INNER JOIN 
	Store (nolock) 
	ON Loc.Store_No = Store.Store_No 
INNER JOIN 
	SubTeam (nolock) 
	ON Loc.SubTeam_No = SubTeam.SubTeam_No
WHERE
	Store.Store_No = @StoreID
	and SubTeam.SubTeam_No = Isnull(@SubTeamID, SubTeam.SubTeam_No)
 ORDER BY
	Loc.InvLoc_Name

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationsByStore] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationsByStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationsByStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocationsByStore] TO [IRMAReportsRole]
    AS [dbo];

