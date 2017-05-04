SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetWarehouses') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetWarehouses
GO


CREATE PROCEDURE dbo.GetWarehouses

AS

BEGIN
    SET NOCOUNT ON

    SELECT Store_No, Store_Name, BusinessUnit_ID, ISNULL(EXEWarehouse, 0) As EXEWarehouse
    FROM Store
    WHERE Distribution_Center = 1

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


