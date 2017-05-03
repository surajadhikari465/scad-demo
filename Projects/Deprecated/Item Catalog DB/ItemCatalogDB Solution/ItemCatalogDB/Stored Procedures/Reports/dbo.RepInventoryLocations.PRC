SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RepInventoryLocations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RepInventoryLocations]
GO


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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


