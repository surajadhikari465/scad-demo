SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryLocation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryLocation]
GO



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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

