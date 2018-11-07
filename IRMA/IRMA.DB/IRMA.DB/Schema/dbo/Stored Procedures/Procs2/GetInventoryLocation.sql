CREATE PROCEDURE dbo.GetInventoryLocation
	@InvLoc_ID int

AS

SET NOCOUNT ON

SELECT 
	Store_No
	,SubTeam_No
	,InvLoc_Name
	,InvLoc_Desc
	,Notes 
FROM 
	InventoryLocation (nolock) 
WHERE 
	InvLoc_ID = @InvLoc_ID

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocation] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocation] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocation] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryLocation] TO [IRMAReportsRole]
    AS [dbo];

