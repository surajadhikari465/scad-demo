SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteInventoryLocations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteInventoryLocations]
GO


CREATE PROCEDURE dbo.DeleteInventoryLocations
@InvLoc_ID int 
AS

SET NOCOUNT ON

DELETE 
FROM InventoryLocationItems
WHERE InvLocID = @InvLoc_ID


DELETE 
FROM InventoryLocation
WHERE InvLoc_ID = @InvLoc_ID

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

