SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryLocationsByStore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryLocationsByStore]
GO



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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

