SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertInventoryLocationItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertInventoryLocationItem]
GO


CREATE PROCEDURE dbo.InsertInventoryLocationItem 
@InvLocID int, 
@Item_Key int
AS 

SET NOCOUNT ON

INSERT INTO InventoryLocationItems (InvLocID, Item_Key)
VALUES (@InvLocID, @Item_Key)

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


