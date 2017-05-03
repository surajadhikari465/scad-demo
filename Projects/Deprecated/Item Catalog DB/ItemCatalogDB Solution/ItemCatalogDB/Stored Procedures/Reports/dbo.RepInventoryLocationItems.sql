SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RepInventoryLocationItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RepInventoryLocationItems]
GO


CREATE PROCEDURE dbo.RepInventoryLocationItems

	@InvLocID int

AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

SELECT
	Store.Store_Name
	,ItemIdentifier.Identifier
	,InventoryLocation.InvLoc_Name
	,Item.Item_Description
FROM
	InventoryLocation
INNER JOIN 
	Store (nolock) ON Store.Store_No = InventoryLocation.Store_No
INNER JOIN
	InventoryLocationItems (nolock) ON InventoryLocation.InvLoc_ID = InventoryLocationItems.InvLocID
INNER JOIN
	Item (nolock) ON InventoryLocationItems.Item_Key = Item.Item_Key
INNER JOIN
	ItemIdentifier (nolock) ON Item.Item_Key = ItemIdentifier.Item_Key
WHERE
	InventoryLocation.InvLoc_ID = @InvLocID
ORDER BY 
	InventoryLocation.InvLoc_Name

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

