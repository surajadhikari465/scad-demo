SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].CheckIfWarehouseItem') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].CheckIfWarehouseItem
GO


CREATE PROCEDURE dbo.CheckIfWarehouseItem
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON

    SELECT dbo.fn_IsEXEDistributed(@Item_Key)

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

