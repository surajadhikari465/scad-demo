SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryLocations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryLocations]
GO


CREATE PROCEDURE dbo.GetInventoryLocations
	@StoreID int = null
	,@SubTeamID int = null
	,@LocName varchar(50) = null
	,@LocDesc varchar(50) = null

AS

SET NOCOUNT ON

SELECT
	Loc.InvLoc_ID
	,Loc.SubTeam_No
	,CASE WHEN 
		((SubTeam.SubTeamType_ID = 2)	-- Manufacturing
		OR (SubTeam.SubTeamType_ID = 3) -- RetailManufacturing
		OR (SubTeam.SubTeamType_ID = 4))-- Expense
	 THEN 1 ELSE 0 END AS Manufacturing
	,Store.Store_No
	,Store.Store_Name
	,SubTeam.SubTeam_Name
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
	Store.Store_No = Isnull(@StoreID, Store.Store_No)
	and SubTeam.SubTeam_No = Isnull(@SubTeamID, SubTeam.SubTeam_No)
	and Loc.InvLoc_Name LIKE IsNull('%' + @LocName + '%', Loc.InvLoc_Name)
	and Loc.InvLoc_Desc LIKE Isnull('%' + @LocDesc + '%', Loc.InvLoc_Desc)
 ORDER BY
	Store.Store_Name, SubTeam.SubTeam_Name, Loc.InvLoc_Name

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

