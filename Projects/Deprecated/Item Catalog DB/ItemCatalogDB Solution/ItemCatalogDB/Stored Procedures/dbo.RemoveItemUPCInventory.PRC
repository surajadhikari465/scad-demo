SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RemoveItemUPCInventory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RemoveItemUPCInventory]
GO


CREATE PROCEDURE dbo.RemoveItemUPCInventory
@UPC varchar(18)
AS 

UPDATE ItemUPC
SET Deleted_UPC = @UPC,
    UPC = '0',
    UPC_Changed = 1
WHERE UPC = @UPC


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



