SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteItemUPCInventory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteItemUPCInventory]
GO


CREATE PROCEDURE dbo.DeleteItemUPCInventory
@UPC varchar(18)
AS 

BEGIN

    DELETE 
    FROM ItemUPC
    WHERE UPC = @UPC AND UPC_Changed = 1

    UPDATE ItemUPC
    SET Deleted_UPC = @UPC,
        UPC = '0',
        UPC_Changed = 1
    WHERE UPC = @UPC

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

