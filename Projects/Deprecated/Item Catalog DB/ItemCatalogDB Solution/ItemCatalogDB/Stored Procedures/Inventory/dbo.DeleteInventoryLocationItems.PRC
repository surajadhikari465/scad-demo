SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteInventoryLocationItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteInventoryLocationItems]
GO



CREATE PROCEDURE dbo.DeleteInventoryLocationItems
@InvLocID int,
@Item_Key int = null
AS 

SET NOCOUNT ON

DELETE 
FROM InventoryLocationItems
WHERE InvLocID = @InvLocID and Item_Key = ISNULL(@Item_Key, Item_Key)

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

